using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Linq;

namespace MissionPlanner.Containers
{
    public class Vessel
    {
        // Holds all the containers on the vessel.
        public List<ContainerObject> ContainersList = new List<ContainerObject>();
        // Holds all the containers details in bays.
        public List<BayObject> BaysList = new List<BayObject>();

        private string dataFile = @"../../Containers/data.txt";
        private string loadContainersFile = @"../../Containers/loadingData.xml";

        public int LastBayNumber = 1;
        public int LastRowNumber = 0;
        public int LastTierNumber = 0;

        // Constructor.
        public Vessel()
        {
            //LoadPreviousData();
        }

        // Function to add the container to the list.
        public void AddContainerToList(ContainerObject _container)
        {
            this.ContainersList.Add(_container);
        }

        public void AddBaysToList(BayObject _bay)
        {
            this.BaysList.Add(_bay);
        }

        public bool isStructureLoaded()
        {
            return this.ContainersList.Count > 0 && this.BaysList.Count > 0;
        }

        public void GenerateContainersFile(List<BayObject> _bay)
        {
            try
            {
                XDocument doc = new XDocument(new XElement("Bays"));
                doc.Root.Add(
            from Bay in _bay
            select new XElement("Bay", new XAttribute("Name", Bay.Id),
                new XElement("Containers",
                    from container in Bay.ContainerList
                    select new XElement("Container", new XAttribute("Tier", container.Tier), new XAttribute("Row", container.Row), new XAttribute("Loaded", true))
                )
            ));
                doc.Save(this.loadContainersFile);
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(ex.Message);
            }
        }

        public void LoadContainersStructureFromXML(string filename)
        {
            BaysList.Clear();
            ContainersList.Clear();
            XDocument xml = XDocument.Load(filename);
            xml.Descendants("Bay").Select(x => new
            {
                Name = x.Attribute("Name").Value.ToString(),
                LcgDeck = Convert.ToDouble(x.Attribute("LcgDeck").Value),
                LcgHold = Convert.ToDouble(x.Attribute("LcgHold").Value),
                NearLivingQuarter = Convert.ToBoolean(x.Attribute("NearLivingQuarter").Value),
                Stack = x.Descendants("Stack").Where(s => Convert.ToInt16(s.Attribute("StartTier").Value) >= 80).Select(s => new
                {
                    Row = Convert.ToInt16(s.Attribute("Row").Value),
                    StartTier = Convert.ToInt16(s.Attribute("StartTier").Value),
                    LastTier = Convert.ToInt16(s.Attribute("LastTier").Value),
                    Container = s.Descendants("ContainerType").Where(c => Convert.ToDouble(c.Attribute("Length").Value) < 13).Where(c => Convert.ToInt16(c.Attribute("StartTier").Value) >= 80).Select(c => new
                    {
                        Length = Convert.ToDouble(c.Attribute("Length").Value),
                        Width = Convert.ToDouble(c.Attribute("Width").Value),
                    }).ToList()
                }).ToList(),
            }).ToList().ForEach(x =>
            {
                // Create Bay object.
                BayObject Bay = new BayObject(x.Name, x.LcgDeck, x.LcgHold, x.NearLivingQuarter);

                int currentBayNumber = Convert.ToInt16(x.Name);

                // Get the bays number.
                if (IsOdd(currentBayNumber) && currentBayNumber > this.LastBayNumber)
                {
                    LastBayNumber = currentBayNumber;
                }

                // Loop through all the bay stacks (floor).
                foreach (var tier in x.Stack)
                {
                    // Get the width and the length of the container type.
                    double width = tier.Container[0].Width;
                    double length = tier.Container[0].Length;
                    if (LastTierNumber < tier.LastTier)
                    {
                        LastTierNumber = tier.LastTier;
                    }
                    // Loop through all the possible tiers.
                    for (var t = tier.StartTier; t <= tier.LastTier; t += 2)
                    {

                        if (Bay.MaxRow < tier.Row)
                        {
                            // Get the rows number.
                            Bay.MaxRow = tier.Row;
                        }
                        // Save the top tier number.
                        Bay.MaxTier = tier.LastTier;
                        // Create the container as a unit object.
                        ContainerObject Container = new ContainerObject(Bay, tier.Row, t, width, length);
                        // Attach container object to the bay object.
                        Bay.AddContainerToList(Container);
                        // Add the container to the container list of the vessel.
                        this.AddContainerToList(Container);

                        if (LastRowNumber < Bay.MaxRow)
                        {
                            LastRowNumber = Bay.MaxRow;
                        }

                    }
                }
                // Add bay to the vessel.
                this.AddBaysToList(Bay);
            });
        }

