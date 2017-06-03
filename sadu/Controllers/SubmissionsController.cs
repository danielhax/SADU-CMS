using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using sadu.DAL;
using sadu.Models;

namespace sadu.Controllers
{
    public class SubmissionsController : Controller
    {
        private SADUContext db = new SADUContext();
        private List<Organization> organizations;
        
        public PartialViewResult GetSubmissions()
        {
            /*
             Always instantiate context every time partial view is update to get updated dataset
             */
            db = new SADUContext();
            organizations = new List<Organization>();

            if ((bool)Session["isAdmin"])
                db.Organizations.ToList().ForEach(o => organizations.Add(o));
            else
                db.Users.FirstOrDefault(u => u.username.Equals(Session["Username"])).Organizations.ToList();

            List<Submission> submissions = new List<Submission>();
            //loop through Model which contains each organization of the current user
            foreach (var org in organizations)
            {
                //loop through submissions of each org
                foreach (var sub in org.Pending_Submissions)
                {
                    submissions.Add(sub);
                }
            }

            return PartialView("~/Views/Shared/_Submissions.cshtml", submissions);
        }

        [HttpPost]
        public ActionResult Create(String submissionOrganization, String submissionTitle, String submissionDetails, String submissionDeadline)
        {
            Submission submission = new Submission();
            submission.Organization = db.Organizations.FirstOrDefault(o => o.name == submissionOrganization);
            submission.title = submissionTitle;
            submission.details = submissionDetails;
            submission.date_created = submissionDeadline;

            try
            {
                db.Submissions.Add(submission);
                db.SaveChanges();
                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        // GET: Submissions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Submission submission = db.Submissions.Find(id);
            if (submission == null)
            {
                return HttpNotFound();
            }
            return View(submission);
        }

        // POST: Submissions/Edit/5
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

        // GET: Submissions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Submission submission = db.Submissions.Find(id);
            if (submission == null)
            {
                return HttpNotFound();
            }
            return View(submission);
        }

        // POST: Submissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Submission submission = db.Submissions.Find(id);
            db.Submissions.Remove(submission);
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
    }
}
