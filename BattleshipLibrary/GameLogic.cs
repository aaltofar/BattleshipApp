using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleshipLibrary.Models;

namespace BattleshipLibrary;

public static class GameLogic
{
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
        {
            foreach (int number in numbers)
            {
                AddGridSpot(model, letter, number);
            }
        }
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
        bool HasShip = false;
        foreach (var l in ShipLocations)
        {
            if (l.SpotLetter == letter && l.SpotNumber == number)
            {
                HasShip = true;
                return true;
            }
        }
        return HasShip;
    }

    public static void PlacePlayerShip(List<GridSpotModel> ShipLocations, string letter, int number)
    {
        var toAdd = new GridSpotModel()
        {
            SpotLetter = letter,
            SpotNumber = number,
            Status = GridSpotStatus.Ship
        };
        ShipLocations.Add(toAdd);
    }

    public static List<GridSpotModel> PlaceComputerShips(PlayerInfoModel user)
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
        var computer = new PlayerInfoModel();
        computer.UserName = RandomName();
        computer.ShipLocations = PlaceComputerShips(computer);
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
            "Captain LeChuck"
        };
        return names[r.Next(0, names.Count)];
    }
}

