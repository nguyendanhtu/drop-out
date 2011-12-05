namespace DemoDropOut.Options
{
    partial class F005_PreprocessingOptions
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkOneOfNEncoding = new System.Windows.Forms.RadioButton();
            this.chkBinaryEncoding = new System.Windows.Forms.RadioButton();
            this.chkNumericEncoding = new System.Windows.Forms.RadioButton();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkNumericEncoding);
            this.groupBox1.Controls.Add(this.chkBinaryEncoding);
            this.groupBox1.Controls.Add(this.chkOneOfNEncoding);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(224, 109);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Categorical Column Encoding";
            // 
            // chkOneOfNEncoding
            // 
            this.chkOneOfNEncoding.AutoSize = true;
            this.chkOneOfNEncoding.Checked = true;
            this.chkOneOfNEncoding.Location = new System.Drawing.Point(6, 20);
            this.chkOneOfNEncoding.Name = "chkOneOfNEncoding";
            this.chkOneOfNEncoding.Size = new System.Drawing.Size(70, 17);
            this.chkOneOfNEncoding.TabIndex = 0;
            this.chkOneOfNEncoding.TabStop = true;
            this.chkOneOfNEncoding.Text = "One Of N";
            this.chkOneOfNEncoding.UseVisualStyleBackColor = true;
            // 
            // chkBinaryEncoding
            // 
            this.chkBinaryEncoding.AutoSize = true;
            this.chkBinaryEncoding.Location = new System.Drawing.Point(6, 43);
            this.chkBinaryEncoding.Name = "chkBinaryEncoding";
            this.chkBinaryEncoding.Size = new System.Drawing.Size(55, 17);
            this.chkBinaryEncoding.TabIndex = 0;
            this.chkBinaryEncoding.Text = "Binary";
            this.chkBinaryEncoding.UseVisualStyleBackColor = true;
            // 
            // chkNumericEncoding
            // 
            this.chkNumericEncoding.AutoSize = true;
            this.chkNumericEncoding.Enabled = false;
            this.chkNumericEncoding.Location = new System.Drawing.Point(6, 66);
            this.chkNumericEncoding.Name = "chkNumericEncoding";
            this.chkNumericEncoding.Size = new System.Drawing.Size(63, 17);
            this.chkNumericEncoding.TabIndex = 0;
            this.chkNumericEncoding.Text = "Numeric";
            this.chkNumericEncoding.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(69, 136);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(164, 136);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // F005_PreprocessingOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(251, 170);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "F005_PreprocessingOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Preprocessing Options";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton chkNumericEncoding;
        private System.Windows.Forms.RadioButton chkBinaryEncoding;
        private System.Windows.Forms.RadioButton chkOneOfNEncoding;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}