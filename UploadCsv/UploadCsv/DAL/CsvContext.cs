using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using UploadCsv.Models;

namespace UploadCsv.DAL
{
    public class CsvContext : DbContext
    {
        public CsvContext() : base()
        {

        }

        public DbSet<CsvFile> CsvFiles { get; set; }
        public DbSet<CsvRecord> CsvRecords { get; set; }
    }
}