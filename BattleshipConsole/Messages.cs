using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipConsole;

internal class Messages
{
    public static void LogoMsg()
    {
        Console.WriteLine(@"
*****************************************************************************************************
*                                                                                                   *
*              ██████   █████  ████████ ████████ ██      ███████ ███████ ██   ██ ██ ██████          *
*              ██   ██ ██   ██    ██       ██    ██      ██      ██      ██   ██ ██ ██   ██         *
*              ██████  ███████    ██       ██    ██      █████   ███████ ███████ ██ ██████          *
*              ██   ██ ██   ██    ██       ██    ██      ██           ██ ██   ██ ██ ██              *
*              ██████  ██   ██    ██       ██    ███████ ███████ ███████ ██   ██ ██ ██              *
*                                                                                                   *
*****************************************************************************************************
");
    }

    public static void ShipMsg()
    {
        Console.WriteLine(@"
                      @#@#@#@#@#@#@#@#@#@#@#@
                                           \ \    __
                                 ___________\_\___|+\_____
                                 |       x       x    x  |
        \------------------------------------------------------------------------------------/
         \        o            o            o            o            o            o        /
          \                                                                                /");
    }

    public static void WavesMsg()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        for (int i = 0; i < Console.WindowWidth / 2; i++)
        {
            Console.Write(@"\/");
        }
        Console.ResetColor();
    }

    public static void MadeByMsg()
    {
        Console.WriteLine();
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(@"
                                     Laget av Marius Aalto
                          www.getacademy.no | www.github.com/aaltofar
");
        Console.ResetColor();
    }

    public static void Intro()
    {
        LogoMsg();
        ShipMsg();
        WavesMsg();
        MadeByMsg();
    }
}



