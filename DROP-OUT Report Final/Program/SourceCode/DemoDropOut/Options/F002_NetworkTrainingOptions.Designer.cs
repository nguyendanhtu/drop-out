namespace DemoDropOut.Options
{
    partial class F002_NetworkTrainingOptions
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDefault = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkUseIterations = new System.Windows.Forms.CheckBox();
            this.chkUseError = new System.Windows.Forms.CheckBox();
            this.txtErrorLimitBox = new System.Windows.Forms.TextBox();
            this.txtIterationsBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.chkAutoRandomizeRange = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtQuickPropagationCoefficientBox = new System.Windows.Forms.TextBox();
            this.txtLearningRateBox = new System.Windows.Forms.TextBox();
            this.txtRandomizationRangeBox = new System.Windows.Forms.TextBox();
            this.txtMomentumBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkAlgQuickPropagation = new System.Windows.Forms.RadioButton();
            this.chkAlgBackPropagation = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnDefault);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 245);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(540, 51);
            this.panel1.TabIndex = 1;
            // 
            // btnDefault
            // 
            this.btnDefault.Location = new System.Drawing.Point(12, 19);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(75, 23);
            this.btnDefault.TabIndex = 2;
            this.btnDefault.Text = "Default";
            this.btnDefault.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(453, 19);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(372, 19);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(540, 245);
            this.panel2.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkUseIterations);
            this.groupBox3.Controls.Add(this.chkUseError);
            this.groupBox3.Controls.Add(this.txtErrorLimitBox);
            this.groupBox3.Controls.Add(this.txtIterationsBox);
            this.groupBox3.Location = new System.Drawing.Point(279, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(249, 106);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Stop Training Conditions";
            // 
            // chkUseIterations
            // 
            this.chkUseIterations.AutoSize = true;
            this.chkUseIterations.Location = new System.Drawing.Point(6, 22);
            this.chkUseIterations.Name = "chkUseIterations";
            this.chkUseIterations.Size = new System.Drawing.Size(113, 17);
            this.chkUseIterations.TabIndex = 2;
            this.chkUseIterations.Text = "Use iterations limit";
            this.chkUseIterations.UseVisualStyleBackColor = true;
            // 
            // chkUseError
            // 
            this.chkUseError.AutoSize = true;
            this.chkUseError.Location = new System.Drawing.Point(6, 45);
            this.chkUseError.Name = "chkUseError";
            this.chkUseError.Size = new System.Drawing.Size(92, 17);
            this.chkUseError.TabIndex = 3;
            this.chkUseError.Text = "Use error limit";
            this.chkUseError.UseVisualStyleBackColor = true;
            // 
            // txtErrorLimitBox
            // 
            this.txtErrorLimitBox.Location = new System.Drawing.Point(124, 43);
            this.txtErrorLimitBox.Name = "txtErrorLimitBox";
            this.txtErrorLimitBox.Size = new System.Drawing.Size(101, 21);
            this.txtErrorLimitBox.TabIndex = 1;
            this.txtErrorLimitBox.Text = "0.01";
            // 
            // txtIterationsBox
            // 
            this.txtIterationsBox.Location = new System.Drawing.Point(125, 20);
            this.txtIterationsBox.Name = "txtIterationsBox";
            this.txtIterationsBox.Size = new System.Drawing.Size(101, 21);
            this.txtIterationsBox.TabIndex = 0;
            this.txtIterationsBox.Text = "500";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButton2);
            this.groupBox2.Controls.Add(this.chkAutoRandomizeRange);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.txtQuickPropagationCoefficientBox);
            this.groupBox2.Controls.Add(this.txtLearningRateBox);
            this.groupBox2.Controls.Add(this.txtRandomizationRangeBox);
            this.groupBox2.Controls.Add(this.txtMomentumBox);
            this.groupBox2.Location = new System.Drawing.Point(12, 124);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(516, 115);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Training algorithm\'s parameters";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(318, 48);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(160, 17);
            this.radioButton2.TabIndex = 4;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Manual randomization range";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // chkAutoRandomizeRange
            // 
            this.chkAutoRandomizeRange.AutoSize = true;
            this.chkAutoRandomizeRange.Location = new System.Drawing.Point(318, 25);
            this.chkAutoRandomizeRange.Name = "chkAutoRandomizeRange";
            this.chkAutoRandomizeRange.Size = new System.Drawing.Size(174, 17);
            this.chkAutoRandomizeRange.TabIndex = 3;
            this.chkAutoRandomizeRange.TabStop = true;
            this.chkAutoRandomizeRange.Text = "Automatic randomization range";
            this.chkAutoRandomizeRange.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(188, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Quick propagation coefficent [0..100]";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(86, 54);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(108, 13);
            this.label16.TabIndex = 9;
            this.label16.Text = "Learning rate [0 .. 1]";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 8.25F);
            this.label3.Location = new System.Drawing.Point(452, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(13, 14);
            this.label3.TabIndex = 6;
            this.label3.Text = "±";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.label2.Location = new System.Drawing.Point(318, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Randomization Range W = ";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(98, 80);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(96, 13);
            this.label15.TabIndex = 8;
            this.label15.Text = "Momentum [0 .. 1]";
            // 
            // txtQuickPropagationCoefficientBox
            // 
            this.txtQuickPropagationCoefficientBox.Enabled = false;
            this.txtQuickPropagationCoefficientBox.Location = new System.Drawing.Point(200, 24);
            this.txtQuickPropagationCoefficientBox.Name = "txtQuickPropagationCoefficientBox";
            this.txtQuickPropagationCoefficientBox.Size = new System.Drawing.Size(101, 21);
            this.txtQuickPropagationCoefficientBox.TabIndex = 0;
            this.txtQuickPropagationCoefficientBox.Text = "1.75";
            // 
            // txtLearningRateBox
            // 
            this.txtLearningRateBox.Location = new System.Drawing.Point(200, 51);
            this.txtLearningRateBox.Name = "txtLearningRateBox";
            this.txtLearningRateBox.Size = new System.Drawing.Size(101, 21);
            this.txtLearningRateBox.TabIndex = 1;
            this.txtLearningRateBox.Text = "0.1";
            // 
            // txtRandomizationRangeBox
            // 
            this.txtRandomizationRangeBox.Location = new System.Drawing.Point(468, 72);
            this.txtRandomizationRangeBox.Name = "txtRandomizationRangeBox";
            this.txtRandomizationRangeBox.Size = new System.Drawing.Size(42, 21);
            this.txtRandomizationRangeBox.TabIndex = 5;
            this.txtRandomizationRangeBox.Text = "0.3";
            // 
            // txtMomentumBox
            // 
            this.txtMomentumBox.Location = new System.Drawing.Point(200, 77);
            this.txtMomentumBox.Name = "txtMomentumBox";
            this.txtMomentumBox.Size = new System.Drawing.Size(101, 21);
            this.txtMomentumBox.TabIndex = 2;
            this.txtMomentumBox.Text = "0.1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkAlgQuickPropagation);
            this.groupBox1.Controls.Add(this.chkAlgBackPropagation);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(261, 106);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Training algorithm";
            // 
            // chkAlgQuickPropagation
            // 
            this.chkAlgQuickPropagation.AutoSize = true;
            this.chkAlgQuickPropagation.Enabled = false;
            this.chkAlgQuickPropagation.Location = new System.Drawing.Point(7, 44);
            this.chkAlgQuickPropagation.Name = "chkAlgQuickPropagation";
            this.chkAlgQuickPropagation.Size = new System.Drawing.Size(112, 17);
            this.chkAlgQuickPropagation.TabIndex = 1;
            this.chkAlgQuickPropagation.Text = "Quick Propagation";
            this.chkAlgQuickPropagation.UseVisualStyleBackColor = true;
            // 
            // chkAlgBackPropagation
            // 
            this.chkAlgBackPropagation.AutoSize = true;
            this.chkAlgBackPropagation.Checked = true;
            this.chkAlgBackPropagation.Location = new System.Drawing.Point(7, 21);
            this.chkAlgBackPropagation.Name = "chkAlgBackPropagation";
            this.chkAlgBackPropagation.Size = new System.Drawing.Size(108, 17);
            this.chkAlgBackPropagation.TabIndex = 0;
            this.chkAlgBackPropagation.TabStop = true;
            this.chkAlgBackPropagation.Text = "Back Propagation";
            this.chkAlgBackPropagation.UseVisualStyleBackColor = true;
            // 
            // F002_NetworkTrainingOptions
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(540, 296);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "F002_NetworkTrainingOptions";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Network Training Preferences";
            this.Load += new System.EventHandler(this.F002_NetworkTrainingOptions_Load_1);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnDefault;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.RadioButton chkAlgQuickPropagation;
        private System.Windows.Forms.RadioButton chkAlgBackPropagation;
        private System.Windows.Forms.CheckBox chkUseError;
        private System.Windows.Forms.TextBox txtErrorLimitBox;
        private System.Windows.Forms.TextBox txtIterationsBox;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtLearningRateBox;
        private System.Windows.Forms.TextBox txtMomentumBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtQuickPropagationCoefficientBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRandomizationRangeBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton chkAutoRandomizeRange;
        private System.Windows.Forms.CheckBox chkUseIterations;
    }
}