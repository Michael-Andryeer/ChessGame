
// Define um namespace chamado ChessLogic para organizar as classes relacionadas à lógica do jogo de xadrez.
namespace ChessLogic
{
    // Define uma classe chamada NormalMove que herda da classe Move e representa um movimento normal no xadrez.
    public class NormalMove : Move
    {
        // Propriedade que sobrescreve a propriedade Type da classe base e retorna o tipo de movimento como Normal.
        public override MoveType Type => MoveType.Normal;

        // Propriedade somente leitura que sobrescreve a propriedade FromPos da classe base e representa a posição de origem da peça.
        public override Position FromPos { get; }

        // Propriedade somente leitura que sobrescreve a propriedade ToPos da classe base e representa a posição de destino da peça.
        public override Position ToPos { get; }

        // Construtor da classe NormalMove que inicializa as propriedades FromPos e ToPos com as posições fornecidas.
        public NormalMove(Position from, Position to)
        {
            FromPos = from;
            ToPos = to;
        }

        // Método sobrescrito que executa o movimento no tabuleiro.
        public override void Execute(Board board)
        {
            // Obtém a peça na posição de origem no tabuleiro.
            Piece piece = board[FromPos];

            // Coloca a peça na posição de destino no tabuleiro.
            board[ToPos] = piece;

            // Remove a peça da posição de origem no tabuleiro.
            board[FromPos] = null;

            // Marca a peça como tendo se movido.
            piece.HasMoved = true;
        }
    }
}
