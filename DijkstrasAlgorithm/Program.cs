using System;
using System.Collections.Generic;
using System.Drawing;

namespace DijkstrasAlgorithm
{
    static class Program
    {
        static void Main()
        {
            #region Dijkstra's Algorithm

            var image = new Bitmap(@"C:\Users\jmolina\Desktop\maze.png");
            var nodes = new Node[image.Height, image.Width];
            var target = default(Node);
            var unvisited = new HashSet<Node>();

            Console.WriteLine("Initializing...");

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    if (image.GetPixel(x, y).R == 0 && image.GetPixel(x, y).G == 0 && image.GetPixel(x, y).B == 0)
                    {
                        continue;
                    }

                    nodes[y, x] = new Node(y, x);
                    unvisited.Add(nodes[y, x]);

                    if (image.GetPixel(x, y).R == 255 && image.GetPixel(x, y).G == 0 && image.GetPixel(x, y).B == 0)
                    {
                        nodes[y, x].Distance = 0;
                    }

                    if (image.GetPixel(x, y).R == 0 && image.GetPixel(x, y).G == 0 && image.GetPixel(x, y).B == 255)
                    {
                        target = nodes[y, x];
                    }
                }
            }

            Console.WriteLine("Computing...");

            while (unvisited.Count > 0)
            {
                var current = GetMin(unvisited);

                if (current == target)
                {
                    Console.WriteLine(current.Distance);
                    break;
                }

                unvisited.Remove(current);

                EvaluateNeighbors(current, nodes);
            }

            Console.WriteLine("Printing...");

            var result = new Bitmap(image.Width, image.Height);

            while (target.Previous != null)
            {
                result.SetPixel(target.X, target.Y, Color.FromArgb(255, 0, 0));

                target = target.Previous;
            }

            result.SetPixel(target.X, target.Y, Color.FromArgb(255, 0, 0));
            result.Save(@"C:\Users\jmolina\Desktop\result.png");

            #endregion

            #region Make Line Thicker

            //var image = new Bitmap(@"C:\Users\jmolina\Desktop\result.png");
            //var result = new Bitmap(image.Width, image.Height);

            //for (int y = 0; y < image.Height; y++)
            //{
            //    for (int x = 0; x < image.Width; x++)
            //    {
            //        if (image.GetPixel(x, y).R == 255 && image.GetPixel(x, y).G == 0 && image.GetPixel(x, y).B == 0)
            //        {
            //            for (int i = -1; i <= 1; i++)
            //            {
            //                for (int j = -1; j <= 1; j++)
            //                {
            //                    try
            //                    {
            //                        result.SetPixel(x + j, y + i, Color.FromArgb(255, 0, 0));
            //                    }
            //                    catch
            //                    {
            //                        // An index is out of bounds.
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            result.Save(@"C:\Users\jmolina\Desktop\result2.png");

            #endregion
        }
        static double Metric(Node a, Node b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }
        static Node GetMin(HashSet<Node> nodes)
        {
            var result = default(Node);

            foreach (var node in nodes)
            {
                if (result == null || node.Distance < result.Distance)
                {
                    result = node;
                }
            }

            return result;
        }
        static void EvaluateNeighbors(Node node, Node[,] nodes)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    if (y == 0 && x == 0)
                    {
                        continue;
                    }

                    try
                    {
                        var neighbor = nodes[node.Y + y, node.X + x];
                        var d = Metric(node, neighbor);

                        if (node.Distance + d < neighbor.Distance)
                        {
                            neighbor.Distance = node.Distance + d;
                            neighbor.Previous = node;
                        }
                    }
                    catch
                    {
                        // Either an index is out of bounds or there is no node.
                    }
                }
            }
        }

        class Node
        {
            public double Distance { get; set; }
            public int Y { get; set; }
            public int X { get; set; }
            public Node Previous { get; set; }

            public Node(int y, int x)
            {
                Y = y;
                X = x;
                Distance = double.PositiveInfinity;
            }
        }
    }
}