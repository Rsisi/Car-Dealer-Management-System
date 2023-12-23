using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
//using team011.Helper;

using System.Data.SqlClient;
using team011.Helper;
using team011.Models;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace team011.Controllers
{
    public class UserController : Controller
    {

        public IActionResult Index()
        {

            return View();


        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }
       
        //public JsonResult checkUserLogin(string username, string password)
        public JsonResult checkUserLogin(Users user)
        {
            SqlCommand cmd;
            SqlConnection cnn;
            SqlDataReader reader;
            var isLogedin = false;
            var loggedInUser = new Users();
            var userCTE = "WITH UserpermissionCTE AS"+
                "(SELECT user_name, 'ServiceWriter'as Userrole FROM ServiceWriter " +
                "UNION "+
                "SELECT user_name, 'SalesPeople' FROM SalesPeople " +
                "UNION "+
                "SELECT user_name, 'Owner' FROM Owner "+
                "UNION "+
                "SELECT user_name, 'Manager' FROM Manager " +
                "UNION "+
                "SELECT user_name, 'InventoryClerk' FROM InventoryClerk)";
            var selectsql = "select u.user_name, u.password, u.first_name, u.last_name, cte.Userrole" +
                " from UserpermissionCTE cte INNER JOIN [dbo].[Users] u on cte.[user_name] = u.[user_name] " +
                "WHERE u.user_name = @username AND u.password = @password";
            var sql = userCTE + selectsql;

            cnn = ConnectionHelper.openConnection();
            cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@username", user.user_name);
            cmd.Parameters.AddWithValue("@password", user.password);
            //cmd.Parameters.AddWithValue("@username", username);
            //cmd.Parameters.AddWithValue("@password", password);
            reader = cmd.ExecuteReader();
            var count = 0;
            while (reader.Read())
            {
                count++;

                isLogedin = true;

                loggedInUser.first_name = reader["first_name"].ToString();
                loggedInUser.last_name = reader["last_name"].ToString();
                loggedInUser.user_name = reader["user_name"].ToString();
                loggedInUser.user_role = reader["Userrole"].ToString();

            }
            ConnectionHelper.closeConnection(cnn, cmd, reader);
            if (isLogedin)
            {
                //if count = 5 then the role should be Rolando
                if (count == 5)
                {
                    HttpContext.Session.SetString("loggedUserRole", "Owner");

                }
                else
                {
                    HttpContext.Session.SetString("loggedUserRole", loggedInUser.user_role);
                }
                HttpContext.Session.SetString("loggedUserFullName", loggedInUser.first_name + " " + loggedInUser.last_name);
                HttpContext.Session.SetString("loggedUserName", loggedInUser.user_name);



                return Json(new { success = isLogedin, loggedInUser = loggedInUser });


            }
            else
            {
                return Json(new { success = isLogedin});
            }
           // HttpContext.Session.SetString("loggedUserUserName", loggedInUser.user_name);


        }
    }
}
