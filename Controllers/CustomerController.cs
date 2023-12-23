using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using team011.Helper;
using team011.Models;
using System.Diagnostics;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace team011.Controllers
{
    public class CustomerController : Controller
    {
        // GET: /<controller>/
        public IActionResult LookupCustomerForm()
        {

            //ViewBag.customer= "this is look up ustomer view";
            return View();
        }

        public JsonResult getCustomer(string customerID, bool isIndividual, string lastURL)
        {
            SqlCommand cmd;
            SqlConnection cnn;
            SqlDataReader reader;
            cnn = ConnectionHelper.openConnection();

            var customer = new Customer();
            customer.customerID = -1; //default
            if (isIndividual)
            {
                var driver_license = customerID;
                var sqlString = "SELECT * FROM Individual i,Customer c WHERE i.customerID = c.customerID AND driver_license = @driver_license";
                cmd = new SqlCommand(sqlString, cnn);
                cmd.Parameters.AddWithValue("@driver_license", driver_license);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    customer.customerID = (int)reader["customerID"];
                    customer.email = reader["email"].ToString();
                    customer.phone_number = reader["phone_number"].ToString();
                    //customer.add_by_user_name = reader["add_by_user_name"].ToString();
                    customer.street_address = reader["street_address"].ToString();
                    customer.city = reader["city"].ToString();
                    customer.state = reader["state"].ToString();
                    customer.postal_code = reader["postal_code"].ToString();
                    customer.driver_license = reader["driver_license"].ToString();
                    customer.first_name = reader["first_name"].ToString();
                    customer.last_name = reader["last_name"].ToString();
                    customer.customerName = reader["first_name"].ToString() + ", " + reader["last_name"].ToString();



                }
            }
            else
            {
                var tax_ID = customerID;
                var sqlString = "SELECT * FROM Business b,Customer c WHERE b.customerID = c.customerID AND tax_ID = @tax_ID";
                cmd = new SqlCommand(sqlString, cnn);
                cmd.Parameters.AddWithValue("@tax_ID", tax_ID);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    customer.customerID = (int)reader["customerID"];
                    customer.email = reader["email"].ToString();
                    customer.phone_number = reader["phone_number"].ToString();
                    //customer.add_by_user_name = reader["add_by_user_name"].ToString();
                    customer.street_address = reader["street_address"].ToString();
                    customer.city = reader["city"].ToString();
                    customer.state = reader["state"].ToString();
                    customer.postal_code = reader["postal_code"].ToString();
                    customer.tax_ID = reader["tax_ID"].ToString();
                    customer.contact_title = reader["contact_title"].ToString();
                    customer.contact_name = reader["contact_name"].ToString();
                    customer.business_name = reader["business_name"].ToString();
                    customer.customerName = customer.business_name;
                }
            }
            customer.isIndividual = isIndividual;

            return Json(customer);
        }

        public IActionResult AddCustomerForm()
        {
            return View();
        }




        [HttpPost]
        public JsonResult addCommon(bool isIndividual,
                    string email,
                    string phone_number,
                    string add_by_user_name,
                    string street_address,
                    string city,
                    string state,
                    string postal_code,
                    string driver_license,
                    string first_name,
                    string last_name,
                    string tax_ID,
                    string contact_title,
                    string contact_name,
                    string business_name)
        {


            SqlCommand cmd;
            SqlConnection cnn;
            SqlDataReader reader;
           

            Customer customer = new Customer();
            customer.isIndividual = isIndividual;
            customer.email = email;
            customer.phone_number = phone_number;
            customer.street_address = street_address;
            customer.city = city;
            customer.state = state;
            customer.postal_code = postal_code;
            customer.driver_license = driver_license;
            customer.first_name = first_name;
            customer.last_name = last_name;
            customer.tax_ID = tax_ID;
            customer.contact_title = contact_title;
            customer.contact_name = contact_name;
            customer.business_name = business_name;

            

            var sqlInsertCustomer = "INSERT INTO Customer (email,phone_number,street_address, city, state, postal_code) " +
                "VALUES (@email,@phone_number,@street_address,@city,@state,@postal_code)";
            var sqlInsertCustomerNoEmail = "INSERT INTO Customer (email,phone_number,street_address, city, state, postal_code) " +
                "VALUES (null,@phone_number,@street_address,@city,@state,@postal_code)";

            cnn = ConnectionHelper.openConnection();
            if (customer.email != null)
            {
                cmd = new SqlCommand(sqlInsertCustomer, cnn);
                cmd.Parameters.AddWithValue("@email", email);
            }
            else
            {
                cmd = new SqlCommand(sqlInsertCustomerNoEmail, cnn);

            }
            cmd.Parameters.AddWithValue("@phone_number", phone_number);
            cmd.Parameters.AddWithValue("@street_address", street_address);
            cmd.Parameters.AddWithValue("@city", city);
            cmd.Parameters.AddWithValue("@state", state);
            cmd.Parameters.AddWithValue("@postal_code", postal_code);
            cmd.ExecuteNonQuery();
            cmd.Dispose();



            var sqlGetCustomerID = "SELECT CustomerID FROM Customer WHERE phone_number = @phone_number " +
                " AND street_address = @street_address AND city = @city AND state = @state AND postal_code = @postal_code";

            cmd = new SqlCommand(sqlGetCustomerID, cnn);
            cmd.Parameters.AddWithValue("@phone_number", phone_number);
            cmd.Parameters.AddWithValue("@street_address", street_address);
            cmd.Parameters.AddWithValue("@city", city);
            cmd.Parameters.AddWithValue("@state", state);
            cmd.Parameters.AddWithValue("@postal_code", postal_code);

            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                customer.customerID = (int) reader["customerID"];   
            }

            var isAdded = 0;
           
            if (customer.isIndividual)
            {
                var sqlInsertIndividual = "INSERT INTO Individual (driver_license,first_name,last_name,customerID) " +
                "VALUES (@driver_license,@first_name,@last_name,@customerID)";
                cmd = new SqlCommand(sqlInsertIndividual, cnn);
                cmd.Parameters.AddWithValue("@driver_license", driver_license);
                cmd.Parameters.AddWithValue("@first_name", first_name);
                cmd.Parameters.AddWithValue("@last_name", last_name);
                cmd.Parameters.AddWithValue("@customerID", customer.customerID);
                try
                {
                    isAdded = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                }
                
                cmd.Dispose();

            }
            else
            {
                var sqlInsertIndividual = "INSERT INTO Business (tax_ID,contact_title,contact_name,business_name,customerID) " +
                "VALUES (@tax_ID,@contact_title,@contact_name,@business_name,@customerID)";
                cmd = new SqlCommand(sqlInsertIndividual, cnn);
                cmd.Parameters.AddWithValue("@tax_ID", tax_ID);
                cmd.Parameters.AddWithValue("@contact_title", contact_title);
                cmd.Parameters.AddWithValue("@contact_name", contact_name);
                cmd.Parameters.AddWithValue("@business_name", business_name);
                cmd.Parameters.AddWithValue("@customerID", customer.customerID);
                
                isAdded = cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            var success = false;
            if (isAdded == 0)
            {
                success = false;
            }
            else
            {
                success = true;
            }
            return Json(success);
        }

    }
}


