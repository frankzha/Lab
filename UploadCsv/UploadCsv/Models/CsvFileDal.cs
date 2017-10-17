using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace UploadCsv.Models
{
    public class CsvFileDal
    {
        public string ConnectionString {get; private set;}

        public CsvFileDal()
        {
            if (WebConfigurationManager.ConnectionStrings.Count > 0)
            {
                ConnectionString = WebConfigurationManager.ConnectionStrings["CsvDbConn"].ConnectionString;
            }
        }

        public CsvFile DownloadCsvFile(int? id)
        {
            CsvFile csvFile;

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetCsvFile";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        sdr.Read();
                        byte[] bytes = (byte[])sdr["FileContent"];
                        string contentType = sdr["ContentType"].ToString();
                        string fileName = sdr["FileName"].ToString();
                        csvFile = new CsvFile(fileName, contentType, bytes);
                    }
                    con.Close();
                }
            }

            return csvFile;
        }

        public void AddCsvFile(CsvFile csvFile)
        {
            using (SqlConnection con = new SqlConnection(this.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "AddCsvFile";
                    cmd.Connection = con;

                    string fileName = csvFile.FileName;
                    string contentType = csvFile.ContentType;
                    byte[] fileContent = csvFile.FileContent;

                    SqlParameter paramFileName = cmd.Parameters.AddWithValue("@fileName", fileName);
                    paramFileName.SqlDbType = SqlDbType.NVarChar;
                    paramFileName.Size = 128;

                    SqlParameter paramCoontentType = cmd.Parameters.AddWithValue("@contentType", contentType);
                    paramFileName.SqlDbType = SqlDbType.NVarChar;
                    paramFileName.Size = 128;

                    SqlParameter paramFileContent = cmd.Parameters.AddWithValue("@fileContent", fileContent);
                    paramFileContent.SqlDbType = SqlDbType.VarBinary;
                    paramFileContent.Size = fileContent.Length;

                    con.Open();
                    cmd.ExecuteScalar();
                }
            }
        }

        public List<CsvFile> GetFiles()
        {
            List<CsvFile> files = new List<CsvFile>();

            using (SqlConnection con = new SqlConnection(this.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetCsvFiles";

                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            int id = Convert.ToInt32(sdr["Id"]);
                            string fileName = sdr["FileName"].ToString();
                            string contentType = sdr["ContentType"].ToString();
                            DateTime lastModified = Convert.ToDateTime(sdr["LastModified"]);
                            files.Add(new CsvFile(id, fileName, contentType, lastModified));
                        }
                    }
                }
            }
            return files;
        }
    }
}