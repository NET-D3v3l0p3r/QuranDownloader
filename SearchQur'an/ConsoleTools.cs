using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace SearchQur_an
{
    public static class ConsoleTools
    {
        public static void WriteLine(string s, ConsoleColor newClr, ConsoleColor rClr, bool newLine)
        {
            Console.ForegroundColor = newClr;
            Console.WriteLine(s);
            if (newLine)
            {
                Console.WriteLine();
            }
            Console.ForegroundColor = rClr;
        }

        public static void ClearLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }

        public static void WriteLine(string s, Dictionary<char, ConsoleColor> charsToChange, ConsoleColor resetColor)
        {
            //Hallo, wie geht es Dir?

            for (int i = 0; i <= s.ToCharArray().Length - 1; i++)
            {
                if (charsToChange.Keys.Contains(s.ToCharArray()[i]))
                {
                    Console.ForegroundColor = charsToChange[s.ToCharArray()[i]];
                    Console.Write(s.ToCharArray()[i]);
                }
                else
                {
                    Console.ForegroundColor = resetColor;
                    Console.Write(s.ToCharArray()[i]);
                }
            }

        }
        public static void WriteLineWithColoredChars(string sentence, string toChangePhrase, ConsoleColor newClr, ConsoleColor resetClr)
        {
            Char[] sentenceLetters = sentence.ToCharArray();
            List<Char> toColorLetters = new List<char>();

            toColorLetters = toChangePhrase.ToList();

            for (int i = 0; i <= sentenceLetters.Length - 1; i++)
            {
                if (toColorLetters.Contains(sentenceLetters[i]))
                {
                    Console.ForegroundColor = newClr;
                    Console.Write(sentenceLetters[i]);
                }
                else
                {
                    Console.ForegroundColor = resetClr;
                    Console.Write(sentenceLetters[i]);
                }
            }

            Console.WriteLine();
        }

        public static void WriteLineWithColoredWord(string sentence, string toChangePhrase, ConsoleColor newClr, ConsoleColor resetClr)
        {
            string[] _Words = sentence.Split(' ');

            foreach (var _word in _Words)
            {
                if (_word == toChangePhrase)
                {
                    Console.ForegroundColor = newClr;
                    Console.Write(_word + " ");
                }
                else
                {
                    Console.ForegroundColor = resetClr;
                    Console.Write(_word + " ");
                }
            }
        }


    }

    public static class ConsoleColorChanger
    {
        static int STD_OUTPUT_HANDLE = -11;                                        // per WinBase.h
        internal static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);    // per WinBase.h

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetConsoleScreenBufferInfoEx(IntPtr hConsoleOutput, ref CONSOLE_SCREEN_BUFFER_INFO_EX csbe);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleScreenBufferInfoEx(IntPtr hConsoleOutput, ref CONSOLE_SCREEN_BUFFER_INFO_EX csbe);

        public static int SetColor(System.ConsoleColor color, byte r, byte g, byte b)
        {
            CONSOLE_SCREEN_BUFFER_INFO_EX csbe = new CONSOLE_SCREEN_BUFFER_INFO_EX();
            csbe.cbSize = (uint)Marshal.SizeOf(csbe);                    // 96 = 0x60
            IntPtr hConsoleOutput = GetStdHandle(STD_OUTPUT_HANDLE);    // 7
            if (hConsoleOutput == INVALID_HANDLE_VALUE)
            {
                return Marshal.GetLastWin32Error();
            }
            bool brc = GetConsoleScreenBufferInfoEx(hConsoleOutput, ref csbe);
            if (!brc)
            {
                return Marshal.GetLastWin32Error();
            }

            csbe.ColorTable[(int)color] = new COLORREF(r, g, b);

            ++csbe.srWindow.Bottom;
            ++csbe.srWindow.Right;
            brc = SetConsoleScreenBufferInfoEx(hConsoleOutput, ref csbe);
            if (!brc)
            {
                return Marshal.GetLastWin32Error();
            }
            return 0;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct COORD
    {
        public short X;
        public short Y;
    }

    public struct SMALL_RECT
    {
        public short Left;
        public short Top;
        public short Right;
        public short Bottom;
    }

    public struct CONSOLE_SCREEN_BUFFER_INFO
    {
        public COORD dwSize;
        public COORD dwCursorPosition;
        public short wAttributes;
        public SMALL_RECT srWindow;
        public COORD dwMaximumWindowSize;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CONSOLE_SCREEN_BUFFER_INFO_EX
    {
        public uint cbSize;
        public COORD dwSize;
        public COORD dwCursorPosition;
        public short wAttributes;
        public SMALL_RECT srWindow;
        public COORD dwMaximumWindowSize;

        public ushort wPopupAttributes;
        public bool bFullscreenSupported;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public COLORREF[] ColorTable;

        public static CONSOLE_SCREEN_BUFFER_INFO_EX Create()
        {
            return new CONSOLE_SCREEN_BUFFER_INFO_EX { cbSize = 96 };
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct COLORREF
    {
        public uint ColorDWORD;

        public COLORREF(byte r, byte g, byte b)
        {
            ColorDWORD = (uint)r + (((uint)g) << 8) + (((uint)b) << 16);
        }
    }

}
