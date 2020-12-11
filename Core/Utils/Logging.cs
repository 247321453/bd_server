using System;

namespace Core.Utils
{
    public static class Logging
    {
        private static readonly object Lock = new object();

        public static void Info(string message, params object[] obj)
        {
            lock (Lock)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(" [ INFO ] ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(message);
                Console.WriteLine();
            }
        }
        public static void InfoTT(string message, params object[] obj)
        {
            lock (Lock)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(" [ INFO ] ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(message, obj);
                Console.WriteLine();
            }
        }

        public static void Session(string message, string address)
        {
            lock (Lock)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write(" [ SESSION ] ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($" [ {address} ] ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(message);
                Console.WriteLine();
            }
        }

        public static void SessionB(string message)
        {
            lock (Lock)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write(" [ SESSION ] ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(message);
                Console.WriteLine();
            }
        }

        public static void User(string message, string name)
        {
            lock (Lock)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write(" [ USER ] ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($" [ {name} ] ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(message);
                Console.WriteLine();
            }
        }

        public static void UserB(string message)
        {
            lock (Lock)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write(" [ USER ] ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(message);
                Console.WriteLine();
            }
        }

        public static void Server(string message)
        {
            lock (Lock)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(" [ SERVER ] ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(message);
                Console.WriteLine();
            }
        }

        public static void Alert(string message)
        {
            lock (Lock)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(" [ ALERT ] ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(message);
                Console.WriteLine();
            }
        }
    }
}