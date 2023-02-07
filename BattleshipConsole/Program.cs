using System.Text;
using Battleship;
using BattleshipConsole;
using BattleshipLibrary;
using BattleshipLibrary.Models;
Console.OutputEncoding = Encoding.UTF8;

BattleshipGame game = new();
var (player, computer) = game.InitializeGame();


Messages.Intro();
game.SetShipLocations(player);

do
{
    game.ShowBoards(player, computer);

    game.MakeShot(player, computer);

    game.DetermineWinner(player, computer);

    game.MakeShot(computer, player);

    game.DetermineWinner(player, computer);

} while (game.Winner == null);

if (game.Winner == player)
    Messages.PlayerWinMessage(player, computer);

else
    Messages.ComputerWinMessage(player, computer);
