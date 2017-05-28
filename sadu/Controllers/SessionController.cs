using System;
using System.Linq;
using System.Web.Mvc;
using sadu.DAL;
using sadu.Models;

namespace sadu.Controllers
{
    public class SessionController : Controller
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
                return RedirectToAction("Index","Users");
            }
        }

        [HttpPost]
        public ActionResult LoginRequest(String username, String password)
        {

            SADUContext db = new SADUContext();

            //find first instance of matching username and password, returns null if no match
            User user = db.Users.FirstOrDefault(u => u.username == username && u.password == password);

            if (user != null)
            {
                System.Web.HttpContext.Current.Session["username"] = user.username;
                System.Web.HttpContext.Current.Session["first_name"] = user.firstName;
                System.Web.HttpContext.Current.Session["last_name"] = user.lastName;
                System.Web.HttpContext.Current.Session["isAdmin"] = user.isAdmin;

                if(user.isAdmin)
                    System.Web.HttpContext.Current.Session["organizations"] = db.Organizations.ToList();
                else
                    System.Web.HttpContext.Current.Session["organizations"] = user.Organizations.ToList();


                return RedirectToAction("Index","Users");
            }
            else
            {
                return Json(false);
            }

        }
    }
}