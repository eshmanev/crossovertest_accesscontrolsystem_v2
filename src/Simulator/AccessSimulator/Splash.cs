using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccessSimulator
{
    public partial class Splash : Form
    {
        public Splash()
        {
            InitializeComponent();
        }

        public string Progress
        {
            get { return label2.Text; }
            set { Invoke(new MethodInvoker(() => label2.Text = value)); }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
