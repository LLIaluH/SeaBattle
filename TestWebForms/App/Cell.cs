using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebForms.App
{
    public class Cell
    {
        public int id;
        public int pX;
        public int pY;
        public int TypeC;
        public Cell(int X, int Y, int type, int id)
        {
            this.id = id;
            this.pX = X;
            this.pY = Y;
            this.TypeC = type;
        }

        public Cell()
        {

        }

        public static bool operator ==(Cell c1, Cell c2)
        {
            if (object.ReferenceEquals(c1, null))
            {
                return object.ReferenceEquals(c2, null);
            }
            return (c1.id == c2.id && c1.pX == c2.pX && c1.pY == c2.pY);
        }

        public static bool operator !=(Cell c1, Cell c2)
        {
            if (object.ReferenceEquals(c1, null))
            {
                return object.ReferenceEquals(c2, null);
            }
            return (c1.id != c2.id || c1.pX != c2.pX || c1.pY != c2.pY);
        }

        public static bool operator <(Cell c1, Cell c2)
        {
            return (c1.pX < c2.pX && c1.pY <= c2.pY) || (c1.pX <= c2.pX && c1.pY < c2.pY);
        }

        public static bool operator >(Cell c1, Cell c2)
        {
            return (c1.pX > c2.pX && c1.pY >= c2.pY) || (c1.pX >= c2.pX && c1.pY > c2.pY);
        }

        public static bool IsNextRigth(Cell c1, Cell c2, int count)
        {
            if (c1.pY == c2.pY && c1.pX + count == c2.pX)
            {
                return true;
            }
            return false;
        }

        public static bool IsNextDown(Cell c1, Cell c2, int count)
        {
            if (c1.pY + count == c2.pY && c1.pX == c2.pX)
            {
                return true;
            }
            return false;
        }
    }
}