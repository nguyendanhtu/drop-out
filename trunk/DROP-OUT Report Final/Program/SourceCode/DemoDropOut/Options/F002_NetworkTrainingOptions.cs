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
    public partial class F002_NetworkTrainingOptions : Form
    {
        public F002_NetworkTrainingOptions()
        {
            InitializeComponent();

            btnOK.DialogResult = DialogResult.OK;
            chkAutoRandomizeRange.CheckedChanged += new EventHandler(chkAutoRandomizeRange_CheckedChanged);
            btnDefault.Click += new EventHandler(btnDefault_Click);
            btnOK.Click += new EventHandler(btnOK_Click);
            this.Load += new EventHandler(F002_NetworkTrainingOptions_Load);
            this.FormClosing += new FormClosingEventHandler(F002_NetworkProperties_FormClosing);
        }

        private void F002_NetworkTrainingOptions_Load(object sender, EventArgs e)
        {
            SetDefault();
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


        private void F002_NetworkProperties_FormClosing(object sender, FormClosingEventArgs e)
        {
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

        private void chkAutoRandomizeRange_CheckedChanged(object sender, EventArgs e)
        {
            var v_obj_sender = sender as RadioButton;
            txtRandomizationRangeBox.Enabled = !v_obj_sender.Checked;
        }
        /// <summary>
        /// Tham số luyện mạng
        /// </summary>
        private TrainingAlgorithmParameters m_training_parameters;

        public TrainingAlgorithmParameters TrainingParameters
        {
            get { return m_training_parameters; }
        }

        private void SetDefault()
        {
            chkAlgBackPropagation.Checked = true;

            chkUseIterations.Checked = true;
            chkUseError.Checked = false;

            chkAutoRandomizeRange.Checked = true;
            txtRandomizationRangeBox.Text = "0.3";

            txtIterationsBox.Text = "500";
            txtErrorLimitBox.Text = "0.01";

            txtQuickPropagationCoefficientBox.Text = "1.75";
            txtLearningRateBox.Text = "0.1";
            txtMomentumBox.Text = "0.1";
        }

        private void SetParameters()
        {
            // Tham số luyện mạng

            //LearningRate = Math.Max(0.00001, Math.Min(1, double.Parse(learningRateBox.Text)));
            //Momentum = Math.Max(0.01, Math.Min(100, double.Parse(alphaBox.Text)));

            //ErrorLimit = Math.Max(0, double.Parse(errorLimitBox.Text));
            //IterationLimit = (uint)Math.Max(0, int.Parse(iterationsBox.Text));

            //IsCheckError = chkErrorLimit.Checked;

            var v_db_quick_coefficient = default(Double);
            var v_db_learning_rate = default(Double);
            var v_db_momentum = default(Double);
            var v_db_error_limit = default(Double);
            var v_int_iterations_limit = 0;
            m_training_parameters.TrainingAlgorithm = getTrainingAlgorithm();
            m_training_parameters.RandomizationRange = getRandomizationRange();
            // alg parameters
            m_training_parameters.QuickPropagationCoefficient = double.TryParse(txtQuickPropagationCoefficientBox.Text, out v_db_quick_coefficient) == true ? v_db_quick_coefficient : 1.75;
            m_training_parameters.LearningRate = double.TryParse(txtLearningRateBox.Text, out v_db_learning_rate) == true ? v_db_learning_rate : 0.1;
            m_training_parameters.Momentum = double.TryParse(txtMomentumBox.Text, out v_db_momentum) == true ? v_db_momentum : 0.1;
            // stop conditions
            if (chkUseIterations.Checked == true)
            {
                m_training_parameters.UseIterationsLimit = true;
                m_training_parameters.IterationsLimit = int.TryParse(txtIterationsBox.Text, out v_int_iterations_limit) == true ? v_int_iterations_limit : 500;
            }
            if (chkUseError.Checked == true)
            {
                m_training_parameters.UseErrorLimit = true;
                m_training_parameters.ErrorLimit = chkUseError.Checked == true && double.TryParse(txtErrorLimitBox.Text, out v_db_error_limit) == true ? v_db_error_limit : 0.01;
            }
            var v_chk_none = m_training_parameters.UseIterationsLimit | m_training_parameters.UseErrorLimit;
            if (v_chk_none == false)
            {
                m_training_parameters.UseIterationsLimit = true;
                m_training_parameters.IterationsLimit = int.TryParse(txtIterationsBox.Text, out v_int_iterations_limit) == true ? v_int_iterations_limit : 500;
            }
        }

        private TrainingAlgorithmEnum getTrainingAlgorithm()
        {
            if (chkAlgBackPropagation.Checked == true)
            {
                return TrainingAlgorithmEnum.BackPropagation;
            }
            else if (chkAlgQuickPropagation.Checked == true)
            {
                return TrainingAlgorithmEnum.QuickPropagation;
            }
            // others
            return TrainingAlgorithmEnum.BackPropagation;
        }

        private double getRandomizationRange()
        {
            var v_db_randomize_range = 0.3;
            if (chkAutoRandomizeRange.Checked == false)
            {
                if (double.TryParse(txtRandomizationRangeBox.Text, out v_db_randomize_range) == true)
                {
                    return v_db_randomize_range;
                }
                return 0.3;
            }
            return v_db_randomize_range;
        }

        private void F002_NetworkTrainingOptions_Load_1(object sender, EventArgs e)
        {

        }
    }
}
