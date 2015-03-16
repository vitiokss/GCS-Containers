namespace MissionPlanner.GCSViews
{
    partial class ContainersView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.GOTGroup = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.gotCalibrationLbl = new MissionPlanner.Controls.MyLabel();
            this.gotCalibrateBtn = new MissionPlanner.Controls.MyButton();
            this.groupGotTransmitters = new System.Windows.Forms.GroupBox();
            this.transmittersList = new System.Windows.Forms.ListBox();
            this.groupGotReceivers = new System.Windows.Forms.GroupBox();
            this.receiversList = new System.Windows.Forms.ListBox();
            this.gotConnectionStatusLbl = new MissionPlanner.Controls.MyLabel();
            this.gotConnect = new MissionPlanner.Controls.MyButton();
            this.droneGroup = new System.Windows.Forms.GroupBox();
            this.containersGroup = new System.Windows.Forms.GroupBox();
            this.importContainersStructureLbl = new MissionPlanner.Controls.MyLabel();
            this.importStructureBtn = new MissionPlanner.Controls.MyButton();
            this.scanBtn = new MissionPlanner.Controls.MyButton();
            this.importContainersLoadingBtn = new MissionPlanner.Controls.MyButton();
            this.GOTGroup.SuspendLayout();
            this.groupGotTransmitters.SuspendLayout();
            this.groupGotReceivers.SuspendLayout();
            this.containersGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // GOTGroup
            // 
            this.GOTGroup.AutoSize = true;
            this.GOTGroup.Controls.Add(this.textBox1);
            this.GOTGroup.Controls.Add(this.gotCalibrationLbl);
            this.GOTGroup.Controls.Add(this.gotCalibrateBtn);
            this.GOTGroup.Controls.Add(this.groupGotTransmitters);
            this.GOTGroup.Controls.Add(this.groupGotReceivers);
            this.GOTGroup.Controls.Add(this.gotConnectionStatusLbl);
            this.GOTGroup.Controls.Add(this.gotConnect);
            this.GOTGroup.Location = new System.Drawing.Point(10, 10);
            this.GOTGroup.Margin = new System.Windows.Forms.Padding(10);
            this.GOTGroup.Name = "GOTGroup";
            this.GOTGroup.Padding = new System.Windows.Forms.Padding(5);
            this.GOTGroup.Size = new System.Drawing.Size(409, 157);
            this.GOTGroup.TabIndex = 0;
            this.GOTGroup.TabStop = false;
            this.GOTGroup.Text = "Games on track position system";
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(9, 81);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(184, 42);
            this.textBox1.TabIndex = 6;
            this.textBox1.Text = "Info: In order to be able to calibrate position system the calibration triangle i" +
    "s needed.";
            // 
            // gotCalibrationLbl
            // 
            this.gotCalibrationLbl.Location = new System.Drawing.Point(8, 51);
            this.gotCalibrationLbl.Name = "gotCalibrationLbl";
            this.gotCalibrationLbl.resize = false;
            this.gotCalibrationLbl.Size = new System.Drawing.Size(75, 23);
            this.gotCalibrationLbl.TabIndex = 5;
            this.gotCalibrationLbl.Text = "Not calibrated";
            // 
            // gotCalibrateBtn
            // 
            this.gotCalibrateBtn.Enabled = false;
            this.gotCalibrateBtn.Location = new System.Drawing.Point(118, 51);
            this.gotCalibrateBtn.Name = "gotCalibrateBtn";
            this.gotCalibrateBtn.Size = new System.Drawing.Size(75, 23);
            this.gotCalibrateBtn.TabIndex = 4;
            this.gotCalibrateBtn.Text = "Calibrate";
            this.gotCalibrateBtn.UseVisualStyleBackColor = true;
            this.gotCalibrateBtn.Click += new System.EventHandler(this.gotCalibrateBtn_Click);
            // 
            // groupGotTransmitters
            // 
            this.groupGotTransmitters.Controls.Add(this.transmittersList);
            this.groupGotTransmitters.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.groupGotTransmitters.Location = new System.Drawing.Point(309, 21);
            this.groupGotTransmitters.Name = "groupGotTransmitters";
            this.groupGotTransmitters.Size = new System.Drawing.Size(92, 114);
            this.groupGotTransmitters.TabIndex = 3;
            this.groupGotTransmitters.TabStop = false;
            this.groupGotTransmitters.Text = "Transmitters";
            // 
            // transmittersList
            // 
            this.transmittersList.BackColor = System.Drawing.SystemColors.Window;
            this.transmittersList.FormattingEnabled = true;
            this.transmittersList.Location = new System.Drawing.Point(7, 20);
            this.transmittersList.Name = "transmittersList";
            this.transmittersList.Size = new System.Drawing.Size(79, 82);
            this.transmittersList.TabIndex = 0;
            // 
            // groupGotReceivers
            // 
            this.groupGotReceivers.Controls.Add(this.receiversList);
            this.groupGotReceivers.Location = new System.Drawing.Point(211, 21);
            this.groupGotReceivers.Name = "groupGotReceivers";
            this.groupGotReceivers.Size = new System.Drawing.Size(92, 114);
            this.groupGotReceivers.TabIndex = 2;
            this.groupGotReceivers.TabStop = false;
            this.groupGotReceivers.Text = "Receivers";
            // 
            // receiversList
            // 
            this.receiversList.BackColor = System.Drawing.SystemColors.Window;
            this.receiversList.FormattingEnabled = true;
            this.receiversList.HorizontalScrollbar = true;
            this.receiversList.Location = new System.Drawing.Point(7, 20);
            this.receiversList.Name = "receiversList";
            this.receiversList.Size = new System.Drawing.Size(79, 82);
            this.receiversList.TabIndex = 0;
            // 
            // gotConnectionStatusLbl
            // 
            this.gotConnectionStatusLbl.Location = new System.Drawing.Point(8, 21);
            this.gotConnectionStatusLbl.Name = "gotConnectionStatusLbl";
            this.gotConnectionStatusLbl.resize = false;
            this.gotConnectionStatusLbl.Size = new System.Drawing.Size(104, 23);
            this.gotConnectionStatusLbl.TabIndex = 1;
            this.gotConnectionStatusLbl.Text = "Status: Offline";
            // 
            // gotConnect
            // 
            this.gotConnect.Location = new System.Drawing.Point(118, 21);
            this.gotConnect.Name = "gotConnect";
            this.gotConnect.Size = new System.Drawing.Size(75, 23);
            this.gotConnect.TabIndex = 0;
            this.gotConnect.Text = "Connect";
            this.gotConnect.UseVisualStyleBackColor = true;
            this.gotConnect.Click += new System.EventHandler(this.gotConnect_Click);
            // 
            // droneGroup
            // 
            this.droneGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.droneGroup.Location = new System.Drawing.Point(429, 10);
            this.droneGroup.Name = "droneGroup";
            this.droneGroup.Size = new System.Drawing.Size(303, 157);
            this.droneGroup.TabIndex = 1;
            this.droneGroup.TabStop = false;
            this.droneGroup.Text = "Drone";
            // 
            // containersGroup
            // 
            this.containersGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.containersGroup.Controls.Add(this.importContainersLoadingBtn);
            this.containersGroup.Controls.Add(this.importContainersStructureLbl);
            this.containersGroup.Controls.Add(this.importStructureBtn);
            this.containersGroup.Location = new System.Drawing.Point(10, 180);
            this.containersGroup.Name = "containersGroup";
            this.containersGroup.Size = new System.Drawing.Size(722, 208);
            this.containersGroup.TabIndex = 2;
            this.containersGroup.TabStop = false;
            this.containersGroup.Text = "Containers information";
            // 
            // importContainersStructureLbl
            // 
            this.importContainersStructureLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.importContainersStructureLbl.Location = new System.Drawing.Point(359, 19);
            this.importContainersStructureLbl.Name = "importContainersStructureLbl";
            this.importContainersStructureLbl.resize = false;
            this.importContainersStructureLbl.Size = new System.Drawing.Size(142, 23);
            this.importContainersStructureLbl.TabIndex = 1;
            this.importContainersStructureLbl.Text = "Import containers structure";
            // 
            // importStructureBtn
            // 
            this.importStructureBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.importStructureBtn.Location = new System.Drawing.Point(622, 19);
            this.importStructureBtn.Name = "importStructureBtn";
            this.importStructureBtn.Size = new System.Drawing.Size(94, 23);
            this.importStructureBtn.TabIndex = 0;
            this.importStructureBtn.Text = "Import vessel structure";
            this.importStructureBtn.UseVisualStyleBackColor = true;
            this.importStructureBtn.Click += new System.EventHandler(this.importStructureBtn_Click);
            // 
            // scanBtn
            // 
            this.scanBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.scanBtn.Location = new System.Drawing.Point(632, 394);
            this.scanBtn.Name = "scanBtn";
            this.scanBtn.Size = new System.Drawing.Size(100, 23);
            this.scanBtn.TabIndex = 3;
            this.scanBtn.Text = "Scan containers";
            this.scanBtn.UseVisualStyleBackColor = true;
            // 
            // importContainersLoadingBtn
            // 
            this.importContainersLoadingBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.importContainersLoadingBtn.Enabled = false;
            this.importContainersLoadingBtn.Location = new System.Drawing.Point(507, 19);
            this.importContainersLoadingBtn.Name = "importContainersLoadingBtn";
            this.importContainersLoadingBtn.Size = new System.Drawing.Size(109, 23);
            this.importContainersLoadingBtn.TabIndex = 2;
            this.importContainersLoadingBtn.Text = "Load the containers on vessel";
            this.importContainersLoadingBtn.UseVisualStyleBackColor = true;
            this.importContainersLoadingBtn.Click += new System.EventHandler(this.importContainersLoadingBtn_Click);
            // 
            // ContainersView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scanBtn);
            this.Controls.Add(this.containersGroup);
            this.Controls.Add(this.droneGroup);
            this.Controls.Add(this.GOTGroup);
            this.Name = "ContainersView";
            this.Size = new System.Drawing.Size(746, 420);
            this.GOTGroup.ResumeLayout(false);
            this.GOTGroup.PerformLayout();
            this.groupGotTransmitters.ResumeLayout(false);
            this.groupGotReceivers.ResumeLayout(false);
            this.containersGroup.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox GOTGroup;
        private System.Windows.Forms.GroupBox droneGroup;
        private System.Windows.Forms.GroupBox containersGroup;
        private Controls.MyLabel importContainersStructureLbl;
        private Controls.MyButton importStructureBtn;
        private Controls.MyButton scanBtn;
        private Controls.MyButton gotConnect;
        private Controls.MyLabel gotConnectionStatusLbl;
        private System.Windows.Forms.GroupBox groupGotTransmitters;
        private System.Windows.Forms.GroupBox groupGotReceivers;
        private System.Windows.Forms.ListBox transmittersList;
        private System.Windows.Forms.ListBox receiversList;
        private Controls.MyButton gotCalibrateBtn;
        private Controls.MyLabel gotCalibrationLbl;
        private System.Windows.Forms.TextBox textBox1;
        private ContainerView.ContainerMap containerMapCtrl;
        private Controls.MyButton importContainersLoadingBtn;

    }
}
