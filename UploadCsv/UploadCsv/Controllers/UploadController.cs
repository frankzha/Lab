using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using UploadCsv.Models;

namespace UploadCsv.Controllers
{
    public class UploadController : Controller
    {
        // GET: Upload
        public ActionResult Index()
        {
            CsvFileDal dal = new CsvFileDal();
            return View(dal.GetFiles());
        }

        [HttpPost]
        public FileResult DownloadFile(int? fileId)
        {
            CsvFileDal dal = new CsvFileDal();
            CsvFile csvFile = dal.DownloadCsvFile(fileId);
            return File(csvFile.FileContent, csvFile.ContentType, csvFile.FileName);
        }

        [HttpGet]
        public ActionResult UploadFile()
        {
            CsvFileDal dal = new CsvFileDal();
            return View(dal.GetFiles());
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            try
            {
                IList<string> lines = new List<string>();
                string content = "";
                string result = "And it doesn't contain cycle. ";
                CsvFileDal dal = new CsvFileDal();

                if (file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string contentType = file.ContentType;
                    byte[] bytes = new byte[file.ContentLength];
                    file.InputStream.Read(bytes, 0, file.ContentLength);

                    content = Encoding.UTF8.GetString(bytes);

                    CsvFile csvFile = new CsvFile(file.FileName, contentType, bytes);
                    dal.AddCsvFile(csvFile);

                    bool b = DirectedGraphUtil.DirectedGraphHasCycle(content);
                    if (b)
                    {
                        result = "But it contains cycle. ";
                    }                    
                }

                ViewBag.Message = "File Uploaded Successfully!! " + result;
                return View(dal.GetFiles());
            }
            catch (Exception e)
            {
                ViewBag.Message = "File upload failed: " + e.ToString();
                return View();
            }
        }
    }
}