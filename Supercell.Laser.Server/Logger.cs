namespace Supercell.Laser.Server
{
    using Supercell.Laser.Titan.Debug;
    using System;

    public static class Logger
    {
        public static void Print(string log)
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("[DEBUG] " + log);
        }

        public static void Init()
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Debugger.SetListener(new DebuggerListener());
        }

        public static void Warning(string log)
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("[WARNING] " + log);
        }

        public static void Error(string log)
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("[ERROR] " + log);
        }
    }

    public class DebuggerListener : IDebuggerListener
    {
        public void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("[LOGIC] Error: " + message);
        }

        public void Print(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("[LOGIC] Info: " + message);
        }

        public void Warning(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("[LOGIC] Warning: " + message);
        }
    }
}
