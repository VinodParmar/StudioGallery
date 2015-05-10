using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Majstic.Models
{
    public class offer
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        public string Dis { get; set; }

        [Display(Name = "Number of Shots")]
        public int NoShots { get; set; }

        [Display(Name = "Number of copies")]
        public int NoCopy { get; set; }

        public decimal Price { get; set; }

        public int points { get; set; }

        public string thumb { get; set; }

        [Display(Name = "Available ")]
        public bool available { get; set; }

    }
}