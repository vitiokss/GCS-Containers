using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MissionPlanner.Containers.TSP
{
    /// <summary>
    /// Event arguments when the TSP class wants the GUI to draw a route.
    /// </summary>
    public class TspEventArgs : EventArgs
    {
        public TspEventArgs() { }

        public Nodes NodesList;
        public Route BestRoute;
        public int Generation;
        public bool Complete = false;

        public TspEventArgs(Nodes nodesList, Route bestRoute, int generation, bool complete)
        {
            this.NodesList = nodesList;
            this.BestRoute = bestRoute;
            this.Generation = generation;
            this.Complete = complete;
        }
    }
}
