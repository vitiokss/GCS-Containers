using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MissionPlanner.Containers.TSP
{
    public class TSP
    {

        public delegate void NewBestRouteEventHandler(Object sender, TspEventArgs e);

        public event NewBestRouteEventHandler foundNewBestRoute;

        Random rand;

        Nodes NodesList;

        Population population;

        public bool Halt = false;

        public TSP() { }

        public void Begin(int populationSize, int maxGenerations, int groupSize, int mutation, int seed, int chanceToUseCloseNodes, Nodes nodesList)
        {
            rand = new Random(seed);

            this.NodesList = nodesList;

            population = new Population();
            population.CreateRandomPopulation(populationSize, nodesList, rand, chanceToUseCloseNodes);

            displayRoute(population.BestRoute, 0, false);

            bool foundNewBestTour = false;
            int generation;
            for (generation = 0; generation < maxGenerations; generation++)
            {
                if (Halt)
                {
                    break;  // GUI has requested we exit.
                }
                foundNewBestTour = makeChildren(groupSize, mutation);

                if (foundNewBestTour)
                {
                    displayRoute(population.BestRoute, generation, false);
                }
            }

            displayRoute(population.BestRoute, generation, true);
        }

        /// <summary>
        /// Randomly select a group of routes from the population. 
        /// The top 2 are chosen as the parent tours.
        /// Crossover is performed on these 2 tours.
        /// The childred tours from this process replace the worst 2 tours in the group.
        /// </summary>
        bool makeChildren(int groupSize, int mutation)
        {
            int[] tourGroup = new int[groupSize];
            int tourCount, i, topTour, childPosition, tempTour;

            // pick random tours to be in the neighborhood city group
            // we allow for the same tour to be included twice
            for (tourCount = 0; tourCount < groupSize; tourCount++)
            {
                tourGroup[tourCount] = rand.Next(population.Count);
            }

            // bubble sort on the neighborhood city group
            for (tourCount = 0; tourCount < groupSize - 1; tourCount++)
            {
                topTour = tourCount;
                for (i = topTour + 1; i < groupSize; i++)
                {
                    if (population[tourGroup[i]].Total < population[tourGroup[topTour]].Total)
                    {
                        topTour = i;
                    }
                }

                if (topTour != tourCount)
                {
                    tempTour = tourGroup[tourCount];
                    tourGroup[tourCount] = tourGroup[topTour];
                    tourGroup[topTour] = tempTour;
                }
            }

            bool foundNewBestTour = false;

            // take the best 2 tours, do crossover, and replace the worst tour with it
            childPosition = tourGroup[groupSize - 1];
            population[childPosition] = Route.Crossover(population[tourGroup[0]], population[tourGroup[1]], NodesList, rand);
            if (rand.Next(100) < mutation)
            {
                population[childPosition].Mutate(rand);
            }
            population[childPosition].DetermineTotal(NodesList);

            // now see if the first new tour has the best fitness
            if (population[childPosition].Total < population.BestRoute.Total)
            {
                population.BestRoute = population[childPosition];
                foundNewBestTour = true;
            }

            // take the best 2 tours (opposite order), do crossover, and replace the 2nd worst tour with it
            childPosition = tourGroup[groupSize - 2];
            population[childPosition] = Route.Crossover(population[tourGroup[1]], population[tourGroup[0]], NodesList, rand);
            if (rand.Next(100) < mutation)
            {
                population[childPosition].Mutate(rand);
            }
            population[childPosition].DetermineTotal(NodesList);

            // now see if the second new tour has the best fitness
            if (population[childPosition].Total < population.BestRoute.Total)
            {
                population.BestRoute = population[childPosition];
                foundNewBestTour = true;
            }

            return foundNewBestTour;
        }

        /// <summary>
        /// Raise an event to the GUI listener to display a tour.
        /// </summary>
        /// <param name="bestRoute">The best tour the algorithm has found so far.</param>
        /// <param name="generationNumber">How many generations have been performed.</param>
        /// <param name="complete">Is the TSP algorithm complete.</param>
        void displayRoute(Route bestRoute, int generationNumber, bool complete)
        {
            if (foundNewBestRoute != null)
            {
                this.foundNewBestRoute(this, new TspEventArgs(NodesList, bestRoute, generationNumber, complete));
            }
        }
    }
}
