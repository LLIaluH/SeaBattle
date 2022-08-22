using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace TestWebForms.App
{
    public class dbConnection
    {
        public static bool GetConnection(out SqlConnection conn)
        {
            string dir = Directory.GetCurrentDirectory();

            string connectionString = @"Data Source = (LocalDB)\MSSQLLocalDB;AttachDbFilename=" + dir + @"\App_Data\DB.mdf;Integrated Security=True; Connect Timeout = 30";

            //const string connectionString = @"C:\Users\MIKHEEV_AV1\source\repos\Polyclinic\Database1.mdf";
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                Console.WriteLine("Подключение открыто");
                return true;
            }
            catch (SqlException ex)
            {
                Logs.LogWriteError(ex.Message);
            }
            conn = null;
            return false;
        }
    }
}