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
using System.IO;
using System.Reflection;
using System.Threading;
using MissionPlanner.Containers.TSP_GA;
using GAF.Extensions;
using GAF.Operators;
using GAF.Threading;

namespace MissionPlanner.GCSViews
{
    public partial class ContainersView : MyUserControl, IActivate, INotifyPropertyChanged
    {
        // Define all posible motions.
        public enum MOTIONS {
            TAKE_OFF,
            LAND,
            MOVE_FORWARD,
            MOVE_BACKWARD,
            MOVE_LEFT,
            MOVE_RIGHT,
            TURN_LEFT,
            TURN_RIGHT,
            MOVE_UP,
            MOVE_DOWN,
            STOP
        }

        // Struct of the motion command.
        public struct MOTION_COMMAND {
            public MOTIONS motion;
            public string value;
            public string getMotionName(MOTIONS m)
            {
                switch (m)
                {
                    case MOTIONS.TAKE_OFF:
                        return "TAKE_OFF";
                    case MOTIONS.LAND:
                        return "LAND";
                    case MOTIONS.MOVE_BACKWARD:
                        return "MOVE_BACKWARD";
                    case MOTIONS.MOVE_DOWN:
                        return "MOVE_DOWN";
                    case MOTIONS.MOVE_FORWARD:
                        return "MOVE_FORWARD";
                    case MOTIONS.MOVE_LEFT:
                        return "MOVE_LEFT";
                    case MOTIONS.MOVE_RIGHT:
                        return "MOVE_RIGHT";
                    case MOTIONS.MOVE_UP:
                        return "MOVE_UP";
                    case MOTIONS.STOP:
                        return "STOP";
                    case MOTIONS.TURN_LEFT:
                        return "TURN_LEFT";
                    case MOTIONS.TURN_RIGHT:
                        return "TURN_RIGHT";
                }
                return String.Empty;
            }
            //@override
            public string ToString()
            {
                string command = getMotionName(motion);
                return String.Format("{0} {1}", command, value);
            }
        }


        double operatingHeight = 0;

        bool keepOperatingHeight = false;
        bool missionStarted = false;

        List<MOTION_COMMAND> commandsList = new List<MOTION_COMMAND>();

        // Master node version.
        public string MasterVersion { get { return this.masterVersion; } set { this.masterVersion = value; OnPropertyChanged("MasterVersion"); } }
        private string masterVersion = "Unknown";

        // Currently connected units.
        public ObservableCollection<Transmitter> ConnectedTransmitters { get; private set; }
        public ObservableCollection<Receiver> ConnectedReceivers { get; private set; }
        public ObservableCollection<Scenario3D> Scenarios { get; private set; }
        public ObservableCollection<Transmitter> CalibrationTransmitters { get; private set; }
        // The latest received measurements
        private Queue<CalculatedPosition> measurementHistory = new Queue<CalculatedPosition>();

        // The latest calibration is automatically saved to this location. This is just a help for testing/development purposes.
        private static readonly string ScenarioFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Calibration.xml");

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
        private Vessel CargoShip = new Vessel();
        // TSP related stuff.
        //Nodes nodesList = new Nodes();
        //TSP tsp;
        Image TSP_Image;
        private static List<ContainerCity> _targets;
        Graphics TSP_Graphics;
        private Font font = new Font("Verdana", 8, FontStyle.Regular);

        // Coordinates.
        double GOT_X;
        double GOT_Y;
        double GOT_Z;

        public ContainersView()
        {
            InitializeComponent();

            this.ConnectedTransmitters = new ObservableCollection<Transmitter>();
            this.ConnectedReceivers = new ObservableCollection<Receiver>();
            this.Scenarios = new ObservableCollection<Scenario3D>();
            this.CalibrationTransmitters = new ObservableCollection<Transmitter>();

            this.gotMaster = new Master2X(WindowsFormsSynchronizationContext.Current);
            this.gotMaster.OnMasterStatusChanged += gotMaster_OnMasterStatusChanged;
            this.gotMaster.OnNewReceiverConnected += gotMaster_OnNewReceiverConnected;
            this.gotMaster.OnNewTransmitterConnected += gotMaster_OnNewTransmitterConnected;
            this.gotMaster.OnMeasurementReceived += gotMaster_OnMeasurementReceived;

            TryOpenLastCalibration();
            
            //this.containerMapCtrl.onContainerSelected += containerMapCtrl_onContainerSelected;
            //this.containerMapCtrl.onContainerUnselected += containerMapCtrl_onContainerUnselected;

            // Save the clicks to the file pickers by passing the file pathes directly.
            CargoShip.LoadContainersStructureFromXML("C:\\Project\\MissionPlanner\\Data_Files\\Vessel Structure.xml");
            CargoShip.LoadContainersPlacement("C:\\Project\\MissionPlanner\\Data_Files\\loadingData.xml");

            // Link the vissel in the ContainerView user control to the BayMap User Control.
            this.mBayMap.setVessel(CargoShip);

            this.mBayMap.BayHovered += mBayMap_BayHovered;
            this.mBayMap.ContainerHovered += mBayMap_ContainerHovered;
        }

