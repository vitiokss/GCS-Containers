using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MissionPlanner.GCSViews.ContainerView
{
    class ContainerItem
    {
        public int Id { get; set; }
        public Rectangle Rect { get; set; }
        public bool Selected { get; set; }

        public ContainerItem(int id, Rectangle rectangle, bool isSelected)
        {
            this.Id = id;
            this.Rect = rectangle;
            this.Selected = isSelected;
        }
    }
}
