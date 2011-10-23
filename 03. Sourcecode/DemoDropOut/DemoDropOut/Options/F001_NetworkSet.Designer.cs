namespace DemoDropOut
{
    partial class F001_NetworkSet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F001_NetworkSet));
            this.toolStripNetworkSet = new System.Windows.Forms.ToolStrip();
            this.c1NetworkSetFlexGrid = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripNetworkSet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.c1NetworkSetFlexGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStripNetworkSet
            // 
            this.toolStripNetworkSet.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
            this.toolStripNetworkSet.Location = new System.Drawing.Point(0, 0);
            this.toolStripNetworkSet.Name = "toolStripNetworkSet";
            this.toolStripNetworkSet.Size = new System.Drawing.Size(634, 25);
            this.toolStripNetworkSet.TabIndex = 0;
            this.toolStripNetworkSet.Text = "toolStrip1";
            // 
            // c1NetworkSetFlexGrid
            // 
            this.c1NetworkSetFlexGrid.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.Light3D;
            this.c1NetworkSetFlexGrid.ColumnInfo = "10,1,0,0,0,90,Columns:";
            this.c1NetworkSetFlexGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.c1NetworkSetFlexGrid.Location = new System.Drawing.Point(0, 25);
            this.c1NetworkSetFlexGrid.Name = "c1NetworkSetFlexGrid";
            this.c1NetworkSetFlexGrid.Size = new System.Drawing.Size(634, 297);
            this.c1NetworkSetFlexGrid.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("c1NetworkSetFlexGrid.Styles"));
            this.c1NetworkSetFlexGrid.TabIndex = 1;
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // F001_NetworkSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 322);
            this.Controls.Add(this.c1NetworkSetFlexGrid);
            this.Controls.Add(this.toolStripNetworkSet);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Name = "F001_NetworkSet";
            this.Text = "Network Set";
            this.toolStripNetworkSet.ResumeLayout(false);
            this.toolStripNetworkSet.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.c1NetworkSetFlexGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripNetworkSet;
        private C1.Win.C1FlexGrid.C1FlexGrid c1NetworkSetFlexGrid;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
    }
}