using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Hooli.Models
{
    public class SoftwareModel
    {
        public int id { get; set; }
        public int admin_id { get; set; }
        [Display(Name="Software name")]
        public string softwareName { get; set; }
        [Display(Name = "Version number")]
        public string fileName { get; set; }
        public string version { get; set; }
        public DateTime date_added { get; set; }
        [Display(Name = "Description")]
        public string description { get; set; }
        public string data { get; set; }
        public string contentType { get; set; }
        //public int downloads { get; set; }
    }
}