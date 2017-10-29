using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AlaskaAir.Models;

namespace AlaskaAir.Controllers
{
    public class HomeController : Controller
    {
        AirlineData airline = new AirlineData();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult FindFlights()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetAllAirports()
        {
            IEnumerable<Airport> airports = airline.Airports;
            return Json(airports, JsonRequestBehavior.AllowGet);
        }

        Airport FindAirport(string city, IList<string> errors)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                errors.Add("City name can't be empty. Please try again.");
                return null;
            }

            string cityBrev = city.Length > 3 ? city.Substring(0, 3) : city;
            Airport airport = airline.Airports.FirstOrDefault(
                a => string.Equals(a.Code, cityBrev, StringComparison.OrdinalIgnoreCase)
                    || a.Name.StartsWith(cityBrev, StringComparison.OrdinalIgnoreCase));

            if (airport == null)
            {
                errors.Add(string.Format("City [{0}] is not available in our market. Please check spelling.", city));
            }

            return airport;
        }

        [HttpPost]
        public ActionResult FindFlights(string departureCity, string arrivalCity, string sortBy, string sortDirection)
        {
            ViewBag.DepartureAirport = departureCity;
            ViewBag.ArrivalAirport = arrivalCity;

            IList<string> errors = new List<string>();

            Airport departureAirport = this.FindAirport(departureCity, errors);
            Airport arrivalAirport = this.FindAirport(arrivalCity, errors);

            if (errors.Count > 0)
            {
                ViewBag.Errors = errors;
                return View();
            }

            ViewBag.DepartureAirport = departureAirport.Code;
            ViewBag.ArrivalAirport = arrivalAirport.Code;
            ViewBag.SortBy = sortBy;
            ViewBag.SortDirection = sortDirection;

            // now search flights
            IEnumerable<Flight> flights = airline.Flights.Where(
                f => string.Equals(f.From, departureAirport.Code, StringComparison.OrdinalIgnoreCase)
                    && string.Equals(f.To, arrivalAirport.Code, StringComparison.OrdinalIgnoreCase));

            switch (sortBy)
            {
                case "Departs":
                    flights = sortDirection == "ASC" ?
                        flights.OrderBy(f => f.Departs) :
                        flights.OrderByDescending(f => f.Departs);
                    break;
                case "MainCabinPrice":
                    flights = sortDirection == "ASC" ?
                        flights.OrderBy(f => f.MainCabinPrice) :
                        flights.OrderByDescending(f => f.MainCabinPrice);
                    break;
            }
            return View(flights);
        }

        [HttpGet]
        public ActionResult FindFlightsForWebApi(string departureCity, string arrivalCity)
        {
            IEnumerable<Flight> flights = new List<Flight>();

            IList<string> errors = new List<string>();

            Airport departureAirport = this.FindAirport(departureCity, errors);
            Airport arrivalAirport = this.FindAirport(arrivalCity, errors);

            if (errors.Count == 0)
            {
                // now search flights
                flights = airline.Flights.Where(
                    f => string.Equals(f.From, departureAirport.Code, StringComparison.OrdinalIgnoreCase)
                        && string.Equals(f.To, arrivalAirport.Code, StringComparison.OrdinalIgnoreCase));
            }

            return Json(flights, JsonRequestBehavior.AllowGet);
        }
    }
}