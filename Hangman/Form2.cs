using Akka.Actor;
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
    public partial class Form2 : Form
    {
        IActorRef actor;
        string level;
        string category;
        int lvl = 0;
        List<char> usedList = new List<char>();

        public Form2(string l, string c)
        {
            InitializeComponent();

            level = l;
            category = c;

            if (level == "Easy")
                lvl = 0;
            else if (level == "Normal")
                lvl = 3;
            else
                lvl = 6;

            var props = Props.Create(() => new ActorWriter(lblGuessingWord, pictureBox1, imageList1, statusStrip1, btnInput, txtInput, btnRepeat, btnClose)).WithDispatcher("akka.actor.synchronized-dispatcher");
            actor = Program.System.ActorOf(props);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            btnRepeat.Visible = false;
            btnClose.Visible = false;

            label1.Text = "Used letters:\n";
            pictureBox1.Image = imageList1.Images[lvl];
            actor.Tell(new Init(level, category));
        }

        private void btnInput_Click(object sender, EventArgs e)
        {
            statusStrip1.Items.Clear();

            try
            {
                char letter = char.Parse(txtInput.Text.ToUpper());
                txtInput.Text = "";
                actor.Tell(new Start(letter));

                if (!usedList.Contains(letter))
                    usedList.Add(letter);

                label1.Text = "Used letters:\n";
                foreach (char c in usedList)
                    label1.Text += c + " ";
            }
            catch
            {
                statusStrip1.Items.Add("Please input one letter!");
            }
        }

        private void txtInput_TextChanged(object sender, EventArgs e)
        {
            string tekst = txtInput.Text;
            txtInput.Text = tekst.ToUpper();
        }

        private void btnRepeat_Click(object sender, EventArgs e)
        {
            label1.Text = "Used letters:\n";
            pictureBox1.Image = imageList1.Images[lvl];
            statusStrip1.Items.Clear();
            usedList.Clear();
            actor.Tell(new Init(level, category));

            btnRepeat.Visible = false;
            btnClose.Visible = false;
            txtInput.Enabled = true;
            btnInput.Enabled = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
