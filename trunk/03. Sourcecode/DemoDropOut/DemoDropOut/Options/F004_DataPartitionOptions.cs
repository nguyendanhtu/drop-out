using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DemoDropOut.Apps.Objects;

namespace DemoDropOut.Options
{
    public partial class F004_DataPartitionOptions : Form
    {
        /// <summary>
        /// Tùy chỉnh phân tập dữ liệu
        /// </summary>
        private DataPartitionOptions m_dt_partition;

        public DataPartitionOptions DataPartition
        {
            get { return m_dt_partition; }
            set { m_dt_partition = value; }
        }

        public F004_DataPartitionOptions()
        {
            InitializeComponent();
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
                this.m_dt_partition.TrainPcent = double.Parse(this.txtPercentOfTrnSet.Text.Trim());
                this.m_dt_partition.ValidPcent = double.Parse(this.txtPercentOfVldSet.Text.Trim());
                this.m_dt_partition.TestPcent = double.Parse(this.txtPercentOfTstSet.Text.Trim());
                if (m_dt_partition.IsOne() == false)
                {
                    throw new Exception("Invalid partition.\r\nSum is not equals to One!");
                }
                this.DialogResult = DialogResult.OK;
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

        internal void LoadPartition(DataPartitionOptions ip_pOptions)
        {
            this.txtPercentOfTrnSet.Text = ip_pOptions.TrainPcent.ToString();
            this.txtPercentOfVldSet.Text = ip_pOptions.ValidPcent.ToString();
            this.txtPercentOfTstSet.Text = ip_pOptions.TestPcent.ToString();
            this.lbTrainingCount.Text = ip_pOptions.GetTrainCount().ToString();
            this.lbValidationCount.Text = ip_pOptions.GetValidCount().ToString();
            this.lbTestCount.Text = ip_pOptions.GetTestCount().ToString();
            this.lbTotalCount.Text = ip_pOptions.Total.ToString();
            this.Tag = ip_pOptions;
            m_dt_partition = ip_pOptions;
        }

        private void txtPercentOfDataSet_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var txtPercentBox = sender as TextBox;
                    this.m_dt_partition.TrainPcent = double.Parse(this.txtPercentOfTrnSet.Text.Trim());
                    this.m_dt_partition.ValidPcent = double.Parse(this.txtPercentOfVldSet.Text.Trim());
                    this.m_dt_partition.TestPcent = double.Parse(this.txtPercentOfTstSet.Text.Trim());
                    if (m_dt_partition.IsOne() == true)
                    {
                        this.lbTrainingCount.Text = m_dt_partition.GetTrainCount().ToString();
                        this.lbValidationCount.Text = m_dt_partition.GetValidCount().ToString();
                        this.lbTestCount.Text = m_dt_partition.GetTestCount().ToString();
                    }
            }
            catch (Exception)
            {
            }
        }
    }
}
