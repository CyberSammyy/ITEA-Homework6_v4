using System;

namespace ITEA_Homework6_v4
{
    public class Enemy
    {
        private char icon = 'E';
        private Random ran = new Random();
        private int health;
        private int damage = 15;
        public char Icon { get { return icon; } }
        public int Damage { get { return damage; } }
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
                    health = 0;
                    IsAlive = false;
                }
                else health = value;
            }
        }
        public bool IsAlive { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Enemy(int x, int y, int health)
        {
            X = x;
            Y = y;
            Health = health;
            IsAlive = true;
        }
    }
}
