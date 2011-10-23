namespace DemoDropOut.Options
{
    partial class F003_NetworkProperties
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
            this.label1 = new System.Windows.Forms.Label();
            this.chkHiddenActivation = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkOutputErrorFx = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkOutputActivationFx = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lbNetArchitectureRecommend = new System.Windows.Forms.Label();
            this.txtInputNeurons = new System.Windows.Forms.TextBox();
            this.txtHiddenNeurons = new System.Windows.Forms.TextBox();
            this.txtOutputNeurons = new System.Windows.Forms.TextBox();
            this.chkDesignNet = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Hidden layer activation FX:";
            // 
            // chkHiddenActivation
            // 
            this.chkHiddenActivation.FormattingEnabled = true;
            this.chkHiddenActivation.Location = new System.Drawing.Point(155, 10);
            this.chkHiddenActivation.Name = "chkHiddenActivation";
            this.chkHiddenActivation.Size = new System.Drawing.Size(121, 21);
            this.chkHiddenActivation.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(62, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Output error FX:";
            // 
            // chkOutputErrorFx
            // 
            this.chkOutputErrorFx.FormattingEnabled = true;
            this.chkOutputErrorFx.Location = new System.Drawing.Point(155, 37);
            this.chkOutputErrorFx.Name = "chkOutputErrorFx";
            this.chkOutputErrorFx.Size = new System.Drawing.Size(121, 21);
            this.chkOutputErrorFx.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Output activation FX:";
            // 
            // chkOutputActivationFx
            // 
            this.chkOutputActivationFx.FormattingEnabled = true;
            this.chkOutputActivationFx.Location = new System.Drawing.Point(155, 64);
            this.chkOutputActivationFx.Name = "chkOutputActivationFx";
            this.chkOutputActivationFx.Size = new System.Drawing.Size(121, 21);
            this.chkOutputActivationFx.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label4.Location = new System.Drawing.Point(14, 147);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(260, 2);
            this.label4.TabIndex = 0;
            // 
            // lbNetArchitectureRecommend
            // 
            this.lbNetArchitectureRecommend.AutoSize = true;
            this.lbNetArchitectureRecommend.Location = new System.Drawing.Point(12, 97);
            this.lbNetArchitectureRecommend.Name = "lbNetArchitectureRecommend";
            this.lbNetArchitectureRecommend.Size = new System.Drawing.Size(207, 13);
            this.lbNetArchitectureRecommend.TabIndex = 0;
            this.lbNetArchitectureRecommend.Text = "Network Architecture Recomend: 0 - 0 - 0";
            // 
            // txtInputNeurons
            // 
            this.txtInputNeurons.Enabled = false;
            this.txtInputNeurons.Location = new System.Drawing.Point(99, 116);
            this.txtInputNeurons.Name = "txtInputNeurons";
            this.txtInputNeurons.Size = new System.Drawing.Size(55, 21);
            this.txtInputNeurons.TabIndex = 2;
            // 
            // txtHiddenNeurons
            // 
            this.txtHiddenNeurons.Enabled = false;
            this.txtHiddenNeurons.Location = new System.Drawing.Point(160, 116);
            this.txtHiddenNeurons.Name = "txtHiddenNeurons";
            this.txtHiddenNeurons.Size = new System.Drawing.Size(55, 21);
            this.txtHiddenNeurons.TabIndex = 2;
            // 
            // txtOutputNeurons
            // 
            this.txtOutputNeurons.Enabled = false;
            this.txtOutputNeurons.Location = new System.Drawing.Point(221, 116);
            this.txtOutputNeurons.Name = "txtOutputNeurons";
            this.txtOutputNeurons.Size = new System.Drawing.Size(55, 21);
            this.txtOutputNeurons.TabIndex = 2;
            // 
            // chkDesignNet
            // 
            this.chkDesignNet.AutoSize = true;
            this.chkDesignNet.Location = new System.Drawing.Point(15, 118);
            this.chkDesignNet.Name = "chkDesignNet";
            this.chkDesignNet.Size = new System.Drawing.Size(78, 17);
            this.chkDesignNet.TabIndex = 3;
            this.chkDesignNet.Text = "Design Net";
            this.chkDesignNet.UseVisualStyleBackColor = true;
            this.chkDesignNet.CheckedChanged += new System.EventHandler(this.chkDesignNet_CheckedChanged);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(107, 162);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(197, 162);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // F003_NetworkProperties
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(291, 197);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.chkDesignNet);
            this.Controls.Add(this.txtOutputNeurons);
            this.Controls.Add(this.txtHiddenNeurons);
            this.Controls.Add(this.txtInputNeurons);
            this.Controls.Add(this.chkOutputActivationFx);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lbNetArchitectureRecommend);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkOutputErrorFx);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkHiddenActivation);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "F003_NetworkProperties";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Network Properties Definition";
            this.Load += new System.EventHandler(this.F003_NetworkProperties_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox chkHiddenActivation;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox chkOutputErrorFx;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox chkOutputActivationFx;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbNetArchitectureRecommend;
        private System.Windows.Forms.TextBox txtInputNeurons;
        private System.Windows.Forms.TextBox txtHiddenNeurons;
        private System.Windows.Forms.TextBox txtOutputNeurons;
        private System.Windows.Forms.CheckBox chkDesignNet;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}