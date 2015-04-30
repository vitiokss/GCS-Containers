using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MissionPlanner.Containers
{
    [Serializable]
    public class BayObject : ContainerStructure
    {
        // Store the bay name (numbering).
        public string Id { get; set; }

        public int integerId { get; set; }

        public double LcgDeck { get; set; }
        public double LcgHold { get; set; }
        public bool NearLivingQuarter { get; set; }

        public int MaxRow { get; set; }
        public int MaxTier { get; set; }

        // Helper attributes for drawings.
        // the drawing rectable for this bay.
        public Rectangle Rect { get; set; }
        // the Is this bay selected?.
        public bool Selected { get; set; }

        // Constructor.
        public BayObject(string _id)
        {
            this.Id = _id;
            this.integerId = Convert.ToInt16(_id);
            this.NearLivingQuarter = false;
        }

        // Constructor
        public BayObject(string _id, double _lcgdeck, double _lcghold)
        {
            this.Id = _id;
            this.integerId = Convert.ToInt16(_id);
            this.LcgDeck = _lcgdeck;
            this.LcgHold = _lcghold;
            this.NearLivingQuarter = false;
        }

        // Constructor
        public BayObject(string _id, double _lcgdeck, double _lcghold, bool _nearlivingquarter)
        {
            this.Id = _id;
            this.integerId = Convert.ToInt16(_id);
            this.LcgDeck = _lcgdeck;
            this.LcgHold = _lcghold;
            this.NearLivingQuarter = _nearlivingquarter;

            this.Selected = false;
        }

        public int GetNumber()
        {
            return Convert.ToInt16(this.Id);
        }

        public override string ToString()
        {
            return String.Format("Bay: {0} - LcgDeck: {1} - LcgDeck: {2}",
                                Id, LcgDeck, LcgHold);
        }
    }
}
