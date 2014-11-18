using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Hooli.Models
{
    public class Software
    {
        public int id { get; set; }
        public int admin_id { get; set; }
        public string name { get; set; }
        public string version { get; set; }
        public DateTime date_added { get; set; }
        [Display(Name = "Description")]
        public string description { get; set; }
        public string download_link { get; set; }
        //make static?
        public int downloads { get; set; }
    }
}