using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using sadu.DAL;
using sadu.Models;
using System.Collections.Generic;
using System;

namespace sadu.Controllers
{
    public class UsersController : Controller
    {
        private SADUContext db = new SADUContext();
        private SubmissionsController s = new SubmissionsController();

        public ActionResult Index()
        {

            if (System.Web.HttpContext.Current.Session["email"] == null)
                return RedirectToAction("Index", "Session");
            else
            {

                ////if user is admin run admin view
                if ((bool)System.Web.HttpContext.Current.Session["isAdmin"])
                {
                    List<String> organizationNames = new List<String>();
                    db.Organizations.ToList().ForEach(o => organizationNames.Add(o.name));
                    db.Dispose();
                    return View("Admin", organizationNames);
                }

                ////if user is not an admin run user view
                else
                {
                    db.Dispose();
                    return View();
                }


            }

        }
        [Route("users")]
        public ActionResult UsersList()
        {
            if (System.Web.HttpContext.Current.Session["email"] == null)
                return RedirectToAction("Index", "Session");
            else
            {
                List<String> organizationsList = new List<String>();
                db.Organizations.ToList().ForEach(o => organizationsList.Add(o.name));
                return View(organizationsList);
            }
        }

        public PartialViewResult GetUsers()
        {
            IEnumerable<User> usersList = db.Users.ToList();
            return PartialView("~/Views/Shared/_UsersTable.cshtml", usersList);
        }

        [Route("profile")]
        public ActionResult UserProfile()
        {
            if (System.Web.HttpContext.Current.Session["email"] == null)
                return RedirectToAction("Index", "Session");
            else
            {
                String email = (String)Session["email"];
                User user = db.Users.FirstOrDefault(u => u.email == email);

                return View(user);
            }
        }

        [HttpPost]
        public ActionResult Create(String organization, String email, String firstName, String lastName, String password, bool isAdmin = false)
        {
            if (System.Web.HttpContext.Current.Session["email"] == null)
                return RedirectToAction("Index", "Session");
            else if ((bool)System.Web.HttpContext.Current.Session["isAdmin"] == false)
                return RedirectToAction("Index", "Users");
            else
            {
                if (db.Users.ToList().FirstOrDefault(u => u.email == email) != null)
                {
                    db.Dispose();
                    return Json(new { success = false, Message = "E-mail address is already registered! Please use another e-mail address." });
                }
                else
                {
                    User user = new User();
                    user.email = email;
                    user.firstName = firstName;
                    user.lastName = lastName;
                    user.password = password;
                    user.isAdmin = isAdmin;

                    if (!isAdmin)
                        user.Organization = db.Organizations.FirstOrDefault(o => o.name == organization);
                    else
                        user.Organization = null;

                    try
                    {
                        db.Users.Add(user);
                        db.SaveChanges();
                        db.Dispose();
                        return Json(new { success = true, Message = "User created" });
                    }
                    catch (Exception)
                    {
                        db.Dispose();
                        return Json(new { success = false, Message = "Something went wrong while creating user" });
                    }
                }
            }
        }


        public ActionResult Edit(String new_email, String new_fname, String new_lname)
        {

            int? id = (int)Session["Id"];
            User user = db.Users.FirstOrDefault(u => u.Id == id);

            if (id != null)
            {
                try
                {
                    user.firstName = new_fname;
                    user.lastName = new_lname;
                    user.email = new_email;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json(new { Message = "Update successful" });

                }
                catch (Exception ex)
                {
                    return Json(new { Message = "Error in updating user profile: " + ex });
                }

            }
            else
            {
                return Json(new { Message = "Cannot find user" });
            }
        }

        // POST: Users/Delete/5
        [HttpPost]
        public ActionResult Delete(int userId)
        {
            if (System.Web.HttpContext.Current.Session["email"] == null)
                return RedirectToAction("Index", "Session");
            else if ((bool)System.Web.HttpContext.Current.Session["isAdmin"] == false)
                return RedirectToAction("Index", "Users");
            else
            {
                User user = db.Users.Find(userId);
                db.Users.Remove(user);
                db.SaveChanges();
                return Json(true);
            }
        }

        public ActionResult LoadBanner()
        {

            String image;

            if ((bool)Session["isAdmin"])
            {
                image = @"\\Assets\\Banners\\admin.png";
            }
            else
            {
                Organization org = (Organization)Session["organization"];
                image = org.OrganizationImage.SidebarImage;
            }

            return Content(image);
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
