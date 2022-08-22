using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;

namespace TestWebForms.App
{
    public static class Support
    {        
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        public static DataTable ToDataTable<T>(List<T> data, string Name)
        {
            DataTable table = new DataTable();
            table.Columns.Add(Name);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                row[Name] = item;
                table.Rows.Add(row);
            }
            return table;
        }

        public static string ConvertToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static List<string> ListRoomsToListNameRooms(List<Models.Room> rooms)
        {

            List<string> returnedRoom = new List<string>();
            foreach (var r in rooms)
            {
                returnedRoom.Add(r.Name);                
            }
            return returnedRoom;
        }
    }
}