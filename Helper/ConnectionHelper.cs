using System;
using System.Data.SqlClient;

namespace team011.Helper
{
    public static class ConnectionHelper
    {
       
        public static SqlConnection openConnection()
        {
            var connetionString = "Server = localhost,1433\\Catalog = Jaunty_Jalopies; Database = Jaunty_Jalopies; User = sa; Password = <YourStrong@Passw0rd>; MultipleActiveResultSets = true";
            SqlConnection cnn;
            cnn = new SqlConnection(connetionString);
            cnn.Open();
            return cnn;



        }
        public  static void closeConnection(SqlConnection cnn,SqlCommand cmd, SqlDataReader reader)
        {

            reader.Close();
            cmd.Dispose();
            cnn.Close();
        }
       
    }
}
