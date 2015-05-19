using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MissionPlanner.Containers.TSP
{
    class Population : List<Tour>
    {
        /// <summary>
        /// Private copy of the best tour found so far by the Genetic Algorithm.
        /// </summary>
        private Tour bestTour = null;
        /// <summary>
        /// The best tour found so far by the Genetic Algorithm.
        /// </summary>
        public Tour BestTour
        {
            set
            {
                bestTour = value;
            }
            get
            {
                return bestTour;
            }
        }

        /// <summary>
        /// Create the initial set of random tours.
        /// </summary>
        /// <param name="populationSize">Number of tours to create.</param>
        /// <param name="cityList">The list of cities in this tour.</param>
        /// <param name="rand">Random number generator. We pass around the same random number generator, so that results between runs are consistent.</param>
        /// <param name="chanceToUseCloseCity">The odds (out of 100) that a city that is known to be close will be used in any given link.</param>
        public void CreateRandomPopulation(int populationSize, Cities cityList, Random rand, int chanceToUseCloseCity)
        {
            int firstCity, lastCity, nextCity;

            for (int tourCount = 0; tourCount < populationSize; tourCount++)
            {
                Tour tour = new Tour(cityList.Count);

                // Create a starting point for this tour
                firstCity = rand.Next(cityList.Count);
                lastCity = firstCity;

                for (int city = 0; city < cityList.Count - 1; city++)
                {
                    do
                    {
                        // Keep picking random cities for the next city, until we find one we haven't been to.
                        if ((rand.Next(100) < chanceToUseCloseCity) && (cityList[city].CloseCities.Count > 0))
                        {
                            // 75% chance will will pick a city that is close to this one
                            nextCity = cityList[city].CloseCities[rand.Next(cityList[city].CloseCities.Count)];
                        }
                        else
                        {
                            // Otherwise, pick a completely random city.
                            nextCity = rand.Next(cityList.Count);
                        }
                        // Make sure we haven't been here, and make sure it isn't where we are at now.
                    } while ((tour[nextCity].Connection2 != -1) || (nextCity == lastCity));

                    // When going from city A to B, [1] on A = B and [1] on city B = A
                    tour[lastCity].Connection2 = nextCity;
                    tour[nextCity].Connection1 = lastCity;
                    lastCity = nextCity;
                }

                // Connect the last 2 cities.
                tour[lastCity].Connection2 = firstCity;
                tour[firstCity].Connection1 = lastCity;

                tour.DetermineFitness(cityList);

                Add(tour);

                if ((bestTour == null) || (tour.Fitness < bestTour.Fitness))
                {
                    BestTour = tour;
                }
            }
        }
    }
}
