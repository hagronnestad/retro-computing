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

            UpdateCurrentStateUi();
        }

        private void FormDebugger_Load(object sender, System.EventArgs e) {
            LoadWatches();
            UpdateMemoryWatch();
        }

        private void BtnStep_Click(object sender, System.EventArgs e) {
            UpdatePreviousStateUi();
            Cpu.Step();
            UpdateCurrentStateUi();
        }

        private void UpdatePreviousStateUi() {
            //lblPreviousOpCode.Text = $"{Cpu.NextOpCode?.Name} ({string.Join(" ", Cpu.Memory.Skip(Cpu.NextOpCodeAddress).Take(Cpu.NextOpCode?.Length ?? 0).Select(x => $"{x:X2}").ToList())})";
            lblPreviousAddressingMode.Text = Cpu.NextOpCode?.AddressingMode.ToString();

            chkPreviousCarry.Checked = Cpu.SR.Carry;
            chkPreviousZero.Checked = Cpu.SR.Zero;
            chkPreviousIrqDisable.Checked = Cpu.SR.IrqDisable;
            chkPreviousDecimalMode.Checked = Cpu.SR.DecimalMode;
            chkPreviousBreakCommand.Checked = Cpu.SR.BreakCommand;
            chkPreviousReserved.Checked = Cpu.SR.Reserved;
            chkPreviousOverflow.Checked = Cpu.SR.Overflow;
            chkPreviousNegative.Checked = Cpu.SR.Negative;

            txtPreviousPC.Text = Cpu.PC.ToString("X2");
            txtPreviousSP.Text = Cpu.SP.ToString("X2");
            txtPreviousAR.Text = Cpu.AR.ToString("X2");
            txtPreviousXR.Text = Cpu.XR.ToString("X2");
            txtPreviousYR.Text = Cpu.YR.ToString("X2");
        }

        private void UpdateCurrentStateUi() {
            //lblCurrentOpCode.Text = $"{Cpu.NextOpCode?.Name} ({string.Join(" ", Cpu.Memory.Skip(Cpu.NextOpCodeAddress).Take(Cpu.NextOpCode?.Length ?? 0).Select(x => $"{x:X2}").ToList())})";
            lblCurrentAddressingMode.Text = Cpu.NextOpCode?.AddressingMode.ToString();

            chkCurrentCarry.Checked = Cpu.SR.Carry;
            chkCurrentZero.Checked = Cpu.SR.Zero;
            chkCurrentIrqDisable.Checked = Cpu.SR.IrqDisable;
            chkCurrentDecimalMode.Checked = Cpu.SR.DecimalMode;
            chkCurrentBreakCommand.Checked = Cpu.SR.BreakCommand;
            chkCurrentReserved.Checked = Cpu.SR.Reserved;
            chkCurrentOverflow.Checked = Cpu.SR.Overflow;
            chkCurrentNegative.Checked = Cpu.SR.Negative;

            txtCurrentPC.Text = Cpu.PC.ToString("X2");
            txtCurrentSP.Text = Cpu.SP.ToString("X2");
            txtCurrentAR.Text = Cpu.AR.ToString("X2");
            txtCurrentXR.Text = Cpu.XR.ToString("X2");
            txtCurrentYR.Text = Cpu.YR.ToString("X2");

            lblSR.Text = $"{Cpu.SR.Register:X2} ({Convert.ToString(Cpu.SR.Register, 2).PadLeft(8, '0')})";

            UpdateMemoryWatch();
            //if (FormMemoryViewer.Visible) FormMemoryViewer.byteViewer.SetBytes(Cpu.Memory);
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
            int address = 0;

            if (intString.ToLower().StartsWith("0x") || intString.ToLower().StartsWith("&h")) {
                if (int.TryParse(intString.Substring(2), NumberStyles.HexNumber, null, out address)) return address;

            } else {
                if (int.TryParse(intString, out address)) return address;
            }

            return null;
        }

        private void BtnReset_Click(object sender, System.EventArgs e) {
            Cpu.Reset();
            UpdatePreviousStateUi();
            UpdateCurrentStateUi();
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
                    UpdateCurrentStateUi();

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
            //FormMemoryViewer.byteViewer.SetBytes(Cpu.Memory);
        }


        void Work() {
            Cpu.Step();

            if (Cpu.TotalInstructions % 100000 == 0) {
                Debug.WriteLine($"{Cpu.TotalInstructions}");
                Debug.WriteLine($"{Cpu.PC:X2}");

                try {
                    Invoke(new Action(() => { UpdateCurrentStateUi(); }));

                } catch (Exception) {
                }
            }
        }
    }
}
