using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DemoDropOut.Options
{
    public partial class F004_DataPartitionOptions : Form
    {
        private double m_db_TrnPercent;
        private double m_db_VldPercent;
        private double m_db_TstPercent;

        public double TrainingPercentage
        {
            get { return m_db_TrnPercent; }
        }

        public double ValidtionPercentage
        {
            get { return m_db_VldPercent; }
        }

        public double TestPercentage
        {
            get { return m_db_TstPercent; }
        }

        public F004_DataPartitionOptions()
        {
            InitializeComponent();
            this.btnOK.DialogResult = DialogResult.OK;
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            try
            {
                SetDefault();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                SetParameters();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SetDefault()
        {
            this.txtPercentOfTrnSet.Text = "0.68";
            this.txtPercentOfVldSet.Text = "0.16";
            this.txtPercentOfTstSet.Text = "0.16";
        }

        private void SetParameters()
        {
            this.m_db_TrnPercent = double.Parse(this.txtPercentOfTrnSet.Text.Trim());
            this.m_db_VldPercent = double.Parse(this.txtPercentOfVldSet.Text.Trim());
            this.m_db_TstPercent = double.Parse(this.txtPercentOfTstSet.Text.Trim());
            if ((m_db_TrnPercent + m_db_VldPercent + m_db_TstPercent) != 1)
            {
                throw new Exception("Sum is not equals to One");
            }
        }

        internal void LoadPartition(DemoDropOut.Apps.Objects.DataPartitionOptions ip_pOptions)
        {
            this.txtPercentOfTrnSet.Text = ip_pOptions.TrainPcent.ToString();
            this.txtPercentOfVldSet.Text = ip_pOptions.ValidPcent.ToString();
            this.txtPercentOfTstSet.Text = ip_pOptions.TestPcent.ToString();
        }
    }
}
