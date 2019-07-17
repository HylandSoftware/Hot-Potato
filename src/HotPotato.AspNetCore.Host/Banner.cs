using System;
using System.Collections.Generic;
using System.Text;

namespace HotPotato.AspNetCore.Host
{
    public static class Banner
    {
        public static bool Display(string[] args)
        {
            foreach (string item in args)
            {
                if (item.Contains("nologo", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }
            return true;
        }

        public static void Show()
        {
            Console.WriteLine(@"     /$$   /$$             /$$           /$$$$$$$             /$$                 /$$               ");
            Console.WriteLine(@"    | $$  | $$            | $$          | $$__  $$           | $$                | $$               ");
            Console.WriteLine(@"    | $$  | $$  /$$$$$$  /$$$$$$        | $$  \ $$ /$$$$$$  /$$$$$$    /$$$$$$  /$$$$$$    /$$$$$$  ");
            Console.WriteLine(@"    | $$$$$$$$ /$$__  $$|_  $$_/        | $$$$$$$//$$__  $$|_  $$_/   |____  $$|_  $$_/   /$$__  $$ ");
            Console.WriteLine(@"    | $$__  $$| $$  \ $$  | $$          | $$____/| $$  \ $$  | $$      /$$$$$$$  | $$    | $$  \ $$ ");
            Console.WriteLine(@"    | $$  | $$| $$  | $$  | $$ /$$      | $$     | $$  | $$  | $$ /$$ /$$__  $$  | $$ /$$| $$  | $$ ");
            Console.WriteLine(@"    | $$  | $$|  $$$$$$/  |  $$$$/      | $$     |  $$$$$$/  |  $$$$/|  $$$$$$$  |  $$$$/|  $$$$$$/ ");
            Console.WriteLine(@"    |__/  |__/ \______/    \___/        |__/      \______/    \___/   \_______/   \___/   \______/  ");
            Console.WriteLine();
        }
    }
}
