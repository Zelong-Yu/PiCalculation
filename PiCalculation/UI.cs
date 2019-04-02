using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiCalculation
{
    public static class UI
    {
        public static int AcceptValidInt(string prompt,
                                          int minValue = int.MinValue,
                                          int maxValue = int.MaxValue)
        {
            var value = 0;
            var validInput = false;

            do
            {
                Console.Write(prompt);

                var input = Console.ReadLine();

                try
                {
                    value = int.Parse(input);

                    if ((value >= minValue) && (value <= maxValue))
                    {
                        validInput = true;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input.\n");
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine($"{input} is not a valid value.");
                    Console.WriteLine($"Valid range is ({minValue}, {maxValue})\n");
                }
            } while (!validInput);

            return value;
        }

        public static long AcceptValidLong(string prompt,
                                          long minValue = long.MinValue,
                                          long maxValue = long.MaxValue)
        {
            long value = 0;
            var validInput = false;

            do
            {
                Console.Write(prompt);

                var input = Console.ReadLine();

                try
                {
                    value = long.Parse(input);

                    if ((value >= minValue) && (value <= maxValue))
                    {
                        validInput = true;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input.\n");
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine($"{input} is not a valid value.");
                    Console.WriteLine($"Valid range is ({minValue}, {maxValue})\n");
                }
            } while (!validInput);

            return value;
        }

        public static int SelectionMenu(List<string> optionslist, string title = "Options (Q to quit): ", string bottomprompt="")
        {
            var done = false;

            int selector = 0;

            int count = optionslist.Count;

            do
            {
                Console.CursorVisible = false;
                var cursorLeftPos = Console.CursorLeft;
                var cursorTopPos = Console.CursorTop;
                Console.WriteLine(title);
                PrintMenu(selector, optionslist);
                var key = PromptForInput(bottomprompt);
                switch (key)
                {
                    case ConsoleKey.DownArrow:
                        selector++;
                        selector %= count;
                        break;
                    case ConsoleKey.UpArrow:
                        selector--;
                        selector = (selector + count) % count;
                        break;
                    case ConsoleKey.Q:
                        done = true;
                        selector = -1;
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.Enter:
                        done = true;
                        break;

                }
                //Console.CursorVisible = true;
                Console.SetCursorPosition(cursorLeftPos, cursorTopPos);
            } while (!done);
            return selector;
        }

        private static void PrintMenu(int selector, List<string> optionslist)
        {
            for (int i = 0; i < optionslist.Count; ++i)
            {
                Console.Write($"- ");
                if (i == selector)
                {
                    UI.Hightlight();

                }
                Console.WriteLine(optionslist[i]);
                Console.ResetColor();
            }
        }

        public static ConsoleKey PromptForInput(string prompt = ">")
        {
            var cursorLeftPos = Console.CursorLeft;
            var cursorTopPos = Console.CursorTop;

            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write(prompt);

            Console.SetCursorPosition(cursorLeftPos, cursorTopPos);

            return Console.ReadKey(true).Key;
        }

        public static void Hightlight()
        {
            var background = Console.BackgroundColor;
            var foreground = Console.ForegroundColor;

            Console.ForegroundColor = background;
            Console.BackgroundColor = foreground;
        }

        //Print a string at the center of line
        public static void CenteredString(string s)
        {
            if (s.Length <= Console.WindowWidth)
            {
                Console.SetCursorPosition((Console.WindowWidth - s.Length) / 2, Console.CursorTop);
                Console.WriteLine(s);
            }
            else
            {
                throw new Exception("Oversided String");
            }
        }
    }
}