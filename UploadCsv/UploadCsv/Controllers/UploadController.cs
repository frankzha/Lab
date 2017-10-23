using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using UploadCsv.DAL;
using UploadCsv.Models;

namespace UploadCsv.Controllers
{
    public class UploadController : Controller
    {
        CsvContext db = new CsvContext();

        [HttpPost]
        public ActionResult DownloadFile(IList<int> fileIds)
        {
            if (fileIds != null)
            {
                string contentType="";
                string fileName="";

                IList<CsvFile> csvFiles = db.CsvFiles.Where(f => fileIds.Contains(f.Id)).ToList();
                IList<CsvRecord> csvRecords = db.CsvRecords.Where(r => fileIds.Contains(r.Id)).ToList();
                if (csvFiles != null && csvFiles.Count > 0)
                {
                    Stream stream = CsvFileDal.ConstructCsvFiles(csvFiles, ref contentType, ref fileName);
                    return File(stream, contentType, fileName);
                }
            }

            return RedirectToAction("UploadFile");
        }

        [HttpGet]
        public ActionResult UploadFile()
        {
            return View(db.CsvFiles);
        }

        [HttpPost]
        public ActionResult UploadFile(IEnumerable<HttpPostedFileBase> files)
        {
            StringBuilder status = new StringBuilder();

            if (files != null && files.Count() > 0)
            {
                foreach (HttpPostedFileBase file in files)
                {
                    if (file == null) continue;

                    try
                    {
                        CsvFile csvFile = CsvFileDal.AddCsvFile(file, db);

                        if (ModelState.IsValid)
                        {
                            db.SaveChanges();
                        }

                        status.AppendFormat("File [{0}] is uploaded successfully! ", file.FileName);

                        IEnumerable<string> result = DirectedGraphUtil.ShortestPath(csvFile.CsvRecords.ToList());
                        if (result != null)
                        {
                            status.AppendFormat(" The shortest path in the grapth is {0}", string.Join("->", result));
                        }
                    }
                    catch (Exception e)
                    {
                        status.AppendFormat("File [{0}] failed to upload due to {1} ", file.FileName, e.Message);
                    }
                }
            }
            else
            {
                status.Append("File upload failed: Error: no file is chosen to upload.");
            }

            ViewBag.Message = status.ToString();
            return View(db.CsvFiles);
        }
    }
}