using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hooli.Models;
using Hooli.MySql;

namespace Hooli.Controllers
{
    public class AddNewSoftwareController : Controller
    {
        //
        // GET: /AddNewSoftware/

        [Authorize(Roles="Admin")]
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
                    byte[] fileBytes = new byte[file.ContentLength];
                    file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                    //Way to convert byte array to string
                    System.Diagnostics.Debug.WriteLine(System.Text.Encoding.Default.GetString(fileBytes));
                   
                    
                    //Construct query 
                    int id = Convert.ToInt32(model.id);
                    int admin_id = 1;
                    string softwareName = model.softwareName;
                    string version = model.version;
                    string date = DateTime.Now.ToString("yyyy-MM-dd");
                    string description = model.description;
                    string data = System.Text.Encoding.Default.GetString(fileBytes);
                    string query = "insert into Software values (" + id + ", " + admin_id 
                        + ", \"" + softwareName + "\", \"" + version + "\", \"" + date + "\", \"" + description 
                        + "\", \"" + data + "\");";
                    //Save data to db
                    DBConnect db = new DBConnect();
                    db.Insert(query);
                }
            }
            return View("Index");
        }

    }
}
