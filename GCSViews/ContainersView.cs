using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MissionPlanner.Controls;
using System.Collections.ObjectModel;
using GOTSDK;
using GOTSDK.Master;
using GOTSDK.Position;
using MissionPlanner.GCSViews.GOT;
using System.Windows.Media.Media3D;
using MissionPlanner.Containers;
using System.Xml.Linq;

namespace MissionPlanner.GCSViews
{
    public partial class ContainersView : MyUserControl, IActivate, INotifyPropertyChanged
    {
        // Master node version.
        public string MasterVersion { get { return this.masterVersion; } set { this.masterVersion = value; OnPropertyChanged("MasterVersion"); } }
        private string masterVersion = "Unknown";

        // Currently connected units.
        public ObservableCollection<Transmitter> ConnectedTransmitters { get; private set; }
        public ObservableCollection<Receiver> ConnectedReceivers { get; private set; }
        public ObservableCollection<Scenario3D> Scenarios { get; private set; }
        public ObservableCollection<Transmitter> CalibrationTransmitters { get; private set; }

        // Calibrator.
        public bool CalibratorTriangleDetected { get { return this.calibratorTriangleDetected; } set { this.calibratorTriangleDetected = value; OnPropertyChanged("CalibratorTriangleDetected"); } }
        private bool calibratorTriangleDetected = false;

        private Master2X gotMaster;
        // Restart the GOT master radio the first time connection is established.
        private bool hasMasterRadioBeenRestarted = false;

        // Used for calibrating the system.
        private CalibratorTriangle calibratorTriangle;
        private GOTCalibrateForm calibrationDialog;

        // The currect scenario to use for position calculation and display.
        private Scenario3D currentScenario;

        public ContainersView()
        {
            InitializeComponent();

            this.ConnectedTransmitters = new ObservableCollection<Transmitter>();
            this.ConnectedReceivers = new ObservableCollection<Receiver>();
            this.CalibrationTransmitters = new ObservableCollection<Transmitter>();

            this.gotMaster = new Master2X(WindowsFormsSynchronizationContext.Current);
            this.gotMaster.OnMasterStatusChanged += gotMaster_OnMasterStatusChanged;
            this.gotMaster.OnNewReceiverConnected += gotMaster_OnNewReceiverConnected;
            this.gotMaster.OnNewTransmitterConnected += gotMaster_OnNewTransmitterConnected;
            this.gotMaster.OnMeasurementReceived += gotMaster_OnMeasurementReceived;

            //this.containerMapCtrl.onContainerSelected += containerMapCtrl_onContainerSelected;
            //this.containerMapCtrl.onContainerUnselected += containerMapCtrl_onContainerUnselected;
        }

        void gotMaster_OnMeasurementReceived(Measurement measurement)
        {
            // Are we currently calibrating ?
            if (this.calibrationDialog != null)
            {
                this.calibrationDialog.AddNewMeasurement(measurement);
            }
            else
            {
                // Check if we have a scenario to handle the measurement
                if (this.currentScenario != null)
                {
                    // Handle position.
                    CalculatedPosition position;
                    PositionCalculator.TryCalculatePosition(measurement, this.currentScenario, out position);

                    MessageBox.Show(string.Format("{0} - {1} - {2}", position.Position.X, position.Position.Y, position.Position.Z));
                }
            }
        }

        void gotMaster_OnNewTransmitterConnected(Transmitter transmitter)
        {
            if (!this.ConnectedTransmitters.Any(t => t.GOTAddress == transmitter.GOTAddress))
            {
                if (!CalibratorTriangle.IsCalibratorTriangleAddress(transmitter.GOTAddress))
                {
                    this.ConnectedTransmitters.Add(transmitter);
                    this.transmittersList.Items.Add(string.Format("{0} - {1}", transmitter.GOTAddress, transmitter.FirmwareVersion));
                }
                else
                {
                    if (!this.CalibrationTransmitters.Any(t => t.GOTAddress == transmitter.GOTAddress)) {
                        this.CalibrationTransmitters.Add(transmitter);
                    }
                }

            }

            // Check if transmitters composing a calibration trangle has been connected.
            if (!this.CalibratorTriangleDetected) {
                this.CalibratorTriangleDetected = CalibratorTriangle.TryFindCalibratorTriangle(this.CalibrationTransmitters.Select(s => s.GOTAddress), out this.calibratorTriangle);
            }
            this.gotCalibrateBtn.Enabled = this.CalibratorTriangleDetected;
        }

