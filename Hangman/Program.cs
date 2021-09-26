using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rand = new Random();                                         //initialize

            if (!File.Exists("words.txt"))
                File.WriteAllText("words.txt", Properties.Resources.words);

            string[] words = File.ReadAllLines("words.txt");
            Console.OutputEncoding = Encoding.Unicode;

            Console.Title = "Hangman";

            Console.SetWindowSize(60, 20);
            Console.SetBufferSize(60, 20);
            Console.CursorVisible = false;

            while (true)
            {
                bool gameOver = false;                                          //set up for next game
                string currentWord = words[rand.Next(0, words.Count() - 1)];
                List<char> guesses = new List<char>();
                int wrong = 0;
                bool won = false;

                Console.ForegroundColor = ConsoleColor.Red;

                Console.Clear();
                Console.WriteLine("\n" + Properties.Resources.title);

                Console.ForegroundColor = ConsoleColor.Gray;

                printCentered(" [Press any key to play]", 7);
                printCentered(" -" + words.Count() + " words loaded-", 19);

                Console.ReadKey();

                printCentered("                                ", 7);
                printCentered("                                ", 19);

                while (!gameOver)                                               //game loop
                {
                    Console.SetCursorPosition(0, 8);

                    drawMan(wrong, 8);

                    Console.SetCursorPosition(22, 14);

                    won = true;
                    for (int i = 0; i < currentWord.Length; i++)
                    {
                        char next = guesses.Contains(currentWord[i]) ? currentWord[i] : '_';
                        Console.Write(next + " ");

                        won = !(next == '_') && won;
                    }

                    Console.SetCursorPosition(19, 16);

                    foreach (char c in guesses) Console.Write(c);

                    Console.SetCursorPosition(19, 14);

                    Console.Write("> ");                                     //input

                    char guess = Console.ReadKey().KeyChar;
                    if (!guesses.Contains(guess))
                    {
                        guesses.Add(guess);

                        if (!currentWord.Contains(guess))
                            wrong++;
                    }

                    gameOver = wrong >= 6 || won;
                }

                if (!won)                                                   //end of game
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(Properties.Resources.lose);
                    Console.ForegroundColor = ConsoleColor.Gray;

                    Console.SetCursorPosition(28, 5);
                    Console.Write("YOU LOSE");
                    Console.SetCursorPosition(28, 6);
                    Console.Write("Word: " + currentWord);
                    Console.ReadKey();
                } else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(Properties.Resources.win + "\n\n");
                    Console.ForegroundColor = ConsoleColor.Gray;

                    Console.ReadKey();
                }
            }
        }

        //draw man based on number of incorrect answers
        static void drawMan(int wrong, int offset)
        {
            //│┌─┐
            string off = "";

            for (int i = 0; i < offset; i++)
                off += " ";

            string blank = off + "│              ";

            Console.WriteLine(off + "┌─────────┐     ");
            Console.WriteLine(off + "│         │     ");

            Console.WriteLine(wrong > 0 ? off + "│         O    " : blank);

            switch (wrong)
            {
                case 2:
                    Console.WriteLine(off + "│         │   ");
                    break;
                case 3:
                    Console.WriteLine(off + "│        /│   ");
                    break;
                case 4:
                case 5:
                case 6:
                    Console.WriteLine(off + "│        /│\\  ");
                    break;
                default:
                    Console.WriteLine(blank);
                    break;
            }
            switch (wrong)
            {
                case 5:
                    Console.WriteLine(off + "│        /    ");
                    break;
                case 6:
                    Console.WriteLine(off + "│        / \\  ");
                    break;
                default:
                    Console.WriteLine(blank);
                    break;
            }

            Console.WriteLine(blank + "\n" + blank);
        }

        //print text centered
        static void printCentered(string text, int line, int length = -1)
        {
            Console.SetCursorPosition((Console.WindowWidth - ((length == -1) ? text.Length : length)) / 2, line);
            Console.Write(text);
        }
    }
}