using System.Web.Mvc;
using System.Web.Security;

namespace Common.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login(string Username, string Password)
        {
            if (Username == "Admin" && Password == "Admin")
            {
                FormsAuthentication.SetAuthCookie(Username.ToString(),false);
                return RedirectToAction("Create", "PetDetails");
            }
            else
            {
                //ModelState.AddModelError("Password", "Invalid Username or Password");
                return View();
            }

        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Search", "PetDetails");
        }
    }
}