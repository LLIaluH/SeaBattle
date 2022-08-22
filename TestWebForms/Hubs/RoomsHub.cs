using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestWebForms.App;
using TestWebForms.Hubs.Storage;
using TestWebForms.Models;

namespace TestWebForms.Hubs
{
    public class RoomsHub : Hub
    {

        public void CreateRoom(string nameNewRoom)
        {
            if (!Container.Rooms.Any(x => x.Name == nameNewRoom))
            {
                if (nameNewRoom.Length > 3 && nameNewRoom.Length <= 30)
                {
                    Container.Rooms.Add(new Models.Room(nameNewRoom));
                    SendRoomToAllExceptUser(Context.ConnectionId);
                    Connect(nameNewRoom);
                }
            }
            else
            {
                //Сказать, что комната с таким названием уже существует
                Clients.Caller.hasRoom();
            }         
        }

        public void Connect(string roomName)
        {
            var id = Context.ConnectionId;
            if (!Container.Users.Any(x => x.ConnectionId == id))
            {
                var newUser = new SheepsUser { ConnectionId = id, NameRoom = roomName, Ready = false };
                var room = Container.Rooms.Find(x => x.Name == roomName);
                Container.Users.Add(newUser);
                // Если вошёл первый игрок в комнату
                if (room.User1 == null)
                {
                    room.User1 = newUser;
                    Clients.Caller.onConnected(1, roomName);
                }
                else if (room.User1.ConnectionId != Context.ConnectionId) //если вошёл второй игрок
                {
                    Clients.User(room.User1.ConnectionId).enemyConnected();//сказать первому игроку что его соперник вошёл в комнату
                    Clients.Caller.onConnected(2, roomName);
                }
                SendRoomToAllExceptUser(Context.ConnectionId);
                // Посылаем сообщение всем пользователям, кроме текущего
                //Clients.AllExcept(id).onNewUserConnected(id, userName);
            }
        }

        /// <summary>
        /// Выдать всем пользователям новый список комнат, которые ожидают второго игрока
        /// </summary>
        public void SendRoomToAllExceptUser(string exceptId = null)
        {
            if (exceptId == null)
            {
                Clients.All.getingRooms(jsonListNames());
            }
            else
            {
                Clients.AllExcept(exceptId).getingRooms(jsonListNames());
            }            
        }

        /// <summary>
        /// Выдать пользователю список доступных комнат
        /// </summary>
        public void GetRooms()
        {
            Clients.Caller.getingRooms(jsonListNames());
        }

        private string jsonListNames()
        {
            dynamic result = new System.Dynamic.ExpandoObject();
            List<Models.Room> rooms = Container.Rooms.FindAll(x => x.User2 == null);
            var listNamesRooms = Support.ListRoomsToListNameRooms(rooms);
            result.GridsData = Support.ToDataTable(listNamesRooms, "Название комнаты");
            return App.Support.ConvertToJson(result);
        }
    }
}