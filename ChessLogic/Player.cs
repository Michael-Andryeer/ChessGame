// Define um namespace chamado ChessLogic para organizar as classes relacionadas à lógica do jogo de xadrez.
namespace ChessLogic
{
    // Define uma enumeração chamada Player para representar os possíveis jogadores no jogo de xadrez.
    public enum Player
    {
        // Valor que representa nenhum jogador.
        None,
        // Valor que representa o jogador com peças brancas.
        White,
        // Valor que representa o jogador com peças pretas.
        Black
    }

    // Define uma classe estática chamada PlayerExtensions que contém métodos de extensão para a enumeração Player.
    public static class PlayerExtensions
    {
        // Define um método de extensão para a enumeração Player que retorna o oponente do jogador atual.
        public static Player Opponent(this Player player)
        {
            // Usa uma estrutura switch para determinar o oponente do jogador atual.
            switch (player)
            {
                // Se o jogador atual for White (Branco), retorna Black (Preto).
                case Player.White:
                    return Player.Black;
                // Se o jogador atual for Black (Preto), retorna White (Branco).
                case Player.Black:
                    return Player.White;
                // Para qualquer outro valor (inclusive None), retorna None.
                default:
                    return Player.None;
            }
        }
    }
}
