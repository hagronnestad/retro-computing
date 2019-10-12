namespace Debugger {
    partial class FormDebugger {
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
            this.btnStep = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkReserved = new System.Windows.Forms.CheckBox();
            this.lblAddressingMode = new System.Windows.Forms.Label();
            this.lblOpCode = new System.Windows.Forms.Label();
            this.txtYR = new System.Windows.Forms.TextBox();
            this.txtXR = new System.Windows.Forms.TextBox();
            this.txtAR = new System.Windows.Forms.TextBox();
            this.txtSP = new System.Windows.Forms.TextBox();
            this.txtPC = new System.Windows.Forms.TextBox();
            this.lblPreviousYR = new System.Windows.Forms.Label();
            this.lblPreviousXR = new System.Windows.Forms.Label();
            this.lblPreviousAR = new System.Windows.Forms.Label();
            this.lblPreviousSP = new System.Windows.Forms.Label();
            this.lblPreviousPC = new System.Windows.Forms.Label();
            this.chkNegative = new System.Windows.Forms.CheckBox();
            this.chkOverflow = new System.Windows.Forms.CheckBox();
            this.chkBreakCommand = new System.Windows.Forms.CheckBox();
            this.chkDecimalMode = new System.Windows.Forms.CheckBox();
            this.chkIrqDisable = new System.Windows.Forms.CheckBox();
            this.chkZero = new System.Windows.Forms.CheckBox();
            this.chkCarry = new System.Windows.Forms.CheckBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.dgWatch = new System.Windows.Forms.DataGridView();
            this.clmAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmValueHex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmValueDecimal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgWatch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStep
            // 
            this.btnStep.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStep.Location = new System.Drawing.Point(815, 692);
            this.btnStep.Margin = new System.Windows.Forms.Padding(4);
            this.btnStep.Name = "btnStep";
            this.btnStep.Size = new System.Drawing.Size(100, 32);
            this.btnStep.TabIndex = 0;
            this.btnStep.Text = "Step";
            this.btnStep.UseVisualStyleBackColor = true;
            this.btnStep.Click += new System.EventHandler(this.BtnStep_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkReserved);
            this.groupBox1.Controls.Add(this.lblAddressingMode);
            this.groupBox1.Controls.Add(this.lblOpCode);
            this.groupBox1.Controls.Add(this.txtYR);
            this.groupBox1.Controls.Add(this.txtXR);
            this.groupBox1.Controls.Add(this.txtAR);
            this.groupBox1.Controls.Add(this.txtSP);
            this.groupBox1.Controls.Add(this.txtPC);
            this.groupBox1.Controls.Add(this.lblPreviousYR);
            this.groupBox1.Controls.Add(this.lblPreviousXR);
            this.groupBox1.Controls.Add(this.lblPreviousAR);
            this.groupBox1.Controls.Add(this.lblPreviousSP);
            this.groupBox1.Controls.Add(this.lblPreviousPC);
            this.groupBox1.Controls.Add(this.chkNegative);
            this.groupBox1.Controls.Add(this.chkOverflow);
            this.groupBox1.Controls.Add(this.chkBreakCommand);
            this.groupBox1.Controls.Add(this.chkDecimalMode);
            this.groupBox1.Controls.Add(this.chkIrqDisable);
            this.groupBox1.Controls.Add(this.chkZero);
            this.groupBox1.Controls.Add(this.chkCarry);
            this.groupBox1.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(380, 344);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Current State";
            // 
            // chkReserved
            // 
            this.chkReserved.AutoSize = true;
            this.chkReserved.Location = new System.Drawing.Point(210, 225);
            this.chkReserved.Margin = new System.Windows.Forms.Padding(4);
            this.chkReserved.Name = "chkReserved";
            this.chkReserved.Size = new System.Drawing.Size(91, 22);
            this.chkReserved.TabIndex = 31;
            this.chkReserved.Text = "Reserved";
            this.chkReserved.UseVisualStyleBackColor = true;
            // 
            // lblAddressingMode
            // 
            this.lblAddressingMode.AutoSize = true;
            this.lblAddressingMode.Location = new System.Drawing.Point(9, 68);
            this.lblAddressingMode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAddressingMode.Name = "lblAddressingMode";
            this.lblAddressingMode.Size = new System.Drawing.Size(120, 18);
            this.lblAddressingMode.TabIndex = 18;
            this.lblAddressingMode.Text = "AddressingMode";
            // 
            // lblOpCode
            // 
            this.lblOpCode.AutoSize = true;
            this.lblOpCode.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOpCode.Location = new System.Drawing.Point(8, 35);
            this.lblOpCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOpCode.Name = "lblOpCode";
            this.lblOpCode.Size = new System.Drawing.Size(82, 24);
            this.lblOpCode.TabIndex = 17;
            this.lblOpCode.Text = "OPCODE";
            // 
            // txtYR
            // 
            this.txtYR.Location = new System.Drawing.Point(49, 235);
            this.txtYR.Margin = new System.Windows.Forms.Padding(4);
            this.txtYR.Name = "txtYR";
            this.txtYR.Size = new System.Drawing.Size(105, 25);
            this.txtYR.TabIndex = 16;
            // 
            // txtXR
            // 
            this.txtXR.Location = new System.Drawing.Point(49, 204);
            this.txtXR.Margin = new System.Windows.Forms.Padding(4);
            this.txtXR.Name = "txtXR";
            this.txtXR.Size = new System.Drawing.Size(105, 25);
            this.txtXR.TabIndex = 15;
            // 
            // txtAR
            // 
            this.txtAR.Location = new System.Drawing.Point(49, 172);
            this.txtAR.Margin = new System.Windows.Forms.Padding(4);
            this.txtAR.Name = "txtAR";
            this.txtAR.Size = new System.Drawing.Size(105, 25);
            this.txtAR.TabIndex = 14;
            // 
            // txtSP
            // 
            this.txtSP.Location = new System.Drawing.Point(49, 140);
            this.txtSP.Margin = new System.Windows.Forms.Padding(4);
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(105, 25);
            this.txtSP.TabIndex = 13;
            // 
            // txtPC
            // 
            this.txtPC.Location = new System.Drawing.Point(49, 108);
            this.txtPC.Margin = new System.Windows.Forms.Padding(4);
            this.txtPC.Name = "txtPC";
            this.txtPC.Size = new System.Drawing.Size(105, 25);
            this.txtPC.TabIndex = 12;
            // 
            // lblPreviousYR
            // 
            this.lblPreviousYR.Location = new System.Drawing.Point(8, 241);
            this.lblPreviousYR.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPreviousYR.Name = "lblPreviousYR";
            this.lblPreviousYR.Size = new System.Drawing.Size(33, 19);
            this.lblPreviousYR.TabIndex = 11;
            this.lblPreviousYR.Text = "YR:";
            // 
            // lblPreviousXR
            // 
            this.lblPreviousXR.Location = new System.Drawing.Point(8, 209);
            this.lblPreviousXR.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPreviousXR.Name = "lblPreviousXR";
            this.lblPreviousXR.Size = new System.Drawing.Size(33, 19);
            this.lblPreviousXR.TabIndex = 10;
            this.lblPreviousXR.Text = "XR:";
            // 
            // lblPreviousAR
            // 
            this.lblPreviousAR.Location = new System.Drawing.Point(8, 177);
            this.lblPreviousAR.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPreviousAR.Name = "lblPreviousAR";
            this.lblPreviousAR.Size = new System.Drawing.Size(33, 19);
            this.lblPreviousAR.TabIndex = 9;
            this.lblPreviousAR.Text = "AR:";
            // 
            // lblPreviousSP
            // 
            this.lblPreviousSP.Location = new System.Drawing.Point(8, 145);
            this.lblPreviousSP.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPreviousSP.Name = "lblPreviousSP";
            this.lblPreviousSP.Size = new System.Drawing.Size(33, 19);
            this.lblPreviousSP.TabIndex = 8;
            this.lblPreviousSP.Text = "SP:";
            // 
            // lblPreviousPC
            // 
            this.lblPreviousPC.Location = new System.Drawing.Point(8, 114);
            this.lblPreviousPC.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPreviousPC.Name = "lblPreviousPC";
            this.lblPreviousPC.Size = new System.Drawing.Size(33, 19);
            this.lblPreviousPC.TabIndex = 7;
            this.lblPreviousPC.Text = "PC:";
            // 
            // chkNegative
            // 
            this.chkNegative.AutoSize = true;
            this.chkNegative.Location = new System.Drawing.Point(210, 287);
            this.chkNegative.Margin = new System.Windows.Forms.Padding(4);
            this.chkNegative.Name = "chkNegative";
            this.chkNegative.Size = new System.Drawing.Size(91, 22);
            this.chkNegative.TabIndex = 6;
            this.chkNegative.Text = "Negative";
            this.chkNegative.UseVisualStyleBackColor = true;
            // 
            // chkOverflow
            // 
            this.chkOverflow.AutoSize = true;
            this.chkOverflow.Location = new System.Drawing.Point(210, 255);
            this.chkOverflow.Margin = new System.Windows.Forms.Padding(4);
            this.chkOverflow.Name = "chkOverflow";
            this.chkOverflow.Size = new System.Drawing.Size(91, 22);
            this.chkOverflow.TabIndex = 5;
            this.chkOverflow.Text = "Overflow";
            this.chkOverflow.UseVisualStyleBackColor = true;
            // 
            // chkBreakCommand
            // 
            this.chkBreakCommand.AutoSize = true;
            this.chkBreakCommand.Location = new System.Drawing.Point(210, 195);
            this.chkBreakCommand.Margin = new System.Windows.Forms.Padding(4);
            this.chkBreakCommand.Name = "chkBreakCommand";
            this.chkBreakCommand.Size = new System.Drawing.Size(123, 22);
            this.chkBreakCommand.TabIndex = 4;
            this.chkBreakCommand.Text = "BreakCommand";
            this.chkBreakCommand.UseVisualStyleBackColor = true;
            // 
            // chkDecimalMode
            // 
            this.chkDecimalMode.AutoSize = true;
            this.chkDecimalMode.Location = new System.Drawing.Point(210, 163);
            this.chkDecimalMode.Margin = new System.Windows.Forms.Padding(4);
            this.chkDecimalMode.Name = "chkDecimalMode";
            this.chkDecimalMode.Size = new System.Drawing.Size(115, 22);
            this.chkDecimalMode.TabIndex = 3;
            this.chkDecimalMode.Text = "DecimalMode";
            this.chkDecimalMode.UseVisualStyleBackColor = true;
            // 
            // chkIrqDisable
            // 
            this.chkIrqDisable.AutoSize = true;
            this.chkIrqDisable.Location = new System.Drawing.Point(210, 131);
            this.chkIrqDisable.Margin = new System.Windows.Forms.Padding(4);
            this.chkIrqDisable.Name = "chkIrqDisable";
            this.chkIrqDisable.Size = new System.Drawing.Size(107, 22);
            this.chkIrqDisable.TabIndex = 2;
            this.chkIrqDisable.Text = "IrqDisable";
            this.chkIrqDisable.UseVisualStyleBackColor = true;
            // 
            // chkZero
            // 
            this.chkZero.AutoSize = true;
            this.chkZero.Location = new System.Drawing.Point(210, 99);
            this.chkZero.Margin = new System.Windows.Forms.Padding(4);
            this.chkZero.Name = "chkZero";
            this.chkZero.Size = new System.Drawing.Size(59, 22);
            this.chkZero.TabIndex = 1;
            this.chkZero.Text = "Zero";
            this.chkZero.UseVisualStyleBackColor = true;
            // 
            // chkCarry
            // 
            this.chkCarry.AutoSize = true;
            this.chkCarry.Location = new System.Drawing.Point(210, 67);
            this.chkCarry.Margin = new System.Windows.Forms.Padding(4);
            this.chkCarry.Name = "chkCarry";
            this.chkCarry.Size = new System.Drawing.Size(67, 22);
            this.chkCarry.TabIndex = 0;
            this.chkCarry.Text = "Carry";
            this.chkCarry.UseVisualStyleBackColor = true;
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Location = new System.Drawing.Point(702, 692);
            this.btnReset.Margin = new System.Windows.Forms.Padding(4);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(100, 32);
            this.btnReset.TabIndex = 4;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.BtnReset_Click);
            // 
            // dgWatch
            // 
            this.dgWatch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgWatch.BackgroundColor = System.Drawing.Color.White;
            this.dgWatch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgWatch.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmAddress,
            this.clmValueHex,
            this.clmValueDecimal,
            this.clmDescription});
            this.dgWatch.EnableHeadersVisualStyles = false;
            this.dgWatch.Location = new System.Drawing.Point(13, 375);
            this.dgWatch.Name = "dgWatch";
            this.dgWatch.Size = new System.Drawing.Size(902, 299);
            this.dgWatch.TabIndex = 6;
            this.dgWatch.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgWatch_CellEndEdit);
            // 
            // clmAddress
            // 
            this.clmAddress.HeaderText = "Address";
            this.clmAddress.Name = "clmAddress";
            this.clmAddress.Width = 150;
            // 
            // clmValueHex
            // 
            this.clmValueHex.HeaderText = "Value Hex";
            this.clmValueHex.Name = "clmValueHex";
            this.clmValueHex.Width = 150;
            // 
            // clmValueDecimal
            // 
            this.clmValueDecimal.HeaderText = "Value Dec";
            this.clmValueDecimal.Name = "clmValueDecimal";
            this.clmValueDecimal.Width = 150;
            // 
            // clmDescription
            // 
            this.clmDescription.HeaderText = "Description";
            this.clmDescription.Name = "clmDescription";
            this.clmDescription.Width = 300;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(432, 23);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(406, 25);
            this.textBox1.TabIndex = 7;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(432, 80);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(483, 277);
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureBox1_Paint);
            // 
            // FormDebugger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 737);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.dgWatch);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnStep);
            this.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormDebugger";
            this.Text = "Debugger";
            this.Load += new System.EventHandler(this.FormDebugger_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgWatch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStep;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkCarry;
        private System.Windows.Forms.CheckBox chkNegative;
        private System.Windows.Forms.CheckBox chkOverflow;
        private System.Windows.Forms.CheckBox chkBreakCommand;
        private System.Windows.Forms.CheckBox chkDecimalMode;
        private System.Windows.Forms.CheckBox chkIrqDisable;
        private System.Windows.Forms.CheckBox chkZero;
        private System.Windows.Forms.Label lblPreviousPC;
        private System.Windows.Forms.Label lblPreviousSP;
        private System.Windows.Forms.Label lblPreviousYR;
        private System.Windows.Forms.Label lblPreviousXR;
        private System.Windows.Forms.Label lblPreviousAR;
        private System.Windows.Forms.TextBox txtYR;
        private System.Windows.Forms.TextBox txtXR;
        private System.Windows.Forms.TextBox txtAR;
        private System.Windows.Forms.TextBox txtSP;
        private System.Windows.Forms.TextBox txtPC;
        private System.Windows.Forms.Label lblAddressingMode;
        private System.Windows.Forms.Label lblOpCode;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.DataGridView dgWatch;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmValueHex;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmValueDecimal;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmDescription;
        private System.Windows.Forms.CheckBox chkReserved;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

