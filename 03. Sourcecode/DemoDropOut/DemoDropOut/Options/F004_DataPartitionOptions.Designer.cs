namespace DemoDropOut.Options
{
    partial class F004_DataPartitionOptions
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
            this.chkRandomizationApproach = new System.Windows.Forms.RadioButton();
            this.chkSpecificOrder = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDefault = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPercentOfTrnSet = new System.Windows.Forms.TextBox();
            this.txtPercentOfVldSet = new System.Windows.Forms.TextBox();
            this.txtPercentOfTstSet = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // chkRandomizationApproach
            // 
            this.chkRandomizationApproach.AutoSize = true;
            this.chkRandomizationApproach.Checked = true;
            this.chkRandomizationApproach.Location = new System.Drawing.Point(12, 12);
            this.chkRandomizationApproach.Name = "chkRandomizationApproach";
            this.chkRandomizationApproach.Size = new System.Drawing.Size(77, 17);
            this.chkRandomizationApproach.TabIndex = 6;
            this.chkRandomizationApproach.TabStop = true;
            this.chkRandomizationApproach.Text = "Randomize";
            this.chkRandomizationApproach.UseVisualStyleBackColor = true;
            // 
            // chkSpecificOrder
            // 
            this.chkSpecificOrder.AutoSize = true;
            this.chkSpecificOrder.Enabled = false;
            this.chkSpecificOrder.Location = new System.Drawing.Point(127, 12);
            this.chkSpecificOrder.Name = "chkSpecificOrder";
            this.chkSpecificOrder.Size = new System.Drawing.Size(92, 17);
            this.chkSpecificOrder.TabIndex = 7;
            this.chkSpecificOrder.Text = "Specific Order";
            this.chkSpecificOrder.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(9, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(271, 2);
            this.label1.TabIndex = 8;
            // 
            // btnDefault
            // 
            this.btnDefault.Location = new System.Drawing.Point(15, 128);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(75, 23);
            this.btnDefault.TabIndex = 3;
            this.btnDefault.Text = "Default";
            this.btnDefault.UseVisualStyleBackColor = true;
            this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(113, 128);
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
            this.btnCancel.Location = new System.Drawing.Point(198, 128);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Training set (%)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(99, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Validation set (%)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(194, 59);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Test set (%)";
            // 
            // txtPercentOfTrnSet
            // 
            this.txtPercentOfTrnSet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPercentOfTrnSet.Location = new System.Drawing.Point(15, 82);
            this.txtPercentOfTrnSet.Name = "txtPercentOfTrnSet";
            this.txtPercentOfTrnSet.Size = new System.Drawing.Size(68, 21);
            this.txtPercentOfTrnSet.TabIndex = 0;
            // 
            // txtPercentOfVldSet
            // 
            this.txtPercentOfVldSet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPercentOfVldSet.Location = new System.Drawing.Point(106, 82);
            this.txtPercentOfVldSet.Name = "txtPercentOfVldSet";
            this.txtPercentOfVldSet.Size = new System.Drawing.Size(68, 21);
            this.txtPercentOfVldSet.TabIndex = 1;
            // 
            // txtPercentOfTstSet
            // 
            this.txtPercentOfTstSet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPercentOfTstSet.Location = new System.Drawing.Point(197, 82);
            this.txtPercentOfTstSet.Name = "txtPercentOfTstSet";
            this.txtPercentOfTstSet.Size = new System.Drawing.Size(68, 21);
            this.txtPercentOfTstSet.TabIndex = 2;
            // 
            // F004_DataPartitionOptions
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(288, 169);
            this.Controls.Add(this.txtPercentOfTstSet);
            this.Controls.Add(this.txtPercentOfVldSet);
            this.Controls.Add(this.txtPercentOfTrnSet);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnDefault);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkSpecificOrder);
            this.Controls.Add(this.chkRandomizationApproach);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "F004_DataPartitionOptions";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Data Partition Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton chkRandomizationApproach;
        private System.Windows.Forms.RadioButton chkSpecificOrder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDefault;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPercentOfTrnSet;
        private System.Windows.Forms.TextBox txtPercentOfVldSet;
        private System.Windows.Forms.TextBox txtPercentOfTstSet;
    }
}