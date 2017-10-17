using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UploadCsv.Models
{
    public class CsvFile
    {
        public int Id { get; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public DateTime LastModified { get; }
        public byte[] FileContent { get; private set; }

        public CsvFile (int id, string fileName, string contentType, DateTime lastModified)
        {
            this.Id = id;
            this.FileName = fileName;
            this.ContentType = contentType;
            this.LastModified = lastModified;
        }

        public CsvFile (string fileName, string contentType, byte[] fileContent)
        {
            this.FileName = fileName;
            this.ContentType = contentType;
            this.FileContent = new byte[fileContent.Length];
            Array.Copy(fileContent, FileContent, fileContent.Length);
        }
    }
}