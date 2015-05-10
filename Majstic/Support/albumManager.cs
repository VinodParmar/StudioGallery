using Majstic.Models;
using Majstic.Models.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Majstic.Support
{
    public class albumManager
    {
        private DB db = new DB();
        internal List<albums> getAlbumsForAdmin(string username)
        {
            List<albums> xalbums = db.Albums.Where(x => x.username == username).Select(
                m => new albums
                {
                    albmName = m.AlbumName,
                    Cdate = m.CreateDate,
                    id = m.Id,
                    status = m.status,
                    NoP = db.Images.Where(y => y.AlbumId == m.Id).Count(),
                    thumnImg = db.Images.Where(s => s.AlbumId == m.Id).FirstOrDefault().themb
                }
                ).ToList();

            return xalbums;
        }




        internal List<albums> getAlbumsForUSers(string username)
        {
            List<albums> xalbums = db.Albums.Where(x => x.username == username).Select(
                m => new albums
                {
                    albmName = m.AlbumName,
                    Cdate = m.CreateDate,
                    id = m.Id,
                    status = m.Share,
                    NoP = db.Images.Where(y => y.AlbumId == m.Id).Count(),
                    thumnImg = db.Images.Where(s => s.AlbumId == m.Id).FirstOrDefault().themb
                }
                ).ToList();

            return xalbums;
        }


        internal List<pic> getAlbumPic(int AlbumID)
        {
            List<pic> albumPic = db.Images.Where(x => x.AlbumId == AlbumID).Select(
                m => new pic
                {
                    Id = m.Id,
                    ImgAlt = m.ImgAlt,
                    imgName = m.imgName,
                    Share = m.Share,
                    themb = m.themb,
                    UserName = m.UserName
                }

            ).ToList();
            return albumPic;
        }


        internal List<ABname> getAlbumNames(string username)
        {
            List<ABname> ABnames = db.Albums.Where(x => x.username == username).Select(

                    m => new ABname
                    {
                        ABI = m.Id,
                        ABNAme = m.AlbumName
                    }
                ).ToList();
            return ABnames;
        }
    }
}