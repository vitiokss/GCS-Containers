using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MissionPlanner.Containers.TSP
{
    /// <summary>
    /// Represents the single instance of a route through all the cities.
    /// </summary>
    public class Route : List<Link>
    {

        /// <summary>
        /// Total route length.
        /// </summary>
        public double Total;

        public Route(int capacity)
            : base(capacity)
        {
            resetRoute(capacity);
        }

        /// <summary>
        /// Reset the route by seting all the connections to -1.
        /// </summary>
        /// <param name="numberOfNodes"></param>
        private void resetRoute(int numberOfNodes)
        {
            this.Clear();
            Link link;
            for (int i = 0; i < numberOfNodes; i++)
            {
                link = new Link();
                link.firstNodeConnection = -1;
                link.secondNodeConnection = -1;
                this.Add(link);
            }
        }

        /// <summary>
        /// Find the total length of an individual tour.
        /// </summary>
        /// <param name="nodes"></param>
        public void DetermineTotal(Nodes nodes)
        {
            Total = 0;
            int lastNode = 0;
            int nextNode = this[0].firstNodeConnection;

            foreach (Link link in this)
            {
                Total += nodes[lastNode].Distances[nextNode];

                // figure out if the next node in the list is [0] or [1].
                if (lastNode != this[nextNode].firstNodeConnection)
                {
                    lastNode = nextNode;
                    nextNode = this[nextNode].firstNodeConnection;
                }
                else
                {
                    lastNode = nextNode;
                    nextNode = this[nextNode].secondNodeConnection;
                }
            }
        }

        /// <summary>
        /// Creates a link between 2 nodes in a route, and then updates the node usage.
        /// </summary>
        private static void joinNodes(Route route, int[] nodeUsage, int node1, int node2)
        {
            if (route[node1].firstNodeConnection == -1)
            {
                route[node1].firstNodeConnection = node2;
            }
            else
            {
                route[node1].secondNodeConnection = node2;
            }

            if (route[node2].firstNodeConnection == -1)
            {
                route[node2].firstNodeConnection = node1;
            }
            else
            {
                route[node2].secondNodeConnection = node1;
            }
            nodeUsage[node1]++;
            nodeUsage[node2]++;
        }

        /// <summary>
        /// Find a link from a given node in the parent route that can be placed in the child route.
        /// </summary>
        private static int findNextNode(Route parent, Route child, Nodes nodesList, int[] nodeUsage, int node)
        {
            if (validateConnection(child, nodesList, nodeUsage, node, parent[node].firstNodeConnection))
            {
                return parent[node].firstNodeConnection;
            }
            else if (validateConnection(child, nodesList, nodeUsage, node, parent[node].secondNodeConnection))
            {
                return parent[node].secondNodeConnection;
            }

            return -1;
        }

        /// <summary>
        /// Determine if it is OK to connect 2 nodes given the existing connections in a child route.
        /// </summary>
        private static bool validateConnection(Route route, Nodes nodesList, int[] nodeUsage, int node1, int node2)
        {
            // Quick check to see if nodes already connected or if they have already 2 links.
            if ((node1 == node2) || (nodeUsage[node1] == 2) || (nodeUsage[node2] == 2))
            {
                return false;
            }

            if ((nodeUsage[node1] == 0) || (nodeUsage[node2] == 0))
            {
                return true;
            }

            // Need to see if the nodes are connected by going in each direction.
            for (int direction = 0; direction < 2; direction++)
            {
                int lastNode = node1;
                int currentNode;
                if (direction == 0)
                {
                    currentNode = route[node1].firstNodeConnection; // on first pass, use the first connection.
                }
                else
                {
                    currentNode = route[node1].secondNodeConnection; // on second pass, use the other connection.
                }
                int routeLength = 0;
                while ((currentNode != -1) && (currentNode != node2) && (routeLength < nodesList.Count - 2))
                {
                    routeLength++;
                    if (lastNode != route[currentNode].firstNodeConnection)
                    {
                        lastNode = currentNode;
                        currentNode = route[currentNode].firstNodeConnection;
                    }
                    else
                    {
                        lastNode = currentNode;
                        currentNode = route[currentNode].secondNodeConnection;
                    }
                }
                // if nodes are connected, but it goes through every node in the list, then OK to join.
                if (routeLength >= nodesList.Count - 2)
                {
                    return true;
                }
                // If the nodes are connected without going through all the nodes, it is NOT OK to join.
                if (currentNode == node2)
                {
                    return false;
                }
            }
            // If nodes were not connected going in either direction, we are OK to join them.
            return true;
        }

        /// <summary>
        /// Perform the crossover operation on 2 parent routes to create a new child route.
        /// </summary>
        public static Route Crossover(Route parent1, Route parent2, Nodes nodesList, Random rand)
        {
            Route child = new Route(nodesList.Count);      // the new route we are making
            int[] nodeUsage = new int[nodesList.Count];  // how many links 0-2 that connect to this node
            int node;                                   // for loop variable
            int nextNode;                               // the other node in this link

            for (node = 0; node < nodesList.Count; node++)
            {
                nodeUsage[node] = 0;
            }

            // Take all links that both parents agree on and put them in the child
            for (node = 0; node < nodesList.Count; node++)
            {
                if (nodeUsage[node] < 2)
                {
                    if (parent1[node].firstNodeConnection == parent2[node].firstNodeConnection)
                    {
                        nextNode = parent1[node].firstNodeConnection;
                        if (validateConnection(child, nodesList, nodeUsage, node, nextNode))
                        {
                            joinNodes(child, nodeUsage, node, nextNode);
                        }
                    }
                    if (parent1[node].secondNodeConnection == parent2[node].secondNodeConnection)
                    {
                        nextNode = parent1[node].secondNodeConnection;
                        if (validateConnection(child, nodesList, nodeUsage, node, nextNode))
                        {
                            joinNodes(child, nodeUsage, node, nextNode);

                        }
                    }
                    if (parent1[node].firstNodeConnection == parent2[node].secondNodeConnection)
                    {
                        nextNode = parent1[node].firstNodeConnection;
                        if (validateConnection(child, nodesList, nodeUsage, node, nextNode))
                        {
                            joinNodes(child, nodeUsage, node, nextNode);
                        }
                    }
                    if (parent1[node].secondNodeConnection == parent2[node].firstNodeConnection)
                    {
                        nextNode = parent1[node].secondNodeConnection;
                        if (validateConnection(child, nodesList, nodeUsage, node, nextNode))
                        {
                            joinNodes(child, nodeUsage, node, nextNode);
                        }
                    }
                }
            }

            // The parents don't agree on whats left, so we will alternate between using
            // links from parent 1 and then parent 2.

            for (node = 0; node < nodesList.Count; node++)
            {
                if (nodeUsage[node] < 2)
                {
                    if (node % 2 == 1)  // we prefer to use parent 1 on odd nodes
                    {
                        nextNode = findNextNode(parent1, child, nodesList, nodeUsage, node);
                        if (nextNode == -1) // but if thats not possible we still go with parent 2
                        {
                            nextNode = findNextNode(parent2, child, nodesList, nodeUsage, node); ;
                        }
                    }
                    else // use parent 2 instead
                    {
                        nextNode = findNextNode(parent2, child, nodesList, nodeUsage, node);
                        if (nextNode == -1)
                        {
                            nextNode = findNextNode(parent1, child, nodesList, nodeUsage, node);
                        }
                    }

                    if (nextNode != -1)
                    {
                        joinNodes(child, nodeUsage, node, nextNode);

                        // not done yet. must have been 0 in above case.
                        if (nodeUsage[node] == 1)
                        {
                            if (node % 2 != 1)  // use parent 1 on even cities
                            {
                                nextNode = findNextNode(parent1, child, nodesList, nodeUsage, node);
                                if (nextNode == -1) // use parent 2 instead
                                {
                                    nextNode = findNextNode(parent2, child, nodesList, nodeUsage, node);
                                }
                            }
                            else // use parent 2
                            {
                                nextNode = findNextNode(parent2, child, nodesList, nodeUsage, node);
                                if (nextNode == -1)
                                {
                                    nextNode = findNextNode(parent1, child, nodesList, nodeUsage, node);
                                }
                            }

                            if (nextNode != -1)
                            {
                                joinNodes(child, nodeUsage, node, nextNode);
                            }
                        }
                    }
                }
            }

            // Remaining links must be completely random.
            // Parent's links would cause multiple disconnected loops.
            for (node = 0; node < nodesList.Count; node++)
            {
                while (nodeUsage[node] < 2)
                {
                    do
                    {
                        nextNode = rand.Next(nodesList.Count);  // pick a random node, until we find one we can link to
                    } while (!validateConnection(child, nodesList, nodeUsage, node, nextNode));

                    joinNodes(child, nodeUsage, node, nextNode);
                }
            }

            return child;
        }

        /// <summary>
        /// Randomly change one of the links in this route.
        /// </summary>
        public void Mutate(Random rand)
        {
            int nodeNumber = rand.Next(this.Count);
            Link link = this[nodeNumber];
            int tmpNodeNumber;

            // Find which 2 cities connect to cityNumber, and then connect them directly
            if (this[link.firstNodeConnection].firstNodeConnection == nodeNumber)   // Conn 1 on Conn 1 link points back to us.
            {
                if (this[link.secondNodeConnection].firstNodeConnection == nodeNumber)// Conn 1 on Conn 2 link points back to us.
                {
                    tmpNodeNumber = link.secondNodeConnection;
                    this[link.secondNodeConnection].firstNodeConnection = link.firstNodeConnection;
                    this[link.firstNodeConnection].firstNodeConnection = tmpNodeNumber;
                }
                else                                                // Conn 2 on Conn 2 link points back to us.
                {
                    tmpNodeNumber = link.secondNodeConnection;
                    this[link.secondNodeConnection].secondNodeConnection = link.firstNodeConnection;
                    this[link.firstNodeConnection].firstNodeConnection = tmpNodeNumber;
                }
            }
            else                                                    // Conn 2 on Conn 1 link points back to us.
            {
                if (this[link.secondNodeConnection].firstNodeConnection == nodeNumber)// Conn 1 on Conn 2 link points back to us.
                {
                    tmpNodeNumber = link.secondNodeConnection;
                    this[link.secondNodeConnection].firstNodeConnection = link.firstNodeConnection;
                    this[link.firstNodeConnection].secondNodeConnection = tmpNodeNumber;
                }
                else                                                // Conn 2 on Conn 2 link points back to us.
                {
                    tmpNodeNumber = link.secondNodeConnection;
                    this[link.secondNodeConnection].secondNodeConnection = link.firstNodeConnection;
                    this[link.firstNodeConnection].secondNodeConnection = tmpNodeNumber;
                }

            }

            int replaceNodeNumber = -1;
            do
            {
                replaceNodeNumber = rand.Next(this.Count);
            }
            while (replaceNodeNumber == nodeNumber);
            Link replaceLink = this[replaceNodeNumber];

            // Now we have to reinsert that city back into the tour at a random location
            tmpNodeNumber = replaceLink.secondNodeConnection;
            link.secondNodeConnection = replaceLink.secondNodeConnection;
            link.firstNodeConnection = replaceNodeNumber;
            replaceLink.secondNodeConnection = nodeNumber;

            if (this[tmpNodeNumber].firstNodeConnection == replaceNodeNumber)
            {
                this[tmpNodeNumber].firstNodeConnection = nodeNumber;
            }
            else
            {
                this[tmpNodeNumber].secondNodeConnection = nodeNumber;
            }
        }
    }
}
