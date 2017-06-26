using sadu.DAL;
using sadu.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sadu.Controllers
{
    public class OrganizationsController : Controller
    {
        SADUContext db = new SADUContext();
        // GET: Organizations
        [Route("organizations")]
        public ActionResult Index()
        {
            if (System.Web.HttpContext.Current.Session["email"] == null)
            {
                return RedirectToAction("Index", "Session");
            }
            else if (!(bool)Session["isAdmin"])
            {
                return RedirectToAction("Index", "Users");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public JsonResult Create()
        {
            String name = Request.Form["name"];
            string imageName = Request.Form["OrganizationImage"];

            //change path
            System.IO.File.Move(Server.MapPath("~\\Assets\\Temp\\") + imageName, Server.MapPath("~\\Assets\\") + imageName);

            Organization organization = new Organization();
            OrganizationImage oi = new OrganizationImage();
            oi.Image = imageName;
            organization.name = name;
            organization.OrganizationImage = oi;

            try
            {
                db.Organizations.Add(organization);
                db.Organization_Images.Add(oi);
                db.SaveChanges();
                return Json(new { Message = name, success = true });
            }
            catch (Exception ex)
            {
                return Json(new { Message = "Problem in creating organization: " + ex });
            }
        }

        public PartialViewResult GetOrganizations()
        {
            IEnumerable<Organization> organizations = db.Organizations.ToList();
            return PartialView("~/Views/Shared/_OrganizationsList.cshtml", organizations);
        }

        public PartialViewResult GetOrganizationInfo(int id)
        {
            Organization organization = db.Organizations.First(o => o.Id == id);
            return PartialView("~/Views/Shared/_OrgInfo.cshtml", organization);
        }

        [HttpPost]
        public ActionResult Delete(int orgId)
        {

            if (System.Web.HttpContext.Current.Session["email"] == null)
                return RedirectToAction("Index", "Session");
            else if ((bool)System.Web.HttpContext.Current.Session["isAdmin"] == false)
                return RedirectToAction("Index", "Users");
            else
            {
                try
                {
                    Organization org = db.Organizations.FirstOrDefault(o => o.Id == orgId);

                    //delete Org Image first
                    db.Organization_Images.Remove(org.OrganizationImage);
                    //delete org
                    db.Organizations.Remove(org);
                    db.SaveChanges();
                    return Json(new { Message = "Organization successfully deleted"});
                }
                catch (Exception ex)
                {
                    return Json(new { Message = "Cannot delete this organization: " + ex });
                }
            }
        }

        [HttpPost]
        public ActionResult StoreTempImage()
        {
            HttpFileCollectionBase files = Request.Files;

            //build Temp path
            String path = Server.MapPath("~\\Assets\\");
            path = Path.Combine(path, "Temp\\");

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            Directory.CreateDirectory(path);

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

                path = Path.Combine(path, fname);
                file.SaveAs(path);

                return Json("\\Assets\\Temp\\" + fname);
            }

            return Json(path);

        }
    }
}