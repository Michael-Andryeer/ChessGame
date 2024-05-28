// Define o namespace ChessLogic para organizar as classes relacionadas à lógica do jogo de xadrez.
namespace ChessLogic
{
    // Define a classe EnPassant que herda da classe base Move.
    public class EnPassant : Move
    {
        // Sobrescreve a propriedade Type da classe base para retornar o tipo de movimento como EnPassant.
        public override MoveType Type => MoveType.EnPassant;

        // Sobrescreve a propriedade FromPos da classe base para retornar a posição de origem do movimento.
        public override Position FromPos { get; }

        // Sobrescreve a propriedade ToPos da classe base para retornar a posição de destino do movimento.
        public override Position ToPos { get; }

        // Declara uma variável somente leitura para armazenar a posição da peça capturada.
        private readonly Position capturePos;

        // Construtor da classe EnPassant que inicializa as propriedades FromPos, ToPos e capturePos.
        public EnPassant(Position from, Position to)
        {
            FromPos = from;  // Inicializa a propriedade FromPos com a posição de origem fornecida.
            ToPos = to;      // Inicializa a propriedade ToPos com a posição de destino fornecida.
            // Inicializa a propriedade capturePos com a posição da peça capturada (mesma linha de origem, coluna de destino).
            capturePos = new Position(from.Row, to.Column);
        }

        // Método sobrescrito Execute que executa o movimento En Passant no tabuleiro.
        public override void Execute(Board board)
        {
            // Executa o movimento normal da posição de origem para a posição de destino.
            new NormalMove(FromPos, ToPos).Execute(board);
            // Remove a peça capturada do tabuleiro, definindo a posição de captura como null.
            board[capturePos] = null;
        }
    }
}
