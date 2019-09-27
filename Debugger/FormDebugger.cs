using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Globalization;
using System.Drawing;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using MicroProcessor.Cpu6502;

namespace Debugger {
    public partial class FormDebugger : Form {

        Cpu Cpu;
        FormMemoryViewer FormMemoryViewer = new FormMemoryViewer();

        public FormDebugger() {
            InitializeComponent();

            UpdateUi();
        }

        private void FormDebugger_Load(object sender, EventArgs e) {
            LoadWatches();
            UpdateMemoryWatch();
        }

        private void BtnStep_Click(object sender, EventArgs e) {
            Cpu.Step();
            UpdateUi();
        }

        private void UpdateUi() {
            lblAddressingMode.Text = Cpu.NextOpCode?.AddressingMode.ToString();

            chkCarry.Checked = Cpu.SR.Carry;
            chkZero.Checked = Cpu.SR.Zero;
            chkIrqDisable.Checked = Cpu.SR.IrqDisable;
            chkDecimalMode.Checked = Cpu.SR.DecimalMode;
            chkBreakCommand.Checked = Cpu.SR.BreakCommand;
            chkReserved.Checked = Cpu.SR.Reserved;
            chkOverflow.Checked = Cpu.SR.Overflow;
            chkNegative.Checked = Cpu.SR.Negative;

            txtPC.Text = Cpu.PC.ToString("X2");
            txtSP.Text = Cpu.SP.ToString("X2");
            txtAR.Text = Cpu.AR.ToString("X2");
            txtXR.Text = Cpu.XR.ToString("X2");
            txtYR.Text = Cpu.YR.ToString("X2");
        }


        private void UpdateMemoryWatch() {
            for (int i = 0; i < dgWatch.Rows.Count; i++) {
                var addressString = dgWatch.Rows[i].Cells[0].Value as string;
                if (addressString == null || string.IsNullOrWhiteSpace(addressString)) continue;

                var address = StringToInt(addressString);

                if (address == null) {
                    dgWatch.Rows[i].DefaultCellStyle.BackColor = Color.LightPink;
                    continue;

                } else {
                    dgWatch.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }

                var valueHex = $"0x{Cpu.Memory[address.Value]:X2}";
                var valueDec = $"{Cpu.Memory[address.Value]}";

                var colorHex = !valueHex.Equals(dgWatch.Rows[i].Cells[1].Value) ? Color.Red : Color.Black;
                dgWatch.Rows[i].Cells[1].Style.ForeColor = colorHex;
                dgWatch.Rows[i].Cells[1].Value = valueHex;

                var colorDec = !valueDec.Equals(dgWatch.Rows[i].Cells[2].Value) ? Color.Red : Color.Black;
                dgWatch.Rows[i].Cells[2].Style.ForeColor = colorDec;
                dgWatch.Rows[i].Cells[2].Value = valueDec;
            }
        }

        private int? StringToInt(string intString) {
            int address;

            if (intString.ToLower().StartsWith("0x") || intString.ToLower().StartsWith("&h")) {
                if (int.TryParse(intString.Substring(2), NumberStyles.HexNumber, null, out address)) return address;

            } else {
                if (int.TryParse(intString, out address)) return address;
            }

            return null;
        }

        private void BtnReset_Click(object sender, System.EventArgs e) {
            Cpu.Reset();
            UpdateUi();
        }

        private void DgWatch_CellEndEdit(object sender, DataGridViewCellEventArgs e) {
            switch (e.ColumnIndex) {
                case 0:
                    UpdateMemoryWatch();
                    break;

                case 1:
                case 2:
                    var addressString = dgWatch.Rows[e.RowIndex].Cells[0].Value as string;
                    if (addressString == null || string.IsNullOrWhiteSpace(addressString)) break;

                    var address = StringToInt(addressString);
                    if (address == null) break;

                    var value = StringToInt(dgWatch.Rows[e.RowIndex].Cells[e.ColumnIndex].Value as string);
                    if (value == null) break;

                    Cpu.Memory[address.Value] = (byte)value.Value;

                    UpdateMemoryWatch();
                    UpdateUi();

                    break;

            }
        }

        private void FormDebugger_FormClosing(object sender, FormClosingEventArgs e) {
            SaveWatches();
        }

        private void SaveWatches() {
            string[,] data = new string[dgWatch.Rows.Count, dgWatch.Columns.Count];

            for (int i = 0; i < dgWatch.Rows.Count; i++) {

                for (int j = 0; j < dgWatch.Rows[i].Cells.Count; j++) {

                    data[i, j] = (string)dgWatch.Rows[i].Cells[j].Value;

                }

            }

            var json = JsonConvert.SerializeObject(data);
            File.WriteAllText("watches.json", json);
        }

        private void LoadWatches() {
            if (!File.Exists("watches.json")) return;

            var json = File.ReadAllText("watches.json");
            var data = JsonConvert.DeserializeObject<string[,]>(json);

            for (int i = 0; i < data.GetLength(0); i++) {
                var rowdata = new string[data.GetLength(1)];

                for (int j = 0; j < data.GetLength(1); j++) {
                    rowdata[j] = data[i, j];
                }

                if (rowdata.All(x => string.IsNullOrWhiteSpace(x))) continue;
                dgWatch.Rows.Add(rowdata);
            }
        }

        private void BtnMemory_Click(object sender, System.EventArgs e) {
            FormMemoryViewer.Show();
        }

    }
}
