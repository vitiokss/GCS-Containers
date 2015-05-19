using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MissionPlanner.Containers.TSP
{
    /// <summary>
    /// An individual link between 2 cities in a tour.
    /// This city connects to 2 other cities.
    /// </summary>
    public class Link
    {
        /// <summary>
        /// Connection to the first city.
        /// </summary>
        private int connection1;
        /// <summary>
        /// Connection to the first city.
        /// </summary>
        public int Connection1
        {
            get
            {
                return connection1;
            }
            set
            {
                connection1 = value; ;
            }
        }

        /// <summary>
        /// Connection to the second city.
        /// </summary>
        private int connection2;
        /// <summary>
        /// Connection to the second city.
        /// </summary>
        public int Connection2
        {
            get
            {
                return connection2;
            }
            set
            {
                connection2 = value;
            }
        }
    }
}
