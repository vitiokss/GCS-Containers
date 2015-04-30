using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MissionPlanner.Containers
{
    [Serializable]
    public class ContainerObject : ContainerStructure
    {
        public enum ContainerTypes { Small, Big }

        // Store the indexed name of the container.
        public string Name { get; set; }

        private BayObject BayObject;
        public string BayNumberString;
        public int Row { get; set; } 
        public int Tier { get; set; }
        public int Bay { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public bool Visible { get; set; }

        // Helper attributes for drawings.
        // the drawing rectable for this container.
        public Rectangle Rect { get; set; }
        // the Is this container selected?.
        public bool Selected { get; set; }

        public bool containerLoaded { get; set; }

        public ContainerTypes ContainerType;

        // Constructor.
        public ContainerObject(BayObject _bay, int _row, int _tier)
        {
            this.Name = generateContainerName(_bay, _row, _tier);
            this.BayObject = _bay;
            this.Bay = Convert.ToInt16(_bay.Id);
            this.Row = _row;
            this.Tier = _tier;
            this.BayNumberString = _bay.Id;
            this.ContainerType = (_bay.GetNumber() % 2 == 0) ? ContainerTypes.Big : ContainerTypes.Small;
            this.Visible = false;
        }

        // Constructor.
        public ContainerObject(BayObject _bay, int _row, int _tier, double _width, double _length)
        {
            this.Name = generateContainerName(_bay, _row, _tier);
            this.BayObject = _bay;
            this.Bay = Convert.ToInt16(_bay.Id);
            this.Row = _row;
            this.Tier = _tier;
            this.Length = _length;
            this.Width = _width;
            this.BayNumberString = _bay.Id;
            this.Visible = false;
            this.ContainerType = (_bay.GetNumber() % 2 == 0) ? ContainerTypes.Big : ContainerTypes.Small;
        }

        // Generates container index naming.
        private string generateContainerName(BayObject b, int r, int t) {
           return b.Id + r.ToString("D2") + t.ToString("D2"); 
        }

        public static string generateContainerName(string id, int r, int t)
        {
            return id + r.ToString() + t.ToString(); 
        }

        public override string ToString()
        {
            return String.Format("Bay: {0} - Row: {1} - Tier: {2}",
                                        BayNumberString, Row, Tier);
        }

    }
}
