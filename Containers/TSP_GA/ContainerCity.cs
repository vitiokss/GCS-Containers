using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MissionPlanner.Containers.TSP_GA
{
    public class ContainerCity
    {

        public string Name { set; get; }
        public double X { set; get; }
        public double Y { set; get; }
        public double Z { set; get; }
        public int BayId { set; get; }
        public int RowId { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The identification name of the container.</param>
        /// <param name="_x">The X placement value of the container.</param>
        /// <param name="_y">The Y placement value of the container.</param>
        public ContainerCity(string name, double _x, double _y, double _z, int bayId, int rowId)
        {
            Name = name;
            X = _x;
            Y = _y;
            Z = _z;
            BayId = bayId;
            RowId = rowId;
        }

        public ContainerCity() { }

        /// <summary>
        /// Calculate the distance between two points.
        /// </summary>
        public double GetDistanceFromPosition(double _x, double _y)
        {
            return Math.Sqrt(Math.Pow((_x - this.X), 2D) + Math.Pow((_y - Y), 2D));
        }

    }
}
