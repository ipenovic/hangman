using Akka.Actor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman
{
    class ActorChild : ReceiveActor
    {
        int picId;
        bool guessed = false;
        string guessedLetters = "";
        List<char> list = new List<char>();
        public ActorChild()
        {
            Receive<Init>(x => ReadFile(x));

            Receive<Letter>(x => CheckLetter(x));
        }

        private void CheckLetter(Letter x)
        {
            picId = x.picId;
            guessedLetters = "";
            string letter = x.letter.ToString().ToUpper();
            char slovo = char.Parse(letter);

            foreach (char c in x.guessed)
            {
                if (c != '_' && c != '|' && c != ' ')
                    list.Add(c);
            }

            foreach (char c in x.word)
            {
                if (c == slovo)
                {
                    guessed = true;
                    list.Add(c);
                }
            }

            foreach (char c in x.word)
            {
                if (list.Contains(c))
                    guessedLetters += c;
                else if (c == ' ')
                    guessedLetters += "|";
                else
                    guessedLetters += "_";
            }

            if (guessed == false)
                picId++;

            Random r = new Random();
            int random = r.Next(100);

            if (random <= 80)
                Sender.Tell(new Response(guessedLetters, picId, x.word, x.id, x.letter));
            else
                Sender.Tell(new Delay());

            guessed = false;
        }

        private void ReadFile(Init x)
        {
            list.Clear();
            guessedLetters = "";
            List<string> wordlist = new List<string>();
            string word;

            StreamReader file = new StreamReader("..\\..\\Directory\\" + x.category + ".txt");
            while (!string.IsNullOrWhiteSpace(word = file.ReadLine()))
            {
                wordlist.Add(word);
            }

            Random r = new Random();
            int num = r.Next(0, wordlist.Count);

            word = wordlist[num];

            string guessingWord = "";

            foreach (char c in word)
            {
                if (c != ' ')
                    guessingWord += "_ ";
                else
                    guessingWord += "| ";
            }

            Context.Parent.Tell(new Word(word.ToUpper(), guessingWord));
        }
    }
}
