using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestWebForms.Models;
using TestWebForms.Hubs.Storage;
using TestWebForms.App;

namespace TestWebForms.Hubs
{
    public class SheepsHub : Hub
    {

        public void TryReady(string MyFleet)
        {
            var currUser = Storage.Container.Users.Find(x => x.ConnectionId == Context.ConnectionId);
            if (currUser.Ready)            
                return;
            
            RoomControl roomControl = new RoomControl();
            List<Cell> Cells = RoomControl.ConvertObjToList(MyFleet);
            Cells = RoomControl.SortCells(Cells);
            var errors = roomControl.CheckMyMap(Cells);
            if (errors.Length > 0)
            {
                Clients.Caller.sendErrorMap(errors);
            }
            else
            {
                currUser.Ready = true;
                Clients.Caller.iAmReady();
                var room = Container.Rooms.Find(x => x.User1 == currUser || x.User2 == currUser);
                if (room != null)
                {
                    if (room.User1 == currUser && room.User2 != null)
                    {
                        Clients.Client(room.User2.ConnectionId).setEnemyIsReady();
                    }
                    else if (room.User2 == currUser && room.User1 != null)
                    {
                        Clients.Client(room.User1.ConnectionId).setEnemyIsReady();
                    }
                }
            }
        }

        public void Connect(string roomName, string oldUserId)
        {
            //изменяем старый id подключения пользователя на новый из-за смены хаба
            var room = Storage.Container.Rooms.Find(x => x.Name == roomName);
            if (room != null)
            {
                if (room.User1?.ConnectionId == oldUserId || room.User2?.ConnectionId == oldUserId)
                {
                    var user = Storage.Container.Users.Find(x => x.ConnectionId == oldUserId);
                    user.ConnectionId = Context.ConnectionId;
                    if (room.User2 != null)
                    {
                        if ((bool)(room.User2.ConnectionId.Equals(user.ConnectionId)))
                        {
                            Clients.Client(room.User1.ConnectionId).enemyConnected(room.User1.Ready);
                            Clients.Caller.setEnemyIsReady();
                        }
                    }
                }
                else
                {
                    Clients.Caller.outRoomError("Произошла ошибка! Эта комната уже занята!");
                }
            }
            else
            {
                Clients.Caller.outRoomError("Произошла ошибка! Этой комнаты не существует!");
            }
        }

        // Отключение пользователя
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var leaver = Container.Users.Find(x => x.ConnectionId == Context.ConnectionId);
            if (leaver != null)
            {
                var room = Container.Rooms.Find(x => x.User1 == leaver || x.User2 == leaver);
                if (room != null)
                {
                    if (room.User2 != null && room.User1?.ConnectionId == Context.ConnectionId)
                    {
                        Clients.Client(room.User2.ConnectionId).outRoomError("Ваш соперник вышел!");
                    }
                    else if (room.User1 != null && room.User2?.ConnectionId == Context.ConnectionId)
                    {
                        Clients.Client(room.User1.ConnectionId).outRoomError("Ваш соперник вышел!");
                    }
                    Container.Users.Remove(room.User1);
                    Container.Users.Remove(room.User2);
                    Container.Rooms.Remove(room);
                }
            }
            return base.OnDisconnected(stopCalled);
        }
    }
}