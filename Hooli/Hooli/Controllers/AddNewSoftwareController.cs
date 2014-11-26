using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using MySql.Data.MySqlClient;
using Hooli.Models;
using Hooli.MySql;

namespace Hooli.Controllers
{
    public class AddNewSoftwareController : Controller
    {
        //
        // GET: /AddNewSoftware/

        //[Authorize(Roles="Admin")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Save(FormCollection formCollection, SoftwareModel model)
        {
            if(Request != null)
            {
                HttpPostedFileBase file = Request.Files["Uploaded File"];

                //Uses User.Identity.Name to find who's logged in-- look up in database
                System.Diagnostics.Debug.WriteLine(User.Identity.Name);

                if((file!=null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;

                    BinaryReader br = new BinaryReader(file.InputStream);
                    Byte[] fileBytes = br.ReadBytes(file.ContentLength);
                    br.Close();                    

                    //Construct query 
                    int admin_id = 1;
                    string softwareName = model.softwareName;
                    string version = model.version;
                    string date = DateTime.Now.ToString("yyyy-MM-dd");
                    string description = model.description;

                    string query = "insert into Software(admin_id, softwareName, fileName, version, date_added, description, data, contentType) values ("
                        + admin_id + ", \"" + softwareName + "\", \"" + fileName + "\", \"" + version + "\", \"" + date + "\", \"" + description + 
                        "\", @data, \"" + fileContentType + "\");";

                    //Save data to db
                    DBConnect db = new DBConnect();
                    using (var cmd = new MySqlCommand(query, db.GetConnection()))
                    {
                        db.GetConnection().Open();
                        cmd.Parameters.Add("@data", MySqlDbType.Blob).Value = fileBytes;
                        cmd.ExecuteNonQuery();
                        db.GetConnection().Close();
                    }
                }
            }
            return View("Index");
        }

    }
}
