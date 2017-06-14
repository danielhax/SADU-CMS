using System;
using System.Linq;
using System.Web.Mvc;
using sadu.DAL;
using sadu.Models;

namespace sadu.Controllers
{
    public class SessionController : Controller
    {
        [Route("login")]
        public ActionResult Index()
        {
            if (Session["email"] == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Users");
            }
        }

        [HttpPost]
        public ActionResult Login(String email, String password)
        {

            SADUContext db = new SADUContext();

            //find first instance of matching username and password, returns null if no match
            User user = db.Users.FirstOrDefault(u => u.email == email && u.password == password);

            if (user != null)
            {
                System.Web.HttpContext.Current.Session["id"] = user.Id;
                System.Web.HttpContext.Current.Session["email"] = user.email;
                System.Web.HttpContext.Current.Session["first_name"] = user.firstName;
                System.Web.HttpContext.Current.Session["last_name"] = user.lastName;
                System.Web.HttpContext.Current.Session["isAdmin"] = user.isAdmin;
                //only user needs to load Organization object, admin only needs organization names (see userscontroller)
                if (!user.isAdmin)
                    System.Web.HttpContext.Current.Session["organization"] = user.Organization;

                return Json(Url.Action("Index", "Users"));
            }
            else
            {
                return Json(false);
            }

        }

        [Route("logout")]
        public ActionResult Logout(bool validRequest = false)
        {
            if (validRequest)
            {
                Session.Abandon();
                return RedirectToAction("Index", "Session");
            }
            else
            {
                return RedirectToAction("Index", "Users");
            }

        }
    }
}