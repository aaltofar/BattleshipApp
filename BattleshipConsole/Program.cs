using System.Runtime.CompilerServices;
using System.Xml;
using BattleshipConsole;
using BattleshipLibrary;
using BattleshipLibrary.Models;

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

static PlayerInfoModel CreatePlayer(PlayerInfoModel computer)
{
    PlayerInfoModel output = new PlayerInfoModel();
    //Spørre spilleren om navn
    output.UserName = AskForUsersName();
    //Laste inn brettet
    GameLogic.InitializeGrid(output);
    //Spørre spilleren hvor den vil ha skipene sine
    SetUserShipLocations(output, computer);
    Console.Clear();

    return output;
}
//DisplayShipGrid(letters, player);
static void DisplayShipGrid(List<string> letters, PlayerInfoModel player)
{
    makeLetterLine(letters);
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

static void makeLetterLine(List<string> letters)
{
    Console.Write("     ");
    for (int i = 0; i < 5; i++)
    {
        Console.Write($"[ {letters[i]} ]");
    }
}

do
{
    DisplayShotGrid(player, computer);
} while (winner == null);

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
            Console.Write($" [ {gridSpot.SpotLetter} {gridSpot.SpotNumber} ] ");
        }
        else if (gridSpot.Status == GridSpotStatus.Hit)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(" X ");
            Console.ResetColor();
        }
        else if (gridSpot.Status == GridSpotStatus.Miss)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(" O ");
        }
        else
        {
            Console.Write(" ? ");
        }
    }
}

static void AskBombLocation()
{

}

static void DetermineWinner()
{

}

static void SetUserShipLocations(PlayerInfoModel model, PlayerInfoModel opponent)
{
    List<string> letters = new()
    {
        "A",
        "B",
        "C",
        "D",
        "E"
    };
    Console.Clear();
    Console.WriteLine($"Din motstander er {opponent.UserName}!");
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("**Planleggingsfasen**");
    Console.ResetColor();
    Console.WriteLine();
    Console.WriteLine("Plasser ut fem skip på brettet, motstanderen din gjør det samme");
    int ShipCount = 1;
    do
    {
        PlaceNextShipMsg(ShipCount);
        string location = Console.ReadLine();

        if (GameLogic.IsOccupied(model.ShipLocations,
                location[0].ToString(),
                int.Parse(location[1].ToString())) == true)
        {
            UnableToPlaceMsg(location);
        }
        else
        {
            var letter = location[0].ToString();
            var number = int.Parse(location[1].ToString());
            if (letters.Contains(letter.ToUpper()) && (number >= 0 && number <= 5))
            {
                GameLogic.PlacePlayerShip(model.ShipLocations, letter, number);
                ShipCount++;
            }
            else
                UnableToPlaceMsg(location);

        }
    } while (model.ShipLocations.Count < 5);
}

static void UnableToPlaceMsg(string location)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine();
    Console.WriteLine($"Jeg klarte ikke å plassere skipet ditt på {location}, mulige plasseringer er A1 til E5");
    Console.WriteLine();
    Console.ResetColor();
}

static void PlaceNextShipMsg(int ShipCount)
{
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine($"Plasserer skip nummer {ShipCount} av 5");
    Console.ResetColor();
    Console.Write($"Hvor vil du plassere skipet ditt?");
    Console.WriteLine();
    Console.Write("Plassering: ");
}

static string AskForUsersName()
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