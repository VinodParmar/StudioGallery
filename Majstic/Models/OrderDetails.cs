 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Majstic.Models
{
    public class OrderDetails
    {
        [Key]
        public int Id { get; set; }

        public int OrderID { get; set; }

        public int PiD { get; set; }

        public string PicName { get; set; }

        public string thumb { get; set; }

        public bool status { get; set; }

        public string OrderedFor { get; set; }
    }
}