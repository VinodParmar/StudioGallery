using Majstic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Majstic.Controllers
{
    public class ShowController : AsyncController
    {
        private DB db = new DB();

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ShowPhotoForAdmin(Int32 id)
        {
            //This is my method for getting the image information
            // including the image byte array from the image column in
            // a database.
            ViewBag.Imgno = id;
            Img image = db.Images.Find(id);
            //As you can see the use is stupid simple.  Just get the image bytes and the
            //  saved content type.  See this is where the contentType comes in real handy.
            ImageResult result = new ImageResult(image.ImageData, image.ContentType);
            return result;
        }



        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ShowPhotoForUser(Int32 id)
        {
            //This is my method for getting the image information
            // including the image byte array from the image column in
            // a database.
            ViewBag.Imgno = id;
            Img image = db.Images.Find(id);
            //As you can see the use is stupid simple.  Just get the image bytes and the
            //  saved content type.  See this is where the contentType comes in real handy.
            ImageResult result = new ImageResult(image.ImageData, image.ContentType);
            return result;
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ShowPhoto(Int32 id)
        {
            //This is my method for getting the image information
            // including the image byte array from the image column in
            // a database.
            ViewBag.Imgno = id;
            Img image = db.Images.Find(id);
            //As you can see the use is stupid simple.  Just get the image bytes and the
            //  saved content type.  See this is where the contentType comes in real handy.
            ImageResult result = new ImageResult(image.ImageData, image.ContentType);
            return result;
        }
	}
}