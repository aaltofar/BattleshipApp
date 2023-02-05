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
    public static int MaxShipCount { get; set; } = 5;

    public static void InitializeGrid(PlayerInfoModel model)
    {
        List<string> letters = new()
        {
            "A",
            "B",
            "C",
            "D",
            "E"
        };
        List<int> numbers = new()
        {
            1,
            2,
            3,
            4,
            5
        };

        foreach (string letter in letters)
            foreach (int number in numbers)
                AddGridSpot(model, letter, number);
    }

    private static void AddGridSpot(PlayerInfoModel model, string letter, int number)
    {
        GridSpotModel spot = new GridSpotModel
        {
            SpotLetter = letter,
            SpotNumber = number,
            Status = GridSpotStatus.Empty,
        };
        model.ShotGrid.Add(spot);
    }

    public static bool IsOccupied(List<GridSpotModel> ShipLocations, string letter, int number)
    {
        foreach (var l in ShipLocations)
            if (l.SpotLetter == letter && l.SpotNumber == number)
                return true;

        return false;
    }

    public static bool IsNotOnGrid(string letter, int number, List<string> letters)
    {
        return letters.Contains(letter.ToUpper()) && number is >= 0 and <= 5;
    }

    public static void PlacePlayerShip(List<GridSpotModel> ShipLocations, string letter, int number)
    {
        var toAdd = new GridSpotModel()
        {
            SpotLetter = letter.ToUpper(),
            SpotNumber = number,
            Status = GridSpotStatus.Ship
        };
        ShipLocations.Add(toAdd);
    }

    public static List<GridSpotModel> PlaceComputerShips()
    {
        var output = new List<GridSpotModel>();
        var r = new Random();
        var letters = new List<string>()
        {
            "A",
            "B",
            "C",
            "D",
            "E"
        };
        while (output.Count < 5)
        {
            string letter = letters[r.Next(0, letters.Count)];
            int number = r.Next(1, 5);
            if (GameLogic.IsOccupied(output, letter, number))
            {
                continue;
            }

            output.Add(new GridSpotModel()
            {
                SpotLetter = letter,
                SpotNumber = number,
                Status = GridSpotStatus.Ship
            });
        }
        return output;
    }

    public static PlayerInfoModel CreateComputer()
    {
        var computer = new PlayerInfoModel()
        {
            UserName = RandomName(),
            ShipLocations = PlaceComputerShips()
        };
        //computer.UserName = RandomName();
        //computer.ShipLocations = PlaceComputerShips(computer);
        return computer;
    }
    static string RandomName()
    {
        var r = new Random();
        List<string> names = new List<string>()
        {
            "Kaptein Sortebill",
            "Kaptein Sabeltann",
            "Løytnant Kabelsatan",
            "Guybrush Threepwood",
            "Captain LeChuck",
            "Simen Rødskjegg",
            "Robert Blåpels",
            "Kaptein Thomassen",
            "Fredrik Tordenbart"
        };
        return names[r.Next(0, names.Count)];
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

    public static bool ValidateShot(string row, int column, PlayerInfoModel player)
    {
        foreach (var s in player.ShotGrid)
        {
            if (s.SpotLetter == row.ToUpper() && s.SpotNumber == column)
                if (s.Status == GridSpotStatus.Empty)
                    return true;
        }
        return false;
    }

    public static void MarkShotResult(PlayerInfoModel player, string row, int column, bool isHit)
    {
        foreach (var s in player.ShotGrid)
        {
            if (s.SpotLetter == row.ToUpper() && s.SpotNumber == column)
            {
                if (isHit)
                    s.Status = GridSpotStatus.Hit;

                else
                    s.Status = GridSpotStatus.Miss;
            }
        }
    }

    public static int GetShotTotalCount(PlayerInfoModel player)
    {
        int count = 0;
        foreach (var s in player.ShotGrid)
            if (s.Status != GridSpotStatus.Empty)
                count++;

        return count;
    }

    public static (bool, string, int) MakeComputerShot(PlayerInfoModel player, PlayerInfoModel computer, List<string> letters)
    {
        var r = new Random();

        string row = letters[r.Next(0, letters.Count)];
        int column = r.Next(0, 5);
        bool isValidShot = GameLogic.ValidateShot(row, column, player);

        while (!isValidShot)
        {
            row = letters[r.Next(0, letters.Count)];
            column = r.Next(0, 5);
            isValidShot = GameLogic.ValidateShot(row, column, player);
        }

        bool isHit = GameLogic.IsHit(player, row, column);
        GameLogic.MarkShotResult(computer, row, column, isHit);
        if (!isHit)
            return (false, row, column);
        else
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

