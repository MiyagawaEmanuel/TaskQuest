using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskQuest.Models;
using TaskQuest.ViewModels;
using Microsoft.AspNet.Identity;
using TaskQuest;
using System.Diagnostics;
using TaskQuest.Data;

namespace TesteMVC.Controllers
{
    [Authorize]
    public class FileController : Controller
    {

        private DbContext db = new DbContext();

        private string[] _DocTypes
        {
            get
            {
                return new string[] { "rar", "zip", "doc", "docx", "pdf", "xlsx", "xls", "ppt", "pptx" };
            }
        }

        private string[] _ImageTypes
        {
            get
            {
                return new string[] { "jpg", "jpeg", "png", "gif" };
            }
        }

        [HttpPost]
        public ActionResult Download(string Id)
        {
            int fileId;
            if (int.TryParse(Util.Decrypt(Id), out fileId))
            {
                var file = db.File.Find(fileId);
                return File(System.IO.File.ReadAllBytes(Path.Combine(Server.MapPath("~/Images/"), file.Url)), System.Net.Mime.MediaTypeNames.Application.Octet, file.Nome);
            }

            TempData["Alerta"] = "Algo deu errado";
            TempData["Classe"] = "yellow-alert";
            return RedirectToAction("Inicio", "Home");
        }

        [HttpPost]
        public JsonResult Upload()
        {
            var files = Request.Files;
            return Json(Upload(files).Select(e => new FileViewModel(e)));
        }

        [HttpPost]
        public JsonResult UploadIntegrante()
        {
            var id = Request["Id"];
            int Id;
            if (int.TryParse(Util.Decrypt(id), out Id))
            {

                var task = db.Task.Find(Id);
                if (User.Identity.HasQuest(task.QuestId))
                {
                    var Requestedfiles = Request.Files;
                    var files = Upload(Requestedfiles);

                    foreach (var file in files)
                    {
                        file.TaskId = Id;
                        file.IsValid = true;
                        db.Entry(file).State = System.Data.Entity.EntityState.Modified;
                    }
                    db.SaveChanges();

                    return Json(files.Select(e => new FileViewModel(e)));
                }
            }
            return Json(null);
        }

        [HttpPost]
        private List<TaskQuest.Models.File> Upload(HttpFileCollectionBase postedFiles)
        {
            try
            {

                List<TaskQuest.Models.File> files = new List<TaskQuest.Models.File>();

                for (int i = 0; i < postedFiles.Count; i++)
                {
                    HttpPostedFileBase file = Request.Files[i];

                    if (!_DocTypes.Any(e => file.FileName.EndsWith(e)) && !_ImageTypes.Any(e => file.FileName.EndsWith(e)))
                        files.Add(new TaskQuest.Models.File { Response = "Error", Nome = file.FileName });
                    else
                    {
                        //Cria um nome único para o arquivo
                        var fileName = Util.CreateRandomString();

                        //Cria o caminho onde o arquivo será salvo
                        var path = Path.Combine(Server.MapPath("~/Images/"), fileName + "." + file.FileName.Split('.').Last());

                        //Salva o arquivo
                        file.SaveAs(path);

                        int userId = User.Identity.GetUserId<int>();

                        if (_ImageTypes.Any(e => file.FileName.EndsWith(e)))
                            files.Add(new TaskQuest.Models.File()
                            {
                                UserId = userId,
                                Response = "Image",
                                Nome = file.FileName,
                                Size = Convert.ToDouble(file.ContentLength) / 1000000.0,
                                Url = CreateThumbnail(path, fileName) + "." + file.FileName.Split('.').Last()
                            });
                        else
                            files.Add(new TaskQuest.Models.File
                            {
                                UserId = userId,
                                Response = "File",
                                Nome = file.FileName,
                                Url = fileName + "." + file.FileName.Split('.').Last(),
                                Size = Convert.ToDouble(file.ContentLength) / 1000000.0,
                            });
                    }
                }

                foreach (var file in files)
                    db.File.Add(file);
                db.SaveChanges();

                return files;
            }
            catch (System.Web.HttpException)
            {
                return null;
            }
        }

        [HttpPost]
        public string Delete(string Id)
        {
            int fileId;
            if (int.TryParse(Util.Decrypt(Id), out fileId))
            {
                var file = db.File.Find(fileId);
                file.IsValid = false;
                
                db.Entry(file).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return "Ok";
            }
            return "Error";
        }

        private string CreateThumbnail(string path, string fileName, int lnWidth = 80, int lnHeight = 80)
        {
            System.Drawing.Bitmap bmpOut = null;

            using (var loBMP = new Bitmap(path))
            {
                ImageFormat loFormat = loBMP.RawFormat;

                bmpOut = new Bitmap(lnWidth, lnHeight);
                Graphics g = Graphics.FromImage(bmpOut);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.FillRectangle(Brushes.White, 0, 0, lnWidth, lnHeight);
                g.DrawImage(loBMP, 0, 0, lnWidth, lnHeight);
            }

            string outputFileName = fileName;
            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(Path.Combine(Server.MapPath("~/Images/"), outputFileName + "-min.jpg"), FileMode.Create, FileAccess.ReadWrite))
                {
                    bmpOut.Save(memory, ImageFormat.Jpeg);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }

            return outputFileName;

        }

    }
}