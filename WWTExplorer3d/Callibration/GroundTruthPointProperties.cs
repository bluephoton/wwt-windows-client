﻿using System;
using System.Globalization;
using System.Windows.Forms;

namespace TerraViewer.Callibration
{
    public partial class GroundTruthPointProperties : Form
    {
        public GroundTruthPointProperties()
        {
            InitializeComponent();
            SetUiStrings();
        }
        private void SetUiStrings()
        {
            label1.Text = Language.GetLocalizedText(763, "Altitude");
            label2.Text = Language.GetLocalizedText(764, "Constraint Type");
            label3.Text = Language.GetLocalizedText(765, "Azimuth");
            label4.Text = Language.GetLocalizedText(766, "Weight");
            OK.Text = Language.GetLocalizedText(759, "Ok");
            Text = Language.GetLocalizedText(767, "GroundTruthPointProperties");
        }

        public GroundTruthPoint Target = null;
        private void GroundTruthPointProperties_Load(object sender, EventArgs e)
        {
            ConstraintTypeCombo.Items.AddRange(Enum.GetNames(typeof(AxisTypes)));
            ConstraintTypeCombo.Items.RemoveAt(ConstraintTypeCombo.Items.Count - 1);
            ConstraintTypeCombo.SelectedIndex = (int)Target.AxisType;

            AzText.Text = Target.Az.ToString(CultureInfo.InvariantCulture);
            AltText.Text = Target.Alt.ToString(CultureInfo.InvariantCulture);
            WeightText.Text = Target.Weight.ToString(CultureInfo.InvariantCulture);
        }

        private void OK_Click(object sender, EventArgs e)
        {
            var failed = false;

            Target.Az = UiTools.ParseAndValidateDouble(AzText, Target.Az, ref failed);
            Target.Alt = UiTools.ParseAndValidateDouble(AltText, Target.Alt, ref failed);
            Target.Weight = UiTools.ParseAndValidateDouble(WeightText, Target.Az, ref failed);

            if (!failed)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void ConstraintTypeCombo_SelectionChanged(object sender, EventArgs e)
        {
            Target.AxisType = (AxisTypes)ConstraintTypeCombo.SelectedIndex;
        }
    }
}
