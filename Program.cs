using System;
using System.IO;

namespace ITEA_Homework6_v4
{
    class Program
    {

        static void Main(string[] args)
        {
            string path = Console.ReadLine();
            Start(path);
        }
        //static char[,] RandomMap()
        //{
        //    var rand = new Random();
        //    for (int i = 0; i < Map.GetLength(0); i++)
        //    {
        //        for (int k = 0; k < Map.GetLength(1); k++)
        //        {
        //            Map[i, k] = new Cell(i, k, rand);
        //        }
        //    }
        //}
        static char[,] LoadMap(string path)
        {
            string[] array;
            array = File.ReadAllLines(path);
            int size_i = array.GetLength(0);
            int size_j = array[0].Length;
            char[,] map = new char[size_i, size_j];
            char[] temp = new char[size_i];
            for (int i = 0; i < map.GetLength(0); i++)
            {
                temp = array[i].ToCharArray();
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = temp[j];
                }
            }
            return map;
        }
        static void Start(string path)
        {
            char[,] map = LoadMap(path);
            Player player = new Player(1, 1, 100, 60);
            Enemy[] enemies = new Enemy[2];
            enemies[0] = new Enemy(4, 1, 100);
            enemies[1] = new Enemy(8, 8, 100);
            Map level = null;
            player.Set(map, player, enemies, ref level);
            level.Move();
        }



        /// <summary>
        /// Use before calling SetPlayerToPosition
        /// </summary>
        /// <param name="mDelta"></param>
        /// <param name="nDelta"></param>
        /// <returns></returns>

        /// <summary>
        /// Only for move actions, Exception if !IsPositionAvailable
        /// </summary>
        /// <param name="mDelta"></param>
        /// <param name="nDelta"></param>



        static void Task()
        {
            int size = 0;
            bool success = false;
            do
            {
                Console.Write("Enter size: ");
                string input = Console.ReadLine();
                if (!double.TryParse(input, out double number))
                {
                    Console.WriteLine("Not a number!");
                    continue;
                }

                if (number <= 0)
                {
                    Console.WriteLine("Size must be more than 0");
                    continue;
                }

                if (number - (int)number > 0)
                {
                    Console.WriteLine("Only integer!");
                    continue;
                }

                success = true;
                size = (int)number;
                Console.WriteLine();
            }
            while (!success);

            Console.WriteLine("Task 1\n");
            int[,] vs = new int[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int k = 0; k < size; k++)
                {
                    vs[i, k] = i >= k ? i + 1 : 0;
                    Console.Write($"{vs[i, k]}\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine();

            Console.WriteLine("Task 2\n");
            int[,] vs1 = new int[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int k = 0; k < size; k++)
                {
                    vs[i, k] = i >= k ? (i + 1) * (k + 1) : 0;
                    Console.Write($"{vs[i, k]}\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }



        static void Recursion(params int[] array)
        {
            if (array.Length > 1)
            {
                int[] newLine = new int[array.Length - 1];
                for (int i = 0; i < newLine.Length; i++)
                {
                    newLine[i] = array[i] + array[i + 1];
                }
                Recursion(newLine);
                foreach (int item in array)
                {
                    Console.Write($"{item}\t");
                }
                Console.WriteLine();
            }
            else if (array.Length == 1)
            {
                Console.WriteLine($"{array[0]}");
            }
            else
            {
                Console.WriteLine("Empty array");
            }
        }
    }
}