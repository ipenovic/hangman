using Akka.Actor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hangman
{
    class ActorWriter : ReceiveActor
    {
        Label lblGuessingWord;
        PictureBox pictures;
        ImageList images;
        StatusStrip status;
        Button btnEnter;
        TextBox txtInput;
        Button btnRepeat;
        Button btnClose;

        string word;
        int picId = 0;
        string guessed = "";
        int id = 0;
        Dictionary<int, ICancelable> repeaters;

        public ActorWriter(Label l, PictureBox p, ImageList i, StatusStrip s, Button b, TextBox t, Button r, Button c)
        {
            repeaters = new Dictionary<int, ICancelable>();

            lblGuessingWord = l;
            pictures = p;
            images = i;
            status = s;
            btnEnter = b;
            txtInput = t;
            btnRepeat = r;
            btnClose = c;

            Receive<Delay>(x => HandleDelay());

            Receive<Response>(x => HandleResponse(x));

            Receive<Start>(x => HandleStart(x));

            Receive<Word>(x => HandleWord(x));

            Receive<Init>(x => HandleInit(x));
        }

        private void HandleDelay()
        {
            btnEnter.Enabled = false;
            txtInput.Enabled = false;
            status.Items.Clear();
            status.Items.Add("Please wait, connection is slow!");
        }

        private void HandleResponse(Response x)
        {
            repeaters[x.id].Cancel();
            repeaters.Remove(x.id);
            
            btnEnter.Enabled = true;
            txtInput.Enabled = true;

            bool end = true;

            guessed = x.guessedLetters;
            lblGuessingWord.Text = "";
            foreach (char c in guessed)
                lblGuessingWord.Text += c + " ";

            status.Items.Clear();
            if (picId == x.picId)
                status.Items.Add("You are correct, letter " + x.letter + " exists");
            else
                status.Items.Add("You are wrong, letter " + x.letter + " doesn't exists");

            picId = x.picId;
            pictures.Image = images.Images[picId];

            if (picId >= 9)
            {
                status.Items.Add("You've lost! Correct answer is: " + x.word);
                btnEnter.Enabled = false;
                txtInput.Enabled = false;
                btnRepeat.Visible = true;
                btnClose.Visible = true;
                end = false;
            }
            else
            {
                foreach (char c in guessed)
                    if (c == '_')
                    {
                        end = false;
                        break;
                    }
            }

            if (end == true)
            {
                status.Items.Add("Congratulations!");
                btnEnter.Enabled = false;
                txtInput.Enabled = false;
                btnRepeat.Visible = true;
                btnClose.Visible = true;
            }
        }

        private void HandleStart(Start x)
        {
            Props props = Props.Create(() => new ActorChild());
            IActorRef actor = Context.ActorOf(props);
            Letter message = new Letter(x.slovo, picId, word, guessed, id);
            actor.Tell(message);

            var cancelable = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(
                TimeSpan.FromSeconds(3),
                TimeSpan.FromSeconds(3),
                actor,
                message,
                Self);
            repeaters.Add(id, cancelable);
            id++;
        }

        private void HandleWord(Word x)
        {
            Sender.Tell(PoisonPill.Instance);
            word = x.word;
            guessed = x.guessingWord;

            lblGuessingWord.Text = x.guessingWord;
        }

        private void HandleInit(Init x)
        {
            guessed = "";
            if (x.level == "Easy")
                picId = 0;
            else if (x.level == "Normal")
                picId = 3;
            else
                picId = 6;

            Props props = Props.Create(() => new ActorChild());
            IActorRef child = Context.ActorOf(props);
            child.Tell(new Init(x.level, x.category));
        }
    }
}