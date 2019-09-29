using System.Windows.Forms;
using System.Globalization;
using System;
using MicroProcessor.Cpu6502;
using Hardware.Memory;
using Memory;
using System.Collections.Concurrent;

namespace Debugger {
    public partial class FormDebugger : Form {

        private readonly Cpu _cpu;
        private readonly IMemory<byte> _memory;

        private ConcurrentDictionary<int, WatchItem> _watchItems = new ConcurrentDictionary<int, WatchItem>();


        public FormDebugger(Cpu cpu, IMemory<byte> memory) {
            InitializeComponent();

            _cpu = cpu;
            _memory = memory;

            _memory.OnWrite += _memory_OnWrite;
            _memory.OnRead += _memory_OnRead;

            UpdateUi();
        }

        private void _memory_OnWrite(object sender, MemoryWriteEventArgs<byte> e) {
            if (!_watchItems.ContainsKey(e.Address)) return;
            _watchItems[e.Address].Value = e.Value;
        }

        private void _memory_OnRead(object sender, MemoryReadEventArgs<byte> e) {

        }

        private void FormDebugger_Load(object sender, EventArgs e) {
            //LoadWatches();
        }

        private void BtnStep_Click(object sender, EventArgs e) {
            _cpu.Step();
            UpdateUi();
        }

        private void UpdateUi() {
            lblAddressingMode.Text = _cpu.NextOpCode?.AddressingMode.ToString();

            chkCarry.Checked = _cpu.SR.Carry;
            chkZero.Checked = _cpu.SR.Zero;
            chkIrqDisable.Checked = _cpu.SR.IrqDisable;
            chkDecimalMode.Checked = _cpu.SR.DecimalMode;
            chkBreakCommand.Checked = _cpu.SR.BreakCommand;
            chkReserved.Checked = _cpu.SR.Reserved;
            chkOverflow.Checked = _cpu.SR.Overflow;
            chkNegative.Checked = _cpu.SR.Negative;

            txtPC.Text = _cpu.PC.ToString("X2");
            txtSP.Text = _cpu.SP.ToString("X2");
            txtAR.Text = _cpu.AR.ToString("X2");
            txtXR.Text = _cpu.XR.ToString("X2");
            txtYR.Text = _cpu.YR.ToString("X2");
        }


        //private void UpdateMemoryWatch(int a, byte v) {
        //    for (int i = 0; i < dgWatch.Rows.Count; i++) {
        //        var addressString = dgWatch.Rows[i].Cells[0].Value as string;
        //        if (addressString == null || string.IsNullOrWhiteSpace(addressString)) continue;

        //        var address = StringToInt(addressString);

        //        if (address == null) {
        //            dgWatch.Rows[i].DefaultCellStyle.BackColor = Color.LightPink;
        //            continue;

        //        } else {
        //            dgWatch.Rows[i].DefaultCellStyle.BackColor = Color.White;
        //        }

        //        var valueHex = $"0x{_cpu.Memory[address.Value]:X2}";
        //        var valueDec = $"{_cpu.Memory[address.Value]}";

        //        var colorHex = !valueHex.Equals(dgWatch.Rows[i].Cells[1].Value) ? Color.Red : Color.Black;
        //        dgWatch.Rows[i].Cells[1].Style.ForeColor = colorHex;
        //        dgWatch.Rows[i].Cells[1].Value = valueHex;

        //        var colorDec = !valueDec.Equals(dgWatch.Rows[i].Cells[2].Value) ? Color.Red : Color.Black;
        //        dgWatch.Rows[i].Cells[2].Style.ForeColor = colorDec;
        //        dgWatch.Rows[i].Cells[2].Value = valueDec;
        //    }
        //}

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
            _cpu.Reset();
            UpdateUi();
        }

        private void DgWatch_CellEndEdit(object sender, DataGridViewCellEventArgs e) {
            string addressString;
            int? address;

            switch (e.ColumnIndex) {
                case 0: // Address
                    addressString = dgWatch.Rows[e.RowIndex].Cells[0].Value as string;
                    if (addressString == null || string.IsNullOrWhiteSpace(addressString)) break;

                    address = StringToInt(addressString);
                    if (address == null) break;

                    if (_watchItems.ContainsKey(address.Value)) break;
                    _watchItems.TryAdd(address.Value, new WatchItem() {
                        Address = address.Value,
                    });

                    break;

                case 1:
                case 2:
                    addressString = dgWatch.Rows[e.RowIndex].Cells[0].Value as string;
                    if (addressString == null || string.IsNullOrWhiteSpace(addressString)) break;

                    address = StringToInt(addressString);
                    if (address == null) break;

                    var value = StringToInt(dgWatch.Rows[e.RowIndex].Cells[e.ColumnIndex].Value as string);
                    if (value == null) break;

                    _cpu.Memory[address.Value] = (byte)value.Value;

                    break;
            }
        }

        private void FormDebugger_FormClosing(object sender, FormClosingEventArgs e) {
            //SaveWatches();
        }

        //private void SaveWatches() {
        //    string[,] data = new string[dgWatch.Rows.Count, dgWatch.Columns.Count];

        //    for (int i = 0; i < dgWatch.Rows.Count; i++) {

        //        for (int j = 0; j < dgWatch.Rows[i].Cells.Count; j++) {

        //            data[i, j] = (string)dgWatch.Rows[i].Cells[j].Value;

        //        }

        //    }

        //    var json = JsonConvert.SerializeObject(data);
        //    File.WriteAllText("watches.json", json);
        //}

        //private void LoadWatches() {
        //    if (!File.Exists("watches.json")) return;

        //    var json = File.ReadAllText("watches.json");
        //    var data = JsonConvert.DeserializeObject<string[,]>(json);

        //    for (int i = 0; i < data.GetLength(0); i++) {
        //        var rowdata = new string[data.GetLength(1)];

        //        for (int j = 0; j < data.GetLength(1); j++) {
        //            rowdata[j] = data[i, j];
        //        }

        //        if (rowdata.All(x => string.IsNullOrWhiteSpace(x))) continue;
        //        dgWatch.Rows.Add(rowdata);
        //    }
        //}

    }
}
