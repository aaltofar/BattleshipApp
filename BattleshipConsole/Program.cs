using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics.Metrics;
using System.Text;
using Battleship;
using BattleshipConsole;
using BattleshipLibrary;
using BattleshipLibrary.Models;
Console.OutputEncoding = Encoding.UTF8;

List<string> letters = new()
{
    "A",
    "B",
    "C",
    "D",
    "E"
};
BattleshipGame game = new();

PlayerInfoModel computer = game.CreateComputer();
PlayerInfoModel player = game.CreatePlayer();



Messages.Intro();
game.SetShipLocations();
do
{
    game.ShowBoards(player, computer);

    player.MakeShot();

    game.DetermineWinner(player, computer);

    computer.MakeShot();

    game.DetermineWinner(player, computer);

} while (game.Winner == null);

if (game.Winner == player)
{
    Messages.PlayerWinMessage(player, computer);
}
