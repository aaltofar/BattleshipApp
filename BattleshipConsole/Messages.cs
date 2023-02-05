using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleshipLibrary;
using BattleshipLibrary.Models;

namespace BattleshipConsole;

public class Messages
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

    public static void UnableToPlaceMsg(string location)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine();
        Console.WriteLine($"Jeg klarte ikke å plassere skipet ditt på {location}, mulige plasseringer er A1 til E5");
        Console.WriteLine();
        Console.ResetColor();
    }

    public static void PlayerWinMessage(PlayerInfoModel player, PlayerInfoModel computer)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(@$"
*********************************************************
  Du vant over {computer.UserName}                      
  Antall skudd: {GameLogic.GetShotTotalCount(player)}   
*********************************************************
");
        Console.ResetColor();

    }

    public static void ComputerShotMsg(string username, string letter, int number, bool hit)
    {
        if (hit)
        {
            Console.WriteLine();
            Console.WriteLine($"{username} skøt mot {letter + number}");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Det var treff!");
            Console.ResetColor();
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine($"{username} skøt mot {letter + number}");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Det var bom!");
            Console.ResetColor();
            Console.WriteLine();
        }
    }

    public static void PlaceNextShipMsg(int ShipCount)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"Plasserer skip nummer {ShipCount} av 5");
        Console.ResetColor();
        Console.Write($"Hvor vil du plassere skipet ditt?");
        Console.WriteLine();
        Console.Write("Plassering: ");
    }

    public static void PlaceShipPhase(string opponentName)
    {
        Console.Clear();
        Console.WriteLine($"Din motstander er {opponentName}!");
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("**Planleggingsfasen**");
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine("Plasser ut fem skip på brettet, motstanderen din gjør det samme");
    }

    public static string AskForUsersName()
    {
        Console.WriteLine("Hva heter du?");
        Console.Write("Navn: ");
        string output = Console.ReadLine();
        while (output.Length <= 1)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Er du sikker på at du skrev inn et ordentlig navn?");
            Console.WriteLine("Prøv igjen");
            Console.ResetColor();
            Console.WriteLine();
            Console.Write("Navn: ");
            output = Console.ReadLine();
        }
        return output;
    }
    public static void InvalidShotMsg(string msg)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Oops, jeg klarte ikke å skyte på {msg}, prøv igjen");
        Console.ResetColor();
    }

    public static string AskForShot()
    {
        Console.WriteLine();
        Console.WriteLine("Hvor vil du skyte?");
        Console.Write("Plassering: ");
        string result = Console.ReadLine();
        return result;
    }

    public static void makeLetterLine(List<string> letters)
    {
        Console.Write("     ");
        for (int i = 0; i < 5; i++)
        {
            Console.Write($"[ {letters[i]} ]");
        }
    }
}