        /// <summary>
        /// Helper function to check if the container is placed in the specific place.
        /// </summary>
        /// <param name="bay"></param>
        /// <param name="tier"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        protected bool containerIsPlaced(int bay, int tier, int row)
        {
            return this.ContainersList.Exists(x => x.Bay.Equals(bay) && x.Tier.Equals(tier) && x.Row.Equals(row) && x.containerLoaded);
        }

        // This function will calculate all the coordinates for container inspection.
        // Based on this coordinates the flight system will be created.
        public void CalculateContainersCoordinates(List<ContainerObject> selectedContainers)
        {
            foreach (var container in selectedContainers)
            {
                int accessibleRow = container.Row;
                int accessibleTier = container.Tier;
                int accessibleBay = container.Bay;

                double x_unit = container.Width / 2;
                double y_unit = container.Length / 2;
                double z_unit = 2.59;

                // Check if the container is surrounded by others.
                // 1. Check if there is any container placed on the top.
                // Check if the bay number is odd (20' containers).
                if (IsOdd(container.Bay))
                {
                    container.Visible = !containerIsPlaced(container.Bay, container.Tier + 2, container.Row);
                    // Check if there is no 40' container placed on the top.
                    if (container.Visible)
                    {
                        container.Visible = !containerIsPlaced(getClosestEvenNumberOfTheBay(container.Bay), container.Tier + 2, container.Row);
                    }
                    // Override the tier index.
                    accessibleTier = container.Visible ? container.Tier + 2 : accessibleTier;
                }
                //40' containers.
                else
                {
                    container.Visible = !containerIsPlaced(container.Bay, container.Tier + 2, container.Row);
                    // If the container is vissible, lets check again the odd 20' containers.
                    if (container.Visible)
                    {
                        if (!containerIsPlaced(container.Bay + 1, container.Tier + 2, container.Row) && !containerIsPlaced(container.Bay - 1, container.Tier + 2, container.Row))
                        {
                            container.Visible = true;
                        }
                        else if ((!containerIsPlaced(container.Bay + 1, container.Tier + 2, container.Row) && containerIsPlaced(container.Bay - 1, container.Tier + 2, container.Row)))
                        {
                            y_unit = y_unit - (y_unit / 2);
                            container.Visible = true;
                        }
                        else if ((containerIsPlaced(container.Bay + 1, container.Tier + 2, container.Row) && !containerIsPlaced(container.Bay - 1, container.Tier + 2, container.Row)))
                        {
                            y_unit = y_unit + (y_unit / 2);
                            container.Visible = true;
                        }
                    }
                    // Override the tier index.
                    accessibleTier = container.Visible ? container.Tier + 2 : accessibleTier;
                }

                // 2. Check if there is possible to access the container from the left side.
                if (!container.Visible)
                {
                    int tmpRow = IsOdd(container.Row) ? (container.Row != 1) ? container.Row - 2 : container.Row + 1 : container.Row + 2;
                    if (IsOdd(container.Bay))
                    {
                        // 20' container.
                        container.Visible = !containerIsPlaced(container.Bay, container.Tier, tmpRow);
                        if (container.Visible)
                        {
                            // Check the 40' container.
                            container.Visible = !containerIsPlaced(getClosestEvenNumberOfTheBay(container.Bay), container.Tier, tmpRow);
                        }
                        accessibleRow = container.Visible ? tmpRow : accessibleRow;
                    }
                    else
                    {
                        // 40' container.
                        container.Visible = !containerIsPlaced(container.Bay, container.Tier, tmpRow);
                        if (container.Visible)
                        {
                            // Check other bays.
                            if (!containerIsPlaced(container.Bay + 1, container.Tier, tmpRow) && containerIsPlaced(container.Bay - 1, container.Tier, tmpRow))
                            {
                                y_unit = y_unit - (y_unit / 2);
                                accessibleBay = container.Bay + 1;
                            }
                            else if (containerIsPlaced(container.Bay + 1, container.Tier, tmpRow) && !containerIsPlaced(container.Bay - 1, container.Tier, tmpRow))
                            {
                                y_unit = y_unit + (y_unit / 2);
                                accessibleBay = container.Bay - 1;
                            }
                        }
                    }
                }

                // 3. Check if there is any container placed on the right side.
                if (!container.Visible)
                {
                    int tmpRow = IsOdd(container.Row) ? container.Row + 2 : (container.Row != 2) ? container.Row - 2 : 1;
                    if (IsOdd(container.Bay))
                    {
                        // 20' container.
                        container.Visible = !containerIsPlaced(container.Bay, container.Tier, tmpRow);
                        if (container.Visible)
                        {
                            // Check the 40' container.
                            container.Visible = !containerIsPlaced(getClosestEvenNumberOfTheBay(container.Bay), container.Tier, tmpRow);
                        }
                        accessibleRow = container.Visible ? tmpRow : accessibleRow;
                    }
                    else
                    {
                        // 40' container.
                        container.Visible = !containerIsPlaced(container.Bay, container.Tier, tmpRow);
                        if (container.Visible)
                        {
                            // Check other bays.
                            if (!containerIsPlaced(container.Bay + 1, container.Tier, tmpRow) && containerIsPlaced(container.Bay - 1, container.Tier, tmpRow))
                            {
                                y_unit = y_unit - (y_unit / 2);
                                accessibleBay = container.Bay + 1;
                            }
                            else if (containerIsPlaced(container.Bay + 1, container.Tier, tmpRow) && !containerIsPlaced(container.Bay - 1, container.Tier, tmpRow))
                            {
                                y_unit = y_unit + (y_unit / 2);
                                accessibleBay = container.Bay - 1;
                            }
                        }
                    }
                }

                // 4. Check if container can be accessed from the front.
                if (!container.Visible)
                {

                    if (IsOdd(container.Bay))
                    {
                        if ((container.Bay - 2) > 1)
                        {
                            container.Visible = !containerIsPlaced(container.Bay - 2, container.Tier, container.Row);
                            if (container.Visible)
                            {
                                // Check that there is no 40 container placed.
                                container.Visible = !containerIsPlaced(getClosestEvenNumberOfTheBay(container.Bay - 2), container.Tier, container.Row);
                            }
                            accessibleBay = container.Visible ? container.Bay - 2 : accessibleBay;
                        }
                    }
                    else
                    {
                        if ((container.Bay - 3) > 1) {
                            container.Visible = !containerIsPlaced(container.Bay - 3, container.Tier, container.Row);
                            if (container.Visible)
                            {
                                container.Visible = !containerIsPlaced(getClosestEvenNumberOfTheBay(container.Bay - 3), container.Tier, container.Row);
                            }
                            accessibleBay = container.Visible ? container.Bay - 3 : accessibleBay;
                        }
                    }

                }

                // 5. Check if container can be accessed from the back.
                if (!container.Visible)
                {

                    if (IsOdd(container.Bay))
                    {
                        container.Visible = !containerIsPlaced(container.Bay + 2, container.Tier, container.Row);
                        if (container.Visible)
                        {
                            // Check that there is no 40 container placed.
                            container.Visible = !containerIsPlaced(getClosestEvenNumberOfTheBay(container.Bay + 2), container.Tier, container.Row);
                        }
                        accessibleBay = container.Visible ? container.Bay + 2 : accessibleBay;
                    }
                    else
                    {
                        container.Visible = !containerIsPlaced(container.Bay + 3, container.Tier, container.Row);
                        if (container.Visible)
                        {
                            container.Visible = !containerIsPlaced(getClosestEvenNumberOfTheBay(container.Bay + 3), container.Tier, container.Row);
                        }
                        accessibleBay = container.Visible ? container.Bay + 3 : accessibleBay;
                    }

                }

                // If the container is not accessible from any side, make sure that we fly only on top of the stack.
                if (!container.Visible)
                {
                    accessibleTier = this.BaysList[0].MaxTier;
                }

                // We place the games on track and start calculating the coordinates from the front and left side.
                // Get the maximum row number from the vessel.
                container.X = x_unit * rowIndex(accessibleRow);
                container.Y = y_unit * bayIndex(accessibleBay);

                container.Z = z_unit * tierIndex(accessibleTier);
                if (container.Tier == accessibleTier)
                {
                    container.Z += 0.5; //offset not to land on the container.
                }
            }
        }

