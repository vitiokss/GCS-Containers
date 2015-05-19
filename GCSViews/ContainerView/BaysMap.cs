using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MissionPlanner.Containers;

namespace MissionPlanner.GCSViews.ContainerView
{


    public partial class BaysMap : UserControl
    {
        public enum Screen { Bays, Containers }

        // the top space where we will write the bay numbers.
        private int descriptionPadding = 30;
        private double distanceBetweenOddBays = 1;
        private double distanceBetweenEvenBays = 4;
        private double distanceBetweenContainers = 2;

        // the number of bays to draw.
        private int nBays;
        private int mSelectedOddBay;
        private int mSelectedEventBay;

        private double bayWidth, bayHeight, containerWidth, containerHeight;

        // typograghy settings.
        private Font font = new Font("Verdana", 8, FontStyle.Regular);
        private Brush mTextBrush = Brushes.Black;
        private Brush mNormalBrush = Brushes.Gray;
        private Brush mSelectedBrush = Brushes.Green;
        private Pen mPen = Pens.Black;
        StringFormat stringFormat = new StringFormat();

        public Screen mCurerntScreen = Screen.Bays;


        // the Vessel object with list of all bays to draw, the containers are inside
        // the bays.
        private Vessel mVessel;

        // Our event delegates.
        public delegate void BayHoverEventHandler(object sender, BayObject bay);
        public delegate void ContainerHoverEventHandler(object sender, ContainerObject container);


        // Event handlers
        [Category("Action")]
        [Description("Fires when Bay is hovered")]
        public event BayHoverEventHandler BayHovered;

        [Category("Action")]
        [Description("Fires when Container is hovered")]
        public event ContainerHoverEventHandler ContainerHovered;

        public BaysMap()
        {
            InitializeComponent();

            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            this.mCurerntScreen = Screen.Bays;
        }



        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (this.mVessel == null)
            {
                return;
            }

            switch (this.mCurerntScreen)
            {
                case Screen.Bays:
                    this.CalculateBaysDimentions();
                    this.DrawBays(e);
                    break;

                case Screen.Containers:
                    this.CalculateContainersDimentions();
                    this.DrawContainers(e);
                    break;

            }
        }

        /// <summary>
        /// Update dimentaitons for bays before drawing, should be called on map initalize and on over resize.
        /// </summary>
        private void CalculateBaysDimentions()
        {
            this.nBays = (mVessel.LastBayNumber + 1) / 2;

            this.bayWidth = (this.Width - (((mVessel.LastBayNumber + 1) / 4 * this.distanceBetweenOddBays) + ((mVessel.LastBayNumber - 1) / 4 * this.distanceBetweenEvenBays))) / (this.nBays);
            this.bayHeight = this.Height - this.descriptionPadding;

            foreach (BayObject bay in mVessel.BaysList)
            {
                // Get the numeric bay number.
                int bayNumber = bay.GetNumber();

                // We only draw odd bay numbers, skip even ones.
                if (bayNumber % 2 != 0)
                {
                    int x = this.Width - (int)((((bayNumber + 1) / 2) * bayWidth) + ((bayNumber + 1) / 4 * this.distanceBetweenOddBays) + ((bayNumber - 1) / 4 * this.distanceBetweenEvenBays));
                    int y = this.descriptionPadding;

                    bay.Rect = new Rectangle(x, y, (int)this.bayWidth, (int)this.bayHeight);
                }
            }
        }


