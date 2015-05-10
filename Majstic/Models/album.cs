using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Majstic.Models
{
    public class album
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string AlbumName { get; set; }

        public DateTime CreateDate { get; set; }

        public string username { get; set; }

        public bool status { get; set; }

        public bool Share { get; set; }

        public int NoOFPics { get; set; }
    }
}