using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using sadu.DAL;
using sadu.Models;
using System.Globalization;
using System.Web;
using System.IO;

namespace sadu.Controllers
{
    public class SubmissionsController : Controller
    {
        private SADUContext db = new SADUContext();

        public PartialViewResult GetSubmittals()
        {
            /*
             Always instantiate context every time partial view is update to get updated dataset
             */
            List<Submission> submissionsList = db.Submittals.ToList();
            return PartialView("~/Views/Shared/_Submission.cshtml", submissionsList);
        }

        [HttpPost]
        public ActionResult Create(String submissionTitle, String submissionDetails, String submissionDeadline)
        {
            Submission submittal = new Submission();
            submittal.title = submissionTitle;
            submittal.details = submissionDetails;
            submittal.date_created = DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture);
            submittal.date_deadline = submissionDeadline;

            try
            {
                db.Submittals.Add(submittal);
                db.SaveChanges();
                return Json(new { success = true, Message = "Submission created" });
            }
            catch (Exception)
            {
                return Json(new { success = false, Message = "Something went wrong while creating submission" });
            }
        }

        [HttpPost]
        public ActionResult Submit()
        {
            //SADUContext context = new SADUContext();
            //only used 'using' because of 'an entity object cannot be referenced...' error
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;

                    int submission_id = int.Parse(Request.Form["Id"]);
                    String email = (String)System.Web.HttpContext.Current.Session["email"];
                    User user = db.Users.First(u => u.email == email);

                    //delete last submission by organization
                    Organization organization = (Organization)Session["organization"];
                    //deleteLastSubmission(submittal_id, organization);



                    //for folder name
                    int org_id = organization.Id;

                    /*
                      Create folder
                      (submittal_id)/(organization id)/(user id)_(Date format: MMddyyyy_HHmm)/file

                        Server.MapPath gets the full physical path of the location
                    */
                    String uploadsFolder = Server.MapPath("~\\Files\\");

                    /*
                       Build path used to clear organization's submission folder
                    */
                    String orgFolderName = submission_id + "\\" + org_id + "\\";
                    String orgFolderPath = Path.Combine(uploadsFolder, orgFolderName);

                    /*
                       Build path used to save organization's and user's submission folder
                     */
                    String folderName = submission_id + "\\" + org_id + "\\" + user.Id + "_" + DateTime.Now.ToString("MMddyyyy_HHmm", CultureInfo.InvariantCulture);
                    String fullPath = Path.Combine(uploadsFolder, folderName);

                    //check if org already submitted files, then delete it
                    if (Directory.Exists(orgFolderPath))
                    {
                        Directory.Delete(orgFolderPath, true);
                    }

                    //create org submittal directory
                    DirectoryInfo di = Directory.CreateDirectory(fullPath);

                    for (int i = 0; i < files.Count; i++)
                    {

                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        fullPath = Path.Combine(fullPath, fname);
                        file.SaveAs(fullPath);

                        //try
                        //{
                        //    db.Submissions.Add(submission);
                        //    db.SaveChanges();
                        //}
                        //catch (Exception ex)
                        //{
                        //    return Json(new { Message = "An error occured while recording to database 2." + ex.Message });
                        //}


                    }
                    // Returns message that successfully uploaded  
                    return Json(new { Message = "File Uploaded Successfully!" + org_id + " " + user.Id });
                }
                catch (Exception ex)
                {
                    return Json(new { Message = "An error occured while uploading the file/s. Error details: " + ex.Message });
                }
            }
            else
            {
                return Json(new { Message = "No file found." });
            }

        }

        public ActionResult GetUploads(int Id)
        {
            //unique submission folder
            String path = Server.MapPath("~\\Files\\" + Id + "\\");

            if (Directory.Exists(path) && !IsDirectoryEmpty(path))
            {
                /*
                 Dictionary structure:
                    [{
                        file_name: {
                            full_path: path, 
                            date_uploaded: date,
                            user: user,
                            organization: organization
                        },
                    }]
                 */
                var submissions = new Dictionary<String, List<KeyValuePair<String, Object>>>();

                try
                {
                    foreach (var orgdir in Directory.GetDirectories(path))
                    {
                        
                        int org_id = int.Parse(orgdir.Substring(orgdir.LastIndexOf("\\") + 1));
                        Organization org = db.Organizations.First(o => o.Id == org_id);
                        path = Path.Combine(path, orgdir);

                        foreach (var userdir in Directory.GetDirectories(orgdir))
                        {
                            String user_folder = userdir.Substring(userdir.LastIndexOf("\\") + 1);

                            List<String> split = user_folder.Split('_').ToList();

                            int user_id = int.Parse(split.ElementAt(0));


                            User user = db.Users.First(u => u.Id == user_id);

                            String date = split.ElementAt(1);
                            String time = split.ElementAt(2);
                            path = Path.Combine(path, userdir);

                            foreach (var file in Directory.GetFiles(userdir))
                            {
                                //get file name and extension only
                                //files.Add(Path.GetFileName(file));

                                String fileName = Path.GetFileName(file);
                                path = Path.Combine(path, fileName);

                                submissions.Add(fileName,
                                    new List<KeyValuePair<String, Object>>() {
                                    new KeyValuePair<String, Object>("file_path", path),
                                    new KeyValuePair<String, Object>("date_uploaded", date),
                                    new KeyValuePair<String, Object>("time_uploaded", time),
                                    new KeyValuePair<String, Object>("user_name", user.firstName + " " + user.lastName),
                                    new KeyValuePair<String, Object>("organization", org)
                                    });
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    return Json(new { Message = "An error occured while building model: " + ex.Message }, JsonRequestBehavior.AllowGet);
                }

                return PartialView("~/Views/Shared/_UploadedFilesList.cshtml", submissions);
            }
            else
            {
                return Json(new { message = "no files related to submission with id " + Id }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Submittals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Submission submission = db.Submittals.Find(id);
            if (submission == null)
            {
                return HttpNotFound();
            }
            return View(submission);
        }

        // POST: Submittals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,title,description,date_created,date_submitted,approved")] Submission submission)
        {
            if (ModelState.IsValid)
            {
                db.Entry(submission).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(submission);
        }

        // GET: Submittals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Submission submission = db.Submittals.Find(id);
            if (submission == null)
            {
                return HttpNotFound();
            }
            return View(submission);
        }

        // POST: Submittals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Submission submission = db.Submittals.Find(id);
            db.Submittals.Remove(submission);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        //private bool deleteLastSubmission(int submittalId, Organization organization)
        //{
        //    using (SADUContext context = new SADUContext())
        //    {
        //        Submission submittal = context.Submittals.First(s => s.Id == submittalId);
        //        Submission lastSubmission = submittal.Submissions.FirstOrDefault(s => s.Organization == organization);

        //        //Delete last submission
        //        if (lastSubmission != null)
        //        {
        //            //    return Json(new { Message = "There is a submission" });
        //            try
        //            {
        //                context.Submittals.First(s => s.Submissions.Remove(lastSubmission));
        //                context.SaveChanges();
        //                return true;
        //            }
        //            catch (Exception)
        //            {
        //                return false;
        //            }

        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //}
    }
}
