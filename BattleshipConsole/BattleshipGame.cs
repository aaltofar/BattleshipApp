using BattleshipConsole;
using BattleshipLibrary.Models;

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
    private readonly Random r = new();

    public (PlayerInfoModel, PlayerInfoModel) InitializeGame()
    {
        var player = CreatePlayer();
        var computer = CreateComputer();

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
        DisplayShotGrid(player);
        Console.WriteLine();
    }

    public PlayerInfoModel CreatePlayer()
    {
        var player = new PlayerInfoModel();
        player.UserName = Messages.AskForUsersName();
        player.InitializeGrid();
        return player;
    }

    public PlayerInfoModel CreateComputer()
    {
        var computer = new PlayerInfoModel();
        computer.RandomName();
        computer.InitializeGrid();
        PlaceComputerShips(computer);
        computer.IsComputer = true;
        return computer;
    }

    public void PlaceComputerShips(PlayerInfoModel computer)
    {
        while (computer.ShipLocations.Count < 5)
        {
            var letter = _letters[r.Next(0, _letters.Count)];
            var number = r.Next(1, 5);
            if (computer.IsOccupied(letter, number)) continue;

            computer.PlaceShip(letter, number);
        }
    }

    public void DetermineWinner(PlayerInfoModel player, PlayerInfoModel computer)
    {
        var hits = 0;
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
            if (player.IsOccupied(letter, number) || IsNotOnGrid(letter, number) == false)
            {
                Messages.UnableToPlaceMsg(location);
            }

            else
            {
                player.PlaceShip(letter, number);
                ShipCount++;
            }
        } while (player.ShipLocations.Count < MaxShipCount);
    }


    public void MakeShot(PlayerInfoModel shooter, PlayerInfoModel target)
    {
        if (shooter.IsComputer)
        {
            var computerHit = MakeComputerShot(target, shooter);
            Messages.ComputerShotMsg(shooter.UserName, computerHit.Item2, computerHit.Item3, computerHit.Item1);
            Thread.Sleep(3000);
        }
        else if (!shooter.IsComputer)
        {
            string? row;
            int column;
            bool isValidShot;
            do
            {
                Console.WriteLine();
                var shot = Messages.AskForShot();
                (row, column) = SplitInputRowCol(shot);
                isValidShot = shooter.ValidateShot(row, column);
                if (!isValidShot)
                    Messages.InvalidShotMsg(shot);
            } while (!isValidShot);

            var isHit = target.IsHit(row, column);
            if (!isHit)
                Messages.PlayerMissShotMessage(row, column);
            else
                Messages.PlayerHitShotMessage(row, column);

            shooter.MarkShotResult(row, column, isHit);
            Thread.Sleep(3000);
        }
    }

    private (bool, string, int) MakeComputerShot(PlayerInfoModel player, PlayerInfoModel computer)
    {
        var row = _letters[r.Next(0, _letters.Count)];
        var column = r.Next(1, MaxGridLength + 1);
        var isValidShot = computer.ValidateShot(row, column);
        var alreadyShotHere = computer.AlreadyShot(row, column);
        while (!isValidShot || alreadyShotHere)
        {
            row = _letters[r.Next(0, _letters.Count)];
            column = r.Next(1, MaxGridLength + 1);
            isValidShot = computer.ValidateShot(row, column);
            alreadyShotHere = computer.AlreadyShot(row, column);
        }

        var isHit = player.IsHit(row, column);
        computer.MarkShotResult(row, column, isHit);
        if (!isHit)
            return (false, row, column);

        return (true, row, column);
    }


    private static void DisplayShotGrid(PlayerInfoModel player)
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

    private void DisplayShipLocations(PlayerInfoModel player, List<string> letters, PlayerInfoModel computer)
    {
        var gridHeigth = 5;
        var gridLength = 5;
        for (var i = 0; i < gridHeigth; i++)
        {
            var currentLetter = letters[i];
            Console.WriteLine();
            for (var j = 1; j <= gridLength; j++)
                if (computer.IsSunk(j, currentLetter))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("[ X ]");
                    Console.ResetColor();
                }

                else if (computer.IsMiss(j, currentLetter))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"[{currentLetter} {j}]");
                    Console.ResetColor();
                }
                else if (player.HasShip(j, currentLetter))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"[{currentLetter} {j}]");
                    Console.ResetColor();
                }
                else
                {
                    Console.Write($"[{currentLetter} {j}]");
                }
        }
    }

    //flytte herfra og ned
    //private bool HasShip(PlayerInfoModel captain, int num, string letter)
    //{
    //    foreach (var s in captain.ShipLocations)
    //        if (s.SpotLetter == letter.ToUpper() && s.SpotNumber == num)
    //            if (s.Status == GridSpotStatus.Ship)
    //                return true;
    //    return false;
    //}

    //public bool AlreadyShot(string row, int column, PlayerInfoModel captain)
    //{
    //    foreach (var s in captain.ShotGrid)
    //        if (s.SpotLetter == row.ToUpper() && s.SpotNumber == column)
    //            if (s.Status != GridSpotStatus.Empty)
    //                return true;
    //    return false;
    //}

    //private bool IsMiss(PlayerInfoModel captain, int num, string letter)
    //{
    //    foreach (var s in captain.ShotGrid)
    //        if (s.SpotLetter == letter.ToUpper() && s.SpotNumber == num)
    //            if (s.Status == GridSpotStatus.Miss)
    //                return true;
    //    return false;
    //}

    //private bool IsSunk(PlayerInfoModel captain, int num, string letters)
    //{
    //    foreach (var s in captain.ShotGrid)
    //        if (s.SpotLetter == letters.ToUpper() && s.SpotNumber == num)
    //            if (s.Status == GridSpotStatus.Hit)
    //                return true;
    //    return false;
    //}

    //private bool ValidateShot(string row, int column, PlayerInfoModel captain)
    //{
    //    foreach (var s in captain.ShotGrid)
    //        if (s.SpotLetter == row.ToUpper() && s.SpotNumber == column)
    //            if (s.Status == GridSpotStatus.Empty)
    //                return true;
    //    return false;
    //}

    //public bool IsOccupied(string letter, int number, PlayerInfoModel captain)
    //{
    //    foreach (var l in captain.ShipLocations)
    //        if (l.SpotLetter == letter && l.SpotNumber == number)
    //            return true;

    //    return false;
    //}
}