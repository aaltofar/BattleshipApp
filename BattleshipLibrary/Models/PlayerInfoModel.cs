using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipLibrary.Models;

public class PlayerInfoModel
{
    public string UserName { get; set; }
    public List<GridSpotModel> ShipLocations { get; set; } = new List<GridSpotModel>();
    public List<GridSpotModel> ShotGrid { get; set; } = new List<GridSpotModel>();
    public bool IsComputer { get; set; }
    public int TotalShots { get; set; }

    private Random r = new Random();

    public List<string> _letters = new()
    {
        "A",
        "B",
        "C",
        "D",
        "E"
    };
    public List<int> _numbers = new()
    {
        1,
        2,
        3,
        4,
        5
    };

    public void RandomName()
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
        UserName = names[r.Next(0, names.Count)];
    }
    public void InitializeGrid()
    {
        foreach (string letter in _letters)
            foreach (int number in _numbers)
                AddGridSpot(letter, number);
    }
    private void AddGridSpot(string letter, int number)
    {
        GridSpotModel spot = new GridSpotModel
        {
            SpotLetter = letter,
            SpotNumber = number,
            Status = GridSpotStatus.Empty,
        };
        ShotGrid.Add(spot);
    }

    public void PlaceComputerShips()
    {
        var output = new List<GridSpotModel>();
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
            if (IsOccupied(letter, number))
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
        ShipLocations = output;
    }

    private void PlacePlayerShip(string letter, int number)
    {
        var toAdd = new GridSpotModel()
        {
            SpotLetter = letter.ToUpper(),
            SpotNumber = number,
            Status = GridSpotStatus.Ship
        };
        ShipLocations.Add(toAdd);
    }

    public void MarkShotResult(string row, int column, bool isHit)
    {
        foreach (var s in ShotGrid)
        {
            if (s.SpotLetter == row.ToUpper() && s.SpotNumber == column)
            {
                if (isHit)
                {
                    s.Status = GridSpotStatus.Hit;
                    //husk sink ship!!!! SinkShip(player, row, column);
                }

                else
                    s.Status = GridSpotStatus.Miss;
            }
        }
    }

    public bool ValidateShot(string row, int column)
    {
        foreach (var s in ShotGrid)
        {
            if (s.SpotLetter == row.ToUpper() && s.SpotNumber == column)
                if (s.Status == GridSpotStatus.Empty)
                    return true;
        }
        return false;
    }

    public void SinkShip(string row, int column)
    {
        foreach (var s in ShipLocations)
        {
            if (s.SpotLetter == row && s.SpotNumber == column)
            {
                s.Status = GridSpotStatus.Sunk;
            }
        }
    }

    public bool AlreadyShot(string row, int column)
    {
        foreach (var s in ShotGrid)
        {
            if (s.SpotLetter == row && s.SpotNumber == column)
            {
                if (s.Status != GridSpotStatus.Empty)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool IsOccupied(string letter, int number)
    {
        foreach (var l in ShipLocations)
            if (l.SpotLetter == letter && l.SpotNumber == number)
                return true;

        return false;
    }

    public void MakeShot() { }

    public void GetShotTotalCount()
    {
        foreach (var s in ShotGrid)
            if (s.Status != GridSpotStatus.Empty)
                TotalShots++;
    }

}

