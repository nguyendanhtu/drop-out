using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DropOut
{
    public partial class F001_MainForm : Form
    {
        public F001_MainForm()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            MessageBox.Show("Test");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void statusbarToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void trainingLogToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
