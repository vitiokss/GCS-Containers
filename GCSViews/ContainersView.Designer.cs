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
            this.myButton1 = new MissionPlanner.Controls.MyButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.gotCalibrationLbl = new MissionPlanner.Controls.MyLabel();
            this.gotCalibrateBtn = new MissionPlanner.Controls.MyButton();
            this.groupGotTransmitters = new System.Windows.Forms.GroupBox();
            this.transmittersList = new System.Windows.Forms.ListBox();
            this.groupGotReceivers = new System.Windows.Forms.GroupBox();
            this.receiversList = new System.Windows.Forms.ListBox();
            this.gotConnectionStatusLbl = new MissionPlanner.Controls.MyLabel();
            this.gotConnect = new MissionPlanner.Controls.MyButton();
            this.zLabel = new MissionPlanner.Controls.MyLabel();
            this.yCoordinateTxt = new MissionPlanner.Controls.MyLabel();
            this.xCoordinateTxt = new MissionPlanner.Controls.MyLabel();
            this.droneGroup = new System.Windows.Forms.GroupBox();
            this.commandLabel = new MissionPlanner.Controls.MyLabel();
            this.containersGroup = new System.Windows.Forms.GroupBox();
            this.importContainersLoadingBtn = new MissionPlanner.Controls.MyButton();
            this.importContainersStructureLbl = new MissionPlanner.Controls.MyLabel();
            this.importStructureBtn = new MissionPlanner.Controls.MyButton();
            this.scanBtn = new MissionPlanner.Controls.MyButton();
            this.lblHoveredEelement = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.myButton2 = new MissionPlanner.Controls.MyButton();
            this.mBayMap = new MissionPlanner.GCSViews.ContainerView.BaysMap();
            this.yawLbl = new MissionPlanner.Controls.MyLabel();
            this.GOTGroup.SuspendLayout();
            this.groupGotTransmitters.SuspendLayout();
            this.groupGotReceivers.SuspendLayout();
            this.droneGroup.SuspendLayout();
            this.containersGroup.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // GOTGroup
            // 
            this.GOTGroup.AutoSize = true;
            this.GOTGroup.Controls.Add(this.myButton1);
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
            this.GOTGroup.Size = new System.Drawing.Size(409, 191);
            this.GOTGroup.TabIndex = 0;
            this.GOTGroup.TabStop = false;
            this.GOTGroup.Text = "Games on track position system";
            // 
            // myButton1
            // 
            this.myButton1.Location = new System.Drawing.Point(9, 129);
            this.myButton1.Name = "myButton1";
            this.myButton1.Size = new System.Drawing.Size(75, 23);
            this.myButton1.TabIndex = 6;
            this.myButton1.Text = "Restart GOT";
            this.myButton1.UseVisualStyleBackColor = true;
            this.myButton1.Click += new System.EventHandler(this.myButton1_Click);
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
            // zLabel
            // 
            this.zLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zLabel.Location = new System.Drawing.Point(11, 21);
            this.zLabel.Name = "zLabel";
            this.zLabel.resize = false;
            this.zLabel.Size = new System.Drawing.Size(369, 25);
            this.zLabel.TabIndex = 2;
            // 
            // yCoordinateTxt
            // 
            this.yCoordinateTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.yCoordinateTxt.Location = new System.Drawing.Point(11, 81);
            this.yCoordinateTxt.Name = "yCoordinateTxt";
            this.yCoordinateTxt.resize = false;
            this.yCoordinateTxt.Size = new System.Drawing.Size(260, 23);
            this.yCoordinateTxt.TabIndex = 1;
            // 
            // xCoordinateTxt
            // 
            this.xCoordinateTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xCoordinateTxt.Location = new System.Drawing.Point(11, 52);
            this.xCoordinateTxt.Name = "xCoordinateTxt";
            this.xCoordinateTxt.resize = false;
            this.xCoordinateTxt.Size = new System.Drawing.Size(260, 23);
            this.xCoordinateTxt.TabIndex = 0;
            // 
            // droneGroup
            // 
            this.droneGroup.Controls.Add(this.yawLbl);
            this.droneGroup.Controls.Add(this.commandLabel);
            this.droneGroup.Controls.Add(this.zLabel);
            this.droneGroup.Controls.Add(this.yCoordinateTxt);
            this.droneGroup.Controls.Add(this.xCoordinateTxt);
            this.droneGroup.Location = new System.Drawing.Point(429, 10);
            this.droneGroup.Name = "droneGroup";
            this.droneGroup.Size = new System.Drawing.Size(478, 190);
            this.droneGroup.TabIndex = 1;
            this.droneGroup.TabStop = false;
            this.droneGroup.Text = "Motion command";
            // 
            // commandLabel
            // 
            this.commandLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.commandLabel.ForeColor = System.Drawing.Color.Green;
            this.commandLabel.Location = new System.Drawing.Point(6, 151);
            this.commandLabel.Name = "commandLabel";
            this.commandLabel.resize = false;
            this.commandLabel.Size = new System.Drawing.Size(466, 33);
            this.commandLabel.TabIndex = 3;
            this.commandLabel.Text = "COMMAND:";
            // 
            // containersGroup
            // 
            this.containersGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.containersGroup.Controls.Add(this.mBayMap);
            this.containersGroup.Location = new System.Drawing.Point(10, 235);
            this.containersGroup.Name = "containersGroup";
            this.containersGroup.Size = new System.Drawing.Size(897, 237);
            this.containersGroup.TabIndex = 2;
            this.containersGroup.TabStop = false;
            this.containersGroup.Text = "Containers information";
            // 
            // importContainersLoadingBtn
            // 
            this.importContainersLoadingBtn.Enabled = false;
            this.importContainersLoadingBtn.Location = new System.Drawing.Point(162, 206);
            this.importContainersLoadingBtn.Name = "importContainersLoadingBtn";
            this.importContainersLoadingBtn.Size = new System.Drawing.Size(109, 23);
            this.importContainersLoadingBtn.TabIndex = 2;
            this.importContainersLoadingBtn.Text = "Load the containers on vessel";
            this.importContainersLoadingBtn.UseVisualStyleBackColor = true;
            this.importContainersLoadingBtn.Click += new System.EventHandler(this.importContainersLoadingBtn_Click);
            // 
            // importContainersStructureLbl
            // 
            this.importContainersStructureLbl.Location = new System.Drawing.Point(10, 206);
            this.importContainersStructureLbl.Name = "importContainersStructureLbl";
            this.importContainersStructureLbl.resize = false;
            this.importContainersStructureLbl.Size = new System.Drawing.Size(142, 23);
            this.importContainersStructureLbl.TabIndex = 1;
            this.importContainersStructureLbl.Text = "Import containers structure";
            // 
            // importStructureBtn
            // 
            this.importStructureBtn.Location = new System.Drawing.Point(280, 206);
            this.importStructureBtn.Name = "importStructureBtn";
            this.importStructureBtn.Size = new System.Drawing.Size(94, 23);
            this.importStructureBtn.TabIndex = 0;
            this.importStructureBtn.Text = "Import vessel structure";
            this.importStructureBtn.UseVisualStyleBackColor = true;
            this.importStructureBtn.Click += new System.EventHandler(this.importStructureBtn_Click);
            // 
            // scanBtn
            // 
            this.scanBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.scanBtn.Location = new System.Drawing.Point(10, 475);
            this.scanBtn.Name = "scanBtn";
            this.scanBtn.Size = new System.Drawing.Size(100, 23);
            this.scanBtn.TabIndex = 3;
            this.scanBtn.Text = "Scan containers";
            this.scanBtn.UseVisualStyleBackColor = true;
            this.scanBtn.Click += new System.EventHandler(this.scanBtn_Click);
            // 
            // lblHoveredEelement
            // 
            this.lblHoveredEelement.AutoSize = true;
            this.lblHoveredEelement.Location = new System.Drawing.Point(10, 177);
            this.lblHoveredEelement.Name = "lblHoveredEelement";
            this.lblHoveredEelement.Size = new System.Drawing.Size(0, 13);
            this.lblHoveredEelement.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Location = new System.Drawing.Point(913, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(97, 488);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Route planner";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 16);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Padding = new System.Windows.Forms.Padding(10);
            this.pictureBox1.Size = new System.Drawing.Size(91, 469);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // myButton2
            // 
            this.myButton2.Location = new System.Drawing.Point(380, 206);
            this.myButton2.Name = "myButton2";
            this.myButton2.Size = new System.Drawing.Size(75, 23);
            this.myButton2.TabIndex = 6;
            this.myButton2.Text = "Play log data";
            this.myButton2.UseVisualStyleBackColor = true;
            // 
            // mBayMap
            // 
            this.mBayMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mBayMap.Location = new System.Drawing.Point(3, 16);
            this.mBayMap.Name = "mBayMap";
            this.mBayMap.Size = new System.Drawing.Size(891, 218);
            this.mBayMap.TabIndex = 0;
            // 
            // yawLbl
            // 
            this.yawLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.yawLbl.ForeColor = System.Drawing.Color.Coral;
            this.yawLbl.Location = new System.Drawing.Point(11, 128);
            this.yawLbl.Name = "yawLbl";
            this.yawLbl.resize = false;
            this.yawLbl.Size = new System.Drawing.Size(202, 23);
            this.yawLbl.TabIndex = 4;
            this.yawLbl.Text = "Yaw: ";
            // 
            // ContainersView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.myButton2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblHoveredEelement);
            this.Controls.Add(this.scanBtn);
            this.Controls.Add(this.importContainersLoadingBtn);
            this.Controls.Add(this.importContainersStructureLbl);
            this.Controls.Add(this.containersGroup);
            this.Controls.Add(this.importStructureBtn);
            this.Controls.Add(this.droneGroup);
            this.Controls.Add(this.GOTGroup);
            this.Name = "ContainersView";
            this.Size = new System.Drawing.Size(1013, 502);
            this.GOTGroup.ResumeLayout(false);
            this.GOTGroup.PerformLayout();
            this.groupGotTransmitters.ResumeLayout(false);
            this.groupGotReceivers.ResumeLayout(false);
            this.droneGroup.ResumeLayout(false);
            this.containersGroup.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
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
        private ContainerView.BaysMap mBayMap;
        private System.Windows.Forms.Label lblHoveredEelement;
        private Controls.MyLabel zLabel;
        private Controls.MyLabel yCoordinateTxt;
        private Controls.MyLabel xCoordinateTxt;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Controls.MyButton myButton1;
        private Controls.MyButton myButton2;
        private Controls.MyLabel commandLabel;
        private Controls.MyLabel yawLbl;

    }
}
