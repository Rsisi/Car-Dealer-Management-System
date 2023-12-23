using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using team011.Helper;
using team011.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace team011.Controllers
{
    public class ReportController : Controller
    {
        // GET: /<controller>/

        //ActionstoputinURL
        public IActionResult Index()
        {
            ViewBag.Current = "Report";
            return View();
        }

        public IActionResult ColorReport()
        {
            //open sql connection
            SqlCommand cmd;
            SqlConnection cnn;
            SqlDataReader reader;
            //init a report list
            var colorreports = new List<SalesByColorReport>();

            cnn = ConnectionHelper.openConnection();

            var reportsql = "SELECT * FROM Sales_By_Color_Report ORDER BY color ASC;";
            //sql command
            cmd = new SqlCommand(reportsql, cnn);
            //read resutlt
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                //put single row into report variable
                var colorreport = new SalesByColorReport();
                colorreport.color = (string)reader["color"];
                colorreport.thirtyDaysSales = (int)reader["thirtyDaysSales"];
                colorreport.lastYearSales = (int)reader["lastYearSales"];
                colorreport.overallSales = (int)reader["overallSales"];

                //add single report vairable into report list
                colorreports.Add(colorreport);
            }

            //append report list to viewbag for frontend reading
            ViewBag.SalesByColorReport = colorreports;
            return View();
        }



        public IActionResult VehicleTypeReport()
        {

            //open sql connection
            SqlCommand cmd;
            SqlConnection cnn;
            SqlDataReader reader;
            //init a report list
            var vehicletypereports = new List<SalesByVehicleTypeReport>();

            cnn = ConnectionHelper.openConnection();

            var reportsql = "SELECT * FROM Sales_By_Vehicle_Type_Report;";
            //sql command
            cmd = new SqlCommand(reportsql, cnn);
            //read resutlt
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                //put single row into report variable
                var vehicletypereport = new SalesByVehicleTypeReport();
                vehicletypereport.vehicleType = (string)reader["vehicleType"];
                vehicletypereport.thirtyDaysSales = (int)reader["thirtyDaysSales"];
                vehicletypereport.lastYearSales = (int)reader["lastYearSales"];
                vehicletypereport.overallSales = (int)reader["overallSales"];

                //add single report vairable into report list
                vehicletypereports.Add(vehicletypereport);
            }

            //append report list to viewbag for frontend reading
            ViewBag.SalesByVehicleTypeReport = vehicletypereports;

            return View();
        }



        public IActionResult ManufacturerReport()
        {

            //open sql connection
            SqlCommand cmd;
            SqlConnection cnn;
            SqlDataReader reader;
            //init a report list
            var manufacturerreports = new List<SalesByManufacturerReport>();

            cnn = ConnectionHelper.openConnection();

            var reportsql = "SELECT * FROM Sales_By_Manufacturer_Report;";
            //sql command
            cmd = new SqlCommand(reportsql, cnn);
            //read resutlt
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                //put single row into report variable
                var manufacturerreport = new SalesByManufacturerReport();
                manufacturerreport.manufacturer = (string)reader["manufacturer"];
                manufacturerreport.thirtyDaysSales = (int)reader["thirtyDaysSales"];
                manufacturerreport.lastYearSales = (int)reader["lastYearSales"];
                manufacturerreport.overallSales = (int)reader["overallSales"];

                //add single report vairable into report list
                manufacturerreports.Add(manufacturerreport);
            }

            //append report list to viewbag for frontend reading
            ViewBag.SalesByManufacturerReport = manufacturerreports;



            return View();
        }


        public IActionResult GrossCustomerIncomeReport()
        {

            //open sql connection
            SqlCommand cmd;
            SqlConnection cnn;
            SqlDataReader reader;
            //init a report list
            var reports = new List<GrossIncomeReport>();

            cnn = ConnectionHelper.openConnection();
            
            var reportsql = "select top 15 * from [dbo].[Gross_Customer_Income_Report] ORDER BY Total_Income DESC, Latest_Service_Date DESC ";
            //sql command
            cmd = new SqlCommand(reportsql, cnn);
            //read resutlt
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                //put single row into report variable
                var report = new GrossIncomeReport();
                report.CustomerID = (int)reader["CustomerID"];
                report.CustomerName = (string)reader["Customer_Name"];
                report.FirstServiceDate = (DateTime)reader["First_Service_Date"];
                report.LatestServiceDate = (DateTime)reader["Latest_Service_Date"];
                report.NumOfRepair = (int)reader["NumOfRepair"];
                report.NumOfSale = (int)reader["NumOfSale"];
                report.TotalIncome = (decimal)reader["Total_Income"];
                //add single report vairable into report list
                reports.Add(report);
            }

            //append report list to viewbag for frontend reading
            ViewBag.GrossIncomeReport = reports;

            return View();
        }

        public IActionResult GrossOneCustomerIncomeReport(int CustomerID)
        {
            //open sql connection
            SqlCommand cmd;
            SqlConnection cnn;
            SqlDataReader reader;
            //init a report list
            var salesreports = new List<GrossOneIncomeSalesReport>();
            cnn = ConnectionHelper.openConnection();
            var salesreportsql =

"WITH VehicleCTE AS( " +
"SELECT v.VIN, description, model_year, model_name, invoice_price,manufacturer_name , " +
"inv_writer_user_name, add_date, 'Car' as vehicle_type " +
"FROM Vehicle v INNER JOIN Car c on v.VIN = c.VIN " +
"UNION " +
"SELECT v.VIN, description, model_year, model_name, invoice_price,manufacturer_name , " +
"inv_writer_user_name, add_date, 'Convertible' " +
"FROM Vehicle v INNER JOIN Convertible c on v.VIN = c.VIN " +
"UNION " +
"SELECT v.VIN, description, model_year, model_name, invoice_price,manufacturer_name , " +
"inv_writer_user_name, add_date, 'Truck' " +
"FROM Vehicle v INNER JOIN Truck t on v.VIN = t.VIN " +
"UNION " +
"SELECT v.VIN, description, model_year, model_name, invoice_price,manufacturer_name , " +
"inv_writer_user_name, add_date, 'Van' " +
"FROM Vehicle v INNER JOIN Van on v.VIN = Van.VIN " +
"UNION " +
"SELECT v.VIN, description, model_year, model_name, invoice_price,manufacturer_name , " +
"inv_writer_user_name, add_date, 'SUV' " +
"FROM Vehicle v INNER JOIN SUV s on v.VIN = s.VIN) " +
"SELECT S.transaction_date, S.sold_price, S.VIN, V.model_year, V.manufacturer_name, " +
"V.model_name, U.first_name, U.last_name " +
"FROM Customer AS C " +
"INNER JOIN SalesTransaction AS S ON C.customerID=S.customerID " +
"INNER JOIN VehicleCTE AS V ON S.VIN=V.VIN " +
"INNER JOIN Users AS U ON U.user_name=S.sales_writer_user_name " +
"WHERE C.customerID=@DisplayedCustomerID "+
"ORDER BY S.transaction_date DESC, S.VIN ASC;";

            //sql command
            cmd = new SqlCommand(salesreportsql, cnn);

            cmd.Parameters.AddWithValue("@DisplayedCustomerID", CustomerID);
            //read resutlt
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var salesreport = new GrossOneIncomeSalesReport();
                salesreport.TransactionDate = (DateTime)reader["transaction_date"];
                salesreport.SoldPrice = (Decimal)reader["sold_price"];
                salesreport.VIN = (string)reader["VIN"];
                salesreport.ModelYear = (int)reader["model_year"];
                salesreport.ManufacturerName = (string)reader["manufacturer_name"];
                salesreport.ModelName = (string)reader["model_name"];
                salesreport.FirstName = (string)reader["first_name"];
                salesreport.LastName = (string)reader["last_name"];
                //add single report vairable into report list
                salesreports.Add(salesreport);    
            }

            var repairreports = new List<GrossOneIncomeRepairReport>();
            var repairreportsql =

