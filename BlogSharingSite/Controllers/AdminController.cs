using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using System.Windows.Forms;
using System.Xml.Linq;
using BlogSharingSite.Models.Entity;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using PagedList;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BlogSharingSite.Controllers
{
    public class AdminController : Controller
    {
        BlogSharingEntities db = new BlogSharingEntities();


        public ActionResult Index(int p = 1)
        {
            if (LoginController._loggedUser != null)
            {
                var degerler = db.TBL_Blogs.OrderByDescending(x => x.blogID).ToList().ToPagedList(p, 5);
                return View(degerler);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }

        [HttpPost]
        public JsonResult UyariSil(int id)
        {
            var blog = db.TBL_Blogs.Find(id);
            db.TBL_Blogs.Remove(blog);
            
            if(db.SaveChanges() > 0)
            {
                return Json("ok");

            }
            else
            {
                return Json("error");
            }
        }

        public ActionResult Messages(int p = 1, int status = 1)
        {
            if (LoginController._loggedUser != null)
            {
                if (status == 1)
                {
                    var degerler = db.TBL_Messages.Where(x => x.messageStatus == true).OrderByDescending(x => x.messageDate).ToList().ToPagedList(p, 5);
                    ViewBag.status = "OKUNMAMIŞ";
                    return View(degerler);
                }
                else
                {
                    var degerler = db.TBL_Messages.Where(x => x.messageStatus == false).OrderByDescending(x => x.messageDate).ToList().ToPagedList(p, 5);
                    ViewBag.status = "OKUNMUŞ";
                    return View(degerler);
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }

        public ActionResult MessageDetails(int id)
        {
            if (LoginController._loggedUser != null)
            {
                var message = db.TBL_Messages.Find(id);
                if (message.messageStatus == true)
                {
                    message.messageStatus = false;
                    db.SaveChanges();
                }
                return View(message);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }

        public ActionResult DeleteFile(int id)
        {
            if (LoginController._loggedUser != null)
            {
                var file = db.TBL_Files.Find(id);
                db.TBL_Files.Remove(file);
                db.SaveChanges();
                return RedirectToAction("FileList");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }

        public ActionResult DeleteMessage(int id)
        {

            if (LoginController._loggedUser != null)
            {
                var message = db.TBL_Messages.Find(id);
                db.TBL_Messages.Remove(message);
                db.SaveChanges();
                return RedirectToAction("Messages");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }


        }

        public ActionResult Comments(int p = 1, int status = 1)
        {
            if (LoginController._loggedUser != null)
            {
                if (status == 1)
                {
                    var degerler = db.TBL_Comments.Where(x => x.commentStatus == true).OrderByDescending(x => x.commentID).ToList().ToPagedList(p, 5);
                    ViewBag.status = "AKTİF";
                    return View(degerler);
                }
                else
                {
                    var degerler = db.TBL_Comments.Where(x => x.commentStatus == false).OrderByDescending(x => x.commentID).ToList().ToPagedList(p, 5);
                    ViewBag.status = "SİLİNEN";
                    return View(degerler);
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }



        }

        public ActionResult DeleteComment(int id)
        {
            if (LoginController._loggedUser != null)
            {
                var comment = db.TBL_Comments.Find(id);
                comment.commentStatus = false;
                db.SaveChanges();
                return RedirectToAction("Comments");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }


        }

        [HttpGet]
        public ActionResult UpdateComment(int id)
        {
            if (LoginController._loggedUser != null)
            {
                var comment = db.TBL_Comments.Find(id);
                return View("UpdateComment", comment);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }


        }

        [HttpPost]
        public ActionResult UpdateComment(TBL_Comments _comment)
        {
            if (LoginController._loggedUser != null)
            {
                var comment = db.TBL_Comments.Find(_comment.commentID);
                comment.commentContent = _comment.commentContent;
                comment.commentUserMail = _comment.commentUserMail;
                comment.commentUserName = _comment.commentUserName;
                comment.commentDate = _comment.commentDate;
                db.SaveChanges();
                return RedirectToAction("Comments");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }


        }

        public ActionResult ActiveComment(int id)
        {
            if (LoginController._loggedUser != null)
            {
                var comment = db.TBL_Comments.Find(id);
                comment.commentStatus = true;
                db.SaveChanges();
                return RedirectToAction("Comments", new { status = 0 });
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }



        }

        [HttpGet]
        public ActionResult NewBlog()
        {

            if (LoginController._loggedUser != null)
            {
                List<SelectListItem> degerler = (from x in db.TBL_Categories.ToList()
                                                 select new SelectListItem
                                                 {
                                                     Text = x.categoryTitle,
                                                     Value = x.categoryID.ToString()
                                                 }).ToList();
                ViewBag.dgr = degerler;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }


        }

        [HttpPost]
        public ActionResult NewBlog(TBL_Blogs _blog)
        {

            if (LoginController._loggedUser != null)
            {
                List<SelectListItem> degerler = (from x in db.TBL_Categories.ToList()
                                                 select new SelectListItem
                                                 {
                                                     Text = x.categoryTitle,
                                                     Value = x.categoryID.ToString()
                                                 }).ToList();
                ViewBag.dgr = degerler;

                var kategori = db.TBL_Categories.Where(x => x.categoryID == _blog.TBL_Categories.categoryID).FirstOrDefault();
                _blog.TBL_Categories = kategori;
                _blog.blogDate = DateTime.Now;
                _blog.blogStatus = true;
                _blog.blogViewCount = 0;

                // File Upload İşlemleri 
                if (Request.Files.Count > 0)
                {
                    DateTime date = new DateTime(2000, 7, 1);
                    TimeSpan diff = DateTime.Now - date;
                    double seconds = diff.TotalSeconds;



                    string fileName = Path.GetFileName(Request.Files[0].FileName);

                    // Dosya adının çakışmaması için sonuna güncel saat bilgisi ekleyelim
                    fileName = seconds.ToString() + fileName;

                    string extension = Path.GetExtension(Request.Files[0].FileName);
                    string path = "~/Images/" + fileName + extension;
                    Request.Files[0].SaveAs(Server.MapPath(path));
                    _blog.blogImage = "~/Images/" + fileName + extension;

                    TBL_Files files = new TBL_Files();
                    files.filePath = "~/Images/" + fileName + extension;
                    files.fileDate = DateTime.Now;
                    files.fileTitle = _blog.blogTitle.ToString() + " --BlogImages";
                    db.TBL_Files.Add(files);
                }

                db.TBL_Blogs.Add(_blog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }



        }

        public ActionResult DeleteBlog(int id)
        {
            if (LoginController._loggedUser != null)
            {
                var blog = db.TBL_Blogs.Find(id);
                db.TBL_Blogs.Remove(blog);
                db.SaveChanges();
                return RedirectToAction("/Index");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }


        }

        [HttpGet]
        public ActionResult UpdateBlog(int id)
        {
            if (LoginController._loggedUser != null)
            {
                List<SelectListItem> degerler = (from x in db.TBL_Categories.ToList()
                                                 select new SelectListItem
                                                 {
                                                     Text = x.categoryTitle,
                                                     Value = x.categoryID.ToString()
                                                 }).ToList();
                ViewBag.dgr = degerler;

                List<SelectListItem> durumlar = new List<SelectListItem>(){
                    new SelectListItem {Text="Aktif",Value=true.ToString()},
                    new SelectListItem {Text="Pasif",Value=false.ToString()}
                };
                ViewBag.drm = durumlar;

                var blog = db.TBL_Blogs.Find(id);
                return View("UpdateBlog", blog);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }



        }

        [HttpPost]
        public ActionResult UpdateBlog(TBL_Blogs _blog)
        {
            if (LoginController._loggedUser != null)
            {
                List<SelectListItem> degerler = (from x in db.TBL_Categories.ToList()
                                                 select new SelectListItem
                                                 {
                                                     Text = x.categoryTitle,
                                                     Value = x.categoryID.ToString()
                                                 }).ToList();
                ViewBag.dgr = degerler;

                List<SelectListItem> durumlar = new List<SelectListItem>()
            {
                new SelectListItem {Text="Aktif",Value=true.ToString()},
                new SelectListItem {Text="Pasif",Value=false.ToString()}
            };
                ViewBag.drm = durumlar;



                var b = db.TBL_Blogs.Find(_blog.blogID);
                b.blogTitle = _blog.blogTitle;
                b.blogImage = _blog.blogImage;
                b.blogCategory = _blog.TBL_Categories.categoryID;
                b.blogContent = _blog.blogContent;
                b.blogStatus = _blog.blogStatus;
                b.blogDate = _blog.blogDate;
                db.SaveChanges();
                return RedirectToAction("/Index");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

            
        }


        public ActionResult Categories(int p = 1)
        {

            if (LoginController._loggedUser != null)
            {
                var categories = db.TBL_Categories.ToList().ToPagedList(p, 5);

                return View(categories);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }


        }

        public ActionResult DeleteCategory(int id)
        {
            if (LoginController._loggedUser != null)
            {
                var category = db.TBL_Categories.Find(id);
                db.TBL_Categories.Remove(category);
                db.SaveChanges();
                return RedirectToAction("Categories");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

            
        }

        [HttpGet]
        public ActionResult NewCategory()
        {
            if (LoginController._loggedUser != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }

        //[HttpPost]
        //public ActionResult NewCategory(TBL_Categories _category)
        //{
        //    db.TBL_Categories.Add(_category);
        //    db.SaveChanges();
        //    return RedirectToAction("Categories");
        //}

        [HttpGet]
        public ActionResult UpdateCategory(int id)
        {
            if (LoginController._loggedUser != null)
            {
                var category = db.TBL_Categories.Find(id);
                return View(category);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
            
        }

        [HttpPost]
        public JsonResult UpdateCategory(TBL_Categories c)
        {
            var durum = db.TBL_Categories.Any(x => x.categoryTitle == c.categoryTitle);
            if(durum) { return Json("already"); }
            else
            {
                var ctg = db.TBL_Categories.Find(c.categoryID);
                ctg.categoryTitle = c.categoryTitle;
                if (db.SaveChanges() > 0)
                { 
                    return Json("ok");
                }
                else
                {
                    return Json("error");
                }
            }
        }

        [HttpPost]
        public JsonResult AddCategory(TBL_Categories c)
        {


            var durum = db.TBL_Categories.Any(x => x.categoryTitle == c.categoryTitle);
            if (durum) { return Json("already"); }
            else
            {
                db.TBL_Categories.Add(c);
                int durum2 = db.SaveChanges();
                if (durum2 > 0)
                    return Json("ok");
                else
                    return Json("error");
            }
        }

        //[HttpPost]
        //public ActionResult UpdateCategory(TBL_Categories _category)
        //{
        //    var category = db.TBL_Categories.Find(_category.categoryID);
        //    category.categoryTitle = _category.categoryTitle;
        //    db.SaveChanges();
        //    return RedirectToAction("Categories");
        //}

        public ActionResult Abouts()
        {
            if (LoginController._loggedUser != null)
            {
                var degerler = db.TBL_About.ToList();
                return View(degerler);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }

        [HttpGet]
        public ActionResult UpdateAbout(int id)
        {

            if (LoginController._loggedUser != null)
            {
                var degerler = db.TBL_About.Find(id);
                return View(degerler);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }


        }

        [HttpPost]
        public ActionResult UpdateAbout(TBL_About _about)
        {
            if (LoginController._loggedUser != null)
            {
                var about = db.TBL_About.Find(_about.aboutID);
                about.aboutContent = _about.aboutContent;
                about.aboutImage = _about.aboutImage;
                db.SaveChanges();
                return RedirectToAction("Abouts");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }



        }

        public ActionResult FileList(int p = 1)
        {
            if (LoginController._loggedUser != null)
            {
                var degerler = db.TBL_Files.OrderByDescending(x => x.fileID).ToList().ToPagedList(p, 5);
                return View(degerler);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }

        public ActionResult FileUpload()
        {
            if (LoginController._loggedUser != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }


        [HttpPost]
        public ActionResult FileUpload(HttpPostedFileBase uploadFile, TBL_Files file)
        {

            if (LoginController._loggedUser != null)
            {
                // File Upload İşlemleri 
                if (uploadFile != null)
                {
                    DateTime date = new DateTime(2000, 7, 1);
                    TimeSpan diff = DateTime.Now - date;
                    double seconds = diff.TotalSeconds;

                    string fileName = Path.GetFileName(uploadFile.FileName);

                    // Dosya adının çakışmaması için sonuna güncel saat bilgisi ekleyelim
                    fileName = seconds.ToString() + fileName;

                    var path = Path.Combine(Server.MapPath("~/Images"), fileName);

                    uploadFile.SaveAs(path);

                    file.filePath = "~/Images/" + fileName;
                    file.fileDate = DateTime.Now;
                    db.TBL_Files.Add(file);
                    db.SaveChanges();
                }
                return RedirectToAction("FileList");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }


        }

        [HttpGet]
        public ActionResult AccountSettings()
        {
            if (LoginController._loggedUser != null)
            {
                var degerler = db.TBL_AdminInfo.Where(x => x.adminUserName == LoginController._loggedUser.adminUserName).FirstOrDefault();
                return View(degerler);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }


        }

        [HttpPost]
        public ActionResult AccountSettings(TBL_AdminInfo _adminInfo)
        {
            if (LoginController._loggedUser != null)
            {
                var admin = db.TBL_AdminInfo.Find(_adminInfo.adminID);
                admin.adminMail = _adminInfo.adminMail;
                admin.adminPassword = _adminInfo.adminPassword;
                admin.adminUserName = _adminInfo.adminUserName;
                db.SaveChanges();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }



        }



        //public ActionResult Test()
        //{
        //    return View();
        //}

        

        public ActionResult BenimVerilerim()
        {
            return View();
        }

        public JsonResult Veriler()
        {
            var veriler = db.TBL_Comments.ToList();

            return Json(new
            {
                Result = from obj in veriler
                         select new
                         {
                             commentID = obj.commentID,
                             commentContent = obj.commentContent,
                             commentUser = obj.commentUserName,
                             commentMail = obj.commentUserMail
                         }
            }, JsonRequestBehavior.AllowGet);
        }




    }
}