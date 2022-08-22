using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebForms.App
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public string NameUser1;
        public bool HasUser2 { get; set; }
        //private short[,] map1;//пользователь создавший комнату
        //private short[,] map2;//пользователь вошедший в комнату

        public Room(int id, string Name, bool HasUser2 = false)
        {
            this.Id = id;
            this.Name = Name;
            //this.NameUser1 = NameUser1;
            this.HasUser2 = HasUser2;
        }
    }

    public class RoomList
    {
        public List<Room> rooms;
        public RoomList(List<Room> List)
        {
            this.rooms = List;
        }

        public RoomList(Room room)
        {
            List<Room> rooms = new List<Room>();
            rooms.Add(room);
            this.rooms = rooms;
        }

        public RoomList()
        {

        }

        public void Add(Room r)
        {
            if (this.rooms != null)
            {
                this.rooms.Add(r);
            }
            else
            {
                this.rooms = new List<Room>();
                this.rooms.Add(r);
            }

        }
    }
}