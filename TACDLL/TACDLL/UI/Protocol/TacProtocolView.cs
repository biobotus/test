﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TACDLL.UI.Protocol
{
    public partial class TacProtocolView : Form
    {
        public TacProtocolView()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            NewTacStep control = new NewTacStep();
            flowLayoutPanel1.Controls.Add(control);
            flowLayoutPanel1.SetFlowBreak(control, true);
        }
    }
}
