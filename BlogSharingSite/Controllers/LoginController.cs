using BlogSharingSite.Models.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace BlogSharingSite.Controllers
{
    public class LoginController : Controller
    {
        BlogSharingEntities db = new BlogSharingEntities();
        
        
        public ActionResult Index()
        {
            return View();
        }

        public static TBL_AdminInfo _loggedUser = new TBL_AdminInfo();

        [HttpPost]
        public ActionResult Index(TBL_AdminInfo _admin)
        {
            var isExist = db.TBL_AdminInfo.Where(x => x.adminUserName == _admin.adminUserName && x.adminPassword == _admin.adminPassword).FirstOrDefault();

            if(isExist != null)
            {
                FormsAuthentication.SetAuthCookie(_admin.adminUserName, false);
                Session["adminUserName"] = _admin.adminUserName;
                _loggedUser = _admin;
                return RedirectToAction("/Index","Admin");
            }
            else
            {
                ViewBag.message = "Kullanıcı adı veya şifre hatalı.";
                return RedirectToAction("Index");
            }
        }

        public ActionResult LogOut()
        {
            _loggedUser = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("/Index","Login");
        }
    }
}