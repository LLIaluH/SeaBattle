using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebForms.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public SheepsUser User1 { get; set; }
        public SheepsUser User2 { get; set; }
        public bool HasUser2 { get; set; }

        public Room(string nameNewRoom)
        {
            this.Name = nameNewRoom;
            //this.Id = hs nameNewRoom;
        }
    }
}