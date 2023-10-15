using System;
using System.Windows.Forms;

namespace Commodore64
{
    public partial class FormLoadSequentialFileOptions : Form
    {
        public bool ClearScreen => chkClearScreen.Checked;
        public int BytesToRemoveFromEnd => (int)numBytesToRemoveFromEnd.Value;

        public FormLoadSequentialFileOptions()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
