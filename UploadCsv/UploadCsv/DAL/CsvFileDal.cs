using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using UploadCsv.Models;

namespace UploadCsv.DAL
{
    public class CsvFileDal
    {
        public static CsvFile AddCsvFile(HttpPostedFileBase file, CsvContext db)
        {
            if (file.ContentLength > 0)
            {
                string fileName = Path.GetFileName(file.FileName);
                string contentType = file.ContentType;
                byte[] bytes = new byte[file.ContentLength];
                file.InputStream.Read(bytes, 0, file.ContentLength);

                CsvFile csvFile = new CsvFile(fileName, contentType);
                IList<CsvRecord> records = CsvRecordDal.RetrieveCsvRecords(bytes, csvFile);
                csvFile.CsvRecords = records;

                db.CsvFiles.Add(csvFile);
                db.CsvRecords.AddRange(records);
                return csvFile;
            }
            else
            {
                throw new Exception ("Error: the uploaded file is empty.");
            }
        }

        public static Stream ConstructCsvFiles(IList<CsvFile> csvFiles, ref string contentType, ref string fileName)
        {
            if (csvFiles.Count == 1)
            {
                contentType = csvFiles[0].ContentType;
                fileName = csvFiles[0].FileName;
                return CsvRecordDal.ConstructCsvFile(csvFiles[0].CsvRecords);
            }

            contentType = "application/zip";
            fileName = "attachment.zip";

            MemoryStream zipToOpen = new MemoryStream();
            using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create, true))
            {
                foreach (CsvFile csvFile in csvFiles)
                {
                    string csvFileName = csvFile.Id + "-" + csvFile.FileName;
                    ZipArchiveEntry csvEntry = archive.CreateEntry(csvFileName);
                    using (Stream csvEntryStream = csvEntry.Open())
                    {
                        CsvRecordDal.ConstructCsvFile(csvFile.CsvRecords, csvEntryStream);
                    }
                }
            }

            zipToOpen.Flush();
            zipToOpen.Position = 0;

            return zipToOpen;
        }
    }
}