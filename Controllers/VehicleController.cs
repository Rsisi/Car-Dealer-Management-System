using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using team011.Helper;
using team011.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace team011.Controllers
{
    public class VehicleController : Controller
    {
        // GET: /<controller>/
        public IActionResult VehicleDetailForm(string VIN)
        {
            
            SqlCommand cmd;
            SqlConnection cnn;
            SqlDataReader reader;
            var vehicle = new Vehicle();          
            var customer = new Customer();
            vehicle.isSold = false;
            customer.isIndividual = false;
            vehicle.isSold = false;

            cnn = ConnectionHelper.openConnection();


            
            //sql command
            var vehicleETEsql = "WITH VehicleCTE AS( SELECT v.VIN, description, model_year, model_name, " +
                "invoice_price,manufacturer_name , inv_writer_user_name, add_date,  'Car' as vehicle_type " +
                "FROM Vehicle v INNER JOIN Car c on  v.VIN = c.VIN " +
                "UNION " +
                "SELECT v.VIN, description, model_year, model_name, invoice_price,manufacturer_name , inv_writer_user_name, add_date, " +
                "'Convertible' FROM Vehicle v INNER JOIN Convertible c on  v.VIN = c.VIN " +
                "UNION " +
                "SELECT v.VIN, description, model_year, model_name, invoice_price,manufacturer_name , inv_writer_user_name," +
                "add_date, 'Truck' FROM Vehicle v INNER JOIN Truck t on  v.VIN = t.VIN " +
                "UNION " +
                "SELECT v.VIN, description, model_year, model_name, invoice_price,manufacturer_name , inv_writer_user_name, " +
                "add_date, 'Van' FROM Vehicle v INNER JOIN Van  on  v.VIN = Van.VIN " +
                "UNION " +
                "SELECT v.VIN, description, model_year, model_name, invoice_price,manufacturer_name , inv_writer_user_name, " +
                "add_date, 'SUV' FROM Vehicle v INNER JOIN SUV s  on  v.VIN = s.VIN )";
            var selectsql = "SELECT VehicleHasColor.VIN, vehicle_type, model_year, manufacturer_name, " +
                "model_name, description, invoice_price, invoice_price * 1.25 as list_price, first_name, last_name, add_date, " +
                "STRING_AGG(color, ',') AS vehicle_color, "+
                "number_of_doors, roof_type, back_seat_count, cargo_capacity, cargo_cover_type, number_of_rear_axies, has_driver_side_back_door, number_of_cupholder, drivetrain_type "+
                "FROM VehicleCTE INNER JOIN VehicleHasColor " +
                "on VehicleCTE.VIN = VehicleHasColor.VIN INNER JOIN Users on Users.user_name=inv_writer_user_name "+
                "LEFT JOIN Car on VehicleCTE.VIN=Car.VIN LEFT JOIN Convertible on VehicleCTE.VIN=Convertible.VIN LEFT JOIN Truck on VehicleCTE.VIN=Truck.VIN "+
                "LEFT JOIN Van on VehicleCTE.VIN=Van.VIN LEFT JOIN SUV on VehicleCTE.VIN=SUV.VIN "+
                "WHERE VehicleCTE.VIN = @vin " +
                "GROUP by VehicleHasColor.VIN, vehicle_type, model_year, manufacturer_name, model_name, description, invoice_price, invoice_price * 1.25,first_name, last_name, add_date, number_of_doors, roof_type, back_seat_count, cargo_capacity, cargo_cover_type, number_of_rear_axies, has_driver_side_back_door, number_of_cupholder, drivetrain_type ";

            var sql = vehicleETEsql + selectsql;
            cnn = ConnectionHelper.openConnection();
            cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@vin", VIN);
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
                {
                while (reader.Read())
                {
                    vehicle.VIN = (string)reader["VIN"];
                    vehicle.vehicle_type = (string)reader["vehicle_type"];
                    vehicle.model_year = (int)reader["model_year"];
                    vehicle.manufacturer_name = (string)reader["manufacturer_name"];
                    vehicle.model_name = (string)reader["model_name"];
                    vehicle.invoice_price = (decimal)reader["invoice_price"];
                    vehicle.list_price = decimal.Round((decimal)reader["list_price"], 2, MidpointRounding.AwayFromZero);
                    vehicle.first_name = (string)reader["first_name"];
                    vehicle.last_name = (string)reader["last_name"];
                    vehicle.add_date = (DateTime)reader["add_date"];
                    vehicle.vehicle_color = reader["vehicle_color"] != DBNull.Value ? (string)reader["vehicle_color"] : "";
                    vehicle.description = reader["description"] != DBNull.Value ? (string)reader["description"] : "";

                    if (vehicle.vehicle_type == "Car")
                    {
                        vehicle.number_of_doors = (int)reader["number_of_doors"];

                    }
                    else if (vehicle.vehicle_type == "Convertible")
                    {
                        vehicle.roof_type = (string)reader["roof_type"];
                        vehicle.back_seat_count = (int)reader["back_seat_count"];
                    }
                    else if (vehicle.vehicle_type == "Truck")
                    {
                        vehicle.cargo_capacity = (decimal)reader["cargo_capacity"];
                        vehicle.cargo_cover_type = reader["cargo_cover_type"] != DBNull.Value ? (string)reader["cargo_cover_type"]:"";
                        vehicle.number_of_rear_axies = (int)reader["number_of_rear_axies"];
                    }
                    else if (vehicle.vehicle_type == "Van")
                    {
                        vehicle.has_driver_side_back_door = (bool)reader["has_driver_side_back_door"];

                    }
                    else if (vehicle.vehicle_type == "SUV")
                    {
                        vehicle.number_of_cupholder = (int)reader["number_of_cupholder"];
                        vehicle.drivetrain_type = (string)reader["drivetrain_type"];
                    }
                }
                }
                

            var buyersql = "SELECT SalesTransaction.customerID, email, phone_number, street_address, city, state, postal_code, " +
                "sold_price, sales_writer_user_name, sold_price, transaction_date, U.first_name AS uf, U.last_name AS ul, " +
                "I.first_name as Indf, I.last_name as Indl, B.business_name, B.contact_name, B.contact_title "+
                "FROM SalesTransaction, Users AS U JOIN SalesPeople AS S on U.user_name=S.user_name, Customer AS C " +
                "LEFT JOIN Individual AS I on C.customerID=I.customerID LEFT JOIN Business AS B on C.customerID=B.customerID " +
                "WHERE SalesTransaction.VIN = @vin AND SalesTransaction.CustomerID = C.customerID " +
                "AND SalesTransaction.sales_writer_user_name=U.user_name;";
            cnn = ConnectionHelper.openConnection();
            cmd = new SqlCommand(buyersql, cnn);
            cmd.Parameters.AddWithValue("@vin", VIN);
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
                {
                vehicle.isSold = true;
                    while (reader.Read())
                    {
                    customer.customerID = (int)reader["customerID"];
                    customer.email = reader["email"] != DBNull.Value ? (string)reader["email"] : "";
                    customer.phone_number = (string)reader["phone_number"];
                    customer.street_address = (string)reader["street_address"];
                    customer.city = (string)reader["city"];
                    customer.state = (string)reader["state"];
                    customer.postal_code = (string)reader["postal_code"];
                    if(reader["business_name"] != DBNull.Value)
                    {
                        customer.isIndividual = false;
                        customer.business_name = (string)reader["business_name"];
                        customer.contact_name = (string)reader["contact_name"];
                        customer.contact_title= (string)reader["contact_title"];
                    }
                    else
                    {
                        customer.isIndividual = true;
                        customer.first_name = (string)reader["Indf"];
                        customer.last_name = (string)reader["Indl"];
                    
                    }


                    vehicle.sold_price = (decimal)reader["sold_price"];
                    vehicle.sold_date = (DateTime)reader["transaction_date"];
                    vehicle.sales_people_name = (string)reader["uf"] + ", " + (string)reader["ul"];

                    }
                }

            var repairHistorys = new List<RepairOrder>();
            var repairHistorySQL = "SELECT B.business_name, I.first_name, I.last_name, R.start_date, complete_date, labor_charge, parts_cost, R.repair_starter, U.first_name AS uf, U.last_name AS ul " +
                "FROM Customer AS C LEFT OUTER JOIN Individual AS I on C.customerID=I.customerID  LEFT OUTER JOIN Business AS B on C.customerID=B.customerID, " +
                "RepairDetails AS R LEFT JOIN " +
                "(SELECT VIN, start_date, SUM (quantity*price) AS parts_cost FROM Part GROUP BY Part.VIN, Part.start_date) AS P " +
                "ON (R.start_date= P.start_date AND R.VIN=P.VIN) "+
                "JOIN USERS as U on R.repair_starter=U.user_name "+
                "WHERE R.customerID = C.customerID AND R.VIN = @vin  " +
                "ORDER BY R.start_date DESC, R.complete_date ASC; ";
              
            cmd = new SqlCommand(repairHistorySQL, cnn);
            cmd.Parameters.AddWithValue("@vin", VIN);
            reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                vehicle.isRepaired = true;
                while (reader.Read())
                {
                    var repairHistory = new RepairOrder();
                    var bn = reader["business_name"] != DBNull.Value ? (string)reader["business_name"] : "";
                    var fn = reader["first_name"] != DBNull.Value ? (string)reader["first_name"] : "";
                    var ln = reader["last_name"] != DBNull.Value ? (string)reader["last_name"] : "";
                    repairHistory.customer_name = bn + fn + " " + ln;
                    repairHistory.repair_starter_name = (string)reader["uf"] + " " + (string)reader["ul"];
                    repairHistory.start_date = (DateTime)reader["start_date"];
                    repairHistory.complete_date = reader["complete_date"]!=DBNull.Value?(DateTime)reader["complete_date"]: new DateTime(1111, 1, 1, 0, 0, 0);
                    repairHistory.labor_charge = (decimal)reader["labor_charge"];
                    repairHistory.parts_cost = reader["parts_cost"] != DBNull.Value ? (decimal)reader["parts_cost"] : 0;
                    repairHistory.total_cost = repairHistory.labor_charge + repairHistory.parts_cost;
                    repairHistorys.Add(repairHistory);

                }
            }

            ViewBag.repairHistorys = repairHistorys;
            ViewBag.vehicle = vehicle;
                ViewBag.customer = customer;

            //close sql connection
            ConnectionHelper.closeConnection(cnn, cmd, reader);





            return View();
            
            //ViewBag.vehicle = "vehicle Detail for VIN:" + VIN;
            //ViewBag.VIN = VIN;
            //return View();
        }

        //POST AddVehicleForm
        public IActionResult AddVehicleForm()
        {
            SqlCommand cmd;
            SqlConnection cnn;
            SqlDataReader reader;
            cnn = ConnectionHelper.openConnection();

          
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

                ViewBag.allcolors = new MultiSelectList(allcolors, "name", "name");
            }
            //get all manufacturer
            var allmanufacturer = new List<Manufacturer>();
            var manufacturersql = "SELECT name FROM Manufacturer;";
            cmd = new SqlCommand(manufacturersql, cnn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var manufacturer = new Manufacturer();
                manufacturer.name = (string)reader["name"];
                allmanufacturer.Add(manufacturer);

                ViewBag.allmanufacturer = new SelectList(allmanufacturer, "name", "name");
            }

            //close sql connection
            ConnectionHelper.closeConnection(cnn, cmd, reader);

            //get current year
            ViewBag.currentYear= DateTime.Now.Year;



            //all vehicle types
            var alltypes = new List<SelectListItem>()
            {
                new SelectListItem{ Value = "Van" , Text="Van"},
                new SelectListItem{ Value = "Truck" , Text="Truck"},
                new SelectListItem{ Value = "SUV" , Text="SUV"},
                new SelectListItem{ Value = "Car" , Text="Car"},
                new SelectListItem{ Value = "Convertible" , Text="Convertible"},
            };
            ViewBag.alltypes = new SelectList(alltypes, "Value", "Text");

            return View();
        }

        public JsonResult searchVehicleByFeature(string color,string manufacture,string type,string year,string model, string keyword, string isSold, string minprice, string maxprice, bool iscaseSensitive) {

            SqlCommand cmd;
            SqlConnection cnn;
            SqlDataReader reader;
            var vehicles =  new List<Vehicle>();

            //var vehicleETEsql = "WITH VehicleCTE AS( SELECT v.VIN, description, model_year, model_name, " +
            //    "invoice_price,manufacturer_name , inv_writer_user_name, add_date,  'Car' as vehicle_type " +
            //    "FROM Vehicle v INNER JOIN Car c on  v.VIN = c.VIN " +
            //    "UNION " +
            //    "SELECT v.VIN, description, model_year, model_name, invoice_price,manufacturer_name , inv_writer_user_name, add_date, " +
            //    "'Convertible' FROM Vehicle v INNER JOIN Convertible c on  v.VIN = c.VIN " +
            //    "UNION " +
            //    "SELECT v.VIN, description, model_year, model_name, invoice_price,manufacturer_name , inv_writer_user_name," +
            //    "add_date, 'Truck' FROM Vehicle v INNER JOIN Truck t on  v.VIN = t.VIN " +
            //    "UNION " +
            //    "SELECT v.VIN, description, model_year, model_name, invoice_price,manufacturer_name , inv_writer_user_name, " +
            //    "add_date, 'Van' FROM Vehicle v INNER JOIN Van  on  v.VIN = Van.VIN " +
            //    "UNION " +
            //    "SELECT v.VIN, description, model_year, model_name, invoice_price,manufacturer_name , inv_writer_user_name, " +
            //    "add_date, 'SUV' FROM Vehicle v INNER JOIN SUV s  on  v.VIN = s.VIN )";

            //var selectsql = "SELECT VehicleHasColor.VIN, vehicle_type, model_year, manufacturer_name, " +
            //    "model_name, description, invoice_price * 1.25 as list_price, " +
            //    "STRING_AGG(color, ',') AS vehicle_color FROM VehicleCTE INNER JOIN VehicleHasColor " +
            //    "on VehicleCTE.VIN = VehicleHasColor.VIN WHERE 1=1 ";
            var selectsql = "select * from Vehicle_Details where 1 = 1 ";
            if (color != null)
            {
                selectsql += " AND color like '%" + color + "%'";
            }
            if(model != null)
            {
                selectsql += " AND model_name = '" + model + "'";
            }
            if(manufacture != null)
            {
                selectsql += " AND manufacturer_name = '" + manufacture + "'";
            }
            if (type != null)
            {
                selectsql += " AND vehicle_type =  '"+ type + "'";
            }
            if (year != null)
            {
                selectsql += " AND model_year =  '" + year + "'";
            }
            if(keyword != null)
            {
                if (iscaseSensitive)
                {
                    selectsql += " AND (model_year LIKE '%" + keyword
                  + "%' OR model_name COLLATE Latin1_General_CS_AI  LIKE '%" + keyword
                  + "%' OR manufacturer_name COLLATE Latin1_General_CS_AI  LIKE '%" + keyword
                  + "%' OR description COLLATE Latin1_General_CS_AI LIKE '%" + keyword + "%') ";
                }
                else
                {
                    selectsql += " AND (model_year LIKE '%" + keyword
                  + "%' OR model_name  LIKE '%" + keyword
                  + "%' OR manufacturer_name   LIKE '%" + keyword
                  + "%' OR description  LIKE '%" + keyword + "%') ";
                }
              
            }
            //sold
            
            if (isSold == "Sold")
            {

                selectsql += " AND isSold = 1";
            }
            //unsold
            else if (isSold == "Unsold")
            {
                selectsql += " AND isSold = 0";
            }

            if(minprice != null)
            {
                selectsql += " AND list_price >= '"+ minprice+"'";
            }
            if (maxprice != null)
            {
                selectsql += " AND list_price <= '" + maxprice + "'";
            }




            //selectsql += " GROUP by VehicleHasColor.VIN, vehicle_type, model_year, manufacturer_name, model_name, description, invoice_price * 1.25";

            var sql = selectsql;
            cnn = ConnectionHelper.openConnection();
            cmd = new SqlCommand(sql, cnn);
           
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var vehicle = new Vehicle();
                vehicle.VIN = (string)reader["VIN"];
                vehicle.vehicle_type = (string)reader["vehicle_type"];
                vehicle.model_year = (int)reader["model_year"]; 
                vehicle.manufacturer_name = (string)reader["manufacturer_name"];
                vehicle.model_name = (string)reader["model_name"];
                vehicle.list_price = decimal.Round((decimal)reader["list_price"], 2, MidpointRounding.AwayFromZero);
                vehicle.vehicle_color = reader["color"] != DBNull.Value ? (string)reader["color"] : "";
                vehicle.description = reader["description"] != DBNull.Value ? (string)reader["description"] : "";

                vehicles.Add(vehicle);


            }
            ConnectionHelper.closeConnection(cnn, cmd, reader);
            return Json(vehicles);

        }

        public JsonResult searchVehicleByVIN(string vin)
        {
            SqlCommand cmd;
            SqlConnection cnn;
            SqlDataReader reader;
            var vehicles = new List<Vehicle>();

            var vehicleETEsql = "WITH VehicleCTE AS( SELECT v.VIN, description, model_year, model_name, " +
                "invoice_price,manufacturer_name , inv_writer_user_name, add_date,  'Car' as vehicle_type " +
                "FROM Vehicle v INNER JOIN Car c on  v.VIN = c.VIN " +
                "UNION " +
                "SELECT v.VIN, description, model_year, model_name, invoice_price,manufacturer_name , inv_writer_user_name, add_date, " +
                "'Convertible' FROM Vehicle v INNER JOIN Convertible c on  v.VIN = c.VIN " +
                "UNION " +
                "SELECT v.VIN, description, model_year, model_name, invoice_price,manufacturer_name , inv_writer_user_name," +
                "add_date, 'Truck' FROM Vehicle v INNER JOIN Truck t on  v.VIN = t.VIN " +
                "UNION " +
                "SELECT v.VIN, description, model_year, model_name, invoice_price,manufacturer_name , inv_writer_user_name, " +
                "add_date, 'Van' FROM Vehicle v INNER JOIN Van  on  v.VIN = Van.VIN " +
                "UNION " +
                "SELECT v.VIN, description, model_year, model_name, invoice_price,manufacturer_name , inv_writer_user_name, " +
                "add_date, 'SUV' FROM Vehicle v INNER JOIN SUV s  on  v.VIN = s.VIN )";
            var selectsql = "SELECT VehicleHasColor.VIN, vehicle_type, model_year, manufacturer_name, " +
                "model_name, description, invoice_price * 1.25 as list_price, " +
                "STRING_AGG(color, ',') AS vehicle_color FROM VehicleCTE INNER JOIN VehicleHasColor " +
                "on VehicleCTE.VIN = VehicleHasColor.VIN WHERE VehicleCTE.VIN = @vin " +
                "GROUP by VehicleHasColor.VIN, vehicle_type, model_year, manufacturer_name, model_name, description, invoice_price * 1.25";

            var sql = vehicleETEsql + selectsql;
            cnn = ConnectionHelper.openConnection();
            cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@vin", vin);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var vehicle = new Vehicle();
                vehicle.VIN = (string)reader["VIN"];
                vehicle.vehicle_type = (string)reader["vehicle_type"];
                vehicle.model_year = (int)reader["model_year"];
                vehicle.manufacturer_name = (string)reader["manufacturer_name"];
                vehicle.model_name = (string)reader["model_name"];
                vehicle.list_price = decimal.Round((decimal)reader["list_price"], 2, MidpointRounding.AwayFromZero);
                vehicle.vehicle_color = (string)reader["vehicle_color"];
                vehicle.description = reader["description"] != DBNull.Value ? (string)reader["description"] : "";

                vehicles.Add(vehicle);


            }
            ConnectionHelper.closeConnection(cnn, cmd, reader);
            return Json(vehicles);

        }

        public JsonResult addVehicle(string vehicle_type, string vin, string description, int model_year, string model_name, float invoice_price, string manufacturer_name, string inv_writer_user_name, string add_date, string[] colors,
            int number_of_doors, string roof_type, int back_seat_count, float cargo_capacity, string cargo_cover_type, int number_of_rear_axies,  string has_driver_side_back_door, int number_of_cupholder, string drivetrain_type )
        {

            try
            {
                SqlCommand cmd;
                SqlConnection cnn;
                cnn = ConnectionHelper.openConnection();
                cmd = new SqlCommand();
                cmd.Connection = cnn;


                //insert into Vehicle
                var sql1 = "INSERT INTO Vehicle Values( @vin, @description, @model_year, @model_name, @invoice_price, @manufacturer_name, @inv_writer_user_name, @add_date); ";

                cmd.Parameters.AddWithValue("@vin", vin);
                if (description == null)
                {
                    cmd.Parameters.AddWithValue("@description", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@description", description);
                }

                cmd.Parameters.AddWithValue("@model_year", model_year);
                cmd.Parameters.AddWithValue("@model_name", model_name);
                cmd.Parameters.AddWithValue("@invoice_price", invoice_price);
                cmd.Parameters.AddWithValue("@manufacturer_name", manufacturer_name);
                cmd.Parameters.AddWithValue("@inv_writer_user_name", inv_writer_user_name);
                cmd.Parameters.AddWithValue("@add_date", DateTime.Parse(add_date));




                //insert into Car Convertible, Truck, Van, SUV accordingly
                var sql2 = " ";
                if (vehicle_type == "Car")
                {
                    sql2 = "INSERT INTO Car (VIN, number_of_doors) Values( @vin, @number_of_doors); ";
                    cmd.Parameters.AddWithValue("@number_of_doors", number_of_doors);
                }
                else if (vehicle_type == "Convertible")
                {
                    sql2 = "INSERT INTO  Convertible (VIN, roof_type, back_seat_count) Values( @vin, @roof_type, @back_seat_count); ";
                    cmd.Parameters.AddWithValue("@roof_type", roof_type);
                    cmd.Parameters.AddWithValue("@back_seat_count", back_seat_count);
                }
                else if (vehicle_type == "Truck")
                {
                    sql2 = "INSERT INTO Truck (VIN, cargo_capacity, cargo_cover_type,number_of_rear_axies) Values( @vin, @cargo_capacity, @cargo_cover_type, @number_of_rear_axies); ";
                    cmd.Parameters.AddWithValue("@cargo_capacity", cargo_capacity);
                    if (cargo_cover_type == null)
                    {
                        cmd.Parameters.AddWithValue("@cargo_cover_type", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@cargo_cover_type", cargo_cover_type);
                    }

                    cmd.Parameters.AddWithValue("@number_of_rear_axies", number_of_rear_axies);
                }
                else if (vehicle_type == "Van")
                {
                    sql2 = "INSERT INTO Van Values( @vin, @has_driver_side_back_door); ";
                    cmd.Parameters.AddWithValue("@has_driver_side_back_door", has_driver_side_back_door);
                }
                else if (vehicle_type == "SUV")
                {
                    sql2 = "INSERT INTO SUV Values( @vin, @number_of_cupholder, @drivetrain_type); ";
                    cmd.Parameters.AddWithValue("@number_of_cupholder", number_of_cupholder);
                    cmd.Parameters.AddWithValue("@drivetrain_type", drivetrain_type);
                }


                //insert into VehicleHasColor
                var sql3 = " ";
                const string refcmdText = "INSERT INTO VehicleHasColor (VIN, color) VALUES (@vin, @color{0}); ";
                int count = 0;
                foreach (string color in colors)
                {
                    sql3 += string.Format(refcmdText, count);
                    cmd.Parameters.AddWithValue(string.Format("@color{0}", count), color);
                    count++;

                }


                cmd.CommandText = sql1 + sql2 + sql3;
                cmd.ExecuteNonQuery();
                cmd.Dispose();




                cnn.Close();


                return Json(true);
            }
            catch(Exception e)
            {
                return Json(false);
            }
            

        }

    }
}
