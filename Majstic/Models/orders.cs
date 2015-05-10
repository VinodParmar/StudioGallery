using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Majstic.Models
{
    public class orders
    {
        [Key]
        public int Id { get; set; }



        public string Username { get; set; }

        public string Status { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime PickUpdate { get; set; }


    }
}