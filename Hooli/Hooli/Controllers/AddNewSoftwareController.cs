using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hooli.Controllers
{
    public class AddNewSoftwareController : Controller
    {
        //
        // GET: /AddNewSoftware/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Save(FormCollection formCollection)
        {
            if(Request != null)
            {
                HttpPostedFileBase file = Request.Files["UploadedFile"];

                if((file!=null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                    //code for saving data to filesystem
                }
            }
            return View("Index");
        }

    }
}
