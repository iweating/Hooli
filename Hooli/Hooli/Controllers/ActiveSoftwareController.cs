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
            DBConnect db = new DBConnect();
            string query = "select * from Software";
            MySqlCommand cmd = new MySqlCommand(query);
            var model = FillSoftwareModel(cmd);
            return View(model);
        }

        public FileResult Download()
        {
            DBConnect db = new DBConnect();
            string softwareId = (string)RouteData.Values["id"];
            string query = "select * from Software where id = " + softwareId + ";";
            MySqlCommand cmd = new MySqlCommand(query);
            DataTable dt = db.GetData(cmd);
            Byte[] bytes = (Byte[])dt.Rows[0]["data"];
            string contentType = (string)dt.Rows[0]["contentType"];
            string fileName = (string)dt.Rows[0]["fileName"];
            UpdateDownloadCount(softwareId, (int)dt.Rows[0]["downloads"]);
            return File(bytes, contentType, fileName);
        }

        private void UpdateDownloadCount(string id, int prevDownloads)
        {
            string query = "Update Software set downloads = @newDownloads where id = @id";
            DBConnect db = new DBConnect();
            MySqlCommand cmd = new MySqlCommand(query);
            cmd.Parameters.Add("@id", MySqlDbType.String).Value = id;
            cmd.Parameters.Add("@newDownloads", MySqlDbType.Int16).Value = prevDownloads + 1;
            db.Update(cmd);
        }

        //Need to have alert saying "Are you sure??" 
        public ActionResult Delete()
        {
//Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Are you sure?');", true);

            string query = "Delete from Software where id = @softwareId";
            MySqlCommand cmd = new MySqlCommand(query);
            cmd.Parameters.Add("@softwareId", MySqlDbType.String).Value = (string)RouteData.Values["id"];

            DBConnect db = new DBConnect();
            db.Delete(cmd);

            query = "select * from Software";
            cmd.CommandText = query;
            var model = FillSoftwareModel(cmd);

            return View("Index", model);
        }

        public ActionResult Edit()
        {
            string query = "select * from Software where id = \"" + RouteData.Values["id"] + "\";";
            MySqlCommand cmd = new MySqlCommand(query);
            var model = FillSoftwareModel(cmd);
            if (model.Count() != 0) return View(model.ElementAt(0));
            else return View("Error");
        }

        public ActionResult Update(SoftwareModel model)
        {
            string query = "Update Software set softwareName=@softwareName, version=@version, date_added=@date, " +
                           "description=@description where id = @id;";
            MySqlCommand cmd = new MySqlCommand(query);
            cmd.Parameters.AddWithValue("@softwareName", model.softwareName);
            cmd.Parameters.AddWithValue("@version", model.version);
            cmd.Parameters.AddWithValue("@date", DateTime.Now);
            cmd.Parameters.AddWithValue("@description", model.description);
            cmd.Parameters.AddWithValue("@id", model.id);

            DBConnect db = new DBConnect();
            db.Update(cmd);

            query = "select * from Software";
            cmd.CommandText = query;
            var modelList = FillSoftwareModel(cmd);

            return View("Index", modelList);
        }

        public ActionResult Search(FormCollection formCollection)
        {
            String softwareName = formCollection.Get("Search_input"), query;
            MySqlCommand cmd = new MySqlCommand();
            var checkedButton = formCollection.Get("searchType");
            ViewBag.Search = true;
            if (checkedButton == "isExactly")
            {
                query = "select * from Software where softwareName = @softwareName";
                cmd.CommandText = query;
                cmd.Parameters.Add("@softwareName", MySqlDbType.String).Value = softwareName;
                var model = FillSoftwareModel(cmd);
                return View("Index", model);
                
            }
            else if (checkedButton == "contains")
            {
                List<SoftwareModel> software = new List<SoftwareModel>();
                query = "select * from Software";
                cmd.CommandText = query;
                var allSoftware = FillSoftwareModel(cmd);
                if (allSoftware.Count() != 0)
                {
                    foreach (SoftwareModel model in allSoftware)
                    {
                        if (model.softwareName.Contains(softwareName))
                        {
                            software.Add(model);
                        }
                    }
                    return View("Index", software);
                }
                else return View("Error");
            }
            else
            {
                return View("Error");
            }
        }

        private IEnumerable<SoftwareModel> FillSoftwareModel(MySqlCommand cmd)
        {
            DBConnect db = new DBConnect();
            List<SoftwareModel> software = new List<SoftwareModel>();
            if (db.GetData(cmd) != null)
            {
                foreach (DataRow row in db.GetData(cmd).Rows)
                {
                    software.Add(new SoftwareModel()
                    {
                        id = (int)row["id"],
                        admin_id = (int)row["admin_id"],
                        softwareName = (string)row["softwareName"],
                        version = (string)row["version"],
                        date_added = (DateTime)row["date_added"],
                        description = (string)row["description"],
                        downloads = (int)row["downloads"]
                    });
                }
            }
            IEnumerable<SoftwareModel> model = software;
            return model;
        }
    }
}
