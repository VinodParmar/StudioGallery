using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Majstic.Models;
using System.Drawing;
using Majstic.Support;

namespace Majstic.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OffersController : Controller
    {
        private DB db = new DB();
        ImageProcess IMGProcess = new ImageProcess();

        // GET: /Offers/
        public ActionResult Index()
        {
            return View(db.Offers.ToList());
        }

        // GET: /Offers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            offer offer = db.Offers.Find(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            return View(offer);
        }

        // GET: /Offers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Offers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Dis,NoShots,NoCopy,Price,points,thumb,available")] offer offer)
        {
            if (ModelState.IsValid)
            {
                db.Offers.Add(offer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(offer);
        }

        // GET: /Offers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            offer offer = db.Offers.Find(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            return View(offer);
        }

        // POST: /Offers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Dis,NoShots,NoCopy,Price,points,thumb,available")] offer offer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(offer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(offer);
        }

        // GET: /Offers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            offer offer = db.Offers.Find(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            return View(offer);
        }

        // POST: /Offers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            offer offer = db.Offers.Find(id);
            db.Offers.Remove(offer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadImg(int OfferId)
        {
            foreach (string file in Request.Files)
            {
                var postedFile = Request.Files[file];
                int fileLenght = postedFile.ContentLength;
                var logopath = postedFile.FileName;
                //if (fileLenght <= 1048576)
                //{
                    Bitmap bmpUploadedImage = new Bitmap(postedFile.InputStream);
                    Image objImage = IMGProcess.ScaleImageandSaveThumb(bmpUploadedImage, 500);
                    objImage.Save(Server.MapPath("~/thumb/offers/") + logopath);
                //}
                offer thisoffer = db.Offers.Find(OfferId);
                thisoffer.thumb = logopath;
                db.Entry(thisoffer).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Edit", "Offers", new { Id = OfferId });
        }





        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
