using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Majstic.Models.VM
{
    public class albums
    {
        public int id { get; set; }
        public string albmName { get; set; }
        public int NoP { get; set; }
        public DateTime Cdate { get; set; }
        public bool status { get; set; }
        public string thumnImg { get; set; }
    }
}