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
            this.lbTotalSamples = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbTrainingCount = new System.Windows.Forms.Label();
            this.lbValidationCount = new System.Windows.Forms.Label();
            this.lbTestCount = new System.Windows.Forms.Label();
            this.txtTotalCountBox = new System.Windows.Forms.TextBox();
            this.lbTotalCount = new System.Windows.Forms.Label();
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
            this.btnDefault.Location = new System.Drawing.Point(19, 226);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(75, 23);
            this.btnDefault.TabIndex = 3;
            this.btnDefault.Text = "Default";
            this.btnDefault.UseVisualStyleBackColor = true;
            this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(117, 226);
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
            this.btnCancel.Location = new System.Drawing.Point(202, 226);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Training set";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Validation set";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 141);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Test set";
            // 
            // txtPercentOfTrnSet
            // 
            this.txtPercentOfTrnSet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPercentOfTrnSet.Location = new System.Drawing.Point(90, 65);
            this.txtPercentOfTrnSet.Name = "txtPercentOfTrnSet";
            this.txtPercentOfTrnSet.Size = new System.Drawing.Size(60, 21);
            this.txtPercentOfTrnSet.TabIndex = 0;
            this.txtPercentOfTrnSet.Text = "0";
            this.txtPercentOfTrnSet.TextChanged += new System.EventHandler(this.txtPercentOfDataSet_TextChanged);
            // 
            // txtPercentOfVldSet
            // 
            this.txtPercentOfVldSet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPercentOfVldSet.Location = new System.Drawing.Point(90, 102);
            this.txtPercentOfVldSet.Name = "txtPercentOfVldSet";
            this.txtPercentOfVldSet.Size = new System.Drawing.Size(60, 21);
            this.txtPercentOfVldSet.TabIndex = 1;
            this.txtPercentOfVldSet.Text = "0";
            this.txtPercentOfVldSet.TextChanged += new System.EventHandler(this.txtPercentOfDataSet_TextChanged);
            // 
            // txtPercentOfTstSet
            // 
            this.txtPercentOfTstSet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPercentOfTstSet.Location = new System.Drawing.Point(90, 139);
            this.txtPercentOfTstSet.Name = "txtPercentOfTstSet";
            this.txtPercentOfTstSet.Size = new System.Drawing.Size(60, 21);
            this.txtPercentOfTstSet.TabIndex = 2;
            this.txtPercentOfTstSet.Text = "0";
            this.txtPercentOfTstSet.TextChanged += new System.EventHandler(this.txtPercentOfDataSet_TextChanged);
            // 
            // lbTotalSamples
            // 
            this.lbTotalSamples.AutoSize = true;
            this.lbTotalSamples.Location = new System.Drawing.Point(12, 175);
            this.lbTotalSamples.Name = "lbTotalSamples";
            this.lbTotalSamples.Size = new System.Drawing.Size(72, 13);
            this.lbTotalSamples.TabIndex = 11;
            this.lbTotalSamples.Text = "Total samples";
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(9, 204);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(271, 2);
            this.label2.TabIndex = 12;
            // 
            // lbTrainingCount
            // 
            this.lbTrainingCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbTrainingCount.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lbTrainingCount.Location = new System.Drawing.Point(171, 63);
            this.lbTrainingCount.Name = "lbTrainingCount";
            this.lbTrainingCount.Size = new System.Drawing.Size(103, 21);
            this.lbTrainingCount.TabIndex = 13;
            this.lbTrainingCount.Text = "0";
            this.lbTrainingCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbValidationCount
            // 
            this.lbValidationCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbValidationCount.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lbValidationCount.Location = new System.Drawing.Point(171, 100);
            this.lbValidationCount.Name = "lbValidationCount";
            this.lbValidationCount.Size = new System.Drawing.Size(103, 21);
            this.lbValidationCount.TabIndex = 13;
            this.lbValidationCount.Text = "0";
            this.lbValidationCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbTestCount
            // 
            this.lbTestCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbTestCount.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lbTestCount.Location = new System.Drawing.Point(171, 137);
            this.lbTestCount.Name = "lbTestCount";
            this.lbTestCount.Size = new System.Drawing.Size(103, 21);
            this.lbTestCount.TabIndex = 13;
            this.lbTestCount.Text = "0";
            this.lbTestCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTotalCountBox
            // 
            this.txtTotalCountBox.BackColor = System.Drawing.SystemColors.Window;
            this.txtTotalCountBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTotalCountBox.HideSelection = false;
            this.txtTotalCountBox.Location = new System.Drawing.Point(90, 173);
            this.txtTotalCountBox.Name = "txtTotalCountBox";
            this.txtTotalCountBox.ReadOnly = true;
            this.txtTotalCountBox.Size = new System.Drawing.Size(60, 21);
            this.txtTotalCountBox.TabIndex = 2;
            this.txtTotalCountBox.Text = "1";
            this.txtTotalCountBox.TextChanged += new System.EventHandler(this.txtPercentOfDataSet_TextChanged);
            // 
            // lbTotalCount
            // 
            this.lbTotalCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbTotalCount.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lbTotalCount.Location = new System.Drawing.Point(171, 171);
            this.lbTotalCount.Name = "lbTotalCount";
            this.lbTotalCount.Size = new System.Drawing.Size(103, 21);
            this.lbTotalCount.TabIndex = 13;
            this.lbTotalCount.Text = "0";
            this.lbTotalCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // F004_DataPartitionOptions
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(308, 261);
            this.Controls.Add(this.lbTotalCount);
            this.Controls.Add(this.lbTestCount);
            this.Controls.Add(this.lbValidationCount);
            this.Controls.Add(this.lbTrainingCount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtTotalCountBox);
            this.Controls.Add(this.txtPercentOfTstSet);
            this.Controls.Add(this.txtPercentOfVldSet);
            this.Controls.Add(this.txtPercentOfTrnSet);
            this.Controls.Add(this.lbTotalSamples);
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
        private System.Windows.Forms.Label lbTotalSamples;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbTrainingCount;
        private System.Windows.Forms.Label lbValidationCount;
        private System.Windows.Forms.Label lbTestCount;
        private System.Windows.Forms.TextBox txtTotalCountBox;
        private System.Windows.Forms.Label lbTotalCount;
    }
}