"SELECT R.start_date,R.complete_date, R.VIN, R.odometer_value, "+
"SUM(P.quantity * P.price)AS parts_cost, R.labor_charge,(SUM(P.quantity * P.price) + R.labor_charge) AS total_cost, U.first_name, U.last_name " +
"FROM RepairDetails AS R INNER JOIN( " +
"SELECT v.VIN, description, model_year, model_name, invoice_price, manufacturer_name, inv_writer_user_name, add_date, 'Car' as vehicle_type FROM Vehicle v INNER JOIN Car c on  v.VIN = c.VIN " +
"UNION SELECT v.VIN, description, model_year, model_name, invoice_price, manufacturer_name, inv_writer_user_name, add_date, 'Convertible' " +
"FROM Vehicle v INNER JOIN Convertible c on  v.VIN = c.VIN " +
"UNION SELECT v.VIN, description, model_year, model_name, invoice_price, manufacturer_name, inv_writer_user_name, add_date, 'Truck' " +
"FROM Vehicle v INNER JOIN Truck t on  v.VIN = t.VIN " +
"UNION SELECT v.VIN, description, model_year, model_name, invoice_price, manufacturer_name, inv_writer_user_name, add_date, 'Van' " +
"FROM Vehicle v INNER JOIN Van  on  v.VIN = Van.VIN " +
"UNION SELECT v.VIN, description, model_year, model_name, invoice_price, manufacturer_name, inv_writer_user_name, add_date, 'SUV' " +
"FROM Vehicle v INNER JOIN SUV s  on  v.VIN = s.VIN " +
") AS V ON R.VIN = V.VIN " +
"Left OUTER JOIN Part AS P ON P.VIN = R.VIN and r.start_date = p.start_date INNER JOIN Users AS U ON U.user_name = R.repair_starter " +
"WHERE(R.customerID = @DisplayedCustomerID) " +
"GROUP BY R.VIN, R.start_date, R.complete_date, R.odometer_value, R.labor_charge, U.first_name,U.last_name " +
"ORDER BY(CASE WHEN R.complete_date IS NULL THEN 0 ELSE 1 END), R.start_date DESC, R.complete_date DESC, R.VIN ASC " 
;

            //sql command
            cmd = new SqlCommand(repairreportsql, cnn);

            cmd.Parameters.AddWithValue("@DisplayedCustomerID", CustomerID);
            //read resutlt
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var repairreport = new GrossOneIncomeRepairReport();
                repairreport.start_date = (DateTime)reader["start_date"];

                if (reader["complete_date"] != DBNull.Value)
                {
                    repairreport.complete_date = (DateTime)reader["complete_date"];
                }
                else
                {
                    repairreport.complete_date = new DateTime(1111, 1, 1, 0, 0, 0);
                }

                repairreport.VIN = (string)reader["VIN"];
                repairreport.odometer_value = (int)reader["odometer_value"];

                if (reader["parts_cost"] != DBNull.Value){
                    repairreport.parts_cost = (decimal)reader["parts_cost"];
                    repairreport.labor_charge = (decimal)reader["labor_charge"];
                    repairreport.total_cost = (decimal)reader["total_cost"];
                }   
                else {
                    repairreport.parts_cost = 0 ;
                    repairreport.labor_charge = (decimal)reader["labor_charge"];
                    repairreport.total_cost = (decimal)reader["labor_charge"];
                }

                repairreport.FirstName = (string)reader["first_name"];
                repairreport.LastName = (string)reader["last_name"];
                //add single report vairable into report list
                repairreports.Add(repairreport);
            }

            var reportsql = "SELECT DISTINCT Customer_Name FROM Gross_Customer_Income_Report WHERE customerID=@DisplayedCustomerID;";
            //sql command
            cmd = new SqlCommand(reportsql, cnn);
            //read resutlt
            cmd.Parameters.AddWithValue("@DisplayedCustomerID", CustomerID);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ViewBag.CustomerName = (string)reader["Customer_Name"];
            }

            ViewBag.GrossOneIncomeSalesReport = salesreports;
            ViewBag.GrossOneIncomeRepairReport = repairreports;
            return View();
        }

        public IActionResult RepairReport()
        {
            //open sql connection
            SqlCommand cmd;
            SqlConnection cnn;
            SqlDataReader reader;
            //init a report list
            var manufacturerrepairreports = new List<ManufacturerRepairReport>();

            cnn = ConnectionHelper.openConnection();

            var reportsql = "SELECT * FROM Manufacturer_Repair_Report;";
            //sql command
            cmd = new SqlCommand(reportsql, cnn);
            //read resutlt
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                //put single row into report variable
                var manufacturerrepairreport = new ManufacturerRepairReport();

                manufacturerrepairreport.manufacturer = (string)reader["ManufacturerName"];
                manufacturerrepairreport.repair_count = (int)reader["RepairCount"];
                if (reader["TotalPartsCost"] != DBNull.Value)
                {
                    manufacturerrepairreport.total_parts_cost = (decimal)reader["TotalPartsCost"];
                }
                else
                {
                    manufacturerrepairreport.total_parts_cost = 0;
                }

                if (reader["TotalLaborCost"] != DBNull.Value)
                {
                    manufacturerrepairreport.total_labor_charge = (decimal)reader["TotalLaborCost"];
                }
                else
                {
                    manufacturerrepairreport.total_labor_charge = 0;
                }

                if (reader["TotalRepairCost"] != DBNull.Value)
                {
                    manufacturerrepairreport.total_repair_cost = (decimal)reader["TotalRepairCost"];
                }
                else
                {
                    manufacturerrepairreport.total_repair_cost = 0;
                }

                //add single report vairable into report list
                manufacturerrepairreports.Add(manufacturerrepairreport);
            }




            //append report list to viewbag for frontend reading
            ViewBag.ManufacturerRepairReport = manufacturerrepairreports;
            return View();
        }



        public IActionResult RepairTypeReport(string ManufacturerName)
        {
            //open sql connection
            SqlCommand cmd;
            SqlConnection cnn;
            SqlDataReader reader;
            //init a report list
            var typerepairreports = new List<TypeRepairReport>();

            //string tempManufacturerName = ManufacturerName;

            cnn = ConnectionHelper.openConnection();

            var reportsql =
"WITH VehicleCTE AS( "+
"SELECT v.VIN, description, model_year, model_name, invoice_price, manufacturer_name , inv_writer_user_name, add_date,  'Car' as vehicle_type " +
"FROM Vehicle v INNER JOIN Car c on  v.VIN = c.VIN " +
"UNION SELECT v.VIN, description, model_year, model_name, invoice_price,manufacturer_name , inv_writer_user_name, add_date, 'Convertible' " +
"FROM Vehicle v INNER JOIN Convertible c on  v.VIN = c.VIN " +
"UNION SELECT v.VIN, description, model_year, model_name, invoice_price,manufacturer_name , inv_writer_user_name, add_date, 'Truck' " +
"FROM Vehicle v INNER JOIN Truck t on  v.VIN = t.VIN " +
"UNION SELECT v.VIN, description, model_year, model_name, invoice_price,manufacturer_name , inv_writer_user_name, add_date, 'Van' " +
"FROM Vehicle v INNER JOIN Van  on v.VIN = Van.VIN " +
"UNION SELECT v.VIN, description, model_year, model_name, invoice_price,manufacturer_name , inv_writer_user_name, add_date, 'SUV' " +
"FROM Vehicle v INNER JOIN SUV s on  v.VIN = s.VIN) " +

"SELECT DISTINCT temp1.vehicle_type as VehicleType, temp2.repair_count as RepairCount, temp1.total_parts_cost as TotalPartsCost, temp2.total_labor_cost as TotalLaborCost,(temp1.total_parts_cost+temp2.total_labor_cost) as TotalRepairCost " +
"FROM( " +
"SELECT V.vehicle_type, SUM(ISNULL(P.quantity, 0 )*ISNULL(P.price, 0 )) AS total_parts_cost, SUM(ISNULL(R.labor_charge, 0 )) AS total_labor_cost, (SUM(ISNULL(P.quantity, 0 )*ISNULL(P.price, 0 ))+SUM(ISNULL(R.labor_charge, 0 ))) AS total_repair_cost " +
"FROM RepairDetails AS R " +
"INNER JOIN VehicleCTE as V ON V.VIN = R.VIN " +
"LEFT OUTER JOIN Part AS P ON P.VIN = R.VIN AND P.start_date=R.start_date " +
"RIGHT OUTER JOIN Manufacturer AS M ON M.name = V.manufacturer_name " +
"WHERE M.name = @DisplayedManufacturer " +
"GROUP BY V.vehicle_type) AS temp1 " +

"LEFT OUTER JOIN( " +
"SELECT temp.vehicle_type, COUNT(VIN) as repair_count, SUM(ISNULL(temp.labor_charge, 0 )) as total_labor_cost " +
"FROM( " +
"SELECT Distinct V.manufacturer_name, V.vehicle_type, V.VIN, R.start_date, R.labor_charge " +
"FROM RepairDetails AS R " +
"INNER JOIN VehicleCTE as V ON V.VIN = R.VIN " +
") AS temp WHERE temp.manufacturer_name = @DisplayedManufacturer " +
"GROUP BY temp.vehicle_type) AS temp2 ON temp1.vehicle_type = temp2.vehicle_type " +
"ORDER BY RepairCount DESC, temp1.vehicle_type;"
            ;

            //sql command
            cmd = new SqlCommand(reportsql, cnn);
            //read resutlt
            cmd.Parameters.AddWithValue("@DisplayedManufacturer", ManufacturerName);
            reader = cmd.ExecuteReader();

            if (reader.HasRows == true)
            {

                while (reader.Read())
                {
                    //put single row into report variable
                    var typerepairreport = new TypeRepairReport();

                    if (reader["VehicleType"] != DBNull.Value)
                    {
                        typerepairreport.vehicle_type = (string)reader["VehicleType"];
                    }
                    else
                    {
                    }

                    if (reader["RepairCount"] != DBNull.Value)
                    {
                        typerepairreport.repair_count = (int)reader["RepairCount"];
                    }
                    else
                    {
                        typerepairreport.repair_count = 0;
                    }

                    if (reader["TotalPartsCost"] != DBNull.Value)
                    {
                        typerepairreport.total_parts_cost = (decimal)reader["TotalPartsCost"];
                    }
                    else
                    {
                        typerepairreport.total_parts_cost = 0;
                    }

                    if (reader["TotalLaborCost"] != DBNull.Value)
                    {
                        typerepairreport.total_labor_charge = (decimal)reader["TotalLaborCost"];
                    }
                    else
                    {
                        typerepairreport.total_labor_charge = 0;
                    }

                    if (reader["TotalRepairCost"] != DBNull.Value)
                    {
                        typerepairreport.total_repair_cost = (decimal)reader["TotalRepairCost"];
                    }
                    else
                    {
                        typerepairreport.total_repair_cost = 0;
                    }

                    //add single report vairable into report list
                    typerepairreports.Add(typerepairreport);
                }

            }
           

            //append report list to viewbag for frontend reading
            ViewBag.TypeRepairReport = typerepairreports;
            ViewBag.tempManufacturerName = ManufacturerName;

            var manufacturewithmodels = new Dictionary<TypeRepairReport, List<ModelRepairReport> > ();
            foreach (var m in typerepairreports)
            {
                var type = m.vehicle_type;

                var modelreport = RepairModelReport(type, ManufacturerName);

                manufacturewithmodels.Add(m, modelreport);

            }


            ViewBag.manufacturewithmodels = manufacturewithmodels;








            return View();
        }


        public List<ModelRepairReport> RepairModelReport(string VehicleType, string tempManufacturerName)
        {
            //open sql connection
            SqlCommand cmd;
            SqlConnection cnn;
            SqlDataReader reader;
            //init a report list
            var modelrepairreports = new List<ModelRepairReport>();
            cnn = ConnectionHelper.openConnection();
            var reportsql =


"WITH VehicleCTE AS( "+
"SELECT v.VIN, description, model_year, model_name, invoice_price, manufacturer_name , inv_writer_user_name, add_date,  'Car' as vehicle_type " +
"FROM Vehicle v INNER JOIN Car c on  v.VIN = c.VIN " +
"UNION SELECT v.VIN, description, model_year, model_name, invoice_price,manufacturer_name , inv_writer_user_name, add_date, 'Convertible' " +
"FROM Vehicle v INNER JOIN Convertible c on  v.VIN = c.VIN " +
"UNION SELECT v.VIN, description, model_year, model_name, invoice_price,manufacturer_name , inv_writer_user_name, add_date, 'Truck' " +
"FROM Vehicle v INNER JOIN Truck t on  v.VIN = t.VIN " +
"UNION SELECT v.VIN, description, model_year, model_name, invoice_price,manufacturer_name , inv_writer_user_name, add_date, 'Van' " +
"FROM Vehicle v INNER JOIN Van  on v.VIN = Van.VIN " +
"UNION SELECT v.VIN, description, model_year, model_name, invoice_price,manufacturer_name , inv_writer_user_name, add_date, 'SUV' " +
"FROM Vehicle v INNER JOIN SUV s on  v.VIN = s.VIN) " +

"SELECT DISTINCT temp1.manufacturer_name AS ManufacturerName, temp1.vehicle_type as VehicleType, temp1.model_name as ModelName, temp2.repair_count as RepairCount, temp1.total_parts_cost as TotalPartsCost, temp2.total_labor_cost as TotalLaborCost,(temp1.total_parts_cost+temp2.total_labor_cost) as TotalRepairCost " +
"FROM( " +
"SELECT V.manufacturer_name, V.vehicle_type, V.model_name, SUM(ISNULL(P.quantity, 0 )*ISNULL(P.price, 0 )) AS total_parts_cost, SUM(ISNULL(R.labor_charge, 0 )) AS total_labor_cost, (SUM(ISNULL(P.quantity, 0 )*ISNULL(P.price, 0 ))+SUM(ISNULL(R.labor_charge, 0 ))) AS total_repair_cost " +
"FROM RepairDetails AS R " +
"INNER JOIN VehicleCTE as V ON V.VIN = R.VIN " +
"LEFT OUTER JOIN Part AS P ON P.VIN = R.VIN AND P.start_date=R.start_date  " +
"RIGHT OUTER JOIN Manufacturer AS M ON M.name = V.manufacturer_name " +
"WHERE M.name = @DisplayedManufacturer AND V.vehicle_type = @DisplayedType " +
"GROUP BY V.model_name, V.manufacturer_name, V.vehicle_type) AS temp1 " +

"LEFT OUTER JOIN( " +
"SELECT temp.model_name, COUNT(VIN) as repair_count, SUM(ISNULL(temp.labor_charge, 0 )) as total_labor_cost   " +
"FROM( " +
"SELECT Distinct V.model_name, V.manufacturer_name, V.vehicle_type, V.VIN, R.start_date, R.labor_charge   " +
"FROM RepairDetails AS R " +
"INNER JOIN VehicleCTE as V ON V.VIN = R.VIN " +
") AS temp WHERE temp.manufacturer_name = @DisplayedManufacturer AND temp.vehicle_type = @DisplayedType " +
"GROUP BY temp.model_name) AS temp2 ON temp1.model_name = temp2.model_name " +
"ORDER BY temp2.repair_count desc; "
;

            //sql command
            cmd = new SqlCommand(reportsql, cnn);
            //read resutlt

            cmd.Parameters.AddWithValue("@DisplayedType", VehicleType);
            cmd.Parameters.AddWithValue("@DisplayedManufacturer", tempManufacturerName);
            reader = cmd.ExecuteReader();

            if (reader.HasRows == true)
            {
                while (reader.Read())
                {
                    //put single row into report variable
                    var modelrepairreport = new ModelRepairReport();

                    if (reader["ManufacturerName"] != DBNull.Value)
                    {
                        modelrepairreport.manufacturer = (string)reader["ManufacturerName"];
                    }
                    else
                    {
                    }

                    if (reader["VehicleType"] != DBNull.Value)
                    {
                        modelrepairreport.vehicle_type = (string)reader["VehicleType"];
                    }
                    else
                    {
                    }

                    if (reader["ModelName"] != DBNull.Value)
                    {
                        modelrepairreport.model_name = (string)reader["ModelName"];
                    }
                    else
                    {
                    }

                    if (reader["RepairCount"] != DBNull.Value)
                    {
                        modelrepairreport.repair_count = (int)reader["RepairCount"];
                    }
                    else
                    {
                        modelrepairreport.repair_count = 0;
                    }

                    if (reader["TotalPartsCost"] != DBNull.Value)
                    {
                        modelrepairreport.total_parts_cost = (decimal)reader["TotalPartsCost"];
                    }
                    else
                    {
                        modelrepairreport.total_parts_cost = 0;
                    }

                    if (reader["TotalLaborCost"] != DBNull.Value)
                    {
                        modelrepairreport.total_labor_charge = (decimal)reader["TotalLaborCost"];
                    }
                    else
                    {
                        modelrepairreport.total_labor_charge = 0;
                    }

                    if (reader["TotalRepairCost"] != DBNull.Value)
                    {
                        modelrepairreport.total_repair_cost = (decimal)reader["TotalRepairCost"];
                    }
                    else
                    {
                        modelrepairreport.total_repair_cost = 0;
                    }

                    //add single report vairable into report list
                    modelrepairreports.Add(modelrepairreport);
                }
            }

            //append report list to viewbag for frontend reading
            ViewBag.ModelRepairReport = modelrepairreports;
            ViewBag.tempVehicleType = VehicleType;
            ViewBag.tempManufacturerNameforModel = tempManufacturerName;

            return modelrepairreports;
           // return View();
        }


        public IActionResult BelowCostSaleReport()
        {    //open sql connection
            SqlCommand cmd;
            SqlConnection cnn;
            SqlDataReader reader;
            //init a report list
            var belowCostSalesReports = new List<BelowCostSalesReport>();
            cnn = ConnectionHelper.openConnection();
            var belowCostSalesReportssql = "SELECT * FROM Below_Cost_Sales_Report;";


            //sql command
            cmd = new SqlCommand(belowCostSalesReportssql, cnn);

            //cmd.Parameters.AddWithValue("@DisplayedCustomerID", CustomerID);
            //read resutlt
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var belowCostSalesReport = new BelowCostSalesReport();
                belowCostSalesReport.customer_name = (string)reader["customer_name"];
                belowCostSalesReport.transaction_date = (DateTime)reader["transaction_date"];
                belowCostSalesReport.sold_price = (decimal)reader["sold_price"];
                belowCostSalesReport.model_year = (int)reader["model_year"];
                belowCostSalesReport.ratio = (decimal)reader["ratio"];//decimal.Round((decimal)reader["ratio"],2,MidpointRounding.AwayFromZero);
                belowCostSalesReport.model_name = (string)reader["model_name"];
                belowCostSalesReport.invoice_price = (decimal)reader["invoice_price"];
                belowCostSalesReport.first_name = (string)reader["first_name"];
                belowCostSalesReport.last_name = (string)reader["last_name"];
                belowCostSalesReport.manufacturer_name = (string)reader["manufacturer_name"];
                belowCostSalesReport.VIN = (string)reader["VIN"];

                //add single report vairable into report list
                belowCostSalesReports.Add(belowCostSalesReport);
            }
            ViewBag.BelowCostSalesReports = belowCostSalesReports;
            return View();
        }


        public IActionResult AverageInventoryTimeReport()
        {
            //open sql connection
            SqlCommand cmd;
            SqlConnection cnn;
            SqlDataReader reader;
            //init a report list
            var averageInventoryReports = new List<AverageInventoryTimeReport>();

            cnn = ConnectionHelper.openConnection();

            var reportsql = "SELECT * FROM Average_Time_Inventory_Report;";

            //sql command
            cmd = new SqlCommand(reportsql, cnn);
            //read resutlt
            reader = cmd.ExecuteReader();



            while (reader.Read())
            {
                //put single row into report variable
                var averageInventoryReport = new AverageInventoryTimeReport();
                averageInventoryReport.vehicle_type = (string)reader["vehicle_type"];
                if ((double)reader["avg_day_in_inventory"] != 0)
                {
                    var count = (double)reader["avg_day_in_inventory"];
                    averageInventoryReport.avg_day_in_inventory = count.ToString();
                }
                else
                {
                    averageInventoryReport.avg_day_in_inventory = "N/A";
                }



                //add single report vairable into report list
                averageInventoryReports.Add(averageInventoryReport);
            }

            //append report list to viewbag for frontend reading
            ViewBag.AverageInventoryTimeReports = averageInventoryReports;
            return View();

        }


        public IActionResult PartReport()
        {
            //open sql connection
            SqlCommand cmd;
            SqlConnection cnn;
            SqlDataReader reader;
            //init a report list
            var partReports = new List<Parts_Statics_Report>();

            cnn = ConnectionHelper.openConnection();

            var reportsql = "SELECT * FROM Parts_Statics_Report order by totalspent desc;";
            //sql command
            cmd = new SqlCommand(reportsql, cnn);
            //read resutlt
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                //put single row into report variable
                var partReport = new Parts_Statics_Report();
                partReport.totalpartssupplied = (int)reader["totalpartssupplied"];
                partReport.totalspent = (decimal)reader["totalspent"];
                partReport.vendor = (string)reader["vendor"];

                //add single report vairable into report list
                partReports.Add(partReport);
            }

            //append report list to viewbag for frontend reading
            ViewBag.part_report = partReports;
            return View();
        }


        public IActionResult MonthlySaleReport()
        {
            //open sql connection
            SqlCommand cmd;
            SqlConnection cnn;
            SqlDataReader reader;
            //init a report list
            var monthReports = new List<MonthlySalesReport>();

            cnn = ConnectionHelper.openConnection();


            var reportsql = "SELECT * FROM Monthly_Sales_Part1;";

            //sql command
            cmd = new SqlCommand(reportsql, cnn);
            //read resutlt
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                //put single row into report variable
                var monthReport = new MonthlySalesReport();

                monthReport.month = (int)reader["month"];
                monthReport.Ratio = decimal.Round((decimal)reader["Ratio"], 2, MidpointRounding.AwayFromZero);
                monthReport.SalesIncome = (decimal)reader["SalesIncome"];
                monthReport.totalNetIncome = (decimal)reader["totalNetIncome"];
                monthReport.totalVehicleSold = (int)reader["totalVehicleSold"];
                monthReport.year = (int)reader["year"];
                //add single report vairable into report list
                monthReports.Add(monthReport);
            }

            //append report list to viewbag for frontend reading
            ViewBag.month_report = monthReports;
            return View();


        }
        public IActionResult MonthlyTopSalesReport(int SaleMonth, int SaleYear)
        {
            //open sql connection
            SqlCommand cmd;
            SqlConnection cnn;
            SqlDataReader reader;
            //init a report list
            var monthTopReports = new List<MonthlyTopSalesReport>();

            cnn = ConnectionHelper.openConnection();


            var reportsql = "SELECT u.first_name, u.last_name, COUNT(VIN) AS TotalVehicleSold, SUM (SalesTransaction.sold_price) AS Sales " +
                "FROM SalesTransaction INNER JOIN Salespeople s " +
                "ON sales_writer_user_name = s.user_name " +
                "INNER JOIN Users u on s.user_name = u.user_name " +
                "WHERE YEAR(transaction_date) = @DisplayedYear and MONTH(transaction_date) = @DisplayedMonth " +
                "GROUP BY u.first_name, u.last_name " +
                "ORDER BY TotalVehicleSold DESC, Sales DESC;";

            //sql command
            cmd = new SqlCommand(reportsql, cnn);
            //read resutlt
            cmd.Parameters.AddWithValue("@DisplayedYear", SaleYear);
            cmd.Parameters.AddWithValue("@DisplayedMonth", SaleMonth);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                //put single row into report variable
                var monthTopReport = new MonthlyTopSalesReport();

                monthTopReport.first_Name = (string)reader["first_Name"];
                monthTopReport.last_Name = (string)reader["last_Name"];

                monthTopReport.Sales = (decimal)reader["Sales"];

                monthTopReport.totalVehicleSold = (int)reader["totalVehicleSold"];

                //add single report vairable into report list
                monthTopReports.Add(monthTopReport);
            }

            //append report list to viewbag for frontend reading
            ViewBag.month_Top_report = monthTopReports;
            ViewBag.TempMonth = SaleMonth;
            ViewBag.TempYear = SaleYear;

            return View();
        }



    }
}
