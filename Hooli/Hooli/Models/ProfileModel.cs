using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Hooli.Models
{
    public class ProfileModel
    {
        public int user_id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}