using System;
using System.Linq;
using static System.Math;

namespace ITEA_Homework6_v4
{
    public class Map
    {
        Random rand = new Random();
        public int XSize { get; set; }
        public int YSize { get; set; }
        public int TurnCount { get; set; }
        Player player;
        Enemy[] enemies;
        Cell[,] cells;
        public Map(int size_x, int size_y, Player player, Enemy[] enemies)
        {
            XSize = size_x;
            YSize = size_y;
            TurnCount = 0;
            cells = new Cell[XSize, YSize];
            this.player = player;
            this.enemies = enemies;
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
        public void MoveEnemy()
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                int temp_x = rand.Next(2) - 1;
                int temp_y = rand.Next(2) - 1;
                if (IsPositionAvailable_Enemy(temp_x, temp_y, ref enemies[i]))
                {
                    SetEnemiesToPositions(temp_x, temp_y, ref enemies[i]);
                }
                else
                {
                    for (int p = 1; p >= -1; p--)
                    {
                        bool flag = true;
                        for (int k = -1; k <= 1; k++)
                        {
                            if(p == temp_x && k == temp_y)
                            {
                                if (p - 1 >= -1) p--;
                                if (k + 1 >= 1) k++;
                            }
                            if (IsPositionAvailable_Enemy(p, k, ref enemies[i]))
                            {
                                SetEnemiesToPositions(p, k, ref enemies[i]);
                                flag = false;
                                break;
                            }
                            if (flag == false) break;
                        }
                        if (flag == false) break;
                    }
                }
            }
        }
        public void Move()
        {
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
                        if (IsPositionAvailable_Enemy(0, 0, ref enemies[i]))
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
                                    if (IsPositionAvailable_Enemy(p, k, ref enemies[i]))
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
                }
                while (!isInputSuccess);
                RenderMap();
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
                for (int i = 1; i >= -1; i--)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (newPosX_Enemies + i > 0 && newPosY_Enemies + j > 0 && cells[newPosX_Enemies + i, newPosY_Enemies + j].CellValue == player.Icon)
                        {
                            player.Health -= 15; //you can customize your Player's damage by adding a weapon. 25 by default
                            enemy.X = xPos_Enemies;
                            enemy.Y = yPos_Enemies;
                            return;
                        }
                    }
                }
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
            SetPlayer(player, ref cells[newPosX_Player, newPosY_Player]);

            if (cells[newPosX_Player, newPosY_Player].CellValue == enemies[0].Icon)
            {
                int[] toCompare_distance = new int[enemies.Length];

                for (int i = 0; i < enemies.Length; i++)
                {
                    toCompare_distance[i] = (int)Sqrt(Abs(player.X - enemies[i].X) * Abs(player.X - enemies[i].X) + Abs(player.Y - enemies[i].Y) * Abs(player.Y - enemies[i].Y));
                }
                int minVal = toCompare_distance.Min();
                enemies[Array.IndexOf(toCompare_distance, minVal)].Health -= 25; //if you add Enemy type parameter to this function you will be able to shoose each enemie's damage. By default - 15 for each enemy
            }

            player.X = newPosX_Player;
            player.Y = newPosY_Player;
        }
        public bool IsPositionAvailable_Enemy(int xDelta_Enemy, int yDelta_Enemy, ref Enemy enemy)
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
            return isInBounds && cells[newPosX_Enemy, newPosY_Enemy].CanMove && cells[newPosX_Enemy, newPosY_Enemy].CellValue != player.Icon;
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
            return isInBounds && cells[newPosX_Player, newPosY_Player].CanMove && cells[newPosX_Player, newPosY_Player].CellValue != enemies[0].Icon;
        }
        public void RenderMap()
        {
            Console.Clear();
            Console.WriteLine($"Turn count: {TurnCount}, HP: {player.Health}");
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