        void gotMaster_OnNewReceiverConnected(Receiver receiver)
        {
            if (!this.ConnectedReceivers.Any( r => r.GOTAddress == receiver.GOTAddress))
            {
                this.ConnectedReceivers.Add(receiver);
                this.receiversList.Items.Add(string.Format("{0} - {1}", receiver.GOTAddress, receiver.FirmwareVersion));
            }
        }

        void gotMaster_OnMasterStatusChanged(IMaster master, MasterStatus newStatus)
        {
            this.gotConnectionStatusLbl.Text = string.Format("Status: {0}", newStatus.ToString());

            if (newStatus == MasterStatus.Connected)
            {
                if (!hasMasterRadioBeenRestarted)
                {
                    // This will cause all currently connected units in the got master to show up.
                    this.gotMaster.RequestRestart(false, true);
                    this.hasMasterRadioBeenRestarted = true;
                }
                else // Request firmware version and serial
                {
                    master.RequestMasterInfo();
                    master.SetSetup(100, 20);
                }
            }
        }

        public void Activate()
        {
            // Activate view
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        // Connect to the Games on track position system.
        private void gotConnect_Click(object sender, EventArgs e)
        {

            var viktoras = MainV2.comPort.GetParam("RC1_MIN");


            MainV2.comPort.setMode("GUIDED");
            MainV2.comPort.doARM(true);

            MainV2.comPort.doCommand(MAVLink.MAV_CMD.TAKEOFF, 0, 0, 0, 0, 0, 0, 20);


            Bay b = new Bay("TEST BAY");
            Tier t = new Tier("TEST TIER");
            Row r = new Row("TEST ROW");

            MissionPlanner.Containers.Container c = new MissionPlanner.Containers.Container("TEST container", b, t, r);
            c.Name = "TEST 22222";

            // Try to connect. This will return false immediately if it fails to find the proper USB port.
            // Otherwise, it will begin connecting and the "Master2X.OnMasterStatusChanged" event will be invoked.
            if (!this.gotMaster.BeginConnect())
            {
                MessageBox.Show("A master could not be detected. Make sure it is connected and the Silabs USB driver has been installed.", "Failed to detect master station", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gotCalibrateBtn_Click(object sender, EventArgs e)
        {
            if (this.CalibratorTriangleDetected && this.CalibrationTransmitters.Count >= 3)
            {
                calibrationDialog = new GOTCalibrateForm(this.ConnectedReceivers, this.calibratorTriangle);
                calibrationDialog.ShowDialog();

                if (calibrationDialog.CalibratedScenario != null)
                {
                    this.currentScenario = calibrationDialog.CalibratedScenario;  
                }
                calibrationDialog = null;
            }
        }

        private void importStructureBtn_Click(object sender, EventArgs e)
        {
            // Import data.
            OpenFileDialog containersFileDialog = new OpenFileDialog();
            containersFileDialog.DefaultExt = "xml";
            containersFileDialog.Filter = "XML file (*.xml)|*.xml";
            if (containersFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Read the file
                try
                {
                    // Check if any file was selected
                    if (containersFileDialog.FileName.Length > 0) {
                        // Read the information from XML.
                        readContainersData(containersFileDialog.FileName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read the file from disk. Original error" + ex.Message);
                }
            }
        }

        private void readContainersData(string FileName)
        {
            XDocument xml = XDocument.Load(FileName);
            var bays = xml.Root.Descendants("Bay").Select(x => x.Attribute("Name")).ToList();
        }
    }
}
