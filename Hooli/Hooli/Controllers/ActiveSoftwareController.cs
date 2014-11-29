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
            string selectQueryString = "select * from Software";
            var model = FillSoftwareModel(selectQueryString);
            db.GetConnection().Close();
            return View(model);
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

        private void UpdateDownloadCount(string id, int prevDownloads)
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
//Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Are you sure?');", true);

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

        public ActionResult Edit()
        {
            string query = "select * from Software where id = \"" + RouteData.Values["id"] + "\";";
            var model = FillSoftwareModel(query);
            if (model.Count() != 0) return View(model.ElementAt(0));
            else return View("Error");
        }

        public ActionResult Search(FormCollection formCollection)
        {
            String softwareName = formCollection.Get("Search_input");
            var checkedButton = formCollection.Get("searchType");
            ViewBag.Search = true;
            if (checkedButton == "isExactly")
            {
                string query = "select * from Software where softwareName = \"" + softwareName + "\";";
                var model = FillSoftwareModel(query);
                if (model.Count() != 0) return View("Index", model.ElementAt(0));
                else return View("Error");
                
            }
            else if (checkedButton == "contains")
            {
                List<SoftwareModel> software = new List<SoftwareModel>();
                string selectAll = "select * from Software";
                var allSoftware = FillSoftwareModel(selectAll);
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

        private IEnumerable<SoftwareModel> FillSoftwareModel(String query)
        {
            DBConnect db = new DBConnect();
            List<SoftwareModel> software = new List<SoftwareModel>();
            if (db.GetData(query) != null)
            {
                foreach (DataRow row in db.GetData(query).Rows)
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
