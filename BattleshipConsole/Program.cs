using System.Data;
using System.Data.Common;
using System.Diagnostics.Metrics;
using BattleshipConsole;
using BattleshipLibrary;
using BattleshipLibrary.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

Messages.Intro();
List<string> letters = new()
{
    "A",
    "B",
    "C",
    "D",
    "E"
};

PlayerInfoModel computer = GameLogic.CreateComputer();

PlayerInfoModel player = CreatePlayer(computer);

PlayerInfoModel winner = null;

do
{
    ShowBoards(player, computer, letters);
    MakeShot(player, computer);
    winner = GameLogic.DetermineWinner(player, computer);
    ComputerShot(player, computer, letters);
    winner = GameLogic.DetermineWinner(player, computer);
    Thread.Sleep(5000);
} while (winner == null);

if (winner == player)
{
    Messages.PlayerWinMessage(player, computer);
}

static void ShowBoards(PlayerInfoModel player, PlayerInfoModel computer, List<string> letters)
{
    Console.Clear();
    Console.WriteLine("Ditt brett:");
    DisplayShipLocations(player, letters);
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine("Motstanderens brett:");
    Console.WriteLine();
    DisplayShotGrid(player, computer);
    Console.WriteLine();
}

static void ComputerShot(PlayerInfoModel player, PlayerInfoModel computer, List<string> letters)
{
    (bool, string, int) computerHit = GameLogic.MakeComputerShot(player, computer, letters);
    Messages.ComputerShotMsg(computer.UserName, computerHit.Item2, computerHit.Item3, computerHit.Item1);
}

static PlayerInfoModel CreatePlayer(PlayerInfoModel computer)
{
    PlayerInfoModel player = new PlayerInfoModel();
    player.UserName = Messages.AskForUsersName();
    GameLogic.InitializeGrid(player);
    SetUserShipLocations(player, computer);
    return player;
}

static void MakeShot(PlayerInfoModel player, PlayerInfoModel opponent)
{
    bool isValidShot = false;
    string row = "";
    int column = 0;
    do
    {

        string shot = Messages.AskForShot();
        (row, column) = GameLogic.SplitInputRowCol(shot);
        isValidShot = GameLogic.ValidateShot(row, column, player);
        if (!isValidShot)
            Messages.InvalidShotMsg(shot);
    } while (!isValidShot);

    bool isHit = GameLogic.IsHit(opponent, row, column);
    if (!isHit)
        Console.WriteLine($"Du skøyt mot {row}{column}, men det var ikke noe skip der.");
    else
        Console.WriteLine($"Du skøyt mot {row}{column} og traff!");

    GameLogic.MarkShotResult(player, row, column, isHit);
}

static void DisplayShotGrid(PlayerInfoModel player, PlayerInfoModel computer)
{
    string currentRow = player.ShotGrid[0].SpotLetter;

    foreach (var gridSpot in player.ShotGrid)
    {
        if (gridSpot.SpotLetter != currentRow)
        {
            Console.WriteLine();
            currentRow = gridSpot.SpotLetter;
        }

        if (gridSpot.Status == GridSpotStatus.Empty)
        {
            Console.Write($"[{gridSpot.SpotLetter} {gridSpot.SpotNumber}]");
        }
        else if (gridSpot.Status == GridSpotStatus.Hit)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("[ X ]");
            Console.ResetColor();
        }
        else if (gridSpot.Status == GridSpotStatus.Miss)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("[ O ]");
            Console.ResetColor();
        }
        else
        {
            Console.Write("[ ? ]");
        }
    }
}

static void DisplayShipLocations(PlayerInfoModel player, List<string> letters)
{
    for (int i = 0; i < 5; i++)
    {
        string currentLetter = letters[i];
        Console.WriteLine();
        for (int j = 1; j <= 5; j++)
        {
            if (HasShip(player, j, currentLetter))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"[{currentLetter} {j}]");
                Console.ResetColor();
            }
            else
                Console.Write($"[{currentLetter} {j}]");
        }
    }
}

static bool HasShip(PlayerInfoModel player, int num, string letter)
{
    foreach (var s in player.ShipLocations)
    {
        if (s.SpotLetter == letter && s.SpotNumber == num)
        {
            if (s.Status == GridSpotStatus.Ship)
            {
                return true;
            }
        }
    }
    return false;
}

static void SetUserShipLocations(PlayerInfoModel player, PlayerInfoModel opponent)
{
    List<string> letters = new()
    {
        "A",
        "B",
        "C",
        "D",
        "E"
    };
    Messages.PlaceShipPhase(opponent.UserName);

    //foreach (var s in opponent.ShipLocations)
    //{
    //    Console.WriteLine(s.SpotLetter + s.SpotNumber + s.Status);
    //}

    int ShipCount = 1;
    do
    {
        Console.Clear();
        DisplayShipLocations(player, letters);
        Messages.PlaceNextShipMsg(ShipCount);
        string location = Console.ReadLine();
        (var letter, var number) = GameLogic.SplitInputRowCol(location);
        if (GameLogic.IsOccupied(player.ShipLocations, letter, number) || GameLogic.IsNotOnGrid(letter, number, letters) == false)
            Messages.UnableToPlaceMsg(location);

        else
        {
            GameLogic.PlacePlayerShip(player.ShipLocations, letter, number);
            ShipCount++;
        }

    } while (player.ShipLocations.Count < 5);
}

static void DisplayShipGrid(List<string> letters, PlayerInfoModel player)
{
    Messages.makeLetterLine(letters);
    Console.WriteLine();
    for (int i = 0; i < 5; i++)
    {
        Console.Write($"[ {i + 1} ]");
        for (int j = 0; j < 5; j++)
        {
            Console.Write($"[   ]");
        }
        Console.WriteLine();
    }
}