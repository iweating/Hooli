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
    public class ActiveSoftwareController : Controller
    {
        //
        // GET: /ActiveSoftware/

        public ActionResult Index()
        {
            return View();
        }
        public FileResult Download()
        {
            DBConnect db = new DBConnect();
            string softwareId = (string)RouteData.Values["id"];
            string query = "select * from Software where id = " + softwareId + ";";
            DataTable dt = db.GetData(query);
            Byte[] bytes = (Byte[])dt.Rows[0]["data"];
            string contentType = (string)dt.Rows[0]["contentType"];
            string fileName = (string)dt.Rows[0]["fileName"];
            UpdateDownloadCount(softwareId, (int)dt.Rows[0]["downloads"]);
            return File(bytes, contentType, fileName);
        }

        public void UpdateDownloadCount(string id, int prevDownloads)
        {
            string query = "Update Software set downloads = @newDownloads";
            DBConnect db = new DBConnect();
            using (var cmd = new MySqlCommand(query, db.GetConnection()))
            {
                db.GetConnection().Open();
                cmd.Parameters.Add("@newDownloads", MySqlDbType.Int16).Value = prevDownloads + 1;
                cmd.ExecuteNonQuery();
                db.GetConnection().Close();
            }
        }
        //Need to have alert saying "Are you sure??" 
        public ActionResult Delete()
        {
            string query = "Delete from Software where id = @softwareId";
            DBConnect db = new DBConnect();
            using (var cmd = new MySqlCommand(query, db.GetConnection()))
            {
                db.GetConnection().Open();
                cmd.Parameters.Add("@softwareId", MySqlDbType.String).Value = (string)RouteData.Values["id"];
                cmd.ExecuteNonQuery();
                db.GetConnection().Close();
            }
            return View("Index");
        }

    }
}
