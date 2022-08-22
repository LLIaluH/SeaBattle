using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace TestWebForms.App
{
    public class RoomControl
    {
        List<Cell> CellsChecked = new List<Cell>();

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
                //Error += SaveMap(Cells, NameRoom) ? "" : "Карта не была сохранена! ";
            }
            if (!(Error.Length > 1))
            {
                result.StartGame = true;
            }
            result.error = Error;
            return Support.ConvertToJson(result);
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

        private bool CheckCountCell(List<Cell> cells)
        {
            if (cells.Count != 20)
            {
                return true;
            }
            return false;
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
    }
}