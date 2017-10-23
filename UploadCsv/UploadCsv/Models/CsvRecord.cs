using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UploadCsv.Models
{
    public class CsvRecord
    {
        [Key, Column(Order = 0), ForeignKey("CsvFile")]
        public int Id { get; set; }

        [Key, Column(Order = 1)]
        public string Parent { get; set; }

        [Key, Column(Order = 2)]
        public string Child { get; set; }

        public int Quantity { get; set; }

        public CsvFile CsvFile { get; set; }
    }
}