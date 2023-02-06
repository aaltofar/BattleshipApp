using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleshipLibrary.Models;

namespace BattleshipLibrary;

public static class GameLogic
{

    public static bool IsNotOnGrid(string letter, int number, List<string> letters)
    {
        return letters.Contains(letter.ToUpper()) && number is >= 0 and <= 5;
    }

    public static PlayerInfoModel CreateComputer()
    {
        var computer = new PlayerInfoModel();
        computer.UserName = RandomName();
        InitializeGrid(computer);
        computer.ShipLocations = PlaceComputerShips();
        return computer;
    }


    public static bool IsHit(PlayerInfoModel opponent, string row, int column)
    {
        foreach (var s in opponent.ShipLocations)
        {
            if (s.SpotLetter == row && s.SpotNumber == column && s.Status == GridSpotStatus.Ship)
                return true;
        }
        return false;
    }

    public static (string row, int column) SplitInputRowCol(string input)
    {
        var letter = input[0].ToString().ToUpper();
        var number = int.Parse(input[1].ToString());
        return (letter, number);
    }

    public static (bool, string, int) MakeComputerShot(PlayerInfoModel player, PlayerInfoModel computer, List<string> letters)
    {
        var r = new Random();

        string row = letters[r.Next(0, letters.Count)];
        int column = r.Next(1, 6);
        bool isValidShot = GameLogic.ValidateShot(row, column, computer);
        bool alreadyShotHere = AlreadyShot(computer, row, column);
        while (!isValidShot || alreadyShotHere)
        {
            row = letters[r.Next(0, letters.Count)];
            column = r.Next(1, 6);
            isValidShot = GameLogic.ValidateShot(row, column, computer);
            alreadyShotHere = AlreadyShot(computer, row, column);
        }

        bool isHit = GameLogic.IsHit(player, row, column);
        GameLogic.MarkComputerShotResult(computer, row, column, isHit, player);
        if (!isHit)
            return (false, row, column);

        return (true, row, column);
    }

    public static PlayerInfoModel DetermineWinner(PlayerInfoModel player, PlayerInfoModel computer)
    {
        int hits = 0;
        foreach (var s in player.ShotGrid)
            if (s.Status == GridSpotStatus.Hit)
                hits++;

        if (hits == 5)
            return player;

        return null;
    }
}

