using Majstic.Models;
using Majstic.Models.VM;
using Majstic.Support;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Majstic.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        #region definition
        ApplicationDbContext Udb = new ApplicationDbContext();
        DB db = new DB();
        ImageProcess imgprocess = new ImageProcess();
        albumManager albumManager = new albumManager();
        sendMail sendmail = new sendMail();
        userdetails Udetails = new userdetails();
        OrderManager Ordermanager = new OrderManager();
        private Utilities Ui = new Utilities();
        #endregion

        //
        // GET: /Admin/
        public ActionResult Index()
        {
            return View();
        }


        // list all users which are not admin

        #region manage users
        // get all non admin users.
        public ActionResult Users()
        {
            var users = Udb.Users.Where(z => z.Roles.Count() == 0).ToList();
            var model = new List<EditUserViewModel>();
            foreach (var user in users)
            {
                var u = new EditUserViewModel(user);
                model.Add(u);
            }
            return View(model);
        }

        public ActionResult Search(string keyword)
        {
            var users = Udb.Users.Where(z => z.UserName == keyword).FirstOrDefault();
            if (users != null)
            {
                return RedirectToAction("UserDetails", "Admin", new { username = keyword });
            }
            else
            {
                string x = keyword.Substring(0, 3);
                var xusers = Udb.Users.Where(z => z.UserName.StartsWith(x)).ToList();
                if (xusers.Count() > 0)
                {
                    var model = new List<EditUserViewModel>();
                    foreach (var user in xusers)
                    {
                        var u = new EditUserViewModel(user);
                        model.Add(u);
                    }
                    return PartialView("_SarchR", model);
                }
            }
            return Content("No Matching Result");
        }

        public ActionResult UserDetails(string username)
        {
            var thisUser = Udb.Users.Where(x => x.UserName == username).FirstOrDefault();
            ViewBag.Name = thisUser.UserName;
            ViewBag.FulName = thisUser.Name;
            ViewBag.Email = thisUser.Email;
            ViewBag.Phone = thisUser.PhoneNumber;
            ViewBag.Points = Udetails.userPoints(username);
            ViewBag.NAlb = db.Albums.Where(x => x.username == username).Count();
            ViewBag.NPic = db.Images.Where(f => f.UserName == username).Count();
            string albumName = db.Albums.Where(d => d.username == username).OrderByDescending(s => s.CreateDate).FirstOrDefault().AlbumName;
            ViewBag.imgs = Udetails.AlbumPreview(albumName);
            ViewBag.lastAlbum = albumName;

            return View();
        }
        #endregion

        #region uploading images
        //1- Select the user , 2- select album or create new album  3- add piictures - 4 - piblish album 

        [HttpGet]
        public ActionResult NewAllbum(string XuName)
        {
            album album = new album();
            album.username = XuName;
            return View(album);
        }

        [HttpPost]
        public ActionResult NewAllbum(album album)
        {
            try
            {
                album.AlbumName = album.AlbumName.ToString();
                album.CreateDate = DateTime.Now;
                album.Share = false;
                album.status = false;
                album.NoOFPics = 0;
                db.Albums.Add(album);
                db.SaveChanges();
                return RedirectToAction("ManageAlbum", "Admin", new { Id = album.Id });
            }
            catch (Exception e)
            {
                return View(album);
            }
        }

        [HttpGet]
        public ActionResult ManageAlbum(int Id)
        {
            Session.Clear();
            ViewBag.status = "Active Album";
            ViewBag.Share = "Public Album";

            album album = db.Albums.Find(Id);
            if (album.status == false)
            {
                ViewBag.status = "Disactive Album";
            }

            if (album.Share == false)
            {
                ViewBag.Share = "Private Album";
            }
            Session["thisAlbum"] = album;
            return View(album);
        }

        [HttpGet]
        public ActionResult ChangeAlbumStatus(int Id, bool status)
        {
            try
            {
                if (status == true)
                {

                    album xalbum = db.Albums.Find(Id);
                    xalbum.status = true;
                    db.Entry(xalbum).State = EntityState.Modified;
                    db.SaveChanges();
                    var xuser = Udb.Users.Where(x => x.UserName == xalbum.username).FirstOrDefault();
                    string Email = xuser.Email;
                    string NAme = xuser.Name;
                    sendmail.AlbumAdded(NAme, Email, xalbum.AlbumName, xalbum.NoOFPics, xalbum.username);
                    return Content(xalbum.AlbumName + " is now Active");
                }
                else
                {
                    album xalbum = db.Albums.Find(Id);
                    xalbum.status = false;
                    db.Entry(xalbum).State = EntityState.Modified;
                    db.SaveChanges();
                    return Content(xalbum.AlbumName + " has been disactivated");
                }

            }
            catch (Exception e)
            {
                return Content("Please try again later");
            }
        }

        [HttpGet]
        public ActionResult AddNewImage(int AlbumId, string UserName)
        {
            Session["UNAme"] = UserName;
            Session["AID"] = AlbumId;
            return PartialView("AddnewImage");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Upload(HttpPostedFileBase[] files)
        {
            try
            {
                string Username = (string)Session["UNAme"];
                int AID = (int)Session["AID"];
                album thisalbum = (album)Session["thisAlbum"];
                foreach (HttpPostedFileBase file in files)
                {
                    Img newImage = new Img();
                    //foreach (string xfile in Request.Files)
                    //{
                    string filename = System.IO.Path.GetFileName(file.FileName);
                    //HttpPostedFileBase file = Request.Files[xfile];
                    newImage.AlbumId = thisalbum.Id;
                    newImage.ImgAlt = "Majestic";
                    newImage.UserName = thisalbum.username;
                    newImage.ContentType = file.ContentType;
                    newImage.PShare = false;
                    newImage.PShareEnd = DateTime.Now;
                    Int32 length = file.ContentLength;
                    byte[] tempImage = new byte[length];
                    file.InputStream.Read(tempImage, 0, length);
                    newImage.ImageData = tempImage;
                    newImage.themb = thisalbum.AlbumName.Replace(" ", string.Empty) + file.FileName;

                    Bitmap bmpUploadedImage = new Bitmap(file.InputStream);
                    Image objImage = imgprocess.ScaleImageandSaveThumb(bmpUploadedImage, 300);
                    objImage.Save(Server.MapPath("~/thumb/") + thisalbum.AlbumName.Replace(" ", string.Empty) + file.FileName);

                    newImage.AlbumId = AID;
                    newImage.UserName = Username;
                    newImage.ImgAlt = "Majestic";
                    newImage.imgName = file.FileName;
                    db.Images.Add(newImage);
                    db.SaveChanges();
                }

                thisalbum.NoOFPics = thisalbum.NoOFPics + files.Count();
                db.Entry(thisalbum).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ManageAlbum", "Admin", new { Id = thisalbum.Id });
            }
            catch (Exception)
            {
                // do nothing ...
            }

            return Content("Somthing wrong happend please try later..");
        }

        [HttpGet]
        public ActionResult Albumthumbs()
        {
            album thisalbum = (album)Session["thisAlbum"];
            var thumbsPics = db.Images.Where(z => z.AlbumId == thisalbum.Id).Select(x => x.themb).ToList();
            return PartialView("Albumthumbs", thumbsPics);
        }

        [HttpGet]
        public ActionResult DeletAlbum()
        {
            album thisalbum = (album)Session["thisAlbum"];
            return View(thisalbum);
        }

        [HttpPost]
        public ActionResult DeletAlbum(album album)
        {
            album thisalbum = (album)Session["thisAlbum"];
            List<Img> relatedImages = db.Images.Where(z => z.AlbumId == thisalbum.Id).ToList();
            foreach (Img z in relatedImages)
            {
                string Path = (Server.MapPath("~/thumb/") + z.themb);
                FileInfo file = new FileInfo(Path);
                if (file.Exists)
                {
                    file.Delete();
                }
            }
            album delalbum = db.Albums.Find(thisalbum.Id);
            db.Albums.Remove(delalbum);
            db.Images.RemoveRange(relatedImages);
            db.SaveChanges();
            return RedirectToAction("Users");
        }

        public ActionResult XDeletImg(string imgName)
        {
            ViewBag.IMGNAME = imgName;
            return PartialView("_XDeletImg");
        }

        [HttpGet]
        public ActionResult DeletImg(string imgName)
        {
            album thisalbum = (album)Session["thisAlbum"];
            Img ximg = db.Images.Where(z => z.themb == imgName).FirstOrDefault();
            thisalbum.NoOFPics = thisalbum.NoOFPics - 1;
            db.Entry(thisalbum).State = EntityState.Modified;
            db.Images.Remove(ximg);
            db.SaveChanges();
            return RedirectToAction("ManageAlbum", "Admin", new { Id = thisalbum.Id });
        }

        public ActionResult ImageDetails(string imageName)
        {
            Img img = db.Images.Where(z => z.themb == imageName).FirstOrDefault();
            return PartialView("ImageDetails", img);
        }


        public ActionResult ManageAlbums(string XuName)
        {
            List<albums> thoseAlbums = albumManager.getAlbumsForAdmin(XuName);
            ViewBag.Total = thoseAlbums.Count();
            ViewBag.UserName = XuName;
            return View(thoseAlbums);
        }


        #endregion


        #region Points


        [HttpGet]
        public ActionResult ChangePoints(string username, int xpoints)
        {
            point points = new point();
            points.points = xpoints;
            points.username = username;
            return PartialView("ChangexPoints", points);
        }

        [HttpPost]
        public ActionResult ChangePoints(string username, int points, DateTime UpdatedDate)
        {
            point userpoint = db.Points.Where(x => x.username == username).FirstOrDefault();
            userpoint.points = points;
            userpoint.UpdatedDate = DateTime.Now;
            db.Entry(userpoint).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("UserDetails", "Admin", new { username = username });
        }


        [HttpGet]
        public ActionResult newmail(string Email)
        {
            email email = new email();
            email.mail = Email;
            return PartialView("_email", email);
        }

        [HttpPost]
        public ActionResult newmail(email xemail)
        {
            sendmail.Msg(xemail.Body, xemail.mail, xemail.Title);
            return Content("Email has been send");
        }
        #endregion


        #region Orders


        public ActionResult orderDetails(int Id)
        {
            orders order = db.Orders.Find(Id);
            return View(order);
        }

        public ActionResult OfferDeyails(int Id)
        {
            offer orderOffer = db.Offers.Find(Id);
            return PartialView("_OfferDeyails", orderOffer);
        }




        public ActionResult ReadyOrder(int Oid)
        {
            DateTime today = DateTime.Now;
            orders order = db.Orders.Find(Oid);
            order.Status = "Ready for Pick Up";
            order.PickUpdate = today.AddDays(2);
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return Content("Order No " + Oid + " is now ready for pickup , user will be notified");
        }

        public ActionResult OrderProcessing(int Oid)
        {

            orders order = db.Orders.Find(Oid);
            order.Status = "Under Processing";
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return Content("Order No " + Oid + " is now Under Processing");
        }


        public ActionResult OrderReject(int Oid)
        {

            orders order = db.Orders.Find(Oid);
            order.Status = "Reject";
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return Content("Order No " + Oid + " has been rejected , User will be notified");
        }



        public ActionResult PicDone(int ID)
        {
            OrderDetails thiOrderDeails = db.OrderDetails.Find(ID);
            thiOrderDeails.status = true;
            db.Entry(thiOrderDeails).State = EntityState.Modified;
            db.SaveChanges();
            return Content("Picture Status has been Chnged");
        }



        public ActionResult Orderx(int Oid)
        {

            orders order = db.Orders.Find(Oid);
            order.Status = "Delivered";
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return Content("Order Has been Delivered , User will be notified");
        }


        public ActionResult NewOrders()
        {
            List<orders> x = db.Orders.Where(z => z.Status == "New Order").ToList();
            return View(x);
        }

        public ActionResult Processing()
        {
            List<orders> x = db.Orders.Where(z => z.Status == "Under Processing").ToList();
            return View(x);
        }

        public ActionResult Rejected()
        {
            List<orders> x = db.Orders.Where(z => z.Status == "Reject").ToList();
            return View(x);
        }

        public ActionResult done()
        {
            List<orders> x = db.Orders.Where(z => z.Status == "Ready for Pick Up").ToList();
            return View(x);
        }

        public ActionResult OrderPics(int Id)
        {
            List<OrderDetails> thisPics = db.OrderDetails.Where(x => x.OrderID == Id).ToList();
            return PartialView("_OrderPics", thisPics);
        }
        #endregion


        #region ManageSlider
        public ActionResult ManageSlider()
        {
            return View();
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadSlider( int x)
        {
            try
            {

                foreach (string file in Request.Files)
                {
                    var postedFile = Request.Files[file];
                    var logopath = postedFile.FileName;
                    int fileLenght = postedFile.ContentLength;

                    
                    Bitmap bmpUploadedImage = new Bitmap(postedFile.InputStream);
                    Image objImage = Ui.ScaleImage(bmpUploadedImage, 1200);
                    switch ( x){
                        case 1:
                            objImage.Save(Server.MapPath("~/Content/MJB/1.png"));
                            break;
                        case 2:
                            objImage.Save(Server.MapPath("~/Content/MJB/2.png"));
                            break;
                        case 3:
                            objImage.Save(Server.MapPath("~/Content/MJB/3.png"));
                            break;
                        case 4:
                            objImage.Save(Server.MapPath("~/Content/MJB/4.png"));
                            break;
                    }

                }
                return RedirectToAction("ManageSlider", "Admin");
            }
            catch (Exception e)
            {

            }
            Session.Clear();
            return Content("Sorry, somthing went wrong , please refresh the page and try again");
        }// end of uploading logo....

        #endregion
    }
}