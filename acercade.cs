using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace webiwabo
{
    public partial class acercade : Form
    {
        public acercade()
        {
            InitializeComponent();
            lblVersion.Text = Verzion.Texto; // Usa el mismo texto que en Form

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
