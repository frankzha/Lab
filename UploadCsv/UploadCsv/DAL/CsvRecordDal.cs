using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using CsvHelper;
using UploadCsv.Models;

namespace UploadCsv.DAL
{
    public class CsvRecordDal
    {
        public static IList<CsvRecord> RetrieveCsvRecords(byte[] fileContent, CsvFile csvFile)
        {
            TextReader textReader = new StreamReader(new MemoryStream(fileContent));
            CsvReader reader = new CsvReader(textReader);
            reader.Configuration.RegisterClassMap<CsvRecordClassMap>();

            reader.Read();
            reader.ReadHeader();

            IList<CsvRecord> records = new List<CsvRecord>();

            while (reader.Read())
            {
                CsvRecord record = reader.GetRecord<CsvRecord>();
                record.CsvFile = csvFile;
                records.Add(record);
            }

            // validate the records
            string result = ValidateCsvRecords(records);
            if (!string.IsNullOrEmpty(result))
            {
                throw new Exception(result);
            }

            return records;
        }

        public static Stream ConstructCsvFile(ICollection<CsvRecord> records)
        {
            MemoryStream ms = new MemoryStream();
            TextWriter textWriter = new StreamWriter(ms);
            CsvWriter writer = new CsvWriter(textWriter);
            writer.Configuration.RegisterClassMap<CsvRecordClassMap>();

            writer.WriteHeader<CsvRecord>();
            writer.NextRecord();
            writer.WriteRecords(records);
            textWriter.Flush();
            ms.Seek(0, 0);

            return ms;
        }

        public static void ConstructCsvFile(ICollection<CsvRecord> records, Stream outputStream)
        {
            TextWriter textWriter = new StreamWriter(outputStream);
            CsvWriter writer = new CsvWriter(textWriter);
            writer.Configuration.RegisterClassMap<CsvRecordClassMap>();

            writer.WriteHeader<CsvRecord>();
            writer.NextRecord();
            writer.WriteRecords(records);
            textWriter.Flush();
        }

        static string ValidateCsvRecords(IList<CsvRecord> records)
        {
            string result = "";

            bool negativeQuantity = records.Any(r => r.Quantity < 0);
            if (negativeQuantity)
            {
                result = "Error: some record has negative quantity column. ";
            }

            IEnumerable<string> cycle = DirectedGraphUtil.DirectedGraphHasCycle(records);
            if (cycle != null)
            {
                result = result + "Error: the records contain cycle: " + string.Join("->", cycle);
            }

            return result;
        }
    }
}