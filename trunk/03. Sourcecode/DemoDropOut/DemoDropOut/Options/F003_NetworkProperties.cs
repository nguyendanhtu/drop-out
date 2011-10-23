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
    public partial class F003_NetworkProperties : Form
    {
        /// <summary>
        /// Tham số mạng
        /// </summary>
        private NetworkParameters m_net_parameters;
        /// <summary>
        /// Tham số mạng
        /// </summary>
        public NetworkParameters NetworkParameters
        {
            get { return m_net_parameters; }
            set { m_net_parameters = value; }
        }

        private F003_NetworkProperties()
        {
            InitializeComponent();
        }

        public F003_NetworkProperties(NetworkParameters ip_net_parameters)
            : this()
        {
            m_net_parameters = ip_net_parameters;
            this.txtHiddenNeurons.KeyDown += new KeyEventHandler(txtHiddenNeurons_KeyDown);
        }

        private void txtHiddenNeurons_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue < 48 || e.KeyValue > 57)
            {
                e.Handled = true;
            }
        }

        private void chkDesignNet_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtHiddenNeurons.Enabled = chkDesignNet.Checked;
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
                m_net_parameters.ActivationFunction = (ActivationFunctionEnum)this.chkHiddenActivation.SelectedItem;
                m_net_parameters.OutputFunction = (ActivationFunctionEnum)this.chkOutputActivationFx.SelectedItem;
                if (string.IsNullOrEmpty(txtHiddenNeurons.Text) == false)
                {
                    m_net_parameters.HiddenNeurons = int.Parse(txtHiddenNeurons.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void F003_NetworkProperties_Load(object sender, EventArgs e)
        {
            try
            {
                this.chkHiddenActivation.Items.Add(ActivationFunctionEnum.Logistic);
                this.chkHiddenActivation.Items.Add(ActivationFunctionEnum.HiperbolicTangent);
                this.chkHiddenActivation.SelectedItem = m_net_parameters.ActivationFunction;

                this.chkOutputErrorFx.Enabled = false;
                this.chkDesignNet.Checked = false;

                this.chkOutputActivationFx.Items.Add(ActivationFunctionEnum.Logistic);
                this.chkOutputActivationFx.Items.Add(ActivationFunctionEnum.HiperbolicTangent);
                this.chkOutputActivationFx.SelectedItem = m_net_parameters.OutputFunction;

                this.txtInputNeurons.Text = m_net_parameters.InputNeurons.ToString();
                this.txtHiddenNeurons.Text = m_net_parameters.HiddenNeurons.ToString();
                this.txtOutputNeurons.Text = m_net_parameters.OutputNeurons.ToString();

                this.lbNetArchitectureRecommend.Text = string.Format("Network Architecture Recomend: {0} - {1} - {2}", m_net_parameters.InputNeurons, m_net_parameters.GenerateHiddenNeurons(), m_net_parameters.OutputFunction);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
