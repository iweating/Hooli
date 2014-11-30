using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hooli.MySql;
using MySql.Data.MySqlClient;
using Hooli.Models;
using System.Data;

namespace Hooli.Controllers
{
    public class ProfileController : Controller
    {
        //
        // GET: /Profile/

        public ActionResult Index()
        {
            DBConnect db = new DBConnect();
            string download_query = "SELECT * FROM Download_history";
            string profile_query = "SELECT * FROM User";

            MySqlCommand download_cmd = new MySqlCommand(download_query);
            MySqlCommand profile_cmd = new MySqlCommand(profile_query);

            var profile_model = FillProfileModel(profile_cmd);
            var download_model = FillDownloadHistoryModel(download_cmd);

            ProfileDownloadViewModel model = new ProfileDownloadViewModel();
            model.profile = profile_model;

            return View(profile_model);
        }

        public FileResult Download()
        {
            DBConnect db = new DBConnect();
            string user_id = (string)RouteData.Values["user_id"];
            string query = "select * from User where user_id = " + user_id + ";";
            MySqlCommand cmd = new MySqlCommand(query);
            DataTable dt = db.GetData(cmd);
            Byte[] bytes = (Byte[])dt.Rows[0]["data"];
            string contentType = (string)dt.Rows[0]["contentType"];
            string fileName = (string)dt.Rows[0]["fileName"];
            UpdateDownloadCount(user_id, (int)dt.Rows[0]["downloads"]);
            return File(bytes, contentType, fileName);
        }

        public ActionResult SearchHistory(MySqlCommand cmd)
        {
            DBConnect db = new DBConnect();
            string user_id = (string)RouteData.Values["user_id"];
            string query = "select user_id from User where user_id = @user_id";
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

        private IEnumerable<ProfileModel> FillProfileModel(MySqlCommand cmd)
        {
            DBConnect db = new DBConnect();
            ProfileModel profile = new ProfileModel();
            if (db.GetData(cmd) != null)
            {
                foreach (DataRow row in db.GetData(cmd).Rows)
                {
                    profile.Add(new ProfileModel()
                    {
                        user_id = (int)row["user_id"],
                        FirstName = (string)row["FirstName"],
                        LastName = (string)row["LastName"]
                    });
                }
            }
            IEnumerable<ProfileModel> model = profile;
            return model;
        }

        private IEnumerable<DownloadHistoryModel> FillDownloadHistoryModel(MySqlCommand cmd)
        {
            DBConnect db = new DBConnect();
            List<DownloadHistoryModel> download = new List<DownloadHistoryModel>();
            if (db.GetData(cmd) != null)
            {
                foreach (DataRow row in db.GetData(cmd).Rows)
                {
                    download.Add(new DownloadHistoryModel()
                    {
                        user_id = (int)row["user_id"],
                        softwareName = (string)row["softwareName"],
                        version = (string)row["version"],
                        id = (int)row["id"],
                        download_data = (DateTime)row["download_data"],
                        dl_version = (string)row["dl_version"]
                    });
                }
            }
            IEnumerable<DownloadHistoryModel> model = download;
            return model;
        }

        private ProfileModel FillProfileModel(MySqlCommand cmd)
        {
            DBConnect db = new DBConnect();
            ProfileModel profile = new ProfileModel();
            if (db.GetData(cmd) != null)
            {
                                
foreach (DataRow row in db.GetData(cmd).Rows)
                {
                    profile.Add(new ProfileModel()
                    {
                        user_id = (int)row["user_id"],
                        FirstName = (string)row["FirstName"],
                        LastName = (string)row["LastName"]
                    });
                }
            }
            ProfileModel model = profile;
            return model;
        }

    }
}
