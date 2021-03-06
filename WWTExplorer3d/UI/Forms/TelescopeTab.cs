using System;
using System.Windows.Forms;

namespace TerraViewer
{
    public partial class TelescopeTab : TabForm
    {
        AscomTelescope scope;

        internal AscomTelescope Scope
        {
            get { return scope; }
            set { scope = value; }
        }
        bool telescopeConnected;

        public bool TelescopeConnected
        {
            get { return telescopeConnected; }
            set { telescopeConnected = value; }
        }

        public TelescopeTab()
        {
            InitializeComponent();
            SetUiStrings();
        }

        private void SetUiStrings()
        {
            toolTip1.SetToolTip(pictureBox1, "Click to install ASCOM platform to allow telescope control.");
            Setup.Text = Language.GetLocalizedText(379, "Setup");
            ra.Text = Language.GetLocalizedText(271, "RA : ");
            telescopeGroup.Text = Language.GetLocalizedText(380, "Telescope Control - Not Connected");
            TrackScope.Text = Language.GetLocalizedText(381, "Track Telescope");
            Park.Text = Language.GetLocalizedText(50, "Park");
            SyncScope.Text = Language.GetLocalizedText(382, "Sync");
            ScopeWest.Text = Language.GetLocalizedText(248, "West");
            ScopeEast.Text = Language.GetLocalizedText(243, "East");
            ScopeSouth.Text = Language.GetLocalizedText(246, "South");
            ScopeNorth.Text = Language.GetLocalizedText(241, "North");
            SlewScope.Text = Language.GetLocalizedText(383, "Slew");
            CenterScope.Text = Language.GetLocalizedText(384, "Center");
            label4.Text = Language.GetLocalizedText(268, "Az :");
            label2.Text = Language.GetLocalizedText(269, "Alt : ");
            label1.Text = Language.GetLocalizedText(270, "Dec : ");
            Choose.Text = Language.GetLocalizedText(385, "Choose");
            ConnectScope.Text = Language.GetLocalizedText(386, "Connect");
            PlatformStatus.Text = Language.GetLocalizedText(387, "Not Installed");
            ScopeStatus.Text = Language.GetLocalizedText(388, "Telescope Status");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            WebWindow.OpenUrl("http://www.ascom-standards.org", true);
        }

        protected override void SetFocusedChild()
        {
           
        }

        public void ConnectScope_Click(object sender, EventArgs e)
        {
            if (!platformInstalled || !ConnectScope.Enabled)
            {
                return;
            }

            if (telescopeConnected)
            {
                scope.Connected = false;
                scope.Dispose();
                scope = null;
                telescopeConnected = false;
                SetControlEnableState(false);
                ConnectScope.Text = Language.GetLocalizedText(386, "Connect");
                telescopeGroup.Text = Language.GetLocalizedText(389, "Telescope Control - Disconnected");
                Choose.Enabled = true;
            }
            else
            {
                if (string.IsNullOrEmpty(Properties.Settings.Default.TelescopeID))
                {
                    ChooseScope();
                }

                if (!string.IsNullOrEmpty(Properties.Settings.Default.TelescopeID))
                {
                    try
                    {
                        scope = new AscomTelescope(Properties.Settings.Default.TelescopeID);
                        scope.Connected = true;
                        telescopeConnected = true;
                        SetControlEnableState(true);
                        ConnectScope.Text = Language.GetLocalizedText(390, "Disconnect");
                        telescopeGroup.Text = Language.GetLocalizedText(391, "Telescope Control - Connected");
                        Choose.Enabled = false;
                    }
                    catch
                    {
                        MessageBox.Show(Language.GetLocalizedText(392, "The telescope failed to connect. Please ensure the correct telescope is chosen, and that its settings are correct."), Language.GetLocalizedText(393, "Telescope control"));
                    }
                }
                else
                {
                    MessageBox.Show(Language.GetLocalizedText(394, "Before you can connect, you need to choose a telescope."), Language.GetLocalizedText(393, "Telescope control"));
                }
            }
        }

        private void SetControlEnableState(bool state)
        {
            ScopeEast.Enabled = state;
            ScopeWest.Enabled = state;
            ScopeNorth.Enabled = state;
            ScopeSouth.Enabled = state;

            CenterScope.Enabled = state;
            SlewScope.Enabled = state;
            TrackScope.Enabled = state;
            if (state)
            {
                SyncScope.Enabled = scope.CanSync && scope.Tracking;
                if (scope.AtPark)
                {
                    Park.Text = Language.GetLocalizedText(395, "Unpark");
                        Park.Enabled = scope.CanUnpark;
                }
                else
                {
                    Park.Text = Language.GetLocalizedText(50, "Park");
                    Park.Enabled = scope.CanPark;
                }
            }
            else
            {
                SyncScope.Enabled = state;
                Park.Enabled = state;
            }
        }

        public void Choose_Click(object sender, EventArgs e)
        {
            if (platformInstalled && Choose.Enabled)
            {
                ChooseScope();
            }
            else if (!platformInstalled)
            {
                MessageBox.Show(Language.GetLocalizedText(396, "You must first install the ASCOM platform and a telescope driver in order to control your telescope. Click on the ASCOM logo on the Telescope Panel for more information."), Language.GetLocalizedText(393, "Telescope control"));
            }
        }

        public void ChooseScope()
        {
            var scopeID = AscomTelescope.ChooseTelescope(Properties.Settings.Default.TelescopeID);
            if (platformInstalled && !String.IsNullOrEmpty(scopeID))
            {
                Properties.Settings.Default.TelescopeID = scopeID;
            }
        }

