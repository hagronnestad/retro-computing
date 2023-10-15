using System.Windows.Forms;

namespace Debugger
{
    public partial class FormMemoryViewer : Form
    {
        public FormMemoryViewer()
        {
            InitializeComponent();
        }

        private void FormMemoryViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
