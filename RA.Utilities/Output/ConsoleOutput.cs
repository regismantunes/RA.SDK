﻿using System;
using System.Collections.Generic;

namespace RA.Utilities.Output
{
    /// <summary>
    /// Implements the IOutput interface to provide console output functionality.
    /// </summary>
    public class ConsoleOutput : IOutput
    {
        private readonly Stack<int> _linesPosition = new();

        public void Clear()
        {
            _linesPosition.Clear();
            ClearConsole(0);
        }

        public void ClearLine()
        {
            var cursorTop = _linesPosition.Pop();
            ClearConsole(cursorTop);
        }

        public void WriteLine(string message)
        {
            _linesPosition.Push(Console.CursorTop);
            Console.WriteLine(message);
        }

        private static void ClearConsole(int cursorTop)
        {
            var currentCursorTop = Console.CursorTop;
            for (var c = cursorTop; c <= currentCursorTop; c++)
            {
                Console.SetCursorPosition(0, c);
                Console.Write(new string(' ', Console.WindowWidth));
            }

            Console.SetCursorPosition(0, cursorTop);
        }
    }
}
