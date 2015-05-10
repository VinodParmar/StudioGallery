using Majstic.Models;
using Majstic.Models.VM;
using Majstic.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Majstic.Controllers
{
    public class HomeController : Controller
    {
        private DB db = new DB();
        private HomeSupport HS = new HomeSupport();
        private ApplicationDbContext UDb = new ApplicationDbContext();
        public UserManager<ApplicationUser> UserManager { get; private set; }
        public sendMail sendmail = new sendMail();

        //[OutputCache(Duration = 60, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(Pass pass)
        {
            var xx = UDb.Users.Where(x => x.Email == pass.Email).FirstOrDefault();
            if (xx == null)
            {
                return Content("This email isn't registred with us. please endter a valid email address");
            }
            var UID = xx.Id;

            var NewPAss = "25_12_" + DateTime.Now.ToString()+ "_MJ_32";


            UserManager<IdentityUser> userManager =
            new UserManager<IdentityUser>(new UserStore<IdentityUser>());

            userManager.RemovePassword(UID);
            userManager.AddPassword(UID, NewPAss);

            sendmail.ResetPAss(pass.Email, NewPAss, xx.UserName);
            return Content("Please Check your registred email");
        }
        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}


        //public ActionResult UploadImage()
        //{
        //    return View();
        //}

        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult Upload(Img photo)
        //{
        //    try
        //    {
        //        Img newImage = new Img();
        //        foreach (string xfile in Request.Files)
        //        {
        //            HttpPostedFileBase file = Request.Files[xfile];
        //            newImage.AlbumId = 1;
        //            newImage.ImgAlt = photo.ImgAlt;
        //            newImage.ContentType = file.ContentType;
        //            Int32 length = file.ContentLength;
        //            byte[] tempImage = new byte[length];
        //            file.InputStream.Read(tempImage, 0, length);
        //            newImage.ImageData = tempImage;
        //            db.Images.Add(newImage);
        //            db.SaveChanges();

        //        }
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception)
        //    {
        //        // do nothing ...
        //    }

        //    return Content("Shit .!!");
        //}

        //[AcceptVerbs(HttpVerbs.Get)]
        //public ActionResult ShowPhoto(Int32 id)
        //{
        //    //This is my method for getting the image information
        //    // including the image byte array from the image column in
        //    // a database.
        //    ViewBag.Imgno = id;
        //    Img image = db.Images.Find(id);
        //    //As you can see the use is stupid simple.  Just get the image bytes and the
        //    //  saved content type.  See this is where the contentType comes in real handy.
        //    ImageResult result = new ImageResult(image.ImageData, image.ContentType);
        //    return result;
        //}

        public ActionResult indexs()
        {
            return View();
        }


        [OutputCache(Duration = 60, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult HomeTimeLineImgs()
        {
            List<TimeLinePic> TimePic = HS.timelinepic(12);
            return PartialView("_HomeTimeLineImgs", TimePic);
        }

        public ActionResult social()
        {
            return PartialView("_Social");
        }

        [OutputCache(Duration = 60, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult TotalNo()
        {
            int Pic = db.Images.Count();
            int Al = db.Albums.Count();
            int U = UDb.Users.Count();

            ViewBag.Pic = Pic;
            ViewBag.Al = Al;
            ViewBag.U = U;
            return PartialView("_TN");
        }

        [OutputCache(Duration = 60, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult Offers()
        {
            List<offer> thisoffers = db.Offers.Where(x => x.available == true).Take(4).ToList();
            return PartialView("_offers", thisoffers);
        }

        [OutputCache(Duration = 60, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult ContactUS()
        {
            return PartialView("_CU");
        }

        [OutputCache(Duration = 60, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult TimeLine()
        {
            List<TimeLinePic> TimePic = HS.timelinepic(5);
            return View(TimePic);
        }

        [OutputCache(Duration = 60, VaryByParam = "UN", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult USerLine(string UN)
        {
            List<TimeLinePic> TimePic = HS.TimeLineUSer(UN);
            return View(TimePic);
        }

        [OutputCache(Duration = 60, VaryByParam = "Id", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult Pic(int Id)
        {
            TimeLinePic thispic = HS.Pic(Id);
            return View(thispic);
        }

        [OutputCache(Duration = 60, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult MOffer()
        {
            List<offer> Mof = db.Offers.Where(x => x.available == true).ToList();
            return View(Mof);
        }

        public ActionResult pShare(string u, int g, bool ffd)
        {
            if (ffd == true)
            {
                var img = db.Images.Find(g);
                if (img.UserName == u && img.PShare == true && img.PShareEnd >= DateTime.Now)
                {
                    TimeLinePic pic = new TimeLinePic();
                    pic.Id = img.Id;
                    pic.PicThumb = img.themb;
                    pic.Thumb = img.themb;
                    pic.UserName = img.UserName;
                    return View(pic);
                }
                return RedirectToAction("Index", "Home");
            }

            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}