using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman
{
    public class Delay { }
    public class Init
    {
        public string level { get; set; }
        public string category { get; set; }
        public Init(string l, string c)
        {
            level = l;
            category = c;
        }
    }

    public class Letter
    {
        public char letter { get; set; }
        public int picId { get; set; }
        public string word { get; set; }
        public string guessed { get; set; }
        public int id { get; set; }
        public Letter(char l, int p, string w, string g, int i)
        {
            letter = l;
            picId = p;
            word = w;
            guessed = g;
            id = i;
        }
    }

    public class Response
    {
        public string guessedLetters { get; set; }
        public int picId { get; set; }
        public string word { get; set; }
        public int id { get; set; }
        public char letter { get; set; }
        public Response(string g, int p, string w, int i, char l)
        {
            guessedLetters = g;
            picId = p;
            word = w;
            id = i;
            letter = l;
        }
    }

    public class Start
    {
        public char slovo { get; set; }
        public Start(char c)
        {
            slovo = c;
        }
    }

    public class Word
    {
        public string word { get; set; }
        public string guessingWord { get; set; }
        public Word(string w, string g)
        {
            word = w;
            guessingWord = g;
        }
    }
}
