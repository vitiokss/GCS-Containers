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
    class Vessel
    {
        // Holds all the containers on the vessel.
        private List<ContainerObject> ContainersList = new List<ContainerObject>();
        // Holds all the containers details in bays.
        public List<BayObject> BaysList = new List<BayObject>();

        private string dataFile = @"../../Containers/data.txt";
        private string loadContainersFile = @"../../Containers/loadingData.xml";

        public int LastBayNumber = 1;
        public int LastRowNumber = 0; 

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
                    Container = s.Descendants("ContainerType").Where(c => Convert.ToDouble(c.Attribute("Length").Value) < 13).Where(c => Convert.ToInt16(c.Attribute("StartTier").Value) >= 80).Select(c => new {
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
                    // Loop through all the possible tiers.
                    for (var t = tier.StartTier; t <= tier.LastTier; t+=2) {

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

            //this.SaveDataToFile();
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
            }).ToList().ForEach(d => { 
                foreach (var item in d.Containers) {
                    ContainerObject container = this.ContainersList.Find(i => i.BayNumber == d.Name && i.Row == item.Row && i.Tier == item.Tier);
                    container.containerLoaded = item.Loaded;
                }
            });
        }

        public static bool IsOdd(int value)
        {
            return value % 2 != 0;
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
