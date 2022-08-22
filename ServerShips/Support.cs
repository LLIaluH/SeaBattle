using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace ServerShips
{
    static class Support
    {
        public static void LogWrite(string str)
        {
            Console.WriteLine(DateTime.Now.ToString(new CultureInfo("ru-RU")) + "\t" + str);
        }
    }

    public static class MemoryStreamExtensions
    {
        public static void Append(this MemoryStream stream, byte value)
        {
            stream.Append(new[] { value });
        }

        public static void Append(this MemoryStream stream, byte[] values)
        {
            stream.Write(values, 0, values.Length);
        }
    }
}
