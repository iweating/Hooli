using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using MySql.Data.MySqlClient;
using Hooli.Models;
using Hooli.MySql;

namespace Hooli.Controllers
{
    public class AddCommentController : Controller
    {
        //
        // GET: /AddComment/

        public ActionResult Index()
        {
            return View();
        }
    
        public ActionResult Submit(FormCollection formCollection)
        {
            String text = formCollection.Get("comment_text"), query;
            MySqlCommand cmd = new MySqlCommand();
            query = "insert into comments (comment, Software_ID) values (@text, )";
            cmd.CommandText = query;
            cmd.Parameters.Add("@text", MySqlDbType.String).Value = text;
        return View("Index");
        }
    }
    
}
