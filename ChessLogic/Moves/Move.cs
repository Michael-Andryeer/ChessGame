
// Define um namespace chamado ChessLogic para organizar as classes relacionadas à lógica do jogo de xadrez.
namespace ChessLogic
{
    // Define uma classe abstrata chamada Move que serve como base para diferentes tipos de movimentos no xadrez.
    public abstract class Move
    {
        // Propriedade abstrata que deve ser implementada nas classes derivadas para indicar o tipo de movimento.
        public abstract MoveType Type { get; }

        // Propriedade abstrata que deve ser implementada nas classes derivadas para representar a posição de origem do movimento.
        public abstract Position FromPos { get; }

        // Propriedade abstrata que deve ser implementada nas classes derivadas para representar a posição de destino do movimento.
        public abstract Position ToPos { get; }

        // Método abstrato que deve ser implementado nas classes derivadas para executar o movimento no tabuleiro.
        public abstract bool Execute(Board board);

        // Método público e virtual que verifica se um movimento é legal em um tabuleiro fornecido
        public virtual bool IsLegal(Board board)
        {
            // Obtém o jogador (cor da peça) na posição de origem do movimento
            Player player = board[FromPos].Color;

            // Cria uma cópia do tabuleiro para simular o movimento
            Board boardCopy = board.Copy();

            // Executa o movimento no tabuleiro copiado
            Execute(boardCopy);

            // Verifica se o jogador que fez o movimento não está em xeque após o movimento
            return !boardCopy.IsInCheck(player);
        }

    }
}
