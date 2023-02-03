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

do
{
    DisplayShotGrid(player, computer);
    MakeShot(player, computer);


} while (winner == null);

static PlayerInfoModel CreatePlayer(PlayerInfoModel computer)
{
    PlayerInfoModel output = new PlayerInfoModel();
    output.UserName = Messages.AskForUsersName();
    GameLogic.InitializeGrid(output);
    SetUserShipLocations(output, computer);
    return output;
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
    Messages.PlaceShipPhase(opponent.UserName);
    int ShipCount = 1;
    do
    {
        Messages.PlaceNextShipMsg(ShipCount);
        string location = Console.ReadLine();
        var letter = location[0].ToString();
        var number = int.Parse(location[1].ToString());

        if (GameLogic.IsOccupied(model.ShipLocations, letter, number) || GameLogic.IsNotInRange(letter, number, letters) == false)
        {
            Messages.UnableToPlaceMsg(location);
        }
        else
        {
            GameLogic.PlacePlayerShip(model.ShipLocations, letter, number);
            ShipCount++;
        }

    } while (model.ShipLocations.Count < 5);
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