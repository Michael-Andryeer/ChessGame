// Define um namespace chamado ChessLogic para organizar as classes relacionadas à lógica do jogo de xadrez.
namespace ChessLogic
{
    // Define uma classe chamada GameState que representa o estado atual de um jogo de xadrez.
    public class GameState
    {
        // Propriedade somente leitura que representa o tabuleiro de xadrez.
        public Board Board { get; }

        // Propriedade que representa o jogador atual. Pode ser alterada apenas dentro desta classe.
        public Player CurrentPlayer { get; private set; }

        // Construtor da classe GameState que inicializa o tabuleiro e o jogador atual.
        public GameState(Board board, Player player)
        {
            // Inicializa a propriedade Board com o tabuleiro fornecido.
            Board = board;

            // Inicializa a propriedade CurrentPlayer com o jogador fornecido.
            CurrentPlayer = player;
        }
    }
}
