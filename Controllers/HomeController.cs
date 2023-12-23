using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using team011.Helper;
using team011.Models;

namespace team011.Controllers
{
    public class HomeController : Controller
    {


        public IActionResult Index()
        {
            ViewBag.Current = "Home";
            //open sql connection
            SqlCommand cmd;
            SqlConnection cnn;
            SqlDataReader reader;

            cnn = ConnectionHelper.openConnection();

            //get unsold vehicles
            var unsoldVehicles = 0;
            var unsoldsql = "SELECT count(VIN) FROM Vehicle WHERE VIN NOT IN (SELECT VIN FROM SalesTransaction);";
            //sql command
            cmd = new SqlCommand(unsoldsql, cnn);
            //read resutlt
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                unsoldVehicles = reader.GetInt32(0);
                ViewData["unsoldVehicles"] = unsoldVehicles;
            }

            //get all colors
            var allcolors = new List<Color>();
            var allColorsql = "SELECT name FROM Color";
            cmd = new SqlCommand(allColorsql, cnn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var color = new Color();
                color.name = (string)reader["name"];
                allcolors.Add(color);

                ViewBag.allcolors = new SelectList(allcolors, "name", "name");
            }
            //get all manufacturer
            var allmanufacturer = new List<Manufacturer>();
            var manufacturersql = "select distinct manufacturer_name from Vehicle; ";
            cmd = new SqlCommand(manufacturersql, cnn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var manufacturer = new Manufacturer();
                manufacturer.name = (string)reader["manufacturer_name"];
                allmanufacturer.Add(manufacturer);

                ViewBag.allmanufacturer = new SelectList(allmanufacturer, "name", "name");
            }
            //get all Vehicle Years
            var allyear = new List<Vehicle>();
            var allyearsql = "SELECT DISTINCT model_year FROM Vehicle WHERE VIN NOT IN (SELECT VIN FROM SalesTransaction);";
            cmd = new SqlCommand(allyearsql, cnn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {

                var vehicle = new Vehicle();
                vehicle.model_year = (int)reader["model_year"];
                allyear.Add(vehicle);

                ViewBag.allyear = new SelectList(allyear, "model_year", "model_year");
            }

            //get all Vehicle Models
            var allmodel = new List<Vehicle>();
            var allmodelsql = "SELECT DISTINCT model_name FROM Vehicle WHERE VIN NOT IN (SELECT VIN FROM SalesTransaction);";
            cmd = new SqlCommand(allmodelsql, cnn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {

                var vehicle = new Vehicle();
                vehicle.model_name = (string)reader["model_name"];
                allmodel.Add(vehicle);

                ViewBag.allmodel = new SelectList(allmodel, "model_name", "model_name");
            }


            //close sql connection
            ConnectionHelper.closeConnection(cnn, cmd, reader);

            //all vehicle types
            var alltypes = new List<SelectListItem>()
            {
                new SelectListItem{ Value = "VAN" , Text="VAN"},
                new SelectListItem{ Value = "Truck" , Text="Truck"},
                new SelectListItem{ Value = "SUV" , Text="SUV"},
                new SelectListItem{ Value = "Car" , Text="Car"},
                new SelectListItem{ Value = "Convertible" , Text="Convertible"},
            };
            ViewBag.alltypes = new SelectList(alltypes, "Value", "Text");


            //all isSold vehicle 
            var isSold = new List<SelectListItem>()
            {
                new SelectListItem{ Value = "All" , Text="All", Selected=true   },
                new SelectListItem{ Value = "Unsold" , Text="Unsold"},
                new SelectListItem{ Value = "Sold" , Text="Sold"},
                
            };
            ViewBag.isSold = new SelectList(isSold, "Value", "Text");



            return View();
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }
    }
}
