using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CsvHelper.Configuration;

namespace UploadCsv.Models
{
    public class CsvRecordClassMap : ClassMap<CsvRecord>
    {
        public CsvRecordClassMap()
        {
            Map(m => m.Parent);
            Map(m => m.Child);
            Map(m => m.Quantity);
        }
    }
}