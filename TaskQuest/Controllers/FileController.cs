using System;
using System.IO;
using System.Web.Mvc;

namespace TaskQuest.Controllers
{
    public class FileController : Controller
    {
        [HttpPost]
        public virtual ActionResult UploadFile()
        {
            var myFile = Request.Files["File"];
            var isUploaded = false;
            var message = "File upload failed";

            if (myFile != null && myFile.ContentLength != 0)
            {
                var pathForSaving = Server.MapPath("~/Uploads");
                if (CreateFolderIfNeeded(pathForSaving))
                    try
                    {
                        myFile.SaveAs(Path.Combine(pathForSaving, myFile.FileName));
                        isUploaded = true;
                        message = "File uploaded successfully!";
                        //Colocar a informação no nome e caminho do arquivo no banco
                    }
                    catch (Exception ex)
                    {
                        message = string.Format("File upload failed: {0}", ex.Message);
                    }
            }
            return Json(new {isUploaded, message}, "text/html");
        }


        private bool CreateFolderIfNeeded(string path)
        {
            var result = true;
            if (!Directory.Exists(path))
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    /*TODO: You must process this exception.*/
                    result = false;
                }
            return result;
        }
    }
}