        protected void DrawBays(PaintEventArgs e)
        {
            foreach (BayObject bay in mVessel.BaysList)
            {
                // Get the numeric bay number.
                int bayNumber = bay.GetNumber();

                // Skip even bays.
                if (bayNumber % 2 != 0)
                {
                    e.Graphics.DrawRectangle(this.mPen, bay.Rect);
                    Brush b = bay.Selected ? mSelectedBrush : mNormalBrush;

                    // draw the bay rectangles.
                    e.Graphics.FillRectangle(b, bay.Rect);

                    // darw bay labels.
                    Rectangle r = new Rectangle(bay.Rect.X, descriptionPadding / 2, (int)bayWidth, descriptionPadding / 2);
                    e.Graphics.DrawString(bay.Id, font, b, r, stringFormat);

                    // draw the even bay labels.
                    if (bayNumber % 4 == 3)
                    {
                        r = new Rectangle(bay.Rect.X, 0, (int)((bayWidth * 2) + distanceBetweenOddBays), descriptionPadding / 2);
                        // TODO: get the event bay label in the correct way so it would be 01 instead of 1.
                        e.Graphics.DrawString((bayNumber - 1).ToString(), font, this.mNormalBrush, r, stringFormat);
                    }
                }
            }
        }


        /// <summary>
        /// Update dimentaitons for Containers before drawing, should be called on map initalize and on over resize.
        /// </summary>
        private void CalculateContainersDimentions()
        {
            BayObject OddBay, EvenBay;
            int maxTier, maxRow, nTires, nRows;

            List<ContainerObject> ContainerList = new List<ContainerObject>();

            OddBay = EvenBay = null;
            int both = 0;
            foreach (BayObject bay in mVessel.BaysList)
            {
                if (bay.GetNumber() == this.mSelectedOddBay)
                {
                    OddBay = bay;
                    ContainerList.AddRange(bay.ContainerList);
                    both++;
                }

                if (bay.GetNumber() == this.mSelectedEventBay)
                {
                    EvenBay = bay;
                    ContainerList.AddRange(bay.ContainerList);
                    both++;
                }

                if (both == 2) {break;}
            }

            if (OddBay == null)
            {
                this.mCurerntScreen = Screen.Bays;
                this.Invalidate();
                return;
            }

            // Getting max tier.
            maxRow = Math.Max(OddBay.MaxRow, EvenBay.MaxRow);
            maxTier = Math.Max(OddBay.MaxTier, EvenBay.MaxTier);

            // Calculating the number of tiers to draw.
            nTires = (maxTier - 80) / 2;
            nTires = Math.Max(nTires, 0);

            // Calculating the number of rows to draw.
            // TODO if the number of rows is odd, then we have a 00 row, which means we should add +1;
            nRows = maxRow;

            this.containerWidth = (this.Width - (this.descriptionPadding * 2) - (distanceBetweenContainers * nRows)) / nRows;
            this.containerHeight = (this.Height - this.descriptionPadding - (distanceBetweenContainers * nTires)) / nTires;

            foreach (ContainerObject c in ContainerList)
            {
                double x = 0, y = 0, xCenter = 0;

                xCenter = this.Width / 2;

                // Odd raws will be drawn on the right side
                if (c.Row % 2 == 0)
                {
                    x = (int)(xCenter - ((c.Row / 2) * (this.containerWidth + distanceBetweenContainers)));
                }
                // Even rows will be raws on the left side.
                else
                {
                    x = (int)(xCenter + (((c.Row - 1) / 2) * (this.containerWidth + distanceBetweenContainers)));
                }

                y = (int)((this.Height - descriptionPadding) - (((c.Tier - 80) / 2) * (containerHeight + distanceBetweenContainers)));

                c.Rect = new Rectangle((int)x, (int)y, (int)this.containerWidth, (int)this.containerHeight);
            }
        }


