using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MissionPlanner.Containers.TSP
{
    /// <summary>
    /// Event arguments when the TSP class wants the GUI to draw a tour.
    /// </summary>
    public class TspEventArgs : EventArgs
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public TspEventArgs()
        {
        }

        /// <summary>
        /// Constructor that sets all the properties.
        /// </summary>
        /// <param name="cityList">The list of cities to draw.</param>
        /// <param name="bestTour">The tour that connects all the cities.</param>
        /// <param name="generation">Which generation is this.</param>
        /// <param name="complete">Is this the last update before we are done.</param>
        public TspEventArgs(Cities cityList, Tour bestTour, int generation, bool complete)
        {
            this.cityList = cityList;
            this.bestTour = bestTour;
            this.generation = generation;
            this.complete = complete;
        }

        /// <summary>Private copy of the list of cities.</summary>
        private Cities cityList;
        /// <summary>Public property for list of cities.</summary>
        public Cities CityList
        {
            get
            {
                return cityList;
            }
        }

        /// <summary>Private copy of the tour of the cities.</summary>
        private Tour bestTour;
        /// <summary>Public property for the tour of the cities.</summary>
        public Tour BestTour
        {
            get
            {
                return bestTour;
            }
        }

        /// <summary>Private copy for which generation this tour came from.</summary>
        private int generation;
        /// <summary>Public property for which generation this tour came from.</summary>
        public int Generation
        {
            get
            {
                return generation;
            }
            set
            {
                generation = value;
            }
        }

        /// <summary>Private copy indicating if the TSP algorithm is complete.</summary>
        private bool complete = false;
        /// <summary>Public property indicating if the TSP algorithm is complete.</summary>
        public bool Complete
        {
            get
            {
                return complete;
            }
            set
            {
                complete = value;
            }
        }
    }
}
