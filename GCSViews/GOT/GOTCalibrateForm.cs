using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GOTSDK;
using GOTSDK.Position;

namespace MissionPlanner.GCSViews.GOT
{
    public partial class GOTCalibrateForm : Form
    {

        public ObservableCollection<ReceiverViewModel> Receivers { get; private set; }

        // Calibrating scenario (if everything went well).
        public Scenario3D CalibratedScenario { get; private set; }
        private CalibratorTriangle calibrator;
        private bool isCalibrating = false;

        public GOTCalibrateForm(IEnumerable<Receiver> receivers, CalibratorTriangle cal)
        {
            this.Receivers = new ObservableCollection<ReceiverViewModel>();
            this.calibrator = cal;

            InitializeComponent();

            foreach (var rec in receivers.OrderBy(r => r.GOTAddress))
            {
                this.Receivers.Add(new ReceiverViewModel(rec));
                this.checkListCalibrationReceivers.Items.Add(string.Format("{0} - {1}", rec.GOTAddress, rec.FirmwareVersion), true);
            }
            // Add event handler for the check/uncheck event.
            checkListCalibrationReceivers.ItemCheck += checkListCalibrationReceivers_ItemCheck;
        }

        void checkListCalibrationReceivers_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            this.Receivers[e.Index].IsSelected = e.NewValue == CheckState.Checked;
        }
        public class ReceiverViewModel
        {
            public Receiver Receiver { get; private set; }
            public bool IsSelected { get; set; }
            public GOTAddress GOTAddress { get { return this.Receiver.GOTAddress; } }

            public ReceiverViewModel(Receiver receiver)
            {
                this.Receiver = receiver;
                this.IsSelected = true;
            }

        }

        private void startCalibrateBtn_Click(object sender, EventArgs e)
        {
            this.calibrator.ClearData();
            this.calibrator.SetTargetReceivers(this.Receivers.Where(r => r.IsSelected).Select(r => r.GOTAddress));

            this.isCalibrating = true;
            this.startCalibrateBtn.Enabled = false;
        }

        public void AddNewMeasurement(Measurement measurement)
        {
            if (this.isCalibrating)
            {
                this.calibrator.AddMeasurement(measurement);

                double progress;
                if (this.calibrator.IsCalibrationFinished(out progress))
                {
                    // Get the finished scenario and close this form.
                    this.isCalibrating = false;
                    this.CalibratedScenario = calibrator.CreateScenario();
                    this.Close();
                }
                else
                {
                    calibrationProgress.Value = (int) (progress * 100);
                }
            }
        }

    }
}
