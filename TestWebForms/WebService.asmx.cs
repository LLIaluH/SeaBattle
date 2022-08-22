using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;
using TestWebForms.App;
using System.Data.SqlClient;

namespace TestWebForms
{
    /// <summary>
    /// Сводное описание для WebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Чтобы разрешить вызывать веб-службу из скрипта с помощью ASP.NET AJAX, раскомментируйте следующую строку. 
    [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {
        List<Cell> CellsChecked = new List<Cell>();
        SqlConnection conn;
        SqlCommand cmd;

        private bool GetCon()
        {
            return dbConnection.GetConnection(out conn);
        }
        private static string ConvertToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        private static DateTime StingToDateTime(string Date)
        {
            DateTime date = new DateTime();
            try
            {
                if (Date == "")
                    date = DateTime.Now;
                else
                    date = DateTime.ParseExact(Date, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
            }
            catch (Exception ex)
            {
                Logs.LogWriteError(ex.Message.ToString());
            }
            return date;
        }
        private bool CheckDiagonal(List<Cell> cells)
        {
            foreach (var c1 in cells)
            {
                foreach (var c2 in cells)
                {
                    if (c1.pX == c2.pX + 1 && c1.pY == c2.pY - 1)
                    {
                        return true;
                    }
                    if (c1.pX == c2.pX - 1 && c1.pY == c2.pY + 1)
                    {
                        return true;
                    }
                    if (c1.pX == c2.pX + 1 && c1.pY == c2.pY + 1)
                    {
                        return true;
                    }
                    if (c1.pX == c2.pX - 1 && c1.pY == c2.pY - 1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private List<Cell> SortCells(List<Cell> cells)
        {
            List<Cell> sortedCells = cells;
            for (int i = 0; i < sortedCells.Count; i++)
            {
                for (int j = 0; j < sortedCells.Count; j++)
                {
                    if (sortedCells[i] == sortedCells[j])
                    {
                        continue;
                    }
                    if (sortedCells[i] < sortedCells[j])
                    {
                        Cell tempCell = sortedCells[j];
                        sortedCells[j] = sortedCells[i];
                        sortedCells[i] = tempCell;
                    }
                }
            }
            return sortedCells;
        }
        private bool CheckCountShipOnType(List<Cell> cells)
        {
            int four = 0;
            int three = 0;
            int two = 0;
            int one = 0;
            foreach (var c1 in cells)
            {
                foreach (var c2 in cells)
                {
                    int countDeck = 0;

                    if (c1 > c2)//одна ячейка
                    {
                        continue;
                    }
                    if (c1.pX + 1 == c2.pX && c1.pY == c2.pY)
                    {
                        countDeck = CheckCellsAnDir(cells, c1);
                        CellsChecked.Add(c1);
                    }
                    else if (c1.pX == c2.pX && c1.pY + 1 == c2.pY)
                    {
                        countDeck = CheckCellsAnDir(cells, c1, "down");
                        CellsChecked.Add(c1);
                    }

                    else if (!HasNextCell(cells, c1))
                    {
                        bool HasChecked = false;
                        foreach (var ch in CellsChecked)//если ячейчка уже считалась как часть корабля, то второй раз её считать не нужно
                        {
                            if (ch == c1)
                            {
                                HasChecked = true;
                                break;
                            }
                        }
                        if (!HasChecked)
                        {
                            CellsChecked.Add(c1);
                            one++;
                        }
                    }

                    switch (countDeck)//помимо первой палубы
                    {
                        case 1:
                            two++;
                            break;
                        case 2:
                            three++;
                            break;
                        case 3:
                            four++;
                            break;
                    }
                }

            }
            if (four == 1 && three == 2 && two == 3 && one == 4)
            {
                return false;
            }
            return true;
        }
        private bool HasNextCell(List<Cell> cells, Cell C)
        {
            foreach (var c2 in cells)
            {
                if (Cell.IsNextDown(C, c2, 1) || Cell.IsNextRigth(C, c2, 1))
                {
                    return true;
                }
            }
            return false;
        }
        private int CheckCellsAnDir(List<Cell> cells, Cell cell, string dir = "rigth")
        {

            foreach (var ch in CellsChecked)//если ячейчка уже считалась как часть корабля, то второй раз её считать не нужно
            {
                if (ch == cell)
                {
                    return 0;
                }
            }
            int countDeck = 0;
            for (int i = 1; i < 4; i++)
            {
                foreach (var item in cells)
                {
                    if (cell == item || cell > item)//одна ячейка
                    {
                        continue;
                    }
                    if (dir == "rigth")
                    {
                        if (Cell.IsNextRigth(cell, item, i))
                        {
                            CellsChecked.Add(item);
                            countDeck++;
                            break;
                        }
                    }
                    else
                    {
                        if (Cell.IsNextDown(cell, item, i))
                        {
                            CellsChecked.Add(item);
                            countDeck++;
                            break;
                        }
                    }
                }
                if (countDeck != i)
                {
                    break;
                }
            }
            return countDeck;
        }
        private List<Cell> ConvertObjToList(object MyFleet)
        {
            List<Cell> Cells = new List<Cell>();
            Regex regex = new Regex(@"\d+", RegexOptions.Compiled);
            try
            {
                IEnumerable enumerable1 = MyFleet as IEnumerable;
                if (enumerable1 != null)
                {
                    int id = 0;
                    foreach (object cell in enumerable1)
                    {
                        Cell c = new Cell();
                        IEnumerable enumerable2 = cell as IEnumerable;
                        foreach (var property in enumerable2)
                        {
                            string prop = property.ToString();
                            var tempStringValue = regex.Match(prop);
                            int value = Convert.ToInt32(tempStringValue.Value);
                            if (prop.Contains("pX"))
                            {
                                c.pX = value;
                            }
                            if (prop.Contains("pY"))
                            {
                                c.pY = value;
                            }
                            if (prop.Contains("TypeC"))
                            {
                                c.TypeC = value;
                            }
                        }
                        c.id = id;
                        Cells.Add(c);
                        id++;
                    }
                }
                return Cells;
            }
            catch (Exception ex)
            {
                Logs.LogWriteError(ex.Message);
                throw;
            }
        }
        private bool SaveMap(List<Cell> cells, string NameRoom)
        {
            return SaveMapInTxt(cells, NameRoom);
        }

        private bool SaveMapInDB(List<Cell> cells, string NameRoom)
        {
            if (GetCon())
            {
                try
                {
                    string json = JsonConvert.SerializeObject(cells.ToArray());
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "";//
                    return true;
                }
                catch (Exception ex) { Logs.LogWriteError("Ошибка сохранения карты. " + ex.Message); }
            }
            return false;
        }

        private bool SaveMapInTxt(List<Cell> cells, string NameRoom)
        {
            try
            {
                string json = JsonConvert.SerializeObject(cells.ToArray());
                string path = HttpContext.Current.Server.MapPath("~/App_Data/Rooms/" + NameRoom + "/");
                System.IO.File.WriteAllText(path + NameRoom + @".txt", json);
                return true;
            }
            catch (Exception ex) { Logs.LogWriteError("Ошибка сохранения карты. " + ex.Message); }
            return false;
        }

        private bool CheckCountCell(List<Cell> cells)
        {
            if (cells.Count != 20)
            {
                return true;
            }
            return false;
        }

        [WebMethod]
        public string SendMyMap(object MyFleet, string NameRoom)
        {
            dynamic result = new System.Dynamic.ExpandoObject();

            List<Cell> Cells = ConvertObjToList(MyFleet);
            Cells = SortCells(Cells);
            string Error;
            Error = CheckCountCell(Cells) ? "Неверное количество палуб! " : "";
            Error += CheckDiagonal(Cells) ? "Неверно заполнено поле! " : "";
            Error += CheckCountShipOnType(Cells) ? "Неверно заполнено поле! " : "";            
            if (!(Error.Length > 1))
            {
                Error += SaveMap(Cells, NameRoom) ? "" : "Карта не была сохранена! ";
            }
            if (!(Error.Length > 1))
            {
                result.StartGame = true;
            }
            result.error = Error;
            return ConvertToJson(result);
        }

        #region Rooms       

        [WebMethod]
        public string GetRooms()
        {
            dynamic result = new System.Dynamic.ExpandoObject();
            string path = HttpContext.Current.Server.MapPath("~/App_Data/");
            try
            {
                string RoomsText = System.IO.File.ReadAllText(path + @"Rooms.txt");
                RoomList Rooms_ = JsonConvert.DeserializeObject<RoomList>(RoomsText);
                if (Rooms_ != null)
                {
                    List<string> returnedRoom = new List<string>();
                    foreach (var r in Rooms_.rooms)
                    {
                        if (r.HasUser2 != true)
                        {
                            returnedRoom.Add(r.Name );
                        }
                    }
                    result.GridsData = Support.ToDataTable(returnedRoom, "Название комнаты");
                } 
                else
                {
                    result.error = "Нет комнат!";
                }
            }
            catch (Exception ex)
            {
                Logs.LogWriteError("Не удалось прочитать файл Rooms.txt " + ex.Message);
                result.error = "Не удалось прочитать файл!";
            }
            return ConvertToJson(result);
        }

        [WebMethod]
        public string CreateNewRoom(string NameRoom)
        {
            dynamic result = new System.Dynamic.ExpandoObject();

            string path = HttpContext.Current.Server.MapPath("~/App_Data/");
            string RoomsText = "";
            try
            {
                RoomsText = System.IO.File.ReadAllText(path + @"Rooms.txt");
            }
            catch (Exception ex)
            { 
                Logs.LogWriteError("Не удалось прочитать файл Rooms.txt " + ex.Message); 
            }

            Regex regex = new Regex("\"" + NameRoom + "\"");

            var tempStringValue = regex.Match(RoomsText);
            if (!String.IsNullOrEmpty(NameRoom) && !(tempStringValue.Length > 0))
            {
                RoomList Rooms_;
                int count = 0;
                try
                {
                    Rooms_ = JsonConvert.DeserializeObject<RoomList>(RoomsText);
                    count = Rooms_.rooms.Count;
                }
                catch { Rooms_ = new RoomList(); }

                Room newRoom = new Room(count, NameRoom);
                Rooms_.Add(newRoom);
                System.IO.File.WriteAllText(path + @"Rooms.txt", JsonConvert.SerializeObject(Rooms_));                
            }
            else
            {
                result.error = "Не удалость создать комнату! Попробуйте ввести другое название.";
            }
            return ConvertToJson(result);
        }

        public string ConnectToRoom(string NameRoom)
        {
            dynamic result = new System.Dynamic.ExpandoObject();
            string path = HttpContext.Current.Server.MapPath("~/App_Data/Rooms/");
            string RoomText = "";
            try
            {
                RoomText = System.IO.File.ReadAllText(path + NameRoom + @".txt");
            }
            catch (Exception ex)
            {
                Logs.LogWriteError("Не удалось прочитать файл Rooms.txt " + ex.Message);
            }
            if (RoomText.Contains(NameRoom))
            {
                var Rooms_ = JsonConvert.DeserializeObject(RoomText);
                List<Room> rooms = new List<Room>();
                IEnumerable enumerable1 = Rooms_ as IEnumerable;
                if (enumerable1 != null)
                {
                    foreach (var item in enumerable1)
                    {
                        Room tempItem = (Room)item;
                        if (tempItem.Name == NameRoom)
                        {
                            tempItem.HasUser2 = true;
                        }
                        rooms.Add(tempItem);
                    }
                }
            }
            return ConvertToJson(result);
        }

        [WebMethod]
        public string ConnectToRoom(string NameRoom, bool wtf = true)
        {
            dynamic result = new System.Dynamic.ExpandoObject();
            string path = HttpContext.Current.Server.MapPath("~/App_Data/");
            string RoomsText = "";
            try
            {
                RoomsText = System.IO.File.ReadAllText(path + @"Rooms.txt");
            }
            catch (Exception ex)
            { 
                Logs.LogWriteError("Не удалось прочитать файл Rooms.txt " + ex.Message); 
            }
            if (RoomsText.Contains(NameRoom))
            {
                RoomList Rooms_ = JsonConvert.DeserializeObject<RoomList>(RoomsText);
                result.CanConnectToGame = false;
                if (!(Rooms_.rooms.FirstOrDefault(r_ => r_.Name == NameRoom)).HasUser2)
                {
                    Rooms_.rooms.FirstOrDefault(r_ => r_.Name == NameRoom).HasUser2 = true;
                    System.IO.File.WriteAllText(path + @"Rooms.txt", JsonConvert.SerializeObject(Rooms_));
                    result.CanConnectToGame = true;
                }
            }
            return ConvertToJson(result);
        }
        #endregion
    }
}