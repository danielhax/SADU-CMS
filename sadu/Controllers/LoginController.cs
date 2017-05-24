using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sadu.DAL;
using sadu.Models;

namespace sadu.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            if (Session["username"] == null)
            {
                return View();
            }
            else
            {
                return Redirect("Users");
            }
        }

        [HttpPost]
        public ActionResult LoginRequest(String username, String password)
        {

            SADUContext db = new SADUContext();

           User user = db.Users.FirstOrDefault(u => u.username == username && u.password == password);

            if (user != null)
            {
                Session["username"] = user.username;
                Session["first_name"] = user.firstName;
                Session["last_name"] = user.lastName;
                Session["isAdmin"] = user.isAdmin;

                return JavaScript("window.location = '" + Url.Action("Index", "Users") + "'");
            }
            else
            {
                return Json(false);
            }

        }
    }
}