        public void Setup_Click(object sender, EventArgs e)
        {
            if (platformInstalled && !String.IsNullOrEmpty(Properties.Settings.Default.TelescopeID))
            {
                if (scope == null)
                {
                    scope = new AscomTelescope(Properties.Settings.Default.TelescopeID);
                }
                scope.SetupDialog();
            }
        }

        public void Park_Click(object sender, EventArgs e)
        {
            if (platformInstalled && scope != null && scope.Connected && scope.CanPark && !scope.AtPark)
            {
                scope.Park();
            }
            else if (scope != null && scope.Connected && scope.CanUnpark && scope.AtPark)
            {
                scope.Unpark();
            }
            SetControlEnableState(scope.Connected);
        }

        public void SyncScope_Click(object sender, EventArgs e)
        {
            if (platformInstalled && scope != null && scope.Connected && scope.CanSync && !scope.AtPark)
            {
                scope.SyncToCoordinates(Earth3d.MainWindow.RA, Earth3d.MainWindow.Dec);
            }
            else
            {
                if (scope.CanSync && scope.AtPark)
                {
                    MessageBox.Show(Language.GetLocalizedText(397, "The telescope is parked. Unpark the scope to Sync"), Language.GetLocalizedText(393, "Telescope control"));
                }
                else
                {
                    if (!scope.CanSync)
                    {
                        MessageBox.Show(Language.GetLocalizedText(398, "The Telescope does not support syncing"), Language.GetLocalizedText(393, "Telescope control"));
                    }
                }
            }
        }

        private void ScopeNorth_Click(object sender, EventArgs e)
        {
            //todo implement
        }

        private void ScopeEast_Click(object sender, EventArgs e)
        {

            //todo implement
        }

        private void ScopeSouth_Click(object sender, EventArgs e)
        {
            //todo implement

        }

        private void ScopeWest_Click(object sender, EventArgs e)
        {
            //todo implement

        }

        public void SlewScope_Click(object sender, EventArgs e)
        {
            if (platformInstalled &&scope != null)
            {
                if (scope.AtPark)
                {
                    scope.Unpark();
                }
                if (!scope.Tracking)
                {
                    if (scope.CanSetTracking)
                    {
                        scope.Tracking = true;
                    }
                }
                scope.SlewToCoordinatesAsync(Earth3d.MainWindow.RA, Earth3d.MainWindow.Dec);
            }
        }

        public void CenterScope_Click(object sender, EventArgs e)
        {
            if (platformInstalled && scope != null && scope.Connected)
            {
                Earth3d.MainWindow.GotoTarget(false, false, new CameraParameters(scope.Declination, Earth3d.MainWindow.RAtoViewLng(scope.RightAscension), Earth3d.MainWindow.viewCamera.Zoom, Earth3d.MainWindow.viewCamera.Rotation, Earth3d.MainWindow.viewCamera.Angle, (float)Earth3d.MainWindow.viewCamera.Opacity), null, null);

            }
        }

        private void TrackScope_CheckedChanged(object sender, EventArgs e)
        {
        }

        double TelescopeRa;
        double TelescopeDec;
        double TelescopeAlt;
        double TelescopeAz;
        private void TelescopeTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (telescopeConnected)
                {
                    if (scope != null && scope.Connected)
                    {
                        if (scope.Slewing)
                        {
                            ScopeStatus.Text = Language.GetLocalizedText(399, "Telescope Status - Slewing");
                        }
                        else if (scope.AtPark)
                        {
                            ScopeStatus.Text = Language.GetLocalizedText(400, "Telescope Status - Parked");
                        }
                        else if (scope.Tracking)
                        {
                            ScopeStatus.Text = Language.GetLocalizedText(401, "Telescope Status - Tracking");
                        }
                        else
                        {
                            ScopeStatus.Text = Language.GetLocalizedText(388, "Telescope Status");
                        }
                        TelescopeRa = scope.RightAscension;
                        TelescopeDec = scope.Declination;
                        TelescopeAlt = scope.Altitude;
                        TelescopeAz = scope.Azimuth;

                        altText.Text = Coordinates.FormatDMS(TelescopeAlt);
                        azText.Text = Coordinates.FormatDMS(TelescopeAz);
                        decText.Text = Coordinates.FormatDMS(TelescopeDec);
                        raText.Text = Coordinates.FormatDMS(TelescopeRa);

                        if (TrackScope.Checked)
                        {
                            Earth3d.MainWindow.GotoTarget(new TourPlace("None", TelescopeDec, Earth3d.MainWindow.RAtoViewLng(TelescopeRa), Classification.BlackHole, Earth3d.MainWindow.Constellation, ImageSetType.Sky, 0), true, true, false);
                            //Earth3d.MainWindow.RA = TelescopeRa;
                            //Earth3d.MainWindow.Dec = TelescopeDec;
                        }
                    }
                    if (scope != null)
                    {
                        SetControlEnableState(scope.Connected);
                    }
                }
            }
            catch
            {
            }
        }

        private void TelescopeTab_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (scope != null && scope.Connected)
            {
                scope.Connected = false;
                scope.Dispose();
                scope = null;
            }
        }
        bool platformInstalled;
        private void TelescopeTab_Load(object sender, EventArgs e)
        {
            if (AscomTelescope.IsPlatformInstalled())
            {
                platformInstalled = true;
                PlatformStatus.Text = Language.GetLocalizedText(402, "Installed");
            }
            else
            {
                PlatformStatus.Text = Language.GetLocalizedText(403, "Not Installed");
                ConnectScope.Enabled = false;
                Choose.Enabled = false;
                Setup.Enabled = false;
            }
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            if (Earth3d.MainWindow.IsWindowOrChildFocused())
            {
                Focus();
            }
        }
    }
}