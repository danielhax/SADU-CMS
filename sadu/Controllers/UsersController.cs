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
                List<String> organizationNames = new List<String>();

                ////if user is admin run admin view
                if ((bool)System.Web.HttpContext.Current.Session["isAdmin"])
                {
                    db.Organizations.ToList().ForEach(o => organizationNames.Add(o.name));
                    return View("Admin", organizationNames);
                }

                ////if user is not an admin run user view
                else
                {
                    return View();
                }


            }

        }
        [Route("users")]
        public ActionResult UsersList()
        {
            List<String> organizationsList = new List<String>();
            db.Organizations.ToList().ForEach(o => organizationsList.Add(o.name));
            return View(organizationsList);
        }

        public PartialViewResult GetUsers()
        {
            return PartialView("~/Views/Shared/_UsersTable.cshtml", db.Users.ToList());
        }

        [HttpPost]
        public ActionResult Create(String organization, String email, String firstName, String lastName, String password, bool isAdmin = false)
        {
            if (db.Users.ToList().FirstOrDefault(u => u.email == email) != null)
            {
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
                user.Organizations.Add(db.Organizations.ToList().Find(o => o.name == organization));

                try
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    return Json(new { success = true, Message = "User created" });
                }
                catch (Exception)
                {
                    return Json(new { success = false, Message = "Something went wrong while creating user" });
                }
            }
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,username,password,firstName,lastName,isAdmin")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        [Route("delete")]
        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