        private void TryOpenLastCalibration()
        {
            if (File.Exists(ScenarioFilePath))
            {
                var doc = XDocument.Load(ScenarioFilePath);
                var loadedScenarios = Scenario3DPersistence.Load(doc);

                foreach (var scenario in loadedScenarios)
                {
                    this.Scenarios.Add(scenario);
                    this.currentScenario = scenario;
                    if (scenario != null)
                    {
                        gotCalibrationLbl.Text = "Calibrated";
                        gotCalibrateBtn.Text = "Re-calibrate";
                    }
                }
            }
        }
        // Event handler change the info string on which container mouse is placed.
        void mBayMap_ContainerHovered(object sender, ContainerObject container)
        {
            this.lblHoveredEelement.Text = container.ToString();
        }
        // Event handler to change the string on which the mouse is hovered.
        void mBayMap_BayHovered(object sender, BayObject bay)
        {
            this.lblHoveredEelement.Text = bay.ToString();
        }
        // Event handler on new measurement received.
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
                    if (position != null)
                    {
                        GOT_X = position.Position.X / 1000;
                        GOT_Y = position.Position.Y / 1000;
                        GOT_Z = position.Position.Z / 1000;
                        xCoordinateTxt.Text = string.Format("X: {0}", GOT_X);
                        yCoordinateTxt.Text = string.Format("Y: {0}", GOT_Y);
                        zCoordinateTxt.Text = string.Format("Z: {0}", GOT_Z);
                    }

