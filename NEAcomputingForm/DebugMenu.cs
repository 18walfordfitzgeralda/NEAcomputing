using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NEAcomputingForm
{

    public partial class DebugMenu : Form
    {
        Form1 form1;
        public DebugMenu()
        {
            InitializeComponent();
        }

        private void DebugMenu_Load(object sender, EventArgs e)
        {
            form1 = (Form1)Application.OpenForms["Form1"];
        }

        private void btnClr_Click(object sender, EventArgs e)
        {
            form1.DebugMenuAccess(0);

        }

        private void btnClub_Click(object sender, EventArgs e)
        {
            form1.DebugMenuAccess(1);
        }
        private void btnDebugOut_Click(object sender, EventArgs e)
        {
            form1.DebugMenuAccess(2);
        }

        private void btnNumpad_Click(object sender, EventArgs e)
        {
            form1.DebugMenuAccess(3);
        }

        private void btnOpenDebug_Click(object sender, EventArgs e)
        {
            form1.DebugMenuAccess(4);
        }

        private void btnCombat_Click(object sender, EventArgs e)
        {
            form1.DebugMenuAccess(5);
        }

        private void btnMegaHeal_Click(object sender, EventArgs e)
        {
            form1.DebugMenuAccess(6);
        }

        private void btnStab_Click(object sender, EventArgs e)
        {
            form1.DebugMenuAccess(7);
        }

        private void btnTestSave_Click(object sender, EventArgs e)
        {
            form1.DebugMenuAccess(8);
        }
    }
}
