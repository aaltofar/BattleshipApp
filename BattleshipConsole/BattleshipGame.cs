using BattleshipConsole;
using BattleshipLibrary.Models;
using BattleshipLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Battleship;

internal class BattleshipGame
{
    public PlayerInfoModel Winner { get; set; }
    public List<string> _letters = new()
    {
        "A",
        "B",
        "C",
        "D",
        "E"
    };

    private const int MaxGridLength = 5;
    private const int MaxShipCount = 5;

    public (PlayerInfoModel, PlayerInfoModel) InitializeGame()
    {
        var computer = CreateComputer();
        var player = CreatePlayer();

        return (player, computer);
    }
    public void ShowBoards(PlayerInfoModel player, PlayerInfoModel computer)
    {
        Console.Clear();
        Console.WriteLine("Ditt brett:");
        DisplayShipLocations(player, _letters, computer);
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("Motstanderens brett:");
        Console.WriteLine();
        DisplayShotGrid(player, computer);
        Console.WriteLine();
    }

    public void ComputerShot(PlayerInfoModel player, PlayerInfoModel computer)
    {
        (bool, string, int) computerHit = MakeComputerShot(player, computer);
        Messages.ComputerShotMsg(computer.UserName, computerHit.Item2, computerHit.Item3, computerHit.Item1);
    }

    public PlayerInfoModel CreatePlayer()
    {
        PlayerInfoModel player = new PlayerInfoModel();
        player.UserName = Messages.AskForUsersName();
        player.InitializeGrid();
        return player;
    }

    public PlayerInfoModel CreateComputer()
    {
        var computer = new PlayerInfoModel();
        computer.RandomName();
        computer.InitializeGrid();
        computer.PlaceComputerShips();
        return computer;
    }

    public void DetermineWinner(PlayerInfoModel player, PlayerInfoModel computer)
    {
        int hits = 0;
        foreach (var s in player.ShotGrid)
            if (s.Status == GridSpotStatus.Hit)
                hits++;

        if (hits == 5)
            Winner = player;
    }

    public static (string row, int column) SplitInputRowCol(string input)
    {
        var letter = input[0].ToString().ToUpper();
        var number = int.Parse(input[1].ToString());
        return (letter, number);
    }

    public bool IsNotOnGrid(string letter, int number)
    {
        return _letters.Contains(letter.ToUpper()) && number is >= 0 and <= MaxGridLength;
    }

    public void SetShipLocations(PlayerInfoModel player)
    {
        Messages.PlaceShipPhase();

        var ShipCount = 1;
        do
        {
            Console.Clear();
            Messages.PlaceNextShipMsg(ShipCount);
            var location = Console.ReadLine();
            var (letter, number) = SplitInputRowCol(location);
            if (IsOccupied(letter, number, player) || IsNotOnGrid(letter, number) == false)
                Messages.UnableToPlaceMsg(location);

            else
            {
                player.PlacePlayerShip(letter, number);
                ShipCount++;
            }
        } while (player.ShipLocations.Count < MaxShipCount);
    }

    public bool IsOccupied(string letter, int number, PlayerInfoModel captain)
    {
        foreach (var l in captain.ShipLocations)
            if (l.SpotLetter == letter && l.SpotNumber == number)
                return true;

        return false;
    }

    public void MakeShot(PlayerInfoModel shooter, PlayerInfoModel target)
    {
        if (shooter.IsComputer)
        {
            var computerHit = MakeComputerShot(target, shooter);
            Messages.ComputerShotMsg(shooter.UserName, computerHit.Item2, computerHit.Item3, computerHit.Item1);
        }
        else
        {
            string? row;
            int column;
            bool isValidShot;
            do
            {
                Console.WriteLine();
                var shot = Messages.AskForShot();
                (row, column) = SplitInputRowCol(shot);
                isValidShot = ValidateShot(row, column, target);
                if (!isValidShot)
                    Messages.InvalidShotMsg(shot);
            } while (!isValidShot);

            var isHit = IsHit(target, row, column);
            if (!isHit)
                Messages.PlayerMissShotMessage(row, column);
            else
                Messages.PlayerHitShotMessage(row, column);

            shooter.MarkShotResult(row, column, isHit);
        }

    }

