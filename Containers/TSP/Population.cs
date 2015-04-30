using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MissionPlanner.Containers.TSP
{
    public class Population : List<Route>
    {
        public Route BestRoute = null;

        public void CreateRandomPopulation(int populationSize, Nodes nodesList, Random rand, int chanceToUseCloseNode)
        {
            int firstNode, lastNode, nextNode;
            for (int routeCount = 0; routeCount < populationSize; routeCount++)
            {
                Route route = new Route(nodesList.Count);

                // Create a starting point for this tour.
                firstNode = rand.Next(nodesList.Count);
                lastNode = firstNode;

                for (int node = 0; node < nodesList.Count - 1; node++)
                {
                    do
                    {
                        // Keep picking random nodes for the next node, until we find one we haven't been to.
                        if ((rand.Next(100) < chanceToUseCloseNode) && (nodesList[node].CloseNodes.Count > 0))
                        {
                            // 75% chance will will pick a node that is close to this one
                            nextNode = nodesList[node].CloseNodes[rand.Next(nodesList[node].CloseNodes.Count)];
                        }
                        else
                        {
                            // Otherwise, pick a completely random node.
                            nextNode = rand.Next(nodesList.Count);
                        }
                        // Make sure we haven't been here, and make sure it isn't where we are at now.
                    } while ((route[nextNode].secondNodeConnection != -1) || (nextNode == lastNode));

                    // When going from node A to B, [1] on A = B and [1] on node B = A
                    route[lastNode].secondNodeConnection = nextNode;
                    route[nextNode].firstNodeConnection = lastNode;
                    lastNode = nextNode;
                }

                // Connect the last 2 nodes.
                route[lastNode].secondNodeConnection = firstNode;
                route[firstNode].firstNodeConnection = lastNode;

                route.DetermineTotal(nodesList);

                Add(route);

                if ((BestRoute == null) || (route.Total < BestRoute.Total))
                {
                    BestRoute = route;
                }
            }
        }
    }
}
