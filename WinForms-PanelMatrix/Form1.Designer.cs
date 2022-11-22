namespace WinForms_PanelMatrix {
    partial class Form1 {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.panelMatrix1 = new WinForms_PanelMatrix.PanelMatrix();
            this.SuspendLayout();
            // 
            // panelMatrix1
            // 
            this.panelMatrix1.CellHeight = 32;
            this.panelMatrix1.CellWidth = 32;
            this.panelMatrix1.Columns = 10;
            this.panelMatrix1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMatrix1.HorizontalAlignment = WinForms_PanelMatrix.PanelMatrix.Alignment.CENTER;
            this.panelMatrix1.Location = new System.Drawing.Point(0, 0);
            this.panelMatrix1.Name = "panelMatrix1";
            this.panelMatrix1.PanelCreationHook = null;
            this.panelMatrix1.Rows = 10;
            this.panelMatrix1.Size = new System.Drawing.Size(496, 473);
            this.panelMatrix1.TabIndex = 0;
            this.panelMatrix1.VerticalAlignment = WinForms_PanelMatrix.PanelMatrix.Alignment.CENTER;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 473);
            this.Controls.Add(this.panelMatrix1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private PanelMatrix panelMatrix1;
    }
}