using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UploadCsv.Models
{
    public class CsvFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public DateTime LastModified { get; set; }
        public byte[] FileContent { get; private set; }

        public ICollection<CsvRecord> CsvRecords { get; set; }

        public CsvFile()
        {

        }

        public CsvFile(string fileName, string contentType)
        {
            this.FileName = fileName;
            this.ContentType = contentType;
            this.LastModified = DateTime.UtcNow;
        }
    }
}