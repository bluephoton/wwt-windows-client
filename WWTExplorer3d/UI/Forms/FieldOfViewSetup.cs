using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace TerraViewer
{
    public partial class FieldOfViewSetup : Form
    {

        static  FieldOfViewSetup fovSetup;
        
        public static void ShowFovSetup(Form owner)
        {
            if (fovSetup == null)
            {
                fovSetup = new FieldOfViewSetup();
                fovSetup.Fov = Earth3d.MainWindow.Fov;
                fovSetup.Owner = owner;
            }
            fovSetup.Show();
        }

        public FieldOfViewSetup()
        {
            InitializeComponent();
            SetUiStrings();
        }

        private void SetUiStrings()
        {
            label9.Text = Language.GetLocalizedText(196, "");
            label7.Text = Language.GetLocalizedText(197, "Focal Ratio");
            label8.Text = Language.GetLocalizedText(198, "Mount Type");
            label6.Text = Language.GetLocalizedText(199, "Aperture");
            label5.Text = Language.GetLocalizedText(200, "Focal Length");
            cameraRadioButton.Text = Language.GetLocalizedText(201, "Camera");
            eyePieceRadioButton.Text = Language.GetLocalizedText(202, "Eyepiece");
            label2.Text = Language.GetLocalizedText(139, "Telescope");
            label1.Text = Language.GetLocalizedText(203, "Organization");
            label14.Text = Language.GetLocalizedText(204, "Camera Rotation");
            label13.Text = Language.GetLocalizedText(205, "Vert Pixels");
            label11.Text = Language.GetLocalizedText(206, "Imaging Surfaces");
            label12.Text = Language.GetLocalizedText(207, "Horiz Pixels");
            label10.Text = Language.GetLocalizedText(208, "Height");
            widthLabel.Text = Language.GetLocalizedText(209, "Width");
            label4.Text = Language.GetLocalizedText(201, "Camera");
            label3.Text = Language.GetLocalizedText(210, "Manufacturer");
            Telescope.Text = Language.GetLocalizedText(139, "Telescope");
            CameraGroup.Text = Language.GetLocalizedText(201, "Camera");
            label15.Text = Language.GetLocalizedText(211, "For More information:");
            label16.Text = Language.GetLocalizedText(211, "For More information:");
            OK.Text = Language.GetLocalizedText(212, "Close");
            Text = Language.GetLocalizedText(213, "Field of View Indicator");
            
        }

        private void TelescopesTab_Click(object sender, EventArgs e)
        {

        }

        private void FieldOfViewSetup_Load(object sender, EventArgs e)
        {
            LoadTelescopes();

            LoadCameras();

        }

        private void LoadCameras()
        {
            var Manufacturers = new List<string>();

            manufactuerCombo.Items.Clear();
            var index = 0;
            var selectedIndex = 0;
            foreach (var cam in FieldOfView.Cameras)
            {
                if (!Manufacturers.Contains(cam.Manufacturer))
                {
                    Manufacturers.Add(cam.Manufacturer);
                    manufactuerCombo.Items.Add(cam.Manufacturer);
                    if (Fov.Camera.Manufacturer == cam.Manufacturer)
                    {
                        selectedIndex = index;
                    }
                    index++;
                }
            }

            if (manufactuerCombo.Items.Count > 0)
            {
                manufactuerCombo.SelectedIndex = selectedIndex;
            }
        }

        private void LoadTelescopes()
        {
            var Manufacturers = new List<string>();

            organizationCombo.Items.Clear();
            var index = 0;
            var selectedIndex = 0;
            foreach (var scope in FieldOfView.Telescopes)
            {
                if (!Manufacturers.Contains(scope.Manufacturer))
                {
                    Manufacturers.Add(scope.Manufacturer);
                    organizationCombo.Items.Add(scope.Manufacturer);
                    if (Fov.Telescope.Manufacturer == scope.Manufacturer)
                    {
                        selectedIndex = index;
                    }
                    index++;
                }
            }

            if (organizationCombo.Items.Count > 0)
            {
                organizationCombo.SelectedIndex = selectedIndex;
            }
        }


        private void organizationCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (organizationCombo.SelectedItem == null)
            {
                return;
            }

            telescopeCombo.Items.Clear();
            var index = 0;
            var selectedIndex = 0;
            var manufacturer = organizationCombo.SelectedItem.ToString();

            foreach (var scope in FieldOfView.Telescopes)
            {
                if (scope.Manufacturer == manufacturer)
                {
                    telescopeCombo.Items.Add(scope);
                    if (Fov.Telescope.Name == scope.Name)
                    {
                        selectedIndex = index;
                    }
                    index++;
                }
            }

            if (telescopeCombo.Items.Count > 0)
            {
                telescopeCombo.SelectedIndex = selectedIndex;
            }

        }

        private void telescopeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            var scope = (Telescope)telescopeCombo.SelectedItem;

            focalLengthText.Text = scope.FocalLength.ToString();
            focalRatioText.Text = (Math.Floor(scope.FRatio*100+.5)/100).ToString("0.00");
            apertureText.Text = scope.Aperture.ToString();
            moutTypeText.Text = scope.MountType;
            opticalDesignText.Text = scope.OpticalDesign;
            manufacturerUrlLink.Text = scope.Url;
            Fov.Telescope = (Telescope)telescopeCombo.SelectedItem;
            Properties.Settings.Default.FovTelescope = (Fov.Telescope.Manufacturer + Fov.Telescope.Name).GetHashCode32();


        }

        void manufacturerUrlLink_Click(object sender, EventArgs e)
        {
            Process.Start(manufacturerUrlLink.Text);
        }
        private void cameraMakerUrlLink_MouseEnter(object sender, EventArgs e)
        {
            cameraMakerUrlLink.ForeColor = Color.Yellow;
        }

        private void cameraMakerUrlLink_MouseLeave(object sender, EventArgs e)
        {
            cameraMakerUrlLink.ForeColor = Color.White;
        }

        private void manufacturerUrlLink_MouseEnter(object sender, EventArgs e)
        {
            manufacturerUrlLink.ForeColor = Color.Yellow;
        }

        private void manufacturerUrlLink_MouseLeave(object sender, EventArgs e)
        {
            manufacturerUrlLink.ForeColor = Color.White;

        }
        public FieldOfView Fov = null;

        private void Ok_Click(object sender, EventArgs e)
        {

        }

        void cameraMakerUrlLink_Click(object sender, EventArgs e)
        {
            Process.Start(cameraMakerUrlLink.Text);
        }

        private void manufactuerCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (manufactuerCombo.SelectedItem == null)
            {
                return;
            }

            cameraCombo.Items.Clear();
            var index = 0;
            var selectedIndex = 0;
            var manufacturer = manufactuerCombo.SelectedItem.ToString();

            foreach (var cam in FieldOfView.Cameras)
            {
                if (cam.Manufacturer == manufacturer)
                {
                    cameraCombo.Items.Add(cam);
                    if (Fov.Camera.Name == cam.Name)
                    {
                        selectedIndex = index;
                    }
                    index++;
                }
            }

            if (cameraCombo.Items.Count > 0)
            {
                cameraCombo.SelectedIndex = selectedIndex;
            }
        }

        private void cameraCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            var camera = (Camera)cameraCombo.SelectedItem;


            widthText.Text = camera.Chips[0].Width.ToString();
            heightText.Text = camera.Chips[0].Height.ToString();
            vertText.Text = camera.Chips[0].VerticalPixels.ToString();
            horizText.Text = camera.Chips[0].HorizontalPixels.ToString();
            imagerCountText.Text = camera.Chips.Count.ToString();

            cameraMakerUrlLink.Text = camera.Url;
            Fov.Camera = (Camera)cameraCombo.SelectedItem;
            Properties.Settings.Default.FovCamera = (Fov.Camera.Manufacturer + Fov.Camera.Name).GetHashCode32();

        }

        private void cameraRotationSpin_ValueChanged(object sender, EventArgs e)
        {
            Fov.angle = ((double)cameraRotationSpin.Value +360)% 360;

            if (Fov.angle != (double)cameraRotationSpin.Value)
            {
                cameraRotationSpin.Value = 0;
            }
        }

        private void OK_Click_1(object sender, EventArgs e)
        {
            Fov.Telescope = (Telescope)telescopeCombo.SelectedItem;
            Fov.Camera = (Camera)cameraCombo.SelectedItem;
            Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FieldOfViewSetup_FormClosed(object sender, FormClosedEventArgs e)
        {
            fovSetup = null;
        }
    }
}