                    //MessageBox.Show(string.Format("{0} - {1} - {2}", position.Position.X, position.Position.Y, position.Position.Z));
                }
            }
        }
        // Event handler on new transmitter connected.
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

            if (CalibratorTriangle.IsCalibratorTriangleAddress(transmitter.GOTAddress))
            {
                this.gotMaster.SetTransmitterState(transmitter.GOTAddress, true, Transmitter.UltraSonicLevel.High);
            }

            // Check if transmitters composing a calibration trangle has been connected.
            if (!this.CalibratorTriangleDetected) {
                this.CalibratorTriangleDetected = CalibratorTriangle.TryFindCalibratorTriangle(this.CalibrationTransmitters.Select(s => s.GOTAddress), out this.calibratorTriangle);
            }
            this.gotCalibrateBtn.Enabled = this.CalibratorTriangleDetected;
        }
        // Event handler on new receiver connected.
        void gotMaster_OnNewReceiverConnected(Receiver receiver)
        {
            if (!this.ConnectedReceivers.Any( r => r.GOTAddress == receiver.GOTAddress))
            {
                gotMaster.SetTransmitterState(receiver.GOTAddress, true, Transmitter.UltraSonicLevel.High);
                this.ConnectedReceivers.Add(receiver);
                this.receiversList.Items.Add(string.Format("{0} - {1}", receiver.GOTAddress, receiver.FirmwareVersion));
            }
        }
        // Event handler on status changed in the GOT.
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
                    master.SetSetup(100, 20, MasterRadioLevel.High);
                }
            }
        }

        public void Activate()
        {
            // Activate view
            Console.Beep();
        }

        public static double getAngleBetween2Points(PointF start, PointF target)
        {
            float xDiff = target.X - start.X;
            float yDiff = target.Y - start.Y;
            double angle = Math.Atan2(yDiff, xDiff);
            return Math.Atan2(yDiff, xDiff) * (180 / Math.PI);
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
            if (this.gotMaster.Status == MasterStatus.Connected)
            {
                this.gotMaster.Close();
                gotConnect.Text = "Connect";
                return;
            }
            
            // Try to connect. This will return false immediately if it fails to find the proper USB port.
            // Otherwise, it will begin connecting and the "Master2X.OnMasterStatusChanged" event will be invoked.
            if (!this.gotMaster.BeginConnect())
            {
                MessageBox.Show("A master could not be detected. Make sure it is connected and the Silabs USB driver has been installed.", "Failed to detect master station", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                gotConnect.Text = "Disconnect";
            }


        }
        // Calibrate the Games on track system.
        private void gotCalibrateBtn_Click(object sender, EventArgs e)
        {
            if (this.CalibratorTriangleDetected && this.ConnectedReceivers.Count >= 3)
            {
                // Make sure all transmitters in the calibration triangle are active and they emit the maximum level of ultra sound.
                SetCalibratorTriangleTransmitterStatus(true);
                calibrationDialog = new GOTCalibrateForm(this.ConnectedReceivers, this.calibratorTriangle, this.gotMaster);
                calibrationDialog.ShowDialog();

                // Will be null if the user cancelled.
                if (calibrationDialog.CalibratedScenario != null)
                {
                    var scenario = calibrationDialog.CalibratedScenario;
                    this.Scenarios.Add(scenario);
                    SaveScenariosToFile();
                    this.currentScenario = calibrationDialog.CalibratedScenario;
                    gotCalibrationLbl.Text = "Calibrated";
                    gotCalibrateBtn.Text = "Re-calibrate";
                }
                calibrationDialog = null;
            }
        }
        // Save the calibration scenario into the file.
        private void SaveScenariosToFile()
        {
            var doc = Scenario3DPersistence.Save(this.Scenarios);
            doc.Save(ScenarioFilePath, System.Xml.Linq.SaveOptions.None);
        }
        // Enable/disable all 3 transmitters in the calibration triangle.
        private void SetCalibratorTriangleTransmitterStatus(bool enabled)
        {
            if (this.calibratorTriangle != null)
            {
                foreach (var address in this.calibratorTriangle.TransmitterAddresses)
                {
                    this.gotMaster.SetTransmitterState(address, enabled, Transmitter.UltraSonicLevel.High);
                }
            }
        }
        // Import the containers structure from the XML file.
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
                    if (containersFileDialog.FileName.Length > 0)
                    {
                        // Read the information from XML.
                        CargoShip.LoadContainersStructureFromXML(containersFileDialog.FileName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read the file from disk. Original error" + ex.Message);
                }
                finally {
                    importContainersLoadingBtn.Enabled = true;
                }
            }
        }
        // Import containers loading data from the XML file.
        private void importContainersLoadingBtn_Click(object sender, EventArgs e)
        {
            if (CargoShip.isStructureLoaded())
            {

                // Load the containers placement.
                OpenFileDialog containersFileDialog = new OpenFileDialog();
                containersFileDialog.DefaultExt = "xml";
                containersFileDialog.Filter = "XML file (*.xml)|*.xml"; 
                if (containersFileDialog.ShowDialog() == DialogResult.OK) {
                    // Read the file
                    try
                    {
                        // Check if file was selected.
                        if (containersFileDialog.FileName.Length > 0) {
                            CargoShip.LoadContainersPlacement(containersFileDialog.FileName);
                        }
                    } catch (Exception ex) {
                        CustomMessageBox.Show("Error: Could not read the file from disk. Original error " + ex.Message);
                    }
                }
            }
            else
            {
                CustomMessageBox.Show("The containers structure is missing, please load the containers structure.");
            }
        }
        // Draw the route.
        public void DrawTSPRoute(List<GAF.Gene> genes)
        {
            if (TSP_Image == null)
            {
                TSP_Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                TSP_Graphics = Graphics.FromImage(TSP_Image);
            }
            // Make the background white.
            TSP_Graphics.FillRectangle(Brushes.White, 0, 0, TSP_Image.Width, TSP_Image.Height);
            var last = genes.Count() - 1;
            for (var i = 0; i <= last; i++)
            {
                ContainerCity first = new ContainerCity();
                ContainerCity next = new ContainerCity();
                if (i == last)
                {
                    first = _targets[(int)genes[last].RealValue];
                    next = _targets[(int)genes[0].RealValue];
                    
                } else
                {
                    first = _targets[(int)genes[i].RealValue];
                    next = _targets[(int)genes[i + 1].RealValue];   
                }
                var xoffset = CargoShip.rowIndex(first.RowId) * 20;
                var yoffset = CargoShip.bayIndex(first.BayId) * 4;

                var nextxoffset = CargoShip.rowIndex(next.RowId) * 20;
                var nextyoffset = CargoShip.bayIndex(next.BayId) * 4;
                // Multiple by 10 just in case to render better on screen visually.
                Point start_point = new Point((int)first.X * 10 + xoffset, (int)first.Y * 10 + yoffset);
                Point next_point = new Point((int)next.X * 10 + nextxoffset, (int)next.Y * 10 + nextyoffset);

                TSP_Graphics.DrawEllipse(Pens.Red, new Rectangle(start_point, new Size(5, 5)));
                TSP_Graphics.DrawString(first.Name, font, Brushes.Black, start_point);
                TSP_Graphics.DrawLine(Pens.Green, start_point, next_point);
            }
            pictureBox1.Image = TSP_Image;
        }
        // Button click event handler for scanning containers.
        private void scanBtn_Click(object sender, EventArgs e)
        {
            // Check if any container was selected from the list.
            var selectedContainers = this.CargoShip.ContainersList.FindAll(x => x.Selected && x.containerLoaded);
            if (selectedContainers.Count == 0)
            {
                CustomMessageBox.Show("There are no containers selected for inspection!");
                return;
            }

            // All the containers is loaded to the system.
            // Let's calculate all the containers cordinates (x, y, z) in meters.
            CargoShip.CalculateContainersCoordinates(selectedContainers);

            foreach (var c in selectedContainers)
            {
                if (!c.Visible)
                {
                    CustomMessageBox.Show(string.Format("The container {0} is not visually seen, so the inspection might be not accurate.", c.Name));
                }
            }
            TSP(CargoShip);
        }
        // Travelling salesman problem solving by using Genetic Algorithm.
        public void TSP(Vessel cargo)
        {
            // Get target containers.
            _targets = CreateTargets(CargoShip).ToList();
            // Each city can be identified by an integer within the range 0-15 our chromosome is a special case as it needs to
            // contain each city only once. Therefore, our chromosome will contain all the integers betwwen 0 and 15 with no duplicates.

            // 100 is our population so create the population
            var population = new GAF.Population(100);
            // create the chromosomes
            for (var p = 0; p < 100; p++)
            {
                var chromosome = new GAF.Chromosome();
                for (var g = 0; g < _targets.Count; g++)
                {
                    chromosome.Genes.Add(new GAF.Gene(g));
                }
                chromosome.Genes.Shuffle();
                population.Solutions.Add(chromosome);
            }

            // create the elite operator.
            var elite = new Elite(5);

            // create the crossover operator.
            var crossover = new Crossover(0.8)
            {
                CrossoverType = CrossoverType.DoublePointOrdered
            };

            // create the mutation operator.
            var mutate = new SwapMutate(0.02);

            // create the GA
            var ga = new GAF.GeneticAlgorithm(population, CalculateFitness);
            // attach events.
            ga.OnGenerationComplete += ga_OnGenerationComplete;
            ga.OnRunComplete += ga_OnRunComplete;
            // add the operators.
            ga.Operators.Add(elite);
            ga.Operators.Add(crossover);
            ga.Operators.Add(mutate);
            // run the Genetic Algorithm.
            ga.Run(Factorial(_targets.Count));
        }
        // Function to count numbers factorial.
        static int Factorial(int n)
        {
            if (n <= 1)
                return 1;
            int result = 1;
            for (int i = 2; i <= n; i++)
            {
                result = result * i;
            }
            return result;
        }
        // Then the shortest path is founded.
        void ga_OnRunComplete(object sender, GAF.GaEventArgs e)
        {
            var fittest = e.Population.GetTop(1)[0];
            // Initialize the drone flight according to the best route.
            MessageBox.Show("Route planning is done, starting the drone");
            // 1. Get the orientation by making fake move.
            // 2. Calculate the differenct and move back to the initial position.
            // 3. When we have already the position we can
            List<ContainerCity> targetPoints = new List<ContainerCity>();
            foreach (var gene in fittest.Genes)
            {
                targetPoints.Add(_targets[(int)gene.RealValue]);
            }
            if (targetPoints.Count > 0)
            {
                NavigateTheDrone(targetPoints);
            }
        }
        
        // Main function which will create the navigation for the drone.
        public void NavigateTheDrone(List<ContainerCity> containersList)
        {
            // Take off to the specific height.
            operatingHeight = 2.59 * CargoShip.tierIndex(CargoShip.LastTierNumber);
            attachCommand(commandsList, new MOTION_COMMAND() { motion = MOTIONS.TAKE_OFF, value = operatingHeight.ToString() });
            // Make fake movement to analyze the orientation of the drone regards local environment.
            PointF current = new PointF((float)GOT_X, (float)GOT_Y);
            commandsList.Add(new MOTION_COMMAND() { motion = MOTIONS.MOVE_FORWARD });
            PointF newPosition = new PointF((float)GOT_X, (float)GOT_Y);
            commandsList.Add(new MOTION_COMMAND() { motion = MOTIONS.MOVE_BACKWARD });

            double orientationAngle = getAngleBetween2Points(current, newPosition);

            foreach (var container in containersList)
            {
                // Go down to scan the container.
                commandsList.Add(new MOTION_COMMAND() { motion = MOTIONS.MOVE_DOWN, value = (container.Z).ToString()});

            }
            // Land the drone.
            commandsList.Add(new MOTION_COMMAND() { motion = MOTIONS.LAND });
            // Initial point.
            LogCommands(commandsList);
        }

        // Attaching movement comamnds to the list.
        public void attachCommand(List<MOTION_COMMAND> cmdList, MOTION_COMMAND cmd)
        {
            Console.Beep();
            commandLabel.Text = cmd.ToString();
            cmdList.Add(cmd);
        }

        // Log commands into file.
        public void LogCommands(List<MOTION_COMMAND> commands)
        {
            string FileName = "comamnds.txt";
            // Clear the OLD command log.
            File.WriteAllText(FileName, String.Empty);
            StreamWriter w = File.AppendText(FileName);
            foreach (MOTION_COMMAND command in commands) {
                Log(command.ToString(), w);
            }
        }

        public void Log(string command, TextWriter text)
        {
            text.WriteLine(command);
        }

        // Generate new route, so draw it on Map.
        void ga_OnGenerationComplete(object sender, GAF.GaEventArgs e)
        {
            var fittest = e.Population.GetTop(1)[0];
            var distanceToTravel = CalculateDistance(fittest);
            DrawTSPRoute(fittest.Genes);
        }

        // Calculate total route distance.
        private static double CalculateDistance(GAF.Chromosome chromosome)
        {
            var distanceToTravel = 0.0;
            ContainerCity previousContainer = null;
            // run through each container in the order specified in the chromosome
            foreach (var gene in chromosome.Genes)
            {
                var currentContainer = _targets[(int)gene.RealValue];

                if (previousContainer != null)
                {
                    var distance = previousContainer.GetDistanceFromPosition(currentContainer.X, currentContainer.Y);
                    distanceToTravel += distance;
                }
                previousContainer = currentContainer;
            }
            return distanceToTravel;
        }
        
        // calculate the fitness of the route.
        public static double CalculateFitness(GAF.Chromosome chromosome)
        {
            var distanceToTravel = CalculateDistance(chromosome);
            return 1 - distanceToTravel / 10000;
        }
        
        // Create target points for the travelling salesman problem.
        public static IEnumerable<ContainerCity> CreateTargets(Vessel cargo)
        {
            var targets = new List<ContainerCity>();
            var selectedContainers = cargo.ContainersList.FindAll(x => x.Selected && x.containerLoaded);
            foreach (var container in selectedContainers)
            {
                targets.Add(new ContainerCity(container.Name, container.X, container.Y, container.Z, container.Bay, container.Row));
            }
            return targets;
        }

        // Restart the games on track system.
        private void myButton1_Click(object sender, EventArgs e)
        {
            if (this.gotMaster != null && this.gotMaster.Status != MasterStatus.Offline)
            {
                this.gotMaster.RequestRestart();
                this.ConnectedReceivers.Clear();
                this.receiversList.Items.Clear();
                this.transmittersList.Items.Clear();
                this.ConnectedTransmitters.Clear();
            }
        }

        private void myButton2_Click(object sender, EventArgs e)
        {
            // Play log data
            // 1. Open log file.
            MainV2.comPort.
        }

    }
}
