﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ConfigBuilder
{
    public partial class frmOptions : Form
    {
        frmMain parentForm;

        public frmOptions(frmMain frm)
        {
            InitializeComponent();
            parentForm = new frmMain();
            parentForm = frm;
        }

        private void frmOptions_Load(object sender, EventArgs e)
        {
            cbActiveDevice.Items.Clear();

            string[] discoveredJoysticks = parentForm.getJoystickNames();
            
            if (discoveredJoysticks != null)
            {
                cbActiveDevice.Items.AddRange(discoveredJoysticks);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            parentForm.setJoystick(cbActiveDevice.SelectedItem.ToString());
            this.Dispose();
        }
    }
}
