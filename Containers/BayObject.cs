using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MissionPlanner.Containers
{
    [Serializable]
    class BayObject : ContainerStructure
    {
        // Store the bay name (numbering).
        public string Id { get; set; }

        public double LcgDeck { get; set; }
        public double LcgHold { get; set; }
        public bool NearLivingQuarter { get; set; }

        public int MaxRow { get; set; }
        public int MaxTier { get; set; }

        // Constructor.
        public BayObject(string _id)
        {
            this.Id = _id;
            this.NearLivingQuarter = false;
        }

        // Constructor
        public BayObject(string _id, double _lcgdeck, double _lcghold)
        {
            this.Id = _id;
            this.LcgDeck = _lcgdeck;
            this.LcgHold = _lcghold;
            this.NearLivingQuarter = false;
        }

        // Constructor
        public BayObject(string _id, double _lcgdeck, double _lcghold, bool _nearlivingquarter)
        {
            this.Id = _id;
            this.LcgDeck = _lcgdeck;
            this.LcgHold = _lcghold;
            this.NearLivingQuarter = _nearlivingquarter;
        }
    }
}
