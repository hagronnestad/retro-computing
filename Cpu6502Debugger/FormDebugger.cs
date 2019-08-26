using System.IO;
using System.Windows.Forms;
using System.Linq;

namespace Cpu6502Debugger {
    public partial class FormDebugger : Form {

        Cpu6502.Cpu6502 Cpu;

        public FormDebugger() {
            InitializeComponent();

            Cpu = new Cpu6502.Cpu6502();

            //ushort startAddress = 0x4000;
            //Cpu.LoadMemory(File.ReadAllBytes(@"TestImages\LoadDecrementMemory0x50.bin"), startAddress);

            ushort startAddress = 0x6210;
            Cpu.LoadMemory(File.ReadAllBytes(@"C:\Users\heina\Downloads\instr_test-v5\instr_test-v5\rom_singles\02-implied.nes"), 0);

            Cpu.PC = (ushort)(startAddress);

            UpdatePreviousStateUi();
            UpdateCurrentStateUi();
        }

        private void FormDebugger_Load(object sender, System.EventArgs e) {

        }

        private void BtnStep_Click(object sender, System.EventArgs e) {
            UpdatePreviousStateUi();
            Cpu.Step();
            UpdateCurrentStateUi();
        }

        private void UpdatePreviousStateUi() {
            lblPreviousOpCode.Text = $"{Cpu.OpCode?.Name} ({string.Join(" ", Cpu.Memory.Skip(Cpu.PC).Take(Cpu.OpCode?.Length ?? 0).Select(x => $"{x:X2}").ToList())})";
            lblPreviousAddressingMode.Text = Cpu.OpCode?.AddressingMode.ToString();

            chkPreviousCarry.Checked = Cpu.SR.Carry;
            chkPreviousZero.Checked = Cpu.SR.Zero;
            chkPreviousIrqDisable.Checked = Cpu.SR.IrqDisable;
            chkPreviousDecimalMode.Checked = Cpu.SR.DecimalMode;
            chkPreviousBreakCommand.Checked = Cpu.SR.BreakCommand;
            chkPreviousOverflow.Checked = Cpu.SR.Overflow;
            chkPreviousNegative.Checked = Cpu.SR.Negative;

            txtPreviousPC.Text = Cpu.PC.ToString("X2");
            txtPreviousSP.Text = Cpu.SP.ToString("X2");
            txtPreviousAR.Text = Cpu.AR.ToString("X2");
            txtPreviousXR.Text = Cpu.XR.ToString("X2");
            txtPreviousYR.Text = Cpu.YR.ToString("X2");
        }

        private void UpdateCurrentStateUi() {
            lblCurrentOpCode.Text = $"{Cpu.OpCode?.Name} ({string.Join(" ", Cpu.Memory.Skip(Cpu.PC).Take(Cpu.OpCode?.Length ?? 0).Select(x => $"{x:X2}").ToList())})";
            lblCurrentAddressingMode.Text = Cpu.OpCode?.AddressingMode.ToString();

            chkCurrentCarry.Checked = Cpu.SR.Carry;
            chkCurrentZero.Checked = Cpu.SR.Zero;
            chkCurrentIrqDisable.Checked = Cpu.SR.IrqDisable;
            chkCurrentDecimalMode.Checked = Cpu.SR.DecimalMode;
            chkCurrentBreakCommand.Checked = Cpu.SR.BreakCommand;
            chkCurrentOverflow.Checked = Cpu.SR.Overflow;
            chkCurrentNegative.Checked = Cpu.SR.Negative;

            txtCurrentPC.Text = Cpu.PC.ToString("X2");
            txtCurrentSP.Text = Cpu.SP.ToString("X2");
            txtCurrentAR.Text = Cpu.AR.ToString("X2");
            txtCurrentXR.Text = Cpu.XR.ToString("X2");
            txtCurrentYR.Text = Cpu.YR.ToString("X2");

            bvMemory.SetBytes(Cpu.Memory);
            btnStep.Focus();
        }

        private void BtnReset_Click(object sender, System.EventArgs e) {
            Cpu.Reset();
            UpdatePreviousStateUi();
            UpdateCurrentStateUi();
        }
    }
}
