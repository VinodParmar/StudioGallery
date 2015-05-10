using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Majstic.Models.VM
{
    public class NewOrder
    {
        public int ID { get; set; }

        public string OfferName { get; set; }

        public string UserName { get; set; }

        public DateTime OrderDate { get; set; }

    }
}