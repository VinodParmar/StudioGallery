using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace Majstic.Support
{
    public class sendMail
    {
        internal void NewAccount(string Name, string Email, string Phone, string UserName, string Pass)
        {
            StreamReader reader = new StreamReader(HttpContext.Current.Request.MapPath("~/Support/html/joinus.html"));
            string htmlfile = reader.ReadToEnd();
            string htmlData = "";
            htmlData = htmlfile;
            htmlData = htmlData.Replace("@Name@", Name);
            htmlData = htmlData.Replace("@Enail@", Email);
            htmlData = htmlData.Replace("@Phon@", Phone);
            htmlData = htmlData.Replace("@UserName@", UserName);
            htmlData = htmlData.Replace("@Pass@", Pass);

            // raping up the mail send to the hotel 
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("no-reply@majesticphotostudio.com");
            msg.To.Add(Email);
            msg.Body = htmlData.ToString();
            msg.IsBodyHtml = true;
            msg.Subject = Name + "Your Majestic Studio accout is ready";
            NoReplySend(msg);
        }





        internal void AlbumAdded(string Name, string Email, string ABNAME, int Nop, string UserName)
        {
            StreamReader reader = new StreamReader(HttpContext.Current.Request.MapPath("~/Support/html/NewAlbum.html"));
            string htmlfile = reader.ReadToEnd();
            string htmlData = "";
            htmlData = htmlfile;
            htmlData = htmlData.Replace("@Name@", Name);
            htmlData = htmlData.Replace("@USerNAme@", UserName);
            htmlData = htmlData.Replace("@Nop@", Nop.ToString());
            htmlData = htmlData.Replace("@ABNAME@", ABNAME);
            htmlData = htmlData.Replace("@Date@", DateTime.Now.ToShortDateString());

            // raping up the mail send to the hotel 
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("no-reply@majesticphotostudio.com");
            msg.To.Add(Email);
            msg.Body = htmlData.ToString();
            msg.IsBodyHtml = true;
            msg.Subject = Name + "New Album has been added to your Majestic account.";
            NoReplySend(msg);
        }






        internal void ResetPAss( string Email, string pass , string username)
        {
            StreamReader reader = new StreamReader(HttpContext.Current.Request.MapPath("~/Support/html/Reset.html"));
            string htmlfile = reader.ReadToEnd();
            string htmlData = "";
            htmlData = htmlfile;
            htmlData = htmlData.Replace("@Uname@", username);
            htmlData = htmlData.Replace("@Pass@", pass);
         
            htmlData = htmlData.Replace("@Date@", DateTime.Now.ToShortDateString());

            // raping up the mail send to the hotel 
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("no-reply@majesticphotostudio.com");
            msg.To.Add(Email);
            msg.Body = htmlData.ToString();
            msg.IsBodyHtml = true;
            msg.Subject =  "Majestic Account Reseting";
            NoReplySend(msg);
        }













        internal void Msg(string xmsg, string Email, string Title)
        {
            StreamReader reader = new StreamReader(HttpContext.Current.Request.MapPath("~/Support/html/msg.html"));
            string htmlfile = reader.ReadToEnd();
            string htmlData = "";
            htmlData = htmlfile;
            htmlData = htmlData.Replace("@@MSG@@", xmsg);

            // raping up the mail send to the hotel 
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("no-reply@majesticphotostudio.com");
            msg.To.Add(Email);
            msg.Body = htmlData.ToString();
            msg.IsBodyHtml = true;
            msg.Subject = Title;
            NoReplySend(msg);
        }









        internal async Task NoReplySend(MailMessage msg)
        {
            try
            {
                SmtpClient smt = new SmtpClient("mail.majesticphotostudio.com");
                smt.Port = 25;
                smt.UseDefaultCredentials = false;

                smt.Credentials = new NetworkCredential("no-reply@majesticphotostudio.com", "dmwajtgp1966_");
                smt.EnableSsl = false;

                await Task.Run(() => smt.Send(msg));
            }
            catch (Exception e)
            {
                NoReplySend(msg);
            }
        }
    }
}