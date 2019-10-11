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

        private void BtnStep_Click(object sender, EventArgs e) {
            _cpu.Step();
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

        private void BtnReset_Click(object sender, EventArgs e) {
            _cpu.Reset();
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

    }
}
