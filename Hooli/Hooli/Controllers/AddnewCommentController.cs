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
    
        public ActionResult Submit(FormCollection formCollection)
        {
            int software_id_dummy_value = 7;

            String text = formCollection.Get("comment_text"), query;
            MySqlCommand cmd = new MySqlCommand();
            query = "insert into comments (comment, Software_ID, Date) values (@text," + software_id_dummy_value + ", @date)";
            cmd.CommandText = query;
            cmd.Parameters.Add("@text", MySqlDbType.String).Value = text;
            cmd.Parameters.AddWithValue("@date", DateTime.Now);
            DBConnect db = new DBConnect();
            db.Insert(cmd);

            // Average new rating with current software rating
            String current_software_rating_query = "SELECT rating FROM Software where id=" + software_id_dummy_value;
            MySqlCommand cmd = new MySqlCommand(current_software_rating_query);
            db.GetData(cmd);


            return View("Index");
        }
    }
    
}