        /// <summary>
        /// From the given bay id, returns the closest even number bay id.
        /// </summary>
        /// <param name="bayId"></param>
        /// <returns></returns>
        protected int getClosestEvenNumberOfTheBay(int bayId)
        {
            if (!IsOdd(bayId))
            {
                return bayId;
            }

            if ((bayId - 1) % 4 == 1)
            {
                return (bayId - 1);
            }
            else
            {
                return bayId + 1;
            }

        }

        public void LoadContainersPlacement(string FileName)
        {
            XDocument xml = XDocument.Load(FileName);
            xml.Descendants("Bay").Select(x => new
            {
                Name = x.Attribute("Name").Value,
                Containers = x.Descendants("Container").Select(c => new
                {
                    Row = Convert.ToInt16(c.Attribute("Row").Value),
                    Tier = Convert.ToInt16(c.Attribute("Tier").Value),
                    Loaded = Convert.ToBoolean(c.Attribute("Loaded").Value)
                })
            }).ToList().ForEach(d =>
            {
                foreach (var item in d.Containers)
                {
                    ContainerObject container = this.ContainersList.Find(i => i.BayNumberString == d.Name && i.Row == item.Row && i.Tier == item.Tier);
                    container.containerLoaded = item.Loaded;
                }
            });
        }

