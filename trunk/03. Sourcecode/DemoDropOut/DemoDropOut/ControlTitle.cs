using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace DropOut.Uc
{
    public class ControlTitle : UserControl
    {
        private Label lbTitle;
    
        private void InitializeComponent()
        {
            this.lbTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.lbTitle.AutoSize = true;
            this.lbTitle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lbTitle.ForeColor = System.Drawing.Color.White;
            this.lbTitle.Location = new System.Drawing.Point(3, 3);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(77, 13);
            this.lbTitle.TabIndex = 0;
            this.lbTitle.Text = "Title Text";
            // 
            // ControlHeader
            // 
            this.BackColor = System.Drawing.Color.Gray;
            this.Controls.Add(this.lbTitle);
            this.Name = "ControlTitle";
            this.Size = new System.Drawing.Size(430, 20);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public ControlTitle()
        {
            InitializeComponent();
            Dock = DockStyle.Top;
        }

        /// <summary>
        /// The delay in millisenconds between animation steps
        /// </summary>
        [Bindable(true), DefaultValue("Title Text"), Description("Title for control.")]
        public string Title
        {
            get { return lbTitle.Text;  }
            set { lbTitle.Text = value; }
        }
    }
}
