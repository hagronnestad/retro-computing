namespace Commodore64
{
    partial class FormLoadSequentialFileOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLoadSequentialFileOptions));
            btnCancel = new System.Windows.Forms.Button();
            btnOK = new System.Windows.Forms.Button();
            chkClearScreen = new System.Windows.Forms.CheckBox();
            numBytesToRemoveFromEnd = new System.Windows.Forms.NumericUpDown();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)numBytesToRemoveFromEnd).BeginInit();
            SuspendLayout();
            // 
            // btnCancel
            // 
            btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(225, 174);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnOK.Location = new System.Drawing.Point(144, 174);
            btnOK.Name = "btnOK";
            btnOK.Size = new System.Drawing.Size(75, 23);
            btnOK.TabIndex = 2;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // chkClearScreen
            // 
            chkClearScreen.AutoSize = true;
            chkClearScreen.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            chkClearScreen.Location = new System.Drawing.Point(12, 12);
            chkClearScreen.Name = "chkClearScreen";
            chkClearScreen.Size = new System.Drawing.Size(91, 19);
            chkClearScreen.TabIndex = 0;
            chkClearScreen.Text = "Clear Screen";
            chkClearScreen.UseVisualStyleBackColor = true;
            // 
            // numBytesToRemoveFromEnd
            // 
            numBytesToRemoveFromEnd.Location = new System.Drawing.Point(14, 97);
            numBytesToRemoveFromEnd.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numBytesToRemoveFromEnd.Name = "numBytesToRemoveFromEnd";
            numBytesToRemoveFromEnd.Size = new System.Drawing.Size(54, 23);
            numBytesToRemoveFromEnd.TabIndex = 1;
            // 
            // label1
            // 
            label1.ForeColor = System.Drawing.SystemColors.GrayText;
            label1.Location = new System.Drawing.Point(12, 34);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(290, 35);
            label1.TabIndex = 4;
            label1.Text = "Check this box to clear the screen before loading the file.";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(12, 79);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(189, 15);
            label2.TabIndex = 5;
            label2.Text = "Remove Bytes From End";
            // 
            // label3
            // 
            label3.ForeColor = System.Drawing.SystemColors.GrayText;
            label3.Location = new System.Drawing.Point(12, 123);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(290, 35);
            label3.TabIndex = 6;
            label3.Text = "This can prevent content scrolling of screen if a \"full page\" sequential file is being loaded.";
            // 
            // FormLoadSequentialFileOptions
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(312, 209);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(numBytesToRemoveFromEnd);
            Controls.Add(chkClearScreen);
            Controls.Add(btnOK);
            Controls.Add(btnCancel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormLoadSequentialFileOptions";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Load Sequential File Options";
            ((System.ComponentModel.ISupportInitialize)numBytesToRemoveFromEnd).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.CheckBox chkClearScreen;
        private System.Windows.Forms.NumericUpDown numBytesToRemoveFromEnd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}