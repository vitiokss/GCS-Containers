using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MissionPlanner.GCSViews.ContainerView
{
    public partial class BaysMap : UserControl
    {
        private int iBayLastIndex;
        private int drawBaysNumber;
        public BaysMap()
        {
            InitializeComponent();
            iBayLastIndex = 91;

            drawBaysNumber = getBaysNumberFromIndex(iBayLastIndex);

        }

        /// <summary>
        /// Get the bays number to draw on the map.
        /// </summary>
        /// <param name="lastIndex">The last index number from the structure</param>
        /// <returns>Drawable bays</returns>
        private int getBaysNumberFromIndex(int lastIndex)
        {
            int bays40Number = (int) (Math.Round((double)(lastIndex / 2)) / 2);
            return lastIndex - bays40Number;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

        }

        public class Bays
        {
            public int Id { get; set; }

            /// <summary>
            /// Class constructor
            /// </summary>
            public Bays()
            {

            }
        }

    }
}
