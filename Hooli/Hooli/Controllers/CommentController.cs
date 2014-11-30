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
    public class CommentController : Controller
    {
        //
        // GET: /Comment/

        public ActionResult Index()
        {
            DBConnect db = new DBConnect();
            string query = "select * from comments";
            MySqlCommand cmd = new MySqlCommand(query);
            var model = FillCommentModel(cmd);
            return View(model);
        }

       
        public ActionResult Open()
        {
            DBConnect db = new DBConnect();
            string softwareId = (string)RouteData.Values["id"];
            string query = "select * from Comments where softwareId = " + softwareId + ";";
            MySqlCommand cmd = new MySqlCommand(query);
            DataTable dt = db.GetData(cmd);
            var model = FillCommentModel(cmd);
        }
        private IEnumerable<CommentModel> FillCommentModel(MySqlCommand cmd)
        {
            DBConnect db = new DBConnect();
            List<CommentModel> comments = new List<CommentModel>();
            if(db.GetData(cmd) != null)
            {
                foreach(DataRow row in db.GetData(cmd).Rows)
                {
                    comments.Add(new CommentModel()
                    {
                        ID = (int)row["ID"],
                        User_ID = (int)row["User_ID"],
                        Software_ID = (int)row["Software_ID"],
                        Date = (DateTime)row["Date"],
                        Text = (string)row["Comment"]
                    });
                }
            }
            IEnumerable<CommentModel> model = comments;
            return model;
        }
    }
}
