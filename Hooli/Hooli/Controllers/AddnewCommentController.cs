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
    public class AddnewCommentController : Controller
    {
        //
        // GET: /AddComment/

        public ActionResult Index()
        {
            return View();
        }
    
        [Authorize]
        public ActionResult Submit(FormCollection formCollection)
        {
            String text = formCollection.Get("comment_text"), query;
            MySqlCommand cmd = new MySqlCommand();
            query = "insert into comments (comment, Software_ID, Date) values (@text, 7, @date)";
            cmd.CommandText = query;
            cmd.Parameters.Add("@text", MySqlDbType.String).Value = text;
            cmd.Parameters.AddWithValue("@date", DateTime.Now);
            DBConnect db = new DBConnect();
            db.Insert(cmd);
        return View("Index");
        }
    }
    
}
