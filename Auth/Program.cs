using Auth.Emu.Network;
using Core.Utils;
using System;

namespace Auth
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Auth Server";
            Logging.Info(" __              __            ________                               ");
            Logging.Info("|  \\            |  \\          |        \\                              ");
            Logging.Info("| $$____    ____| $$  ______  | $$$$$$$$ ______ ____   __    __       ");
            Logging.Info("| $$    \\  /      $$ /      \\ | $$__    |      \\    \\ |  \\  |  \\      ");
            Logging.Info("| $$$$$$$\\|  $$$$$$$|  $$$$$$\\| $$  \\   | $$$$$$\\$$$$\\| $$  | $$      ");
            Logging.Info("| $$  | $$| $$  | $$| $$  | $$| $$$$$   | $$ | $$ | $$| $$  | $$      ");
            Logging.Info("| $$__/ $$| $$__| $$| $$__/ $$| $$_____ | $$ | $$ | $$| $$__/ $$      ");
            Logging.Info("| $$    $$ \\$$    $$ \\$$    $$| $$     \\| $$ | $$ | $$ \\$$    $$      ");
            Logging.Info(" \\$$$$$$$   \\$$$$$$$  \\$$$$$$  \\$$$$$$$$ \\$$  \\$$  \\$$  \\$$$$$$       ");
            Logging.Info("==================AuthServer====================");
            Logging.Info("========== THIS IS NOT A FREE SOFTWARE ==========");
            Logging.Info("Revision.................................0.0.1");
            Logging.Info("=================================================");
            new Connection().AsyncMain().GetAwaiter().GetResult();
        }
    }
}
