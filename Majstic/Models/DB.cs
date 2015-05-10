using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Majstic.Models
{
    public class DB : DbContext
    {
        //public DB()
        //    : base("DConnectionX")
        //{
        //}

        public DbSet<Img> Images { get; set; }
        public DbSet<album> Albums { get; set; }
        public DbSet<point> Points { get; set; }
        public DbSet<offer> Offers { get; set; }
        public DbSet<propic> ProPics { get; set; }
        public DbSet<orders> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }

  


        //static DB()
        //{
        //    Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DB>());
        //}

       
    }
}