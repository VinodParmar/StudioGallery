using Majstic.Models;
using Majstic.Support;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Majstic.Models.VM;
using System.Web.UI;


namespace Majstic.Controllers
{
    [Authorize]
    public class MembersController : Controller
    {
        ApplicationDbContext UDb = new ApplicationDbContext();
        DB db = new DB();
        albumManager AlbumManager = new albumManager();
        ImageProcess IMGPRO = new ImageProcess();

        //


        #region inex
        [OutputCache(Duration = 10, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult Index()
        {
            propic thiuser = new propic();
            thiuser.username = User.Identity.Name;
            thiuser.proimg = "whopci.png";
            db.ProPics.Add(thiuser);
            point zpoint = new point();
            zpoint.username = User.Identity.Name;
            zpoint.points = 0;
            zpoint.UpdatedDate = DateTime.Now;
            db.Points.Add(zpoint);
            db.SaveChanges();

            return View();
        }


        public ActionResult NewOrder()
        {
            Session["xlist"] = null;
            return View();
        }


        public ActionResult Prview()
        {
            string username = User.Identity.Name;
            var xuser = UDb.Users.Where(d => d.UserName == username).FirstOrDefault();
            string Proppic = db.ProPics.Where(x => x.username == username).FirstOrDefault().proimg;
            int Points = db.Points.Where(a => a.username == username).FirstOrDefault().points;
            Upro thisUser = new Upro();

            thisUser.Email = xuser.Email;
            thisUser.Name = xuser.Name;
            thisUser.PhoneNo = xuser.PhoneNumber;
            thisUser.username = xuser.UserName;

            thisUser.Points = Points;
            thisUser.proimg = Proppic;

            return PartialView("_Prview", thisUser);
        }

        public ActionResult ChangeProPic(string thumb)
        {
            propic thisUser = db.ProPics.Where(x => x.username == User.Identity.Name).FirstOrDefault();
            thisUser.proimg = thumb;
            db.Entry(thisUser).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Prview");
        }

        [OutputCache(Duration = 60, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult GetMyAlbums()
        {
            List<albums> thosealbums = AlbumManager.getAlbumsForUSers(User.Identity.Name);
            int x = thosealbums.Count();
            ViewBag.No = x;
            return PartialView("_MyAlbums", thosealbums);
        }

        [OutputCache(Duration = 60, VaryByParam = "albumId", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult AlbumDetails(int albumId, string AlbumName, int NoPic, bool Status, DateTime Cdate)
        {
            List<pic> thosePic = AlbumManager.getAlbumPic(albumId);
            ViewBag.AlbumName = AlbumName;
            ViewBag.NoPic = NoPic;
            ViewBag.Status = Status;
            ViewBag.Cdate = Cdate;

            Session["Cdate"] = Cdate;
            Session["Status"] = Status;
            Session["NoPic"] = NoPic;
            Session["AlbumName"] = AlbumName;
            Session["albumId"] = albumId;

            return PartialView("_AlbumDetails", thosePic);
        }

        [OutputCache(Duration = 60, VaryByParam = "ImgId", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult ImgDetails(int ImgId, string imgName, string ImgAlt, bool Share, string thumb)
        {
            pic thisPic = new pic();
            thisPic.imgName = imgName;
            thisPic.ImgAlt = ImgAlt;
            thisPic.Share = Share;
            thisPic.themb = thumb;
            thisPic.Id = ImgId;

            ViewBag.AlbumName = (string)Session["AlbumName"];
            ViewBag.NoPic = (int)Session["NoPic"];
            ViewBag.Status = (bool)Session["Status"];
            ViewBag.Cdate = (DateTime)Session["Cdate"];
            ViewBag.ABI = (int)Session["albumId"];

            return PartialView("_ImgDetails", thisPic);
        }

        public ActionResult ChanceStatue(int Id, bool status)
        {
            Img thisImg = db.Images.Find(Id);
            thisImg.Share = status;
            db.Entry(thisImg).State = EntityState.Modified;
            db.SaveChanges();

            if (status == true)
            {
                ViewBag.url = "http://majesticphotostudio.com/Home/pic/" + thisImg.Id;
                return PartialView("_Share");
            }

            else
            {
                return Content("Image is now Private , you can Not share this Image now!");
            }
        }

        [HttpGet]
        public ActionResult PShare(int id)
        {
            PriavteShare x = new PriavteShare();
            x.Id = id;
            x.EndDate = DateTime.Now.AddDays(2);
            return PartialView("_PrivateShare");
        }

        [HttpPost]
        public ActionResult PShare(PriavteShare Pshare)
        {
            Img x = db.Images.Find(Pshare.Id);
            x.PShare = true;
            x.PShareEnd = Pshare.EndDate;

            db.Entry(x).State = EntityState.Modified;
            db.SaveChanges();

            ViewBag.link = "http://www.majesticphotostudio.com/home/pShare?u=" + x.UserName + "&g=" + x.Id + "&ffd=" + true;
            return View();
        }

        [OutputCache(Duration = 60, VaryByParam = "UserName", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult MyPublicImg(string UserName)
        {
            List<pic> MyImages = IMGPRO.TimeLine(UserName);
            return PartialView("_MyImages", MyImages);
        }
        #endregion


        #region offers
        [OutputCache(Duration = 60, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult Offers()
        {
            Session.Clear();
            List<offer> AVoffers = db.Offers.Where(x => x.available == true).ToList();
            return PartialView("_Offers", AVoffers);
        }

        public ActionResult Order(int OfferID, string Details, int Nop, int NoC, decimal Price, int Points, string Name, string thumb)
        {

            ViewBag.OfferID = OfferID;
            ViewBag.Details = Details;
            ViewBag.Nop = Nop;
            ViewBag.NoC = NoC;
            ViewBag.Price = Price;
            ViewBag.Points = Points;
            ViewBag.Name = Name;
            ViewBag.thumb = thumb;
            Session["Nop"] = Nop;
            Session["OfferID"] = OfferID;
            List<int> IDs = (List<int>)Session["IDs"];
            ViewBag.AddNop = IDs == null ? 0 : IDs.Count();
            return View();
        }


        public ActionResult LatestPic()
        {
            List<pic> MyImg = IMGPRO.TopPic(User.Identity.Name);
            return PartialView("_LP", MyImg);
        }

        public ActionResult AlbumsNames()
        {
            List<ABname> abnames = AlbumManager.getAlbumNames(User.Identity.Name);
            return PartialView("_AlbumsNames", abnames);
        }

        public ActionResult GetPicByAlbum(int AID)
        {

            if (Session["IDs"] != null)
            {
                List<int> IDs = (List<int>)Session["IDs"];
                List<pic> MyImg = IMGPRO.AlbumPic(User.Identity.Name, AID);

                foreach (int i in IDs)
                {
                    pic thispic = MyImg.Where(x => x.Id == i).FirstOrDefault();
                    MyImg.Remove(thispic);
                }

                return PartialView("_LP", MyImg);

            }

            else
            {
                List<pic> MyImg = IMGPRO.AlbumPic(User.Identity.Name, AID);
                return PartialView("_LP", MyImg);
            }
        }

        public ActionResult AddPicToOrder(int Id, string thumb, int Ty, string PicName)
        {
            try
            {
                OrderDetails x = new OrderDetails();
                x.PicName = PicName;
                x.PiD = Id;
                x.thumb = thumb;
                switch (Ty)
                {
                    case 1:
                        x.OrderedFor = "Passport";
                        break;

                    case 2:
                        x.OrderedFor = "Postal";
                        break;

                    case 31:
                        x.OrderedFor = "Enlargement - 30*40";
                        break;

                    case 32:
                        x.OrderedFor = "Enlargement - 40*50";
                        break;

                    case 33:
                        x.OrderedFor = "Enlargement - 50*60";
                        break;

                    case 34:
                        x.OrderedFor = "Enlargement - 50*70";
                        break;

                    case 35:
                        x.OrderedFor = "Enlargement - 60*90";
                        break;

                    case 36:
                        x.OrderedFor = "Enlargement - 70*100";
                        break;

                    case 37:
                        x.OrderedFor = "Enlargement - 100*150";
                        break;

                    case 4:
                        x.OrderedFor = "Mini Book";
                        break;

                    case 5:
                        x.OrderedFor = "Large Book";
                        break;
                }

                List<OrderDetails> xlist = (List<OrderDetails>)Session["xlist"];
                if (xlist == null)
                {
                    xlist = new List<OrderDetails>();
                }

                if (Ty == 4)
                {
                    if (xlist.Count() >= 25)
                    {
                        return Content("you have selected " + xlist.Count + " out of 25  ");
                    }
                    else
                    {
                        xlist.Add(x);
                        Session["xlist"] = xlist;
                        return Content("you have selected " + xlist.Count + " out of 25 ");
                    }
                }

                else if (Ty == 5)
                {
                    if (xlist.Count() >= 35)
                    {
                        return Content("you have selected " + xlist.Count + " out of 35  ");
                    }
                    else
                    {
                        xlist.Add(x);
                        Session["xlist"] = xlist;
                        return Content("you have selected " + xlist.Count + " out of 35 ");
                    }
                }
                else
                {

                    xlist.Add(x);
                    Session["xlist"] = xlist;
                    return Content("you have selected " + xlist.Count + " pictures ");
                }
            }
            catch (Exception e)
            {
                return Content("Somthing went wrong , try again later");
            }
        }

        public ActionResult CH()
        {
            List<OrderDetails> xlist = (List<OrderDetails>)Session["xlist"];
            return View(xlist);
        }

        public ActionResult CHD()
        {
            orders z = new orders();
            z.OrderDate = DateTime.Now;
            z.Username = User.Identity.Name;
            z.PickUpdate = DateTime.Now.AddDays(10);
            z.Status = "New Order";
            db.Orders.Add(z);
            db.SaveChanges();

            List<OrderDetails> xlist = (List<OrderDetails>)Session["xlist"];
            foreach (var x in xlist)
            {
                x.OrderID = z.Id;
            }
            db.OrderDetails.AddRange(xlist);
            db.SaveChangesAsync();

            Session.Clear();
            ViewBag.ID = z.Id;
            return View();

        }

        public ActionResult XLatestPic()
        {
            int x = (int)Session["xno"];
            ViewBag.x = x;
            List<pic> MyImg = IMGPRO.XTopPic(User.Identity.Name);
            return PartialView("_XLP", MyImg);
        }

        public ActionResult MiniBook(int x)
        {
            ViewBag.x = x;
            Session["xno"] = x;
            Session["xlist"] = null;
            return View();
        }



        public ActionResult EditOrder()
        {
            List<OrderVM> Xlist = (List<OrderVM>)Session["Xlist"];
            return View(Xlist);
        }


        public ActionResult RemovePicToOrder(int Id, string thumb)
        {

            int Nop = (int)Session["Nop"];
            List<OrderVM> Xlist = (List<OrderVM>)Session["Xlist"];
            List<string> Thumbs = (List<string>)Session["thumbs"];
            List<int> IDs = (List<int>)Session["IDs"];

            OrderVM x = new OrderVM();
            x.Id = Id;
            x.thumb = thumb;

            Xlist.Remove(x);
            Thumbs.Remove(thumb);
            IDs.Remove(Id);
            Session["IDs"] = IDs;
            Session["thumbs"] = Thumbs;
            Session["Xlist"] = Xlist;
            if (IDs.Count() == 0)
            {
                return Content("your order is empty , you have removed all pictures in this order ");
            }
            ViewBag.AddNop = IDs.Count();
            return Content(IDs.Count + "/" + Nop);
        }


        public ActionResult MyOrders()
        {
            List<orders> MOrders = db.Orders.Where(x => x.Username == User.Identity.Name).ToList();
            return PartialView("_MyOrders", MOrders);
        }


        #endregion


    }
}