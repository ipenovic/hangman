using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hangman
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null && comboBox2.SelectedItem != null)
            {
                string level = comboBox1.SelectedItem.ToString();
                string category = comboBox2.SelectedItem.ToString();

                Form2 frmIgra = new Form2(level, category);
                frmIgra.ShowDialog();
            }
            else
            {
                statusStrip1.Items.Clear();
                statusStrip1.Items.Add("Please choose level and category!");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            statusStrip1.Items.Clear();
        }
    }
}
