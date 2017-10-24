using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using CsvHelper;

namespace AlaskaAir.Models
{
    public class AirlineData
    {
        public IList<Airport> Airports { get; private set; }
        public IList<Flight> Flights { get; private set; }

        public AirlineData()
        {
            LoadData();
        }

        void LoadData()
        {
            string filePath = HttpContext.Current.Server.MapPath("~/App_Data/airports.csv");

            Airports = new List<Airport>();
            using (TextReader textReader = File.OpenText(filePath))
            {
                CsvReader reader = new CsvReader(textReader);

                reader.Read();
                reader.ReadHeader();

                while (reader.Read())
                {
                    Airport record = reader.GetRecord<Airport>();
                    Airports.Add(record);
                }
            }

            filePath = HttpContext.Current.Server.MapPath("~/App_Data/flights.csv");
            Flights = new List<Flight>();

            using (TextReader textReader = File.OpenText(filePath))
            {
                CsvReader reader = new CsvReader(textReader);

                reader.Read();
                reader.ReadHeader();

                while (reader.Read())
                {
                    Flight record = reader.GetRecord<Flight>();
                    Flights.Add(record);
                }
            }

        }
    }
}