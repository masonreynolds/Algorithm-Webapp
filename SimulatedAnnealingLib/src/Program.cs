using System;
using System.Collections.Generic;
using NQueens;
using Graph;

namespace Simulated_Annealing
{
    class Program
    {
        static void Main(string[] args)
        {
            getUserInput();
        }

        static void getUserInput()
        {
            do
            {
                int input;

                do
                {
                    Console.WriteLine("\nWelcome to the Simulated Annealing Tester. Please select an option: ");
                    Console.WriteLine("1. Enter locations with latitude and longitude ");
                    Console.WriteLine("2. Run test for Traveling Salesman problem ");
                    Console.WriteLine("3. Run test for N-Queens problem ");
                    Console.WriteLine("4. Exit program ");
                }
                while (!int.TryParse(Console.ReadLine(), out input) || input < 1  || input > 4);

                if (input == 1)
                {
                    List<Position> poses = new List<Position>();
                    string name = "filled";
                    double lat;
                    double lon;

                    for (int i = 1; name.Length != 0; i++)
                    {
                        Console.Write("\nEnter the name of location " + i + " (Just press enter to stop adding cities): ");
                        name = Console.ReadLine().Trim();

                        do
                        {
                            Console.Write("Enter the latitude of " + name + ": ");
                        }
                        while (!double.TryParse(Console.ReadLine(), out lat) || lat < -90 || lat > 90);

                        do
                        {
                            Console.Write("Enter the longitude of " + name + ": ");
                        }
                        while (!double.TryParse(Console.ReadLine(), out lon) || lon < -180 || lon > 180);

                        poses.Add(new Position(name, lat, lon));
                    }

                    WeightedGraph custom = new WeightedGraph(poses);
                    WeightedGraph best = SimAnnealingGraph.startSimAnneal(custom, 1000, 0.999, 10);
                    Console.WriteLine("\nShortest possible path: ");
                    Console.WriteLine(best.toString(""));
                }
                else if (input == 2)
                {
                    int numPoses;
                    int runs;

                    do
                    {
                        Console.Write("\nEnter the number of locations: ");
                    }
                    while (!int.TryParse(Console.ReadLine(), out numPoses) || numPoses < 2);

                    do
                    {
                        Console.Write("\nEnter the number of tests to run: ");
                    }
                    while (!int.TryParse(Console.ReadLine(), out runs) || runs < 1);

                    SimAnnealingGraphTest.RunTests(numPoses, runs);
                }
                else if (input == 3)
                {
                    int size;
                    int runs;

                    do
                    {
                        Console.Write("\nEnter the dimensions of the board: ");
                    }
                    while (!int.TryParse(Console.ReadLine(), out size) || size < 2);

                    do
                    {
                        Console.Write("\nEnter the number of tests to run: ");
                    }
                    while (!int.TryParse(Console.ReadLine(), out runs) || runs < 1);

                    SimAnnealingNQueensTest.RunTests(size, runs);
                }
                else if (input == 4)
                {
                    break;
                }

                Console.Write("\nWould you like to rerun the program? (Y/N) ");
            }
            while (Console.ReadLine().ToLower()[0] != 'n');

            Console.WriteLine();
        }
    }
}
