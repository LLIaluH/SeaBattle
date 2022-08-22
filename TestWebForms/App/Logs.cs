using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace TestWebForms.App
{
    public class Logs
    {
        const string ErrorLogPath = "ErrorLog.txt";
        //const string SampleLogPath = "SampleLog.txt";
        static string path = HttpContext.Current.Server.MapPath("~/LogFiles/");

        public static void LogWriteError(string exText)
        {
            string date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            try
            {
                using (StreamWriter sw = new StreamWriter(path + ErrorLogPath, true, System.Text.Encoding.Default))
                {
                    sw.WriteLine(date + " | " + exText + "\n");
                }
                Console.WriteLine("Запись выполнена");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}