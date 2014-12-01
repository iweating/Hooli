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
            int software_id_dummy_value = 17;
            int rating_dummy_value = 3;

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
            MySqlCommand current_software_rating_cmd = new MySqlCommand(current_software_rating_query);
            int current_rating = (int) db.GetData(current_software_rating_cmd).Rows[0][0];
            int new_rating = (int) (.75 * current_rating + .25 * rating_dummy_value);
            String new_software_rating_query = "INSERT INTO Software (rating) VALUES ('" + new_rating+ "');";
            MySqlCommand new_software_rating_cmd = new MySqlCommand(new_software_rating_query);
            db.Insert(new_software_rating_cmd);

            return View("Index");
        }
    }
    
}
