using ChessLogic;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ChessInterface
{
    /// <summary>
    /// Interação lógica para GameOverMenu.xam
    /// </summary>
    public partial class GameOverMenu : UserControl
    {
        public event Action<Option> OptionSelected;

        public GameOverMenu(GameState gameState)
        {
            InitializeComponent();

            Result result = gameState.Result;

            WinnerText.Text = GetWinnerText(result.Winner);

            ReasonText.Text = GetReasonText(result.Reason, gameState.CurrentPlayer);
        }


        private static string GetWinnerText(Player winner)
        {
            return winner switch
            {
                Player.White => "BRANCO GANHOU!",
                Player.Black => "PRETO GANHOU!",
                _ => "Empate"
            };
        }

        private static string PlayerString(Player player)
        {
            return player switch {
                Player.White => "BRANCO",
                Player.Black => "PRETO",
                _ => ""
            };
        }

        private static string GetReasonText(EndReason reason, Player currentPlayer)
        {
            return reason switch
            {
                EndReason.Stelemate => $"AFOGAMENTO - {PlayerString(currentPlayer)}NÃO PODE SE MOVER",
                EndReason.Checkmate => $"CHECKMATE  - {PlayerString(currentPlayer)}NÃO PODE SE MOVER",
                EndReason.FiftyMoveRule => "REGRA DOS 50 MOVIMENTOS",
                EndReason.InsufficienteMaterial => "MATERIAL INSUFICIENTE",
                EndReason.ThreefoldRepetition => "TRÍPLICE REPETIÇÃO DE POSIÇÃO",
                _ => ""
            };
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            OptionSelected?.Invoke(Option.Restart);
        }


        private void Exit_Click(object sender, RoutedEventArgs e)
        {

            OptionSelected?.Invoke(Option.Exit);
        }
    }
}
