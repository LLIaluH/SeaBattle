using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestWebForms.Models;

namespace TestWebForms.Hubs.Storage
{
    public static class Container
    {
        public static List<Room> Rooms = new List<Room>();
        public static List<SheepsUser> Users = new List<SheepsUser>();
    }
}