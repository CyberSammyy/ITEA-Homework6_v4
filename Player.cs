using System;

namespace ITEA_Homework6_v4
{
    public class Player
    {
        private int health, ammo;
        public char Icon { get; set; }
        public bool IsAlive { get; set; }
        public int Health
        {
            get
            {
                return health;
            }
            set
            {
                if (value > 100)
                {
                    health = 100;
                }
                else if (value < 1)
                {
                    Console.WriteLine("GAME OVER");
                    IsAlive = false;
                    health = 0;
                    Icon = '×';
                }
                else health = value;
            }
        }
        public int Ammo
        {
            get
            {
                return ammo;
            }
            set
            {
                if (value < 1)
                {
                    ammo = 0;
                }
                else if (value > 120)
                {
                    ammo = 120;
                }
                else ammo = value;
            }
        }
        public int X { get; set; }
        public int Y { get; set; }
        public Player(int x, int y, int health, int ammo)
        {
            X = x;
            Y = y;
            Health = health;
            Ammo = ammo;
            IsAlive = true;
            Icon = '☻';
        }
        public void Set(char[,] symbols, Player player, Enemy[] enemies, ref Map map)
        {
            map = new Map(symbols.GetLength(0), symbols.GetLength(1), player, enemies);
            map.CreateMap(symbols);
        }


    }
}
