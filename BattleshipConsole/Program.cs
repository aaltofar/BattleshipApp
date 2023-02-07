using System.Text;
using Battleship;
using BattleshipConsole;
using BattleshipLibrary;
using BattleshipLibrary.Models;
Console.OutputEncoding = Encoding.UTF8;

BattleshipGame game = new();
Messages.Intro();
var (player, computer) = game.InitializeGame();
//var player = game.CreatePlayer();
//var computer = game.CreateComputer();
Console.WriteLine("Hei");
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

//else
//    Messages.ComputerWinMessage(player, computer);
