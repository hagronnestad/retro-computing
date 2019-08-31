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
            this.chkPreviousReserved = new System.Windows.Forms.CheckBox();
            this.lblPreviousAddressingMode = new System.Windows.Forms.Label();
            this.lblPreviousOpCode = new System.Windows.Forms.Label();
            this.txtPreviousYR = new System.Windows.Forms.TextBox();
            this.txtPreviousXR = new System.Windows.Forms.TextBox();
            this.txtPreviousAR = new System.Windows.Forms.TextBox();
            this.txtPreviousSP = new System.Windows.Forms.TextBox();
            this.txtPreviousPC = new System.Windows.Forms.TextBox();
            this.lblPreviousYR = new System.Windows.Forms.Label();
            this.lblPreviousXR = new System.Windows.Forms.Label();
            this.lblPreviousAR = new System.Windows.Forms.Label();
            this.lblPreviousSP = new System.Windows.Forms.Label();
            this.lblPreviousPC = new System.Windows.Forms.Label();
            this.chkPreviousNegative = new System.Windows.Forms.CheckBox();
            this.chkPreviousOverflow = new System.Windows.Forms.CheckBox();
            this.chkPreviousBreakCommand = new System.Windows.Forms.CheckBox();
            this.chkPreviousDecimalMode = new System.Windows.Forms.CheckBox();
            this.chkPreviousIrqDisable = new System.Windows.Forms.CheckBox();
            this.chkPreviousZero = new System.Windows.Forms.CheckBox();
            this.chkPreviousCarry = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkCurrentReserved = new System.Windows.Forms.CheckBox();
            this.lblSR = new System.Windows.Forms.Label();
            this.lblCurrentAddressingMode = new System.Windows.Forms.Label();
            this.lblCurrentOpCode = new System.Windows.Forms.Label();
            this.txtCurrentYR = new System.Windows.Forms.TextBox();
            this.chkCurrentNegative = new System.Windows.Forms.CheckBox();
            this.txtCurrentXR = new System.Windows.Forms.TextBox();
            this.chkCurrentOverflow = new System.Windows.Forms.CheckBox();
            this.txtCurrentAR = new System.Windows.Forms.TextBox();
            this.chkCurrentBreakCommand = new System.Windows.Forms.CheckBox();
            this.txtCurrentSP = new System.Windows.Forms.TextBox();
            this.chkCurrentDecimalMode = new System.Windows.Forms.CheckBox();
            this.txtCurrentPC = new System.Windows.Forms.TextBox();
            this.lblCurrentYR = new System.Windows.Forms.Label();
            this.chkCurrentIrqDisable = new System.Windows.Forms.CheckBox();
            this.lblCurrentXR = new System.Windows.Forms.Label();
            this.chkCurrentZero = new System.Windows.Forms.CheckBox();
            this.lblCurrentAR = new System.Windows.Forms.Label();
            this.chkCurrentCarry = new System.Windows.Forms.CheckBox();
            this.lblCurrentSP = new System.Windows.Forms.Label();
            this.lblCurrentPC = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.dgWatch = new System.Windows.Forms.DataGridView();
            this.clmAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmValueHex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmValueDecimal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnMemory = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgWatch)).BeginInit();
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
            this.groupBox1.Controls.Add(this.chkPreviousReserved);
            this.groupBox1.Controls.Add(this.lblPreviousAddressingMode);
            this.groupBox1.Controls.Add(this.lblPreviousOpCode);
            this.groupBox1.Controls.Add(this.txtPreviousYR);
            this.groupBox1.Controls.Add(this.txtPreviousXR);
            this.groupBox1.Controls.Add(this.txtPreviousAR);
            this.groupBox1.Controls.Add(this.txtPreviousSP);
            this.groupBox1.Controls.Add(this.txtPreviousPC);
            this.groupBox1.Controls.Add(this.lblPreviousYR);
            this.groupBox1.Controls.Add(this.lblPreviousXR);
            this.groupBox1.Controls.Add(this.lblPreviousAR);
            this.groupBox1.Controls.Add(this.lblPreviousSP);
            this.groupBox1.Controls.Add(this.lblPreviousPC);
            this.groupBox1.Controls.Add(this.chkPreviousNegative);
            this.groupBox1.Controls.Add(this.chkPreviousOverflow);
            this.groupBox1.Controls.Add(this.chkPreviousBreakCommand);
            this.groupBox1.Controls.Add(this.chkPreviousDecimalMode);
            this.groupBox1.Controls.Add(this.chkPreviousIrqDisable);
            this.groupBox1.Controls.Add(this.chkPreviousZero);
            this.groupBox1.Controls.Add(this.chkPreviousCarry);
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
            // chkPreviousReserved
            // 
            this.chkPreviousReserved.AutoSize = true;
            this.chkPreviousReserved.Location = new System.Drawing.Point(210, 225);
            this.chkPreviousReserved.Margin = new System.Windows.Forms.Padding(4);
            this.chkPreviousReserved.Name = "chkPreviousReserved";
            this.chkPreviousReserved.Size = new System.Drawing.Size(91, 22);
            this.chkPreviousReserved.TabIndex = 31;
            this.chkPreviousReserved.Text = "Reserved";
            this.chkPreviousReserved.UseVisualStyleBackColor = true;
            // 
            // lblPreviousAddressingMode
            // 
            this.lblPreviousAddressingMode.AutoSize = true;
            this.lblPreviousAddressingMode.Location = new System.Drawing.Point(9, 68);
            this.lblPreviousAddressingMode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPreviousAddressingMode.Name = "lblPreviousAddressingMode";
            this.lblPreviousAddressingMode.Size = new System.Drawing.Size(120, 18);
            this.lblPreviousAddressingMode.TabIndex = 18;
            this.lblPreviousAddressingMode.Text = "AddressingMode";
            // 
            // lblPreviousOpCode
            // 
            this.lblPreviousOpCode.AutoSize = true;
            this.lblPreviousOpCode.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPreviousOpCode.Location = new System.Drawing.Point(8, 35);
            this.lblPreviousOpCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPreviousOpCode.Name = "lblPreviousOpCode";
            this.lblPreviousOpCode.Size = new System.Drawing.Size(82, 24);
            this.lblPreviousOpCode.TabIndex = 17;
            this.lblPreviousOpCode.Text = "OPCODE";
            // 
            // txtPreviousYR
            // 
            this.txtPreviousYR.Location = new System.Drawing.Point(49, 235);
            this.txtPreviousYR.Margin = new System.Windows.Forms.Padding(4);
            this.txtPreviousYR.Name = "txtPreviousYR";
            this.txtPreviousYR.Size = new System.Drawing.Size(105, 25);
            this.txtPreviousYR.TabIndex = 16;
            // 
            // txtPreviousXR
            // 
            this.txtPreviousXR.Location = new System.Drawing.Point(49, 204);
            this.txtPreviousXR.Margin = new System.Windows.Forms.Padding(4);
            this.txtPreviousXR.Name = "txtPreviousXR";
            this.txtPreviousXR.Size = new System.Drawing.Size(105, 25);
            this.txtPreviousXR.TabIndex = 15;
            // 
            // txtPreviousAR
            // 
            this.txtPreviousAR.Location = new System.Drawing.Point(49, 172);
            this.txtPreviousAR.Margin = new System.Windows.Forms.Padding(4);
            this.txtPreviousAR.Name = "txtPreviousAR";
            this.txtPreviousAR.Size = new System.Drawing.Size(105, 25);
            this.txtPreviousAR.TabIndex = 14;
            // 
            // txtPreviousSP
            // 
            this.txtPreviousSP.Location = new System.Drawing.Point(49, 140);
            this.txtPreviousSP.Margin = new System.Windows.Forms.Padding(4);
            this.txtPreviousSP.Name = "txtPreviousSP";
            this.txtPreviousSP.Size = new System.Drawing.Size(105, 25);
            this.txtPreviousSP.TabIndex = 13;
            // 
            // txtPreviousPC
            // 
            this.txtPreviousPC.Location = new System.Drawing.Point(49, 108);
            this.txtPreviousPC.Margin = new System.Windows.Forms.Padding(4);
            this.txtPreviousPC.Name = "txtPreviousPC";
            this.txtPreviousPC.Size = new System.Drawing.Size(105, 25);
            this.txtPreviousPC.TabIndex = 12;
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
            // chkPreviousNegative
            // 
            this.chkPreviousNegative.AutoSize = true;
            this.chkPreviousNegative.Location = new System.Drawing.Point(210, 287);
            this.chkPreviousNegative.Margin = new System.Windows.Forms.Padding(4);
            this.chkPreviousNegative.Name = "chkPreviousNegative";
            this.chkPreviousNegative.Size = new System.Drawing.Size(91, 22);
            this.chkPreviousNegative.TabIndex = 6;
            this.chkPreviousNegative.Text = "Negative";
            this.chkPreviousNegative.UseVisualStyleBackColor = true;
            // 
            // chkPreviousOverflow
            // 
            this.chkPreviousOverflow.AutoSize = true;
            this.chkPreviousOverflow.Location = new System.Drawing.Point(210, 255);
            this.chkPreviousOverflow.Margin = new System.Windows.Forms.Padding(4);
            this.chkPreviousOverflow.Name = "chkPreviousOverflow";
            this.chkPreviousOverflow.Size = new System.Drawing.Size(91, 22);
            this.chkPreviousOverflow.TabIndex = 5;
            this.chkPreviousOverflow.Text = "Overflow";
            this.chkPreviousOverflow.UseVisualStyleBackColor = true;
            // 
            // chkPreviousBreakCommand
            // 
            this.chkPreviousBreakCommand.AutoSize = true;
            this.chkPreviousBreakCommand.Location = new System.Drawing.Point(210, 195);
            this.chkPreviousBreakCommand.Margin = new System.Windows.Forms.Padding(4);
            this.chkPreviousBreakCommand.Name = "chkPreviousBreakCommand";
            this.chkPreviousBreakCommand.Size = new System.Drawing.Size(123, 22);
            this.chkPreviousBreakCommand.TabIndex = 4;
            this.chkPreviousBreakCommand.Text = "BreakCommand";
            this.chkPreviousBreakCommand.UseVisualStyleBackColor = true;
            // 
            // chkPreviousDecimalMode
            // 
            this.chkPreviousDecimalMode.AutoSize = true;
            this.chkPreviousDecimalMode.Location = new System.Drawing.Point(210, 163);
            this.chkPreviousDecimalMode.Margin = new System.Windows.Forms.Padding(4);
            this.chkPreviousDecimalMode.Name = "chkPreviousDecimalMode";
            this.chkPreviousDecimalMode.Size = new System.Drawing.Size(115, 22);
            this.chkPreviousDecimalMode.TabIndex = 3;
            this.chkPreviousDecimalMode.Text = "DecimalMode";
            this.chkPreviousDecimalMode.UseVisualStyleBackColor = true;
            // 
            // chkPreviousIrqDisable
            // 
            this.chkPreviousIrqDisable.AutoSize = true;
            this.chkPreviousIrqDisable.Location = new System.Drawing.Point(210, 131);
            this.chkPreviousIrqDisable.Margin = new System.Windows.Forms.Padding(4);
            this.chkPreviousIrqDisable.Name = "chkPreviousIrqDisable";
            this.chkPreviousIrqDisable.Size = new System.Drawing.Size(107, 22);
            this.chkPreviousIrqDisable.TabIndex = 2;
            this.chkPreviousIrqDisable.Text = "IrqDisable";
            this.chkPreviousIrqDisable.UseVisualStyleBackColor = true;
            // 
            // chkPreviousZero
            // 
            this.chkPreviousZero.AutoSize = true;
            this.chkPreviousZero.Location = new System.Drawing.Point(210, 99);
            this.chkPreviousZero.Margin = new System.Windows.Forms.Padding(4);
            this.chkPreviousZero.Name = "chkPreviousZero";
            this.chkPreviousZero.Size = new System.Drawing.Size(59, 22);
            this.chkPreviousZero.TabIndex = 1;
            this.chkPreviousZero.Text = "Zero";
            this.chkPreviousZero.UseVisualStyleBackColor = true;
            // 
            // chkPreviousCarry
            // 
            this.chkPreviousCarry.AutoSize = true;
            this.chkPreviousCarry.Location = new System.Drawing.Point(210, 67);
            this.chkPreviousCarry.Margin = new System.Windows.Forms.Padding(4);
            this.chkPreviousCarry.Name = "chkPreviousCarry";
            this.chkPreviousCarry.Size = new System.Drawing.Size(67, 22);
            this.chkPreviousCarry.TabIndex = 0;
            this.chkPreviousCarry.Text = "Carry";
            this.chkPreviousCarry.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkCurrentReserved);
            this.groupBox2.Controls.Add(this.lblSR);
            this.groupBox2.Controls.Add(this.lblCurrentAddressingMode);
            this.groupBox2.Controls.Add(this.lblCurrentOpCode);
            this.groupBox2.Controls.Add(this.txtCurrentYR);
            this.groupBox2.Controls.Add(this.chkCurrentNegative);
            this.groupBox2.Controls.Add(this.txtCurrentXR);
            this.groupBox2.Controls.Add(this.chkCurrentOverflow);
            this.groupBox2.Controls.Add(this.txtCurrentAR);
            this.groupBox2.Controls.Add(this.chkCurrentBreakCommand);
            this.groupBox2.Controls.Add(this.txtCurrentSP);
            this.groupBox2.Controls.Add(this.chkCurrentDecimalMode);
            this.groupBox2.Controls.Add(this.txtCurrentPC);
            this.groupBox2.Controls.Add(this.lblCurrentYR);
            this.groupBox2.Controls.Add(this.chkCurrentIrqDisable);
            this.groupBox2.Controls.Add(this.lblCurrentXR);
            this.groupBox2.Controls.Add(this.chkCurrentZero);
            this.groupBox2.Controls.Add(this.lblCurrentAR);
            this.groupBox2.Controls.Add(this.chkCurrentCarry);
            this.groupBox2.Controls.Add(this.lblCurrentSP);
            this.groupBox2.Controls.Add(this.lblCurrentPC);
            this.groupBox2.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(432, 13);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(380, 344);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Next Instruction";
            // 
            // chkCurrentReserved
            // 
            this.chkCurrentReserved.AutoSize = true;
            this.chkCurrentReserved.Location = new System.Drawing.Point(211, 225);
            this.chkCurrentReserved.Margin = new System.Windows.Forms.Padding(4);
            this.chkCurrentReserved.Name = "chkCurrentReserved";
            this.chkCurrentReserved.Size = new System.Drawing.Size(91, 22);
            this.chkCurrentReserved.TabIndex = 30;
            this.chkCurrentReserved.Text = "Reserved";
            this.chkCurrentReserved.UseVisualStyleBackColor = true;
            // 
            // lblSR
            // 
            this.lblSR.AutoSize = true;
            this.lblSR.Location = new System.Drawing.Point(47, 277);
            this.lblSR.Name = "lblSR";
            this.lblSR.Size = new System.Drawing.Size(56, 18);
            this.lblSR.TabIndex = 29;
            this.lblSR.Text = "label1";
            // 
            // lblCurrentAddressingMode
            // 
            this.lblCurrentAddressingMode.AutoSize = true;
            this.lblCurrentAddressingMode.Location = new System.Drawing.Point(9, 68);
            this.lblCurrentAddressingMode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrentAddressingMode.Name = "lblCurrentAddressingMode";
            this.lblCurrentAddressingMode.Size = new System.Drawing.Size(120, 18);
            this.lblCurrentAddressingMode.TabIndex = 28;
            this.lblCurrentAddressingMode.Text = "AddressingMode";
            // 
            // lblCurrentOpCode
            // 
            this.lblCurrentOpCode.AutoSize = true;
            this.lblCurrentOpCode.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentOpCode.Location = new System.Drawing.Point(8, 35);
            this.lblCurrentOpCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrentOpCode.Name = "lblCurrentOpCode";
            this.lblCurrentOpCode.Size = new System.Drawing.Size(82, 24);
            this.lblCurrentOpCode.TabIndex = 27;
            this.lblCurrentOpCode.Text = "OPCODE";
            // 
            // txtCurrentYR
            // 
            this.txtCurrentYR.Location = new System.Drawing.Point(50, 239);
            this.txtCurrentYR.Margin = new System.Windows.Forms.Padding(4);
            this.txtCurrentYR.Name = "txtCurrentYR";
            this.txtCurrentYR.Size = new System.Drawing.Size(105, 25);
            this.txtCurrentYR.TabIndex = 26;
            // 
            // chkCurrentNegative
            // 
            this.chkCurrentNegative.AutoSize = true;
            this.chkCurrentNegative.Location = new System.Drawing.Point(211, 286);
            this.chkCurrentNegative.Margin = new System.Windows.Forms.Padding(4);
            this.chkCurrentNegative.Name = "chkCurrentNegative";
            this.chkCurrentNegative.Size = new System.Drawing.Size(91, 22);
            this.chkCurrentNegative.TabIndex = 13;
            this.chkCurrentNegative.Text = "Negative";
            this.chkCurrentNegative.UseVisualStyleBackColor = true;
            // 
            // txtCurrentXR
            // 
            this.txtCurrentXR.Location = new System.Drawing.Point(50, 207);
            this.txtCurrentXR.Margin = new System.Windows.Forms.Padding(4);
            this.txtCurrentXR.Name = "txtCurrentXR";
            this.txtCurrentXR.Size = new System.Drawing.Size(105, 25);
            this.txtCurrentXR.TabIndex = 25;
            // 
            // chkCurrentOverflow
            // 
            this.chkCurrentOverflow.AutoSize = true;
            this.chkCurrentOverflow.Location = new System.Drawing.Point(211, 255);
            this.chkCurrentOverflow.Margin = new System.Windows.Forms.Padding(4);
            this.chkCurrentOverflow.Name = "chkCurrentOverflow";
            this.chkCurrentOverflow.Size = new System.Drawing.Size(91, 22);
            this.chkCurrentOverflow.TabIndex = 12;
            this.chkCurrentOverflow.Text = "Overflow";
            this.chkCurrentOverflow.UseVisualStyleBackColor = true;
            // 
            // txtCurrentAR
            // 
            this.txtCurrentAR.Location = new System.Drawing.Point(50, 175);
            this.txtCurrentAR.Margin = new System.Windows.Forms.Padding(4);
            this.txtCurrentAR.Name = "txtCurrentAR";
            this.txtCurrentAR.Size = new System.Drawing.Size(105, 25);
            this.txtCurrentAR.TabIndex = 24;
            // 
            // chkCurrentBreakCommand
            // 
            this.chkCurrentBreakCommand.AutoSize = true;
            this.chkCurrentBreakCommand.Location = new System.Drawing.Point(211, 195);
            this.chkCurrentBreakCommand.Margin = new System.Windows.Forms.Padding(4);
            this.chkCurrentBreakCommand.Name = "chkCurrentBreakCommand";
            this.chkCurrentBreakCommand.Size = new System.Drawing.Size(123, 22);
            this.chkCurrentBreakCommand.TabIndex = 11;
            this.chkCurrentBreakCommand.Text = "BreakCommand";
            this.chkCurrentBreakCommand.UseVisualStyleBackColor = true;
            // 
            // txtCurrentSP
            // 
            this.txtCurrentSP.Location = new System.Drawing.Point(50, 143);
            this.txtCurrentSP.Margin = new System.Windows.Forms.Padding(4);
            this.txtCurrentSP.Name = "txtCurrentSP";
            this.txtCurrentSP.Size = new System.Drawing.Size(105, 25);
            this.txtCurrentSP.TabIndex = 23;
            // 
            // chkCurrentDecimalMode
            // 
            this.chkCurrentDecimalMode.AutoSize = true;
            this.chkCurrentDecimalMode.Location = new System.Drawing.Point(211, 163);
            this.chkCurrentDecimalMode.Margin = new System.Windows.Forms.Padding(4);
            this.chkCurrentDecimalMode.Name = "chkCurrentDecimalMode";
            this.chkCurrentDecimalMode.Size = new System.Drawing.Size(115, 22);
            this.chkCurrentDecimalMode.TabIndex = 10;
            this.chkCurrentDecimalMode.Text = "DecimalMode";
            this.chkCurrentDecimalMode.UseVisualStyleBackColor = true;
            // 
            // txtCurrentPC
            // 
            this.txtCurrentPC.Location = new System.Drawing.Point(50, 111);
            this.txtCurrentPC.Margin = new System.Windows.Forms.Padding(4);
            this.txtCurrentPC.Name = "txtCurrentPC";
            this.txtCurrentPC.Size = new System.Drawing.Size(105, 25);
            this.txtCurrentPC.TabIndex = 22;
            // 
            // lblCurrentYR
            // 
            this.lblCurrentYR.Location = new System.Drawing.Point(9, 244);
            this.lblCurrentYR.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrentYR.Name = "lblCurrentYR";
            this.lblCurrentYR.Size = new System.Drawing.Size(33, 19);
            this.lblCurrentYR.TabIndex = 21;
            this.lblCurrentYR.Text = "YR:";
            // 
            // chkCurrentIrqDisable
            // 
            this.chkCurrentIrqDisable.AutoSize = true;
            this.chkCurrentIrqDisable.Location = new System.Drawing.Point(211, 131);
            this.chkCurrentIrqDisable.Margin = new System.Windows.Forms.Padding(4);
            this.chkCurrentIrqDisable.Name = "chkCurrentIrqDisable";
            this.chkCurrentIrqDisable.Size = new System.Drawing.Size(107, 22);
            this.chkCurrentIrqDisable.TabIndex = 9;
            this.chkCurrentIrqDisable.Text = "IrqDisable";
            this.chkCurrentIrqDisable.UseVisualStyleBackColor = true;
            // 
            // lblCurrentXR
            // 
            this.lblCurrentXR.Location = new System.Drawing.Point(9, 212);
            this.lblCurrentXR.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrentXR.Name = "lblCurrentXR";
            this.lblCurrentXR.Size = new System.Drawing.Size(33, 19);
            this.lblCurrentXR.TabIndex = 20;
            this.lblCurrentXR.Text = "XR:";
            // 
            // chkCurrentZero
            // 
            this.chkCurrentZero.AutoSize = true;
            this.chkCurrentZero.Location = new System.Drawing.Point(211, 99);
            this.chkCurrentZero.Margin = new System.Windows.Forms.Padding(4);
            this.chkCurrentZero.Name = "chkCurrentZero";
            this.chkCurrentZero.Size = new System.Drawing.Size(59, 22);
            this.chkCurrentZero.TabIndex = 8;
            this.chkCurrentZero.Text = "Zero";
            this.chkCurrentZero.UseVisualStyleBackColor = true;
            // 
            // lblCurrentAR
            // 
            this.lblCurrentAR.Location = new System.Drawing.Point(9, 180);
            this.lblCurrentAR.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrentAR.Name = "lblCurrentAR";
            this.lblCurrentAR.Size = new System.Drawing.Size(33, 19);
            this.lblCurrentAR.TabIndex = 19;
            this.lblCurrentAR.Text = "AR:";
            // 
            // chkCurrentCarry
            // 
            this.chkCurrentCarry.AutoSize = true;
            this.chkCurrentCarry.Location = new System.Drawing.Point(211, 67);
            this.chkCurrentCarry.Margin = new System.Windows.Forms.Padding(4);
            this.chkCurrentCarry.Name = "chkCurrentCarry";
            this.chkCurrentCarry.Size = new System.Drawing.Size(67, 22);
            this.chkCurrentCarry.TabIndex = 7;
            this.chkCurrentCarry.Text = "Carry";
            this.chkCurrentCarry.UseVisualStyleBackColor = true;
            // 
            // lblCurrentSP
            // 
            this.lblCurrentSP.Location = new System.Drawing.Point(9, 149);
            this.lblCurrentSP.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrentSP.Name = "lblCurrentSP";
            this.lblCurrentSP.Size = new System.Drawing.Size(33, 19);
            this.lblCurrentSP.TabIndex = 18;
            this.lblCurrentSP.Text = "SP:";
            // 
            // lblCurrentPC
            // 
            this.lblCurrentPC.Location = new System.Drawing.Point(9, 117);
            this.lblCurrentPC.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrentPC.Name = "lblCurrentPC";
            this.lblCurrentPC.Size = new System.Drawing.Size(33, 19);
            this.lblCurrentPC.TabIndex = 17;
            this.lblCurrentPC.Text = "PC:";
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
            // btnMemory
            // 
            this.btnMemory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMemory.Location = new System.Drawing.Point(524, 692);
            this.btnMemory.Margin = new System.Windows.Forms.Padding(4);
            this.btnMemory.Name = "btnMemory";
            this.btnMemory.Size = new System.Drawing.Size(138, 32);
            this.btnMemory.TabIndex = 7;
            this.btnMemory.Text = "Show Memory";
            this.btnMemory.UseVisualStyleBackColor = true;
            this.btnMemory.Click += new System.EventHandler(this.BtnMemory_Click);
            // 
            // FormDebugger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 737);
            this.Controls.Add(this.btnMemory);
            this.Controls.Add(this.dgWatch);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnStep);
            this.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormDebugger";
            this.Text = "Debugger";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormDebugger_FormClosing);
            this.Load += new System.EventHandler(this.FormDebugger_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgWatch)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStep;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkPreviousCarry;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkPreviousNegative;
        private System.Windows.Forms.CheckBox chkPreviousOverflow;
        private System.Windows.Forms.CheckBox chkPreviousBreakCommand;
        private System.Windows.Forms.CheckBox chkPreviousDecimalMode;
        private System.Windows.Forms.CheckBox chkPreviousIrqDisable;
        private System.Windows.Forms.CheckBox chkPreviousZero;
        private System.Windows.Forms.CheckBox chkCurrentNegative;
        private System.Windows.Forms.CheckBox chkCurrentOverflow;
        private System.Windows.Forms.CheckBox chkCurrentBreakCommand;
        private System.Windows.Forms.CheckBox chkCurrentDecimalMode;
        private System.Windows.Forms.CheckBox chkCurrentIrqDisable;
        private System.Windows.Forms.CheckBox chkCurrentZero;
        private System.Windows.Forms.CheckBox chkCurrentCarry;
        private System.Windows.Forms.Label lblPreviousPC;
        private System.Windows.Forms.Label lblPreviousSP;
        private System.Windows.Forms.Label lblPreviousYR;
        private System.Windows.Forms.Label lblPreviousXR;
        private System.Windows.Forms.Label lblPreviousAR;
        private System.Windows.Forms.TextBox txtPreviousYR;
        private System.Windows.Forms.TextBox txtPreviousXR;
        private System.Windows.Forms.TextBox txtPreviousAR;
        private System.Windows.Forms.TextBox txtPreviousSP;
        private System.Windows.Forms.TextBox txtPreviousPC;
        private System.Windows.Forms.TextBox txtCurrentYR;
        private System.Windows.Forms.TextBox txtCurrentXR;
        private System.Windows.Forms.TextBox txtCurrentAR;
        private System.Windows.Forms.TextBox txtCurrentSP;
        private System.Windows.Forms.TextBox txtCurrentPC;
        private System.Windows.Forms.Label lblCurrentYR;
        private System.Windows.Forms.Label lblCurrentXR;
        private System.Windows.Forms.Label lblCurrentAR;
        private System.Windows.Forms.Label lblCurrentSP;
        private System.Windows.Forms.Label lblCurrentPC;
        private System.Windows.Forms.Label lblPreviousAddressingMode;
        private System.Windows.Forms.Label lblPreviousOpCode;
        private System.Windows.Forms.Label lblCurrentAddressingMode;
        private System.Windows.Forms.Label lblCurrentOpCode;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.DataGridView dgWatch;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmValueHex;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmValueDecimal;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmDescription;
        private System.Windows.Forms.Button btnMemory;
        private System.Windows.Forms.Label lblSR;
        private System.Windows.Forms.CheckBox chkPreviousReserved;
        private System.Windows.Forms.CheckBox chkCurrentReserved;
    }
}

