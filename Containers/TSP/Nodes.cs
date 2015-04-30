using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MissionPlanner.Containers.TSP
{
    public class Nodes : List<Node>
    {

        public void CalculateNodeDistances(int numberOfCloseNodes)
        {
            foreach (Node node in this)
            {
                node.Distances.Clear();

                for (int i = 0; i < Count; i++)
                {
                    node.Distances.Add(Math.Sqrt(Math.Pow((node.Location.X - this[i].Location.X), 2D) +
                                       Math.Pow((node.Location.Y - this[i].Location.Y), 2D)));
                }
            }

            foreach (Node node in this)
            {
                node.FindClosestNodes(numberOfCloseNodes);
            }
        }

        /// <summary>
        /// Load the containers to the node list.
        /// </summary>
        /// <param name="containers"></param>
        public void LoadNodes(List<ContainerObject> containers)
        {
            this.Clear();
            foreach (var c in containers)
            {
                this.Add(new Node(Convert.ToInt32(c.X, CultureInfo.CurrentCulture), Convert.ToInt32(c.Y, CultureInfo.CurrentCulture), c.Name));
            }
        }

    }
}
