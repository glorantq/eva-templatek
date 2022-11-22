namespace WinForms_ButtonMatrix {
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
            this.buttonMatrix1 = new WinForms_ButtonMatrix.ButtonMatrix();
            this.SuspendLayout();
            // 
            // buttonMatrix1
            // 
            this.buttonMatrix1.CellHeight = 24;
            this.buttonMatrix1.CellWidth = 48;
            this.buttonMatrix1.Columns = 6;
            this.buttonMatrix1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMatrix1.HorizontalAlignment = WinForms_ButtonMatrix.ButtonMatrix.Alignment.CENTER;
            this.buttonMatrix1.Location = new System.Drawing.Point(0, 0);
            this.buttonMatrix1.Name = "buttonMatrix1";
            this.buttonMatrix1.Rows = 6;
            this.buttonMatrix1.Size = new System.Drawing.Size(496, 473);
            this.buttonMatrix1.TabIndex = 0;
            this.buttonMatrix1.VerticalAlignment = WinForms_ButtonMatrix.ButtonMatrix.Alignment.CENTER;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 473);
            this.Controls.Add(this.buttonMatrix1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private ButtonMatrix buttonMatrix1;
    }
}