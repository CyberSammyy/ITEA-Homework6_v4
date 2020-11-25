using System;

namespace ITEA_Homework6_v4
{
    public class Cell
    {
        private char initValue;
        private char cellValue;

        public char CellValue
        {
            get { return cellValue; }
            set { cellValue = value; }
        }

        public bool IsPlayerKeeper { get; set; }
        public bool IsEnemyKeeper { get; set; }
        public bool CanMove { get; }

        public int InMapPositionX { get; }
        public int InMapPositionY { get; }

        public Cell(int x, int y, Random rand)
        {
            InMapPositionX = x;
            InMapPositionY = y;

            char element = ' ';
            if (x == 1 && y == 1)
            {
                CanMove = true;
            }
            else
            {
                int randValue = rand.Next(0, 3);
                element = randValue == 1 ? '#' : randValue == 2 ? '@' : ' ';
                CanMove = randValue != 1;
            }

            initValue = element;
            CellValue = element;
        }
        public Cell(int x, int y, char symbol, Enemy enemy)
        {
            InMapPositionX = x;
            InMapPositionY = y;
            if (x == 1 && y == 1)
            {
                CanMove = true;
            }
            else
            {
                if (symbol == '#') CanMove = false;
                else CanMove = true;
            }
            initValue = symbol;
            CellValue = symbol;
        }

        public void Reset()
        {
            IsPlayerKeeper = false;
            IsEnemyKeeper = false;
            CellValue = initValue;
        }

        public override string ToString()
        {
            return $" {CellValue} ";
        }
    }
}