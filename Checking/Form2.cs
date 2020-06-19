using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Checking
{
    public partial class Preview_window : Form
    {
        public Preview_window(string input_test, string log1,string log2,bool b)
        {
            InitializeComponent();
            Log.Clear();
            Log.AppendText("Input" + Environment.NewLine + input_test + Environment.NewLine + "Output" + Environment.NewLine + "Accepted Code: " + log1 + Environment.NewLine + "Code Check: " + log2 + Environment.NewLine);
        }

        private void Log_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
