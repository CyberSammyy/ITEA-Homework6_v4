using System;
using System.Linq;
using static System.Math;

namespace ITEA_Homework6_v4
{
    public class Map
    {
        public int XSize { get; set; }
        public int YSize { get; set; }
        public int TurnCount { get; set; }
        Player player;
        Enemy[] enemies;
        Cell[,] cells;
        Random[] rands;
        Random rand = new Random();
        int fix_counter_left = 0;
        int fix_counter_right = 0;
        int fix_counter_up = 0;
        int fix_counter_down = 0;
        public Map(int size_x, int size_y, Player player, Enemy[] enemies)
        {
            XSize = size_x;
            YSize = size_y;
            TurnCount = 0;
            cells = new Cell[XSize, YSize];
            this.player = player;
            this.enemies = enemies;
            rands = new Random[enemies.Length];
        }
        public Map()
        {
            TurnCount = 0;
            XSize = YSize = 0;
            cells = null;
        }
        public void SetEnemy(Enemy enemy, ref Cell cell)
        {
            if (cell.CanMove)
            {
                cell.IsEnemyKeeper = true;
                cell.CellValue = enemy.Icon;
            }
            else
            {
                throw new Exception("CanMove = false");
            }
        }
        public void SetPlayer(Player player, ref Cell cell)
        {
            if (cell.CanMove)
            {
                cell.IsPlayerKeeper = true;
                cell.CellValue = player.Icon;
            }
            else
            {
                throw new Exception("CanMove = false");
            }
        }
        public int[] Generator(int XpY, int XmY)
        {
            Random rand1 = new Random((int)DateTime.Now.Day);
            Random rand2 = new Random((int)DateTime.Now.Day - (int)DateTime.Now.Second);
            int[] ret = new int[2];
            do
            {
                ret[0] = rand1.Next(-1, 1);
                ret[1] = rand2.Next(-1, 1);
            }
            while (ret[0] == ret[1]);
            return ret;
        }
        public (bool, int, int) IsPlayerNear(Enemy enemy)
        {
            bool res;
            int X, Y;
            if (cells[enemy.X + 0, enemy.Y - 1].CellValue == player.Icon)
            {
                res = true;
                X = enemy.X + 0;
                Y = enemy.Y - 1;
                var tuple = (res, X, Y);
                return tuple;
            }
            else if (cells[enemy.X - 1, enemy.Y + 0].CellValue == player.Icon)
            {
                res = true;
                X = enemy.X - 1;
                Y = enemy.Y + 0;
                var tuple = (res, X, Y);
                return tuple;
            }
            else if (cells[enemy.X + 0, enemy.Y + 1].CellValue == player.Icon)
            {
                res = true;
                X = enemy.X + 0;
                Y = enemy.Y + 1;
                var tuple = (res, X, Y);
                return tuple;
            }
            else if (cells[enemy.X + 1, enemy.Y + 0].CellValue == player.Icon)
            {
                res = true;
                X = enemy.X + 1;
                Y = enemy.Y + 0;
                var tuple = (res, X, Y);
                return tuple;
            }
            else return (false, 0, 0);
        }
        public void MoveEnemy()
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                int index = rand.Next(0, 4);
                bool[] indicators = new bool[4];
                if (IsPositionAvailable_Enemy(1, 0, enemies[i]))
                {
                    indicators[0] = true;
                }
                else indicators[0] = false;
                if (IsPositionAvailable_Enemy(-1, 0, enemies[i]))
                {
                    indicators[1] = true;
                }
                else indicators[1] = false;
                if (IsPositionAvailable_Enemy(0, -1, enemies[i]))
                {
                    indicators[2] = true;
                }
                else indicators[2] = false;
                if (IsPositionAvailable_Enemy(0, 1, enemies[i]))
                {
                    indicators[3] = true;
                }
                else indicators[3] = false;
                if (indicators[0] == true || indicators[1] == true || indicators[2] == true || indicators[3] == true)
                {
                    while (indicators[index] != true)
                    {
                        index = rand.Next(0, 4);
                    }
                    switch (index)
                    {
                        case 0:
                            SetEnemiesToPositions(1, 0, ref enemies[i]);
                            if (IsPlayerNear(enemies[i]).Item1)
                            {
                                player.Health -= 15;
                                enemies[i].Health -= 25;
                            }
                            break;
                        case 1:
                            SetEnemiesToPositions(-1, 0, ref enemies[i]);
                            if (IsPlayerNear(enemies[i]).Item1)
                            {
                                player.Health -= 15;
                                enemies[i].Health -= 25;
                            }
                            break;
                        case 2:
                            SetEnemiesToPositions(0, -1, ref enemies[i]);
                            if (IsPlayerNear(enemies[i]).Item1)
                            {
                                player.Health -= 15;
                                enemies[i].Health -= 25;
                            }
                            break;
                        case 3:
                            SetEnemiesToPositions(0, 1, ref enemies[i]);
                            if (IsPlayerNear(enemies[i]).Item1)
                            {
                                player.Health -= 15;
                                enemies[i].Health -= 25;
                            }
                            break;
                        default:
                            break;
                    }
                }
                else if (indicators[0] == indicators[1] == indicators[2] == indicators[3] == false)
                {
                    if (IsPlayerNear(enemies[i]).Item1)
                    {
                        player.Health -= 15;
                        enemies[i].Health -= 25;
                    }
                }
                if (IsPlayerNear(enemies[i]).Item1)
                {
                    player.Health -= 15;
                    enemies[i].Health -= 25;
                }
            }
        }
        public void Move()
        {
            RenderMap();
            while (player.IsAlive)
            {
                bool isInputSuccess;
                do
                {
                    if (IsPositionAvailable(0, 0)) //default values - 0,0
                    {
                        SetPlayerToPosition(0, 0); //default values - 0,0
                    }
                    for (int i = 0; i < enemies.Length; i++)
                    {
                        if (IsPositionAvailable_Enemy(0, 0, enemies[i]))
                        {
                            SetEnemiesToPositions(0, 0, ref enemies[i]);
                        }
                        else
                        {
                            for (int p = 1; p >= -1; p--)
                            {
                                bool flag = true;
                                for (int k = -1; k <= 1; k++)
                                {
                                    if (IsPositionAvailable_Enemy(p, k, enemies[i]))
                                    {
                                        SetEnemiesToPositions(p, k, ref enemies[i]);
                                        flag = false;
                                        break;
                                    }
                                }
                                if (flag == false) break;
                            }
                        }
                    }
                    RenderMap();
                    ConsoleKey key = Console.ReadKey().Key;
                    isInputSuccess = true;
                    switch (key)
                    {
                        case ConsoleKey.A:
                            {

                                if (IsPositionAvailable(0, -1))
                                {

                                    SetPlayerToPosition(0, -1);
                                    TurnCount++;
                                    player.Icon = 'P';
                                }
                                else
                                {
                                    Console.WriteLine("Position is unavailable");
                                    isInputSuccess = false;
                                }
                                MoveEnemy();
                                break;
                            }
                        case ConsoleKey.W:
                            {

                                if (IsPositionAvailable(-1, 0))
                                {
                                    SetPlayerToPosition(-1, 0);
                                    TurnCount++;
                                    player.Icon = '↑';
                                }
                                else
                                {
                                    Console.WriteLine("Position is unavailable");
                                    isInputSuccess = false;
                                }
                                MoveEnemy();
                                break;
                            }
                        case ConsoleKey.D:
                            {

                                if (IsPositionAvailable(0, 1))
                                {

                                    SetPlayerToPosition(0, 1);
                                    TurnCount++;
                                    player.Icon = '→';
                                }
                                else
                                {
                                    Console.WriteLine("Position is unavailable");
                                    isInputSuccess = false;
                                }
                                MoveEnemy();
                                break;
                            }
                        case ConsoleKey.S:
                            {

                                if (IsPositionAvailable(1, 0))
                                {

                                    SetPlayerToPosition(1, 0);
                                    TurnCount++;
                                    player.Icon = '↓';
                                }
                                else
                                {
                                    Console.WriteLine("Position is unavailable");
                                    isInputSuccess = false;
                                }
                                MoveEnemy();
                                break;
                            }
                        default:
                            {
                                Console.WriteLine("Wrong key, use \"W,A,S,D\"");
                                isInputSuccess = false;
                                break;
                            }
                    }
                    RenderMap();
                }
                while (!isInputSuccess);
            }
        }
        public void CreateMap(char[,] symbols)
        {
            int size_x = symbols.GetLength(0);
            int size_y = symbols.GetLength(1);
            for (int i = 0; i < size_x; i++)
            {
                for (int j = 0; j < size_y; j++)
                {
                    cells[i, j] = new Cell(i, j, symbols[i, j], enemies[0]);
                }
            }
        }
        public void ToConsole(string str, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.Write(str);
            Console.ResetColor();
        }
        public void SetEnemiesToPositions(int xDelta_Enemy, int yDelta_Enemy, ref Enemy enemy)
        {
            int xPos_Enemies = enemy.X;
            int yPos_Enemies = enemy.Y;

            int newPosX_Enemies = xPos_Enemies + xDelta_Enemy;
            int newPosY_Enemies = yPos_Enemies + yDelta_Enemy;
            if (enemy.IsAlive)
            {
                if (newPosX_Enemies < 0 || newPosY_Enemies < 0)
                {
                    newPosX_Enemies = newPosY_Enemies = 1;
                }
                //for (int i = 1; i >= -1; i--)
                //{
                //    for (int j = -1; j <= 1; j++)
                //    {
                //        if (newPosX_Enemies + i > 0 && newPosY_Enemies + j > 0 && cells[newPosX_Enemies + i, newPosY_Enemies + j].CellValue == player.Icon)
                //        {
                //            player.Health -= 15; //you can customize your Player's damage by adding a weapon. 25 by default
                //            enemy.X = xPos_Enemies;
                //            enemy.Y = yPos_Enemies;
                //            return;
                //        }
                //    }
                //}
                if (xPos_Enemies == newPosX_Enemies && yPos_Enemies == newPosY_Enemies)
                {
                    SetEnemy(enemy, ref cells[newPosX_Enemies, newPosY_Enemies]);
                }
                else
                {
                    cells[xPos_Enemies, yPos_Enemies].Reset();
                    SetEnemy(enemy, ref cells[newPosX_Enemies, newPosY_Enemies]);
                    enemy.X = newPosX_Enemies;
                    enemy.Y = newPosY_Enemies;
                }
            }
            else
            {
                cells[xPos_Enemies, yPos_Enemies].Reset();
            }
        }
        public void SetPlayerToPosition(int xDelta_Player, int yDelta_Player)
        {
            int xPos_Player = player.X;
            int yPos_Player = player.Y;

            int newPosX_Player = xPos_Player + xDelta_Player;
            int newPosY_Player = yPos_Player + yDelta_Player;

            cells[xPos_Player, yPos_Player].Reset();
            if (player.IsAlive)
                SetPlayer(player, ref cells[newPosX_Player, newPosY_Player]);
            else
            {
                player.Icon = 'X';
                SetPlayer(player, ref cells[xPos_Player, yPos_Player]);
            }

            //if (cells[newPosX_Player, newPosY_Player].CellValue == enemies[0].Icon)
            //{
            //    int[] toCompare_distance = new int[enemies.Length];

            //    for (int i = 0; i < enemies.Length; i++)
            //    {
            //        toCompare_distance[i] = (int)Sqrt(Abs(player.X - enemies[i].X) * Abs(player.X - enemies[i].X) + Abs(player.Y - enemies[i].Y) * Abs(player.Y - enemies[i].Y));
            //    }
            //    int minVal = toCompare_distance.Min();
            //    enemies[Array.IndexOf(toCompare_distance, minVal)].Health -= 25; //if you add Enemy type parameter to this function you will be able to shoose each enemie's damage. By default - 15 for each enemy
            //}

            player.X = newPosX_Player;
            player.Y = newPosY_Player;
        }
        public bool IsPositionAvailable_Enemy(int xDelta_Enemy, int yDelta_Enemy, Enemy enemy)
        {
            int xPos_Enemy = enemy.X;
            int yPos_Enemy = enemy.Y;

            bool isInBounds = true;
            int newPosX_Enemy = xPos_Enemy + xDelta_Enemy;
            int newPosY_Enemy = yPos_Enemy + yDelta_Enemy;

            if (xPos_Enemy + xDelta_Enemy < 0 || xPos_Enemy + xDelta_Enemy >= cells.GetLength(0) ||
                yPos_Enemy + yDelta_Enemy < 0 || yPos_Enemy + yDelta_Enemy >= cells.GetLength(1))
            {
                isInBounds = false;
            }
            return isInBounds && cells[newPosX_Enemy, newPosY_Enemy].CanMove && !cells[newPosX_Enemy, newPosY_Enemy].IsPlayerKeeper;
        }
        public bool IsPositionAvailable(int xDelta_Player, int yDelta_Player) //read the second comment and add Enemy enemy to the parameters if you want to customize damage
        {
            int xPos_Player = player.X;
            int yPos_Player = player.Y;

            bool isInBounds = true;
            int newPosX_Player = xPos_Player + xDelta_Player;
            int newPosY_Player = yPos_Player + yDelta_Player;


            if (xPos_Player + xDelta_Player < 0 || xPos_Player + xDelta_Player >= cells.GetLength(0) ||
                yPos_Player + yDelta_Player < 0 || yPos_Player + yDelta_Player >= cells.GetLength(1))
            {
                isInBounds = false;
            }
            return isInBounds && cells[newPosX_Player, newPosY_Player].CanMove && !cells[newPosX_Player, newPosY_Player].IsEnemyKeeper;
        }
        public void RenderMap()
        {
            Console.Clear();
            Console.WriteLine($"Turn count: {TurnCount}, HP: {player.Health}, up: {fix_counter_up}, down: {fix_counter_down}, left: {fix_counter_left}, right: {fix_counter_right}");
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                Console.WriteLine(new string('-', cells.GetLength(1) * 4 + 1));
                Console.Write("|");
                for (int k = 0; k < cells.GetLength(1); k++)
                {
                    if (cells[i, k].IsPlayerKeeper)
                    {
                        ToConsole(cells[i, k].ToString(), ConsoleColor.Yellow);
                        Console.Write("|");
                    }
                    else
                    {
                        if (cells[i, k].CellValue == '#')
                        {
                            ToConsole(cells[i, k].ToString(), ConsoleColor.White);
                            Console.Write("|");
                        }
                        else if (cells[i, k].CellValue == '@')
                        {
                            ToConsole(cells[i, k].ToString(), ConsoleColor.Magenta);
                            Console.Write("|");
                        }
                        else if (cells[i, k].CellValue == 'E')
                        {
                            ToConsole(cells[i, k].ToString(), ConsoleColor.Red);
                            Console.Write("|");
                        }
                        else if (cells[i, k].CellValue == 'A' || cells[i, k].CellValue == 'H')
                        {
                            ToConsole(cells[i, k].ToString(), ConsoleColor.Blue);
                            Console.Write("|");
                        }
                        else
                        {
                            Console.Write($"{cells[i, k]}|");
                        }
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine(new string('-', cells.GetLength(1) * 4 + 1));
        }
    }
}
