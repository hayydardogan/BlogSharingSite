using BlogSharingSite.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogSharingSite.Controllers
{
    public class ContactController : Controller
    {
        BlogSharingEntities db = new BlogSharingEntities();
        // GET: Contact
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(TBL_Messages _message)
        {
            _message.messageDate = DateTime.Now;
            _message.messageStatus = true;
            db.TBL_Messages.Add(_message);
            db.SaveChanges();
            TempData["contacted"] = true;
            return View("Index");

        }
    }
}