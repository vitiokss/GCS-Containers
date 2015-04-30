using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MissionPlanner.Containers.TSP
{
    public class Node
    {
        /// <summary>
        /// Location of the node.
        /// </summary>
        public Point Location;
        /// <summary>
        /// The list of the distances from every node.
        /// </summary>
        public List<double> Distances = new List<double>();
        public List<int> CloseNodes = new List<int>();
        public String Name;
        /// <summary>
        /// Constructor to create the node
        /// </summary>
        public Node(int x, int y, String name)
        {
            Location = new Point(x * 50, (y * 6) + 20);
            Name = name;
        }

        /// <summary>
        /// Find the nodes that are closest.
        /// </summary>
        /// <param name="numberCloseNodes"></param>
        public void FindClosestNodes(int numberCloseNodes)
        {
            double shortestDistance;
            int shortestNode = 0;
            double[] dist = new double[Distances.Count];
            Distances.CopyTo(dist);

            if (numberCloseNodes > Distances.Count - 1)
            {
                numberCloseNodes = Distances.Count - 1;
            }

            for (int i = 0; i < numberCloseNodes; i++)
            {
                shortestDistance = Double.MaxValue;
                for (int nodeNum = 0; nodeNum < Distances.Count; nodeNum++)
                {
                    if (dist[nodeNum] < shortestDistance)
                    {
                        shortestDistance = dist[nodeNum];
                        shortestNode = nodeNum;
                    }
                }
                CloseNodes.Add(shortestNode);
                dist[shortestNode] = Double.MaxValue;
            }
        }

    }
}
