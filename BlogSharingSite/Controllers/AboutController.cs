using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogSharingSite.Models.Entity;

namespace BlogSharingSite.Controllers
{
    public class AboutController : Controller
    {
        // GET: About

        BlogSharingEntities db = new BlogSharingEntities();
        public ActionResult Index()
        {
            var degerler = db.TBL_About.ToList();
            return View(degerler);
        }
    }
}