        protected void DrawContainers(PaintEventArgs e)
        {
            foreach (BayObject bay in mVessel.BaysList)
            {
                // Get the numeric bay number.
                int bayNumber = bay.GetNumber();

                // Skip even bays.
                if (bayNumber == this.mSelectedEventBay || bayNumber == this.mSelectedOddBay)
                {
                    foreach (ContainerObject c in bay.ContainerList)
                    {
                        if (!c.containerLoaded)
                        {
                            continue;
                        }

                        string containerSize = c.ContainerType == ContainerObject.ContainerTypes.Big ? "40" : "20";
                        e.Graphics.DrawRectangle(this.mPen, c.Rect);

                        Brush b = c.Selected ? mSelectedBrush : mNormalBrush;

                        e.Graphics.FillRectangle(b, c.Rect);
                        e.Graphics.DrawString(containerSize, font, this.mTextBrush, c.Rect, stringFormat);

                        // Draw row number labels
                        Rectangle r = new Rectangle(c.Rect.X, this.Height - descriptionPadding, (int)containerWidth, descriptionPadding);
                        e.Graphics.DrawString(c.Row.ToString(), font, b, r, stringFormat);

                        // Draw Tier number labels
                        r = new Rectangle(0, c.Rect.Y, descriptionPadding, (int)containerHeight);
                        e.Graphics.DrawString(c.Tier.ToString(), font, b, r, stringFormat);

                        r = new Rectangle(this.Width - descriptionPadding, c.Rect.Y, descriptionPadding, (int)containerHeight);
                        e.Graphics.DrawString(c.Tier.ToString(), font, b, r, stringFormat);
                    }
                }
            }
        }


        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            switch (mCurerntScreen)
            {
                case Screen.Bays:
                    foreach (BayObject bay in mVessel.BaysList)
                    {
                        if (bay.Rect.Contains(e.Location.X, e.Location.Y))
                        {
                            if (bay.Selected)
                            {
                                bay.Selected = false;
                            }
                            else
                            {
                                bay.Selected = true;

                                this.mSelectedOddBay = bay.GetNumber();

                                if (mSelectedOddBay % 4 == 1)
                                {
                                    mSelectedEventBay = mSelectedOddBay + 1;
                                }
                                else
                                {
                                    mSelectedEventBay = mSelectedOddBay - 1;
                                }
                            }
                        }
                        else
                        {
                            bay.Selected = false;
                        }
                    }
                    break;

                case Screen.Containers:
                    foreach (BayObject bay in mVessel.BaysList)
                    {
                        // Get the numeric bay number.
                        int bayNumber = bay.GetNumber();

                        // if it is one of the selected bays.
                        if (bayNumber == this.mSelectedEventBay || bayNumber == this.mSelectedOddBay)
                        {
                            foreach (ContainerObject c in bay.ContainerList)
                            {
                                if (c.Rect.Contains(e.Location))
                                {
                                    c.Selected = !c.Selected;
                                }
                            }
                        }
                    }
                    break;
            }


            // Redraw everything.
            this.Invalidate();

        }


        protected override void OnAutoSizeChanged(EventArgs e)
        {
            base.OnAutoSizeChanged(e);

            this.Invalidate();
        }


        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Invalidate();
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);

            // Switch to the next screen.
            if (this.mCurerntScreen == Screen.Bays)
            {
                this.mCurerntScreen = Screen.Containers;
            }
            else
            {
                this.mCurerntScreen = Screen.Bays;
            }

            // Redraw.
            this.Invalidate();

        }

        internal void setVessel(Vessel CargoShip)
        {
            this.mVessel = CargoShip;
            this.Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            Point Location = this.PointToClient(Cursor.Position);
            base.OnMouseMove(e);

            switch (mCurerntScreen)
            {
                case Screen.Bays:
                    foreach (BayObject bay in mVessel.BaysList)
                    {
                        if (bay.Rect.Contains(Location.X, Location.Y))
                        {
                            BayHovered(this, bay);
                        }
                    }
                    break;

                case Screen.Containers:
                    foreach (BayObject bay in mVessel.BaysList)
                    {
                        // Get the numeric bay number.
                        int bayNumber = bay.GetNumber();

                        // if it is one of the selected bays.
                        if (bayNumber == this.mSelectedEventBay || bayNumber == this.mSelectedOddBay)
                        {
                            foreach (ContainerObject c in bay.ContainerList)
                            {
                                if (!c.containerLoaded)
                                {
                                    continue;
                                }

                                if (c.Rect.Contains(Location.X, Location.Y))
                                {
                                    ContainerHovered(this, c);
                                }
                            }
                        }
                    }
                    break;
            }
        }
    }
}
