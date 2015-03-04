namespace MissionPlanner.GCSViews.GOT
{
    partial class GOTCalibrateForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GOTCalibrateForm));
            this.checkListCalibrationReceivers = new System.Windows.Forms.CheckedListBox();
            this.calibrationProgress = new System.Windows.Forms.ProgressBar();
            this.startCalibrateBtn = new MissionPlanner.Controls.MyButton();
            this.SuspendLayout();
            // 
            // checkListCalibrationReceivers
            // 
            this.checkListCalibrationReceivers.FormattingEnabled = true;
            this.checkListCalibrationReceivers.Location = new System.Drawing.Point(12, 12);
            this.checkListCalibrationReceivers.Name = "checkListCalibrationReceivers";
            this.checkListCalibrationReceivers.Size = new System.Drawing.Size(192, 184);
            this.checkListCalibrationReceivers.TabIndex = 0;
            // 
            // calibrationProgress
            // 
            this.calibrationProgress.Location = new System.Drawing.Point(12, 202);
            this.calibrationProgress.Name = "calibrationProgress";
            this.calibrationProgress.Size = new System.Drawing.Size(192, 23);
            this.calibrationProgress.TabIndex = 1;
            // 
            // startCalibrateBtn
            // 
            this.startCalibrateBtn.Location = new System.Drawing.Point(94, 232);
            this.startCalibrateBtn.Name = "startCalibrateBtn";
            this.startCalibrateBtn.Size = new System.Drawing.Size(110, 23);
            this.startCalibrateBtn.TabIndex = 2;
            this.startCalibrateBtn.Text = "Start Calibration";
            this.startCalibrateBtn.UseVisualStyleBackColor = true;
            this.startCalibrateBtn.Click += new System.EventHandler(this.startCalibrateBtn_Click);
            // 
            // GOTCalibrateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(216, 262);
            this.Controls.Add(this.startCalibrateBtn);
            this.Controls.Add(this.calibrationProgress);
            this.Controls.Add(this.checkListCalibrationReceivers);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GOTCalibrateForm";
            this.Text = "GOT calibration";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkListCalibrationReceivers;
        private System.Windows.Forms.ProgressBar calibrationProgress;
        private Controls.MyButton startCalibrateBtn;
    }
}