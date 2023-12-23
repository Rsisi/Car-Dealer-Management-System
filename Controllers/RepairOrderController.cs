using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using team011.Helper;
using team011.Models;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace team011.Controllers
{
  
    public class RepairOrderController : Controller
    {
    //    // GET: /<controller>/
    //    public IActionResult NewRepairForm()
    //    {
    //        ViewBag.repair = "This is start new repair order view";
    //        return View();
    //    }
    //    public IActionResult UpdateRepairForm()
    //    {
    //        ViewBag.updaterepair = "This is update  repair order view";
    //        return View();
    //    }
   
        public IActionResult Index()
        {
            ViewBag.Current = "RepairOrder";
            //open sql connection
            SqlCommand cmd;
            SqlConnection cnn;
            SqlDataReader reader;

            cnn = ConnectionHelper.openConnection();
            var allCustomersql = "SELECT distinct [customerID], CONCAT( CustomerName,'_',DL_Tax_ID) concat_customerName_identityID " +
                        "FROM[Jaunty_Jalopies].[dbo].[Customer_Details]";


            var allcustomers = new List<Customer>();

            cmd = new SqlCommand(allCustomersql, cnn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var customer = new Customer();
                customer.customerID = (int)reader["customerID"];
                customer.concat_customerName_identityID = (string)reader["concat_customerName_identityID"];
                allcustomers.Add(customer);

                ViewBag.allcustomers = new SelectList(allcustomers, "customerID", "concat_customerName_identityID");
            }



            ConnectionHelper.closeConnection(cnn, cmd, reader);
            return View();
        }

        public JsonResult SearchOrderByVIN(string VIN)
        {
            SqlCommand cmd;
            SqlConnection cnn;
            SqlDataReader reader;
            var repairOrder = new RepairOrder();
            var hasOpenOrder = false;
            var soldVehicle = new Vehicle();

            cnn = ConnectionHelper.openConnection();
            var issoldVin = "SELECT [customerID] ,[sales_writer_user_name] ,[sold_price] ,[transaction_date], v.* "+
                    "FROM [dbo].[SalesTransaction] s "+
                    "inner join [dbo].[Vehicle_Details] v on s.VIN = v.VIN where v.VIN = @vin";
            cmd = new SqlCommand(issoldVin, cnn);
            cmd.Parameters.AddWithValue("@vin", VIN);
            reader = cmd.ExecuteReader();
            var issoldVehicle = false;
            while (reader.Read())
            {
                //if sold count > 0 then this is sold vehicle
                //if sold count = 0 then this is vin is not from transaction table
                issoldVehicle = true;
                soldVehicle.VIN = VIN;
                soldVehicle.model_name = (string)reader["model_name"];
                soldVehicle.model_year = (int)reader["model_year"];
                soldVehicle.vehicle_type = (string)reader["vehicle_type"];
                soldVehicle.manufacturer_name = (string)reader["manufacturer_name"];
                soldVehicle.vehicle_color = (string)reader["color"];

            }

            if (issoldVehicle)
            {
                var checkOpenRepairOrdersql = "select r.*, c.[CustomerName], CONCAT(u.first_name, ', ', u.last_name) as ServiceWriter ,"+
                                            "vd.description as vehicle_description, vd.model_year, vd.model_year, vd.model_name, vd.invoice_price," +
                                            "vd.manufacturer_name, vd.inv_writer_user_name, vd.add_date,vd.vehicle_type, vd.color "+
                                            "from[dbo].[RepairDetails] r "+
                                            "inner join[dbo].[Customer_Details] c on r.customerID = c.CustomerID "+
                                            "inner join[dbo].[Users] u on r.repair_starter = u.[user_name] "+
                                            "inner join Vehicle_Details vd on vd.VIN = r.VIN " +
                                            "WHERE r.VIN = @vin and r.complete_date is null ";
                //sql command
                cmd = new SqlCommand(checkOpenRepairOrdersql, cnn);
                cmd.Parameters.AddWithValue("@vin", VIN);
                //read resutlt
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    hasOpenOrder = true;
                    while (reader.Read())
                    {
                        if (reader["complete_date"] != DBNull.Value)
                        {
                            repairOrder.complete_date = (DateTime)reader["complete_date"];
                        }
                        repairOrder.labor_charge = reader["labor_charge"] != DBNull.Value ? (decimal)reader["labor_charge"] : 0;
                        repairOrder.description = reader["description"] != DBNull.Value ? (string)reader["description"] : null;
                        repairOrder.VIN = (string)reader["VIN"];
                        repairOrder.customerID = (int)reader["customerID"];
                        repairOrder.customer_name = (string)reader["CustomerName"];
                        repairOrder.repair_starter = (string)reader["repair_starter"];
                        repairOrder.odometer_value = (int)reader["odometer_value"];
                        repairOrder.start_date = DateTime.Parse(reader["start_date"].ToString());
                        repairOrder.repair_starter_name = (string)reader["ServiceWriter"];
                        repairOrder.model_name = (string)reader["model_name"];
                        repairOrder.model_year=(int)reader["model_year"];
                        repairOrder.vehicle_type = (string)reader["vehicle_type"];
                        repairOrder.manufacturer_name = (string)reader["manufacturer_name"];
                        repairOrder.colors = (string)reader["color"];


                    }
                }
                //ViewData["hasOpenOrder"] = hasOpenOrder;
                //ViewBag.repairOrder = repairOrder;
                var parts = new List<Part>();
                if (hasOpenOrder)
                {
                    var totalchargeSql = "select isnull(labor_charge,0) + isnull(sum(p.price*quantity),0) from RepairDetails  r " +
                              "left join[dbo].[Part] p on r.VIN = p.vin and r.start_date=p.start_date " +
                              "where r.vin = @vin and r.start_date=@start_date " +
                              "GROUP by labor_charge";
                    cmd = new SqlCommand(totalchargeSql, cnn);
                    cmd.Parameters.AddWithValue("@start_date", repairOrder.start_date);
                    cmd.Parameters.AddWithValue("@vin", VIN);
                    var totalcharge = cmd.ExecuteScalar();
                    repairOrder.total_cost = (decimal)totalcharge;




                   
                    var partSQL = "Select * from part where vin = @vin and start_date = @start_date ";
                    cmd = new SqlCommand(partSQL, cnn);
                    cmd.Parameters.AddWithValue("@vin", VIN);
                    cmd.Parameters.AddWithValue("@start_date", repairOrder.start_date);
                    reader = cmd.ExecuteReader();

                    
                    while (reader.Read())
                    {
                        var part = new Part();
                        part.quantity = (int)reader["quantity"];
                        part.start_date = (DateTime)reader["start_date"];
                        part.part_number = (string)reader["part_number"];
                        part.price = (decimal)reader["price"];
                        part.vendor = (string)reader["vendor"];
                        parts.Add(part);

                    }
                    //ViewBag.Parts = parts;
                }
              
                ConnectionHelper.closeConnection(cnn, cmd, reader);
                return Json(new { issoldVehicle = issoldVehicle, hasOpenOrder = hasOpenOrder, repairOrder = repairOrder, parts = parts, soldVehicle=soldVehicle });

            }
            else
            {
                //the vehicle either not sold yet or not sold by us
                return Json(new { issoldVehicle});
            }





        }
        [HttpPost]
        //updagte repair order info
        public JsonResult UpdateRepairOrder(double laborcharge, string description, string VIN, string start_date)
        {
            SqlCommand cmd;
            SqlConnection cnn;

            cnn = ConnectionHelper.openConnection();
            var updatelaborchargesql = "update RepairDetails set labor_charge =@laborcharge, description = @description " +
                " where VIN =@VIN and start_date =@start_date ";
            //sql command
            cmd = new SqlCommand(updatelaborchargesql, cnn);
            var date = DateTime.Parse(start_date);
            cmd.Parameters.AddWithValue("@VIN", VIN);
            cmd.Parameters.AddWithValue("@laborcharge", laborcharge);
            cmd.Parameters.AddWithValue("@start_date", DateTime.Parse(start_date));
            cmd.Parameters.AddWithValue("@description", description == null ? "" : description);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            cnn.Close();

            return Json(true);
        }

        public JsonResult AddPart(List<Part> parts)
        {

            SqlCommand cmd;
            SqlConnection cnn;
            SqlDataReader reader;
            cnn = ConnectionHelper.openConnection();

            List<string> existPN = new List<string>();



            foreach (var p in parts)
            {
                var ispart_numExist = false;
                var sql = "select count(1) count from Part where VIN = @VIN and start_date = @start_date and part_number = @part_number";
                cmd = new SqlCommand(sql, cnn);

                cmd.Parameters.AddWithValue("@VIN", p.VIN);
                cmd.Parameters.AddWithValue("@part_number", p.part_number);
                cmd.Parameters.AddWithValue("@start_date", p.start_date);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ispart_numExist = (int)reader["count"] == 0 ? false : true;

                }
                if (ispart_numExist)
                {
                    existPN.Add(p.part_number);
                }
                else
                {
                    var addpartsql = "Insert into Part Values( @VIN, @start_date, @part_number, " +
                    " @quantity, @price,@vendor)";
                    //sql command
                    cmd = new SqlCommand(addpartsql, cnn);

                    cmd.Parameters.AddWithValue("@VIN", p.VIN);
                    cmd.Parameters.AddWithValue("@part_number", p.part_number);
                    cmd.Parameters.AddWithValue("@start_date", p.start_date);
                    cmd.Parameters.AddWithValue("@quantity", p.quantity);
                    cmd.Parameters.AddWithValue("@price", p.price);
                    cmd.Parameters.AddWithValue("@vendor", p.vendor);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                }



            }


            cnn.Close();

            return Json(new { success = true, existPN = existPN });

        }

        public JsonResult UpdatePartQuantity(List<Part> parts)
        {

            SqlCommand cmd;
            SqlConnection cnn;

            cnn = ConnectionHelper.openConnection();
            foreach (var p in parts)
            {
                var addpartsql = "update Part set quantity =@quantity  where VIN =@VIN and [start_date] = @start_date  and [part_number] =@part_number ";
                //sql command
                cmd = new SqlCommand(addpartsql, cnn);

                cmd.Parameters.AddWithValue("@VIN", p.VIN);
                cmd.Parameters.AddWithValue("@part_number", p.part_number);
                cmd.Parameters.AddWithValue("@start_date", p.start_date);
                cmd.Parameters.AddWithValue("@quantity", p.quantity);

                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }


            cnn.Close();

            return Json(true);

        }

        public JsonResult CreateNewRepair(string vin, string customer, string odometer, string description)
        {
            try
            {
                SqlCommand cmd;
                SqlConnection cnn;
                SqlDataReader reader;
                var sameDayRepair = false;

                cnn = ConnectionHelper.openConnection();
                var date = DateTime.Now;
                //check if vin has completed repairs today
                var sql = " select * from RepairDetails where  complete_date =@today and VIN = @vin";
                cmd = new SqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@vin", vin);
                cmd.Parameters.AddWithValue("@today", date);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    sameDayRepair = true;
                    return Json(false);
                }

                else
                {
                    var newRepair = new RepairOrder();
                    sql = "insert into [dbo].[RepairDetails] (VIN, start_date,customerID,repair_starter,odometer_value,[description]) " +
                                "values (@vin, @start_date, @customerID, @repair_starter, @odometer, @description) ";
                    cmd = new SqlCommand(sql, cnn);

                    var repair_starter = this.HttpContext.Session.GetString("loggedUserName");
                    cmd.Parameters.AddWithValue("@vin", vin);
                    cmd.Parameters.AddWithValue("@start_date", date);
                    cmd.Parameters.AddWithValue("@customerID", customer);
                    cmd.Parameters.AddWithValue("@repair_starter", repair_starter);
                    cmd.Parameters.AddWithValue("@odometer", odometer);
                    cmd.Parameters.AddWithValue("@description", description == null ? "":description);
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                    cmd.Dispose();
                    return Json(true);
                }
               
                
            }
            catch(Exception e)
            {
                return Json(false);
            }



        }

        public JsonResult CompleteRepairOrder(string vin, string start_date)
        {
            SqlCommand cmd;
            SqlConnection cnn;
            cnn = ConnectionHelper.openConnection();
            var sql = "Update RepairDetails set [complete_date]=@complete_date where vin = @vin and start_date = @start_date";
            cmd = new SqlCommand(sql, cnn);
            var startdate = DateTime.Parse(start_date);
            var completedate = DateTime.Now;
            cmd.Parameters.AddWithValue("@vin", vin);
            cmd.Parameters.AddWithValue("@start_date", startdate);
            cmd.Parameters.AddWithValue("@complete_date", completedate);
            cmd.ExecuteNonQuery();
            cnn.Close();
            cmd.Dispose();
            return Json(true);

        }

    }
}
