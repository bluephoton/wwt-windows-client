﻿using System;

namespace TerraViewer
{
    public partial class DataWizardInfoPopup : PropPage
    {
        public DataWizardInfoPopup()
        {
            InitializeComponent();
            SetUiStrings();
        }
        private void SetUiStrings()
        {
            label1.Text = Language.GetLocalizedText(847, "You can select which data column you want shown when hovering over a marker, and optionally add a hyperlink to allow a user to drill into related data thru a web page.");
            label2.Text = Language.GetLocalizedText(846, "Hover Text Column");
         }
        TimeSeriesLayer layer;

        public override void SetData(object data)
        {

            layer = data as TimeSeriesLayer;
        }

        public override bool Save()
        {
            layer.NameColumn = nameColumnCombo.SelectedIndex - 1;
            return true;

        }

        private void DataWizardInfoPopup_Load(object sender, EventArgs e)
        {
            nameColumnCombo.Items.Add(Language.GetLocalizedText(832, "None"));
            nameColumnCombo.Items.AddRange(layer.Header);
            nameColumnCombo.SelectedIndex = layer.NameColumn + 1;
        }

        private void nameColumnCombo_SelectionChanged(object sender, EventArgs e)
        {
            layer.NameColumn = nameColumnCombo.SelectedIndex - 1;

        }
    }
}
