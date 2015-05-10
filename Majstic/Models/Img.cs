using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Majstic.Models
{
    public class Img
    {
        [Key]
        public int Id { get; set; }

        public int AlbumId { get; set; }


        public string imgName { get; set; }



        public string ImgAlt { get; set; }

        public byte[] ImageData { get; set; }

        public string ContentType { get; set; }

        public string UserName { get; set; }

        public string themb { get; set; }

        public bool Share { get; set; }

        public bool PShare { get; set; }

        public DateTime PShareEnd { get; set; }

    }
}