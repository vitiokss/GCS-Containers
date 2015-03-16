using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MissionPlanner.Containers
{
    [Serializable]
    class ContainerObject : ContainerStructure
    {
        // Store the indexed name of the container.
        public string Name { get; set; }

        private BayObject Bay;
        public string BayNumber;
        public int Row { get; set; } 
        public int Tier { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public bool containerLoaded { get; set; }

        // Constructor.
        public ContainerObject(BayObject _bay, int _row, int _tier)
        {
            this.Name = _bay.Id + _row.ToString() + _tier.ToString();
            this.Bay = _bay;
            this.Row = _row;
            this.Tier = _tier;
            this.BayNumber = _bay.Id;
        }

        // Constructor.
        public ContainerObject(BayObject _bay, int _row, int _tier, double _width, double _length)
        {
            this.Name = _bay.Id + _row.ToString() + _tier.ToString();
            this.Bay = _bay;
            this.Row = _row;
            this.Tier = _tier;
            this.Length = _length;
            this.Width = _width;
            this.BayNumber = _bay.Id;
        }

        // Generates container index naming.
        private string generateContainerName(BayObject b, int r, int t) {
           return b.Id + r.ToString() + t.ToString(); 
        }

        public static string generateContainerName(string id, int r, int t)
        {
            return id + r.ToString() + t.ToString(); 
        }

    }
}