    private (bool, string, int) MakeComputerShot(PlayerInfoModel player, PlayerInfoModel computer)
    {
        var r = new Random();

        var row = _letters[r.Next(0, _letters.Count)];
        var column = r.Next(1, MaxGridLength + 1);
        var isValidShot = ValidateShot(row, column, computer);
        var alreadyShotHere = AlreadyShot(row, column, computer);
        while (!isValidShot || alreadyShotHere)
        {
            row = _letters[r.Next(0, _letters.Count)];
            column = r.Next(1, MaxGridLength + 1);
            isValidShot = ValidateShot(row, column, computer);
            alreadyShotHere = AlreadyShot(row, column, computer);
        }

        var isHit = IsHit(player, row, column);
        computer.MarkShotResult(row, column, isHit);
        if (!isHit)
            return (false, row, column);

        return (true, row, column);
    }

    public bool IsHit(PlayerInfoModel target, string row, int column)
    {
        foreach (var s in target.ShipLocations)
            if (s.SpotLetter == row && s.SpotNumber == column && s.Status == GridSpotStatus.Ship)
                return true;
        return false;
    }

    private static void DisplayShotGrid(PlayerInfoModel player, PlayerInfoModel computer)
    {
        var currentRow = player.ShotGrid[0].SpotLetter;

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

    static void DisplayShipLocations(PlayerInfoModel player, List<string> letters, PlayerInfoModel computer)
    {
        int gridHeigth = 5;
        int gridLength = 5;
        for (int i = 0; i < gridHeigth; i++)
        {
            string currentLetter = letters[i];
            Console.WriteLine();
            for (int j = 1; j <= gridLength; j++)
            {
                if (HasShip(player, j, currentLetter))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"[{currentLetter} {j}]");
                    Console.ResetColor();
                }
                else if (IsMiss(computer, j, currentLetter))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"[{currentLetter} {j}]");
                    Console.ResetColor();
                }
                else if (IsSunk(player, j, currentLetter))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"[ X ]");
                    Console.ResetColor();
                }
                else
                    Console.Write($"[{currentLetter} {j}]");
            }
        }
    }

    private static bool HasShip(PlayerInfoModel captain, int num, string letter)
    {
        foreach (var s in captain.ShipLocations)
            if (s.SpotLetter == letter.ToUpper() && s.SpotNumber == num)
                if (s.Status == GridSpotStatus.Ship)
                    return true;
        return false;
    }

    public bool AlreadyShot(string row, int column, PlayerInfoModel captain)
    {
        foreach (var s in captain.ShotGrid)
            if (s.SpotLetter == row && s.SpotNumber == column)
                if (s.Status != GridSpotStatus.Empty)
                    return true;
        return false;
    }

    private bool IsMiss(PlayerInfoModel captain, int num, string letter)
    {
        foreach (var s in captain.ShotGrid)
            if (s.SpotLetter == letter.ToUpper() && s.SpotNumber == num)
                if (s.Status == GridSpotStatus.Miss)
                    return true;
        return false;
    }

    private bool IsSunk(PlayerInfoModel captain, int num, string letters)
    {
        foreach (var s in captain.ShipLocations)
            if (s.SpotLetter == letters && s.SpotNumber == num)
                if (s.Status == GridSpotStatus.Hit)
                    return true;
        return false;
    }

    private bool ValidateShot(string row, int column, PlayerInfoModel captain)
    {
        foreach (var s in captain.ShotGrid)
            if (s.SpotLetter == row.ToUpper() && s.SpotNumber == column)
                if (s.Status == GridSpotStatus.Empty)
                    return true;
        return false;
    }

}

