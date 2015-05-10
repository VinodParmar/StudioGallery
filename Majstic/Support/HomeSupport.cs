using System;
using System.Collections.Generic;
using System.Linq;
using Majstic.Models;
using System.Web;
using Majstic.Models.VM;

namespace Majstic.Support
{
    public class HomeSupport
    {
        private DB db = new DB();

        internal List<TimeLinePic> TimeLineUSer(string USer)
        {
            try
            {
                List<TimeLinePic> thosePic = db.Images.Where(x => x.Share == true && x.UserName == USer).Take(10).Select(
                    m => new TimeLinePic
                    {

                        Id = m.Id,
                        Thumb = m.themb,
                        UserName = m.UserName,
                        PicThumb = db.ProPics.Where(d => d.username == m.UserName).FirstOrDefault().proimg
                    }
                    ).ToList();

                return thosePic;
            }
            catch (Exception e)
            {

            }
            return null;
        }




        internal List<TimeLinePic> timelinepic(int no)
        {
            List<TimeLinePic> pics = db.Images.Where(x => x.Share == true).Take(no).Select(
                    d => new TimeLinePic
                        {
                            Id = d.Id,
                            Thumb = d.themb,
                            UserName = d.UserName,
                            PicThumb = db.ProPics.Where(f => f.username == d.UserName).FirstOrDefault().proimg
                        }
                ).ToList();
            return pics;
        }


        internal TimeLinePic Pic(int Id)
        {
            Img pics = db.Images.Find(Id);
            TimeLinePic thispic = new TimeLinePic();
            thispic.UserName = pics.UserName;
            thispic.Thumb = pics.themb;
            thispic.Id = pics.Id;
            thispic.PicThumb = db.ProPics.Where(x => x.username == pics.UserName).FirstOrDefault().proimg;

            return thispic;
        }
    }
}