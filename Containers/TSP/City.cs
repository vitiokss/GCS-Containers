using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MissionPlanner.Containers.TSP
{
    /// <summary>
    /// An individual City in our tour.
    /// </summary>
    public class City
    {
        /// <summary>
        /// Constructor that provides the city location.
        /// </summary>
        /// <param name="x">X position of the city.</param>
        /// <param name="y">Y position of the city.</param>
        public City(double x, double y, double z, string name, int bayId, int rowId)
        {
            X = x;
            Y = y;
            Z = z;
            Name = name;
            BayId = bayId;
            RowId = rowId;
            Location = new PointF((float)x, (float)y);
        }

        public string Name { set; get; }
        public double X { set; get; }
        public double Y { set; get; }
        public double Z { set; get; }
        public int BayId { set; get; }
        public int RowId { set; get; }
        /// <summary>
        /// Private copy of the location of this city.
        /// </summary>
        private PointF location;
        /// <summary>
        /// The location of this city.
        /// </summary>
        public PointF Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
            }
        }

        /// <summary>
        /// Private copy of the distance from this city to every other city.
        /// The index in this array is the number of the city linked to.
        /// </summary>
        private List<double> distances = new List<double>();
        /// <summary>
        /// The distance from this city to every other city.
        /// </summary>
        public List<double> Distances
        {
            get
            {
                return distances;
            }
            set
            {
                distances = value;
            }
        }

        /// <summary>
        /// Private copy of the list of the cities that are closest to this one.
        /// </summary>
        private List<int> closeCities = new List<int>();
        private double p1;
        private double p2;
        private double p3;
        private string p4;
        private int p5;
        private int p6;
        /// <summary>
        /// A list of the cities that are closest to this one.
        /// </summary>
        public List<int> CloseCities
        {
            get
            {
                return closeCities;
            }
        }

        /// <summary>
        /// Find the cities that are closest to this one.
        /// </summary>
        /// <param name="numberOfCloseCities">When creating the initial population of tours, this is a greater chance
        /// that a nearby city will be chosen for a link. This is the number of nearby cities that will be considered close.</param>
        public void FindClosestCities(int numberOfCloseCities)
        {
            double shortestDistance;
            int shortestCity = 0;
            double[] dist = new double[Distances.Count];
            Distances.CopyTo(dist);

            if (numberOfCloseCities > Distances.Count - 1)
            {
                numberOfCloseCities = Distances.Count - 1;
            }

            closeCities.Clear();

            for (int i = 0; i < numberOfCloseCities; i++)
            {
                shortestDistance = Double.MaxValue;
                for (int cityNum = 0; cityNum < Distances.Count; cityNum++)
                {
                    if (dist[cityNum] < shortestDistance)
                    {
                        shortestDistance = dist[cityNum];
                        shortestCity = cityNum;
                    }
                }
                closeCities.Add(shortestCity);
                dist[shortestCity] = Double.MaxValue;
            }
        }
    }
}
