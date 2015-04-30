using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MissionPlanner.Containers.TSP
{
    /// <summary>
    /// Indivitual link between 2 nodes in the path.
    /// </summary>
    public class Link
    {
        public int firstNodeConnection;
        public int secondNodeConnection;
    }
}
