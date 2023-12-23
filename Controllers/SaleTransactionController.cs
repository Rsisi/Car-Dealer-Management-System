using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using team011.Helper;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace team011.Controllers
{
    public class SaleTransactionController : Controller
    {
        // GET: /<controller>/
        public IActionResult CreateTransactionForm(string VIN )
        {
            ViewBag.VIN = VIN;

            SqlCommand cmd;
            SqlConnection cnn;
            SqlDataReader reader;
            cnn = ConnectionHelper.openConnection();

            
            var invoice_pricesql = "SELECT invoice_price FROM Vehicle WHERE Vehicle.VIN = @vin ";
            cmd = new SqlCommand(invoice_pricesql, cnn);
            cmd.Parameters.AddWithValue("@vin", VIN);
            reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    ViewBag.invoice_price = (decimal)reader["invoice_price"];
                }
            }



            
            ViewBag.customerID = 1;
            ViewBag.customer_name = "Customer not selected yet";
            return View();
        }

        public JsonResult CreateTransaction(string vin, int customerID, string salespeople, string sold_price, string sold_date)
        {
            SqlCommand cmd;
            SqlConnection cnn;

            cnn = ConnectionHelper.openConnection();
            var createTransactionSql = "INSERT INTO SalesTransaction Values( @vin, @customerID, @salespeople, @sold_price, @sold_date) ";
            //sql command
            cmd = new SqlCommand(createTransactionSql, cnn);

            cmd.Parameters.AddWithValue("@vin", vin);
            cmd.Parameters.AddWithValue("@customerID", customerID);
            cmd.Parameters.AddWithValue("@salespeople", salespeople);
            cmd.Parameters.AddWithValue("@sold_price", float.Parse(sold_price));
            cmd.Parameters.AddWithValue("@sold_date", DateTime.Parse(sold_date));
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            cnn.Close();

            //return Json(new { Url = "~/Views/Vehicle/VehicleDetailForm.cshtml" });
            return Json(true);
        }



    }



}
