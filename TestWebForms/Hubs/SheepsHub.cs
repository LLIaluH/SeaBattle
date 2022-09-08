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
                currUser.Cells = Cells;
                Clients.Caller.iAmReady();
                var room = Container.Rooms.Find(x => x.User1 == currUser || x.User2 == currUser);
                if (room != null)
                {
                    if (room.User1 == currUser && room.User2 != null)
                    {
                        Clients.Client(room.User2.ConnectionId).setEnemyIsReady();
                        tryStartGame(room);
                    }
                    else if (room.User2 == currUser && room.User1 != null)
                    {
                        Clients.Client(room.User1.ConnectionId).setEnemyIsReady();
                        tryStartGame(room);
                    }
                }
            }
        }

        private void tryStartGame(TestWebForms.Models.Room room)
        {
            if (room != null && room.User1 != null && room.User2 != null)
            {
                if (room.User1.Ready && room.User2.Ready)
                {
                    Random r = new Random();
                    if (r.Next(0, 2) == 0)
                    {
                        room.Turn = 1;
                        Clients.Client(room.User1.ConnectionId).startGame("FirstStep");
                        Clients.Client(room.User2.ConnectionId).startGame("SecondStep");
                    }
                    else
                    {
                        room.Turn = 2;
                        Clients.Client(room.User1.ConnectionId).startGame("SecondStep");
                        Clients.Client(room.User2.ConnectionId).startGame("FirstStep");
                    }
                }
            }
        }

        public void Shot(int px, int py) 
        {
            var currUser = Storage.Container.Users.Find(x => x.ConnectionId == Context.ConnectionId);
            var room = Container.Rooms.Find(x => x.User1 == currUser || x.User2 == currUser);

            if (currUser == room.User1 && room.Turn == 1)
            {
                if (RoomControl.SearchCell(px, py, room.User2.Cells))//попал
                {
                    Clients.Caller.catchShot(2, px, py, 2);//Сообщить стреляющему о том, что он попал
                    Clients.Client(room.User2.ConnectionId).catchShot(1, px, py, 2);//сообщить оппоненту о попадании по его кораблю
                }
                else
                {
                    Clients.Caller.catchShot(2, px, py, 3);
                    Clients.Client(room.User2.ConnectionId).catchShot(1, px, py, 3);//сообщить оппоненту о промахе
                    SwitchTurn(room);
                }
            }
            else if (currUser == room.User2 && room.Turn == 2)
            {
                if (RoomControl.SearchCell(px, py, room.User1.Cells))//попал
                {
                    Clients.Caller.catchShot(2, px, py, 2);//Сообщить стреляющему о том, что он попал
                    Clients.Client(room.User1.ConnectionId).catchShot(1, px, py, 2);//сообщить оппоненту о попадании по его кораблю
                }
                else
                {
                    Clients.Caller.catchShot(2, px, py, 3);
                    Clients.Client(room.User1.ConnectionId).catchShot(1, px, py, 3);//сообщить оппоненту о промахе
                    SwitchTurn(room);
                }
            }
        }

        private void SwitchTurn(Models.Room room)
        {
            if (room.Turn == 2)
            {
                room.Turn = 1;
                Clients.Client(room.User1.ConnectionId).swichTurn(true);
                Clients.Client(room.User2.ConnectionId).swichTurn(false);
            }
            else
            {
                room.Turn = 2;
                Clients.Client(room.User1.ConnectionId).swichTurn(false);
                Clients.Client(room.User2.ConnectionId).swichTurn(true);
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
                            if (room.User1.Ready)
                            {
                                Clients.Caller.setEnemyIsReady();
                            }
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