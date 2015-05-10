using Majstic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Majstic.Support
{
    public class userdetails
    {
        private DB db = new DB();



        internal int userPoints(string username)
        {
            try
            {
                point userpoint = db.Points.Where(x => x.username == username).FirstOrDefault();
                if (userpoint == null)
                {
                    point newpoints = new point();
                    newpoints.username = username;
                    newpoints.points = 0;
                    newpoints.UpdatedDate = DateTime.Now;
                    db.Points.Add(newpoints);
                    db.SaveChanges();
                    return 0;

                }
                int userpoints = userpoint.points;
                return userpoints;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        internal List<string> AlbumPreview(string albumName)
        {
            album thisAlbum = db.Albums.Single(w => w.AlbumName == albumName);
            List<string> imgs = db.Images.Where(d => d.AlbumId == thisAlbum.Id).Select(f => f.themb).Take(4).ToList();
            return imgs;
        }


        internal void RemoveUserData(string UserName)
        {
            List<album> albums = db.Albums.Where(z=> z.username == UserName).ToList();
            List<Img> imgs = db.Images.Where(x => x.UserName == UserName).ToList();
            db.Images.RemoveRange(imgs);
            db.Albums.RemoveRange(albums);
            db.SaveChangesAsync();
        }
    }
}