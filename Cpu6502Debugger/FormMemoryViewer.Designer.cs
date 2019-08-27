namespace Cpu6502Debugger {
    partial class FormMemoryViewer {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.byteViewer = new System.ComponentModel.Design.ByteViewer();
            this.SuspendLayout();
            // 
            // byteViewer
            // 
            this.byteViewer.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.byteViewer.ColumnCount = 1;
            this.byteViewer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.byteViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.byteViewer.Location = new System.Drawing.Point(0, 0);
            this.byteViewer.Name = "byteViewer";
            this.byteViewer.RowCount = 1;
            this.byteViewer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.byteViewer.Size = new System.Drawing.Size(702, 434);
            this.byteViewer.TabIndex = 0;
            this.byteViewer.Text = "button1";
            // 
            // FormMemoryViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 434);
            this.Controls.Add(this.byteViewer);
            this.Name = "FormMemoryViewer";
            this.Text = "6502 Memory";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMemoryViewer_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        public System.ComponentModel.Design.ByteViewer byteViewer;
    }
}