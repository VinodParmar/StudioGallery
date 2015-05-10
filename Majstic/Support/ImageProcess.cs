using Majstic.Models;
using Majstic.Models.VM;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Majstic.Support
{
    public class ImageProcess
    {
        private DB db = new DB();

        // image scaling ,,,
        internal Image ScaleImageandSaveThumb(Image image, int maxImageHeight)
        {
            var ratio = (double)maxImageHeight / image.Height;
            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);
            var newImage = new Bitmap(newWidth, newHeight);
            using (var g = Graphics.FromImage(newImage))
            {
                g.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return newImage;

        }



        internal List<pic> TimeLine(string UserName)
        {
            List<pic> MyPic = db.Images.Where(s => s.UserName == UserName & s.Share == true).Select(
                    m => new pic
                    {
                        Id = m.Id,
                        ImgAlt = m.ImgAlt,
                        imgName = m.imgName,
                        themb = m.themb,
                        Share = m.Share,
                        UserName = m.UserName
                    }
                ).ToList();
            return MyPic;
        }



        internal List<pic> TopPic(string username)
        {
            List<pic> MyPic = db.Images.Where(s => s.UserName == username).Take(20).Select(
                   m => new pic
                   {
                       Id = m.Id,
                       ImgAlt = m.ImgAlt,
                       imgName = m.imgName,
                       themb = m.themb,
                       Share = m.Share,
                       UserName = m.UserName
                   }
               ).ToList();
            return MyPic;
        }

        internal List<pic> XTopPic(string username)
        {
            List<pic> MyPic = db.Images.Where(s => s.UserName == username).Select(
                   m => new pic
                   {
                       Id = m.Id,
                       ImgAlt = m.ImgAlt,
                       imgName = m.imgName,
                       themb = m.themb,
                       Share = m.Share,
                       UserName = m.UserName
                   }
               ).ToList();
            return MyPic;
        }


        internal List<pic> AlbumPic(string username , int ABID)
        {
            List<pic> MyPic = db.Images.Where(s => s.UserName == username & s.AlbumId == ABID).Take(20).Select(
                   m => new pic
                   {
                       Id = m.Id,
                       ImgAlt = m.ImgAlt,
                       imgName = m.imgName,
                       themb = m.themb,
                       Share = m.Share,
                       UserName = m.UserName
                   }
               ).ToList();
            return MyPic;
        }

    }
}