        public static bool IsOdd(int value)
        {
            return value % 2 != 0;
        }

        // Calculate the position of the bay in terms of cartasian coordinate system. (y)
        public int bayIndex(int bay_number)
        {
            int bayIndex = 0;

            if (IsOdd(bay_number))
            {
                bayIndex = (bay_number + 1) / 2;
            }
            else
            {
                bayIndex = (bay_number + 2) / 4;
            }
            return bayIndex;
        }

        // Calculate the position of the tier in terms of cartasian coordinate system. (z)
        public int tierIndex(int tier_number)
        {
            int tierIndex = 0;
            tierIndex = (tier_number - 80) / 2;
            return tierIndex;
        }

        // Calculate the position of the row in terms of cartasian coordiante system. (x)
        public int rowIndex(int row_number)
        {
            int rowIndex = 0;

            int lastEvenNumberOfContainer = IsOdd(LastRowNumber) ? LastRowNumber - 1 : LastRowNumber;
            if (IsOdd(row_number))
            {
                rowIndex = (lastEvenNumberOfContainer + row_number + 1) / 2;
            }
            else
            {
                if (row_number == lastEvenNumberOfContainer)
                {
                    rowIndex = 1;
                }
                else
                {
                    rowIndex = (lastEvenNumberOfContainer - row_number) / 2;
                }
            }
            return rowIndex;
        }

        public void LoadPreviousData()
        {

            // Check if we had previously Save information of our friends
            // previously
            if (File.Exists(this.dataFile))
            {

                try
                {
                    FileInfo file = new FileInfo(this.dataFile);
                    if (file.Length != 0)
                    {
                        // Load data from file.
                        FileStream reader = new FileStream(dataFile, FileMode.Open, FileAccess.Read);
                        reader.Position = 0;
                        BinaryFormatter formatter = new BinaryFormatter();
                        Dictionary<string, ArrayList> data = (Dictionary<string, ArrayList>)formatter.Deserialize(reader);

                        //this.BaysList = data["Bays"];
                        //this.ContainersList = data["Containers"];

                        reader.Close();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("There seems to be a file that contains " +
                        "friends information but somehow there is a problem " +
                        "with reading it.");
                } // end try-catch

            } // end if
        }

        /// <summary>
        /// Save containers data into the file. Reason is that no need to load same file each execution.
        /// </summary>
        public void SaveDataToFile()
        {

            Dictionary<string, ContainerStructure> data = new Dictionary<string, ContainerStructure>();
            //data.Add("Bays", BaysList);
            //data.Add("Containers", ContainersList);

            // Gain code access to the file that we are going
            // to write to
            try
            {
                // Create a FileStream that will write data to file.
                FileStream writerFileStream =
                    new FileStream(this.dataFile, FileMode.Create, FileAccess.Write);
                // Save our dictionary of friends to file
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(writerFileStream, data);

                // Close the writerFileStream when we are done.
                writerFileStream.Close();
            }
            catch (Exception)
            {
                Console.WriteLine("Unable to save containers information");
            } // end try-catch

        }
    }
}
