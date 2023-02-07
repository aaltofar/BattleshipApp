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

    private List<string> _letters = new()
    {
        "A",
        "B",
        "C",
        "D",
        "E"
    };
    private List<int> _numbers = new()
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

        while (output.Count < 5)
        {
            var letter = _letters[r.Next(0, _letters.Count)];
            var number = r.Next(1, 5);
            if (IsOccupied(letter, number)) continue;

            PlaceShip(letter, number);
        }

        ShipLocations = output;
    }

    private bool IsOccupied(string letter, int number)
    {
        foreach (var l in ShipLocations)
            if (l.SpotLetter == letter && l.SpotNumber == number)
                return true;

        return false;
    }

    public void PlaceShip(string letter, int number)
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
            if (s.SpotLetter == row.ToUpper() && s.SpotNumber == column)
            {
                if (isHit)
                    s.Status = GridSpotStatus.Hit;

                else
                    s.Status = GridSpotStatus.Miss;
            }

        TotalShots++;
    }

    public void SinkShip(string row, int column)
    {
        foreach (var s in ShipLocations)
            if (s.SpotLetter == row && s.SpotNumber == column)
                s.Status = GridSpotStatus.Sunk;
    }

}

