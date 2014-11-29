using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hooli.MySql;

namespace Hooli.Controllers
{
    public class ProfileController : Controller
    {
        //
        // GET: /Profile/

        public ActionResult Index()
        {
            DBConnect connect = new DBConnect();
            //connect.Insert("Insert into Ratings values(1,1,1,3,3,3,3,8,11/16/14)");
            return View();
        }

    }
}
