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

        public string CheckMyMap(List<Cell> Cells)
        {
            string error = "";
            error = CheckCountCell(Cells) ? "Неверное количество палуб!\n" : "";
            error += CheckDiagonal(Cells) ? "Корабли не могут стоять вплотную!\n" : "";
            string configurationFleet = "";
            error += CheckCountShipOnType(Cells, out configurationFleet) ? "Должно быть 4 однопалубника, 3 двухпалубника, 2 трёхпалубника и 1 четырёхпалубник!\n" +
                "А у вас: " + configurationFleet : "";            
            return error;
        }

        public static List<Cell> ConvertObjToList(string MyFleet)
        {
            return JsonConvert.DeserializeObject<List<Cell>>(MyFleet);
        }

        public static List<Cell> SortCells(List<Cell> cells)
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

        private bool CheckCountShipOnType(List<Cell> cells, out string conf)
        {
            conf = "";
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
            conf = $"{one} одно, {two} двух, {three} трёх и {four} четырёх... палубники";
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

        public static bool SearchCell(int x, int y, List<Cell> cells)
        {
            //var cell = cells.Find(c => c.pX == x && c.pY == y && c.TypeC == 1);
            //if (cell != null)
            //{
            //    cell.TypeC = 2;
            //    return true;
            //}
            foreach (var cell in cells)
            {
                if (cell.pX == x && cell.pY == y && cell.TypeC == 1)
                {
                    cell.TypeC = 2;
                    return true;
                }
            }
            return false;
        }

        public static bool CheckWin(List<Cell> cells)
        {
            var cell = cells.FindAll(c => c.TypeC == 2);
            if (cell != null && cell.Count == 20)
            {
                return true;
            }
            return false;
        }
    }
}