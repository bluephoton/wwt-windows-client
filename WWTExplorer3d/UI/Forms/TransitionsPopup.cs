﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace TerraViewer
{
    public partial class TransitionsPopup : Form
    {
        public TransitionsPopup()
        {
            InitializeComponent();
            Earth3d.NoStealFocus = true;
            transitionPicker.SelectedIndexChanged += transitionPicker_SelectedIndexChanged;
        }

        private void SetUiStrings()
        {
            spreadsheetNameLabel.Text = Language.GetLocalizedText(1122, "A Time");
            label1.Text = Language.GetLocalizedText(1123, "B Time");
            label2.Text = Language.GetLocalizedText(1124, "Hold Time");
            Text = Language.GetLocalizedText(1125, "Transitions");

        }

        public event EventHandler TargetWasChanged;

        void transitionPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            target.Transition = (TransitionType)transitionPicker.SelectedIndex;
            ProcessChange();
            EnableControls();
            
        }

        private void EnableControls()
        {
            var aOn = false;
            var bOn = false;
            var hOn = false;
            switch (target.Transition)
            {
                case TransitionType.Slew:
                    break;
                case TransitionType.CrossFade:
                    hOn = true;
                    bOn = true;
                    break;
                case TransitionType.CrossCut:
                    hOn = true;
                    bOn = true;
                    break;
                case TransitionType.FadeOutIn:
                    aOn = true;
                    bOn = true;
                    hOn = true;
                    break;
                case TransitionType.FadeIn:
                    bOn = true;
                    hOn = true;
                    break;
                case TransitionType.FadeOut:
                    aOn = true;
                    break;
                default:
                    break;
            }

            TimeA.Enabled = aOn;
            TimeB.Enabled = bOn;
            TimeHold.Enabled = hOn;
        }

        void ProcessChange()
        {
            if (TargetWasChanged != null)
            {
                TargetWasChanged.Invoke(this, new EventArgs());
            }
        }

        private TourStop target;

        public TourStop Target
        {
            get { return target; }
            set
            {
                target = value;

                transitionPicker.SelectedIndex = (int)target.Transition;

            }
        }

        private void transitionPicker_Load(object sender, EventArgs e)
        {
            TimeA.Text = target.TransitionOutTime.ToString();
            TimeB.Text = target.TransitionTime.ToString();
            TimeHold.Text = target.TransitionHoldTime.ToString();
            EnableControls();
        }

        private void TransitionsPopup_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                target.Transition = (TransitionType)transitionPicker.SelectedIndex;
                Earth3d.NoStealFocus = false;
            }
            catch
            {
            }
        }

        private void TransitionsPopup_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Close();
            }
        }

        private void TransitionsPopup_Paint(object sender, PaintEventArgs e)
        {
            var p = new Pen(Color.FromArgb(62, 73, 92));
            e.Graphics.DrawRectangle(p, new Rectangle(0, 0, Width - 1, Height - 1));
            p.Dispose();
        }

        private void TransitionsPopup_Deactivate(object sender, EventArgs e)
        {
            Close();
        }

        private void TimeA_TextChanged(object sender, EventArgs e)
        {
            var failed = false;

            Target.TransitionOutTime = UiTools.ParseAndValidateCoordinate(TimeA, Target.TransitionOutTime, ref failed);
            ProcessChange();
        }

        private void TimeB_TextChanged(object sender, EventArgs e)
        {
            var failed = false;

            Target.TransitionTime = UiTools.ParseAndValidateCoordinate(TimeB, Target.TransitionTime, ref failed);
            ProcessChange();
        }

        private void spreadsheetNameLabel_Click(object sender, EventArgs e)
        {

        }

        private void timeHold_TextChanged(object sender, EventArgs e)
        {
            var failed = false;

            Target.TransitionHoldTime = UiTools.ParseAndValidateCoordinate(TimeHold, Target.TransitionHoldTime, ref failed);
            ProcessChange();
        }

    }
}
