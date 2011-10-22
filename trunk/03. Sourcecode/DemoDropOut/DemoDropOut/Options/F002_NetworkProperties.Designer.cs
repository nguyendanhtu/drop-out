namespace DemoDropOut.Options
{
    partial class F002_NetworkProperties
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDefault = new System.Windows.Forms.Button();
            this.rdbtnBackPropagation = new System.Windows.Forms.RadioButton();
            this.rdbtnQuickPropagation = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnDefault);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 301);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(540, 54);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(540, 301);
            this.panel2.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdbtnQuickPropagation);
            this.groupBox1.Controls.Add(this.rdbtnBackPropagation);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(261, 149);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Training algorithm";
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(12, 167);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(261, 115);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Training algorithm\'s parameters";
            // 
            // groupBox3
            // 
            this.groupBox3.Location = new System.Drawing.Point(279, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(249, 270);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Stop Training Conditions";
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
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(453, 19);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnDefault
            // 
            this.btnDefault.Location = new System.Drawing.Point(12, 19);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(75, 23);
            this.btnDefault.TabIndex = 1;
            this.btnDefault.Text = "Default";
            this.btnDefault.UseVisualStyleBackColor = true;
            // 
            // rdbtnBackPropagation
            // 
            this.rdbtnBackPropagation.AutoSize = true;
            this.rdbtnBackPropagation.Checked = true;
            this.rdbtnBackPropagation.Location = new System.Drawing.Point(7, 21);
            this.rdbtnBackPropagation.Name = "rdbtnBackPropagation";
            this.rdbtnBackPropagation.Size = new System.Drawing.Size(108, 17);
            this.rdbtnBackPropagation.TabIndex = 0;
            this.rdbtnBackPropagation.TabStop = true;
            this.rdbtnBackPropagation.Text = "Back Propagation";
            this.rdbtnBackPropagation.UseVisualStyleBackColor = true;
            // 
            // rdbtnQuickPropagation
            // 
            this.rdbtnQuickPropagation.AutoSize = true;
            this.rdbtnQuickPropagation.Enabled = false;
            this.rdbtnQuickPropagation.Location = new System.Drawing.Point(7, 44);
            this.rdbtnQuickPropagation.Name = "rdbtnQuickPropagation";
            this.rdbtnQuickPropagation.Size = new System.Drawing.Size(112, 17);
            this.rdbtnQuickPropagation.TabIndex = 0;
            this.rdbtnQuickPropagation.Text = "Quick Propagation";
            this.rdbtnQuickPropagation.UseVisualStyleBackColor = true;
            // 
            // F002_NetworkProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 355);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "F002_NetworkProperties";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Network Properties";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
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
        private System.Windows.Forms.RadioButton rdbtnQuickPropagation;
        private System.Windows.Forms.RadioButton rdbtnBackPropagation;
    }
}