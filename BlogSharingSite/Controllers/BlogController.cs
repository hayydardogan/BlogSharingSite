using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using BlogSharingSite.Models.Entity;
using PagedList;
using PagedList.Mvc;

namespace BlogSharingSite.Controllers
{
    public class BlogController : Controller
    {
        BlogSharingEntities db = new BlogSharingEntities();
        // GET: Blog

        public ActionResult Index(int p = 1, string search = "")
        {
            if (search == "")
            {
                var degerler = db.TBL_Blogs.Where(m => m.blogStatus == true).OrderByDescending(x => x.blogID).ToList().ToPagedList(p, 8);
                return View(degerler);
            }
            else
            {
                var degerler = db.TBL_Blogs.Where(x => x.blogTitle.Contains(search) || x.blogContent.Contains(search)).OrderByDescending(x => x.blogID).ToList().ToPagedList(p, 8);
                return View(degerler);
            }

        }


        public ActionResult Category(int? id)
        {
            var degerler = db.TBL_Blogs.Where(x => x.TBL_Categories.categoryID == id).ToList();
            return View(degerler);
        }


        public ActionResult BlogDetails(int id)
        {
            var blog = db.TBL_Blogs.Find(id);
            blog.blogViewCount = blog.blogViewCount + 1;
            db.SaveChanges();
            return View(blog);
        }

        public PartialViewResult Comments(int id)
        {
            var degerler = db.TBL_Comments.Where(x => x.blogID == id && x.commentStatus == true).OrderByDescending(x => x.commentDate).ToList();
            return PartialView(degerler);
        }

        [HttpGet]
        public PartialViewResult newComment(int id)
        {
            ViewBag.deger = id;
            return PartialView();
        }

        [HttpPost]
        public PartialViewResult newComment(TBL_Comments _comment)
        {
            _comment.commentStatus = true;
            _comment.commentDate = DateTime.Now;
            db.TBL_Comments.Add(_comment);
            db.SaveChanges();
            TempData["commented"] = true;
            return PartialView();
        }




    }
}