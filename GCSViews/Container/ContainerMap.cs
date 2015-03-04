using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MissionPlanner.GCSViews.Container
{
    public partial class ContainerMap : UserControl
    {
        private int[] mContainerMap;
        private int mPadding = 10;
        private Pen mNormal = Pens.Green;
        private Pen mSelected = Pens.Green;

        List<ContainerItem> containerList = new List<ContainerItem>();

        public ContainerMap()
        {
            InitializeComponent();
            mContainerMap = new int[5] { 1, 1, 1, 0, 1 };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (this.containerList.Count == 0)
            {
                this.initializeContainerList();
            }

            foreach (ContainerItem container in this.containerList)
            {
                if (container.Selected)
                {
                    e.Graphics.DrawRectangle(this.mSelected, container.Rect);
                    e.Graphics.FillRectangle(Brushes.Green, container.Rect);
                    e.Graphics.DrawString("TEST", new Font("Arial", 20, FontStyle.Bold), Brushes.Black, new PointF(30, 30));
                }
                else
                {
                    e.Graphics.DrawRectangle(this.mNormal, container.Rect);
                    e.Graphics.FillRectangle(Brushes.Blue, container.Rect);
                    e.Graphics.DrawString("TEST", new Font("Arial", 20, FontStyle.Bold), Brushes.Black, new PointF(30, 30));
                }
            }
        }

        protected void initializeContainerList()
        {
            int width = this.Width;
            int height = this.Height;
            int nContainers = this.mContainerMap.Length;

            int itemHeight = this.Height - (this.mPadding * 2);
            int itemWidth = (this.Width - ((nContainers + 1) * this.mPadding)) / nContainers;

            for (int i = 0; i < nContainers; i++)
            {
                if (this.mContainerMap[i] == 1) {
                    Rectangle r = new Rectangle(this.mPadding + i * (itemWidth + this.mPadding), this.mPadding, itemWidth, itemHeight);
                    containerList.Add(new ContainerItem(i, r, false));
                }
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            foreach (ContainerItem container in containerList)
            {
                if (container.Rect.Contains(e.Location.X, e.Location.Y)) {
                    if (container.Selected)
                    {
                        container.Selected = false;
                        //this.onContainerUnselected.Invoke(this, container.Id);
                    }
                    else
                    {
                        container.Selected = true;
                        //this.onContainerSelected.Invoke(this, container.Id);
                    }
                }
            }
            this.Invalidate();
        }

    }
}
