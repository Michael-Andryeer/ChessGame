// Define o namespace ChessLogic.Moves para organizar as classes de movimentos relacionadas à lógica do jogo de xadrez.
namespace ChessLogic.Moves
{
    // Define a classe DoublePawn que herda da classe base Move.
    public class DoublePawn : Move
    {
        // Sobrescreve a propriedade Type da classe base para retornar o tipo de movimento como DoublePawn.
        public override MoveType Type => MoveType.DoublePawn;

        // Sobrescreve a propriedade FromPos da classe base para retornar a posição de origem do movimento.
        public override Position FromPos { get; }

        // Sobrescreve a propriedade ToPos da classe base para retornar a posição de destino do movimento.
        public override Position ToPos { get; }

        // Declara uma variável somente leitura para armazenar a posição intermediária (pulada) pelo peão.
        private readonly Position skippedPos;

        // Construtor da classe DoublePawn que inicializa as propriedades FromPos, ToPos e skippedPos.
        public DoublePawn(Position from, Position to)
        {
            FromPos = from;  // Inicializa a propriedade FromPos com a posição de origem fornecida.
            ToPos = to;      // Inicializa a propriedade ToPos com a posição de destino fornecida.
            // Calcula e inicializa a propriedade skippedPos com a posição intermediária pulada pelo peão.
            skippedPos = new Position((from.Row + to.Row) / 2, from.Column);
        }

        // Método sobrescrito Execute que executa o movimento de peão duplo no tabuleiro.
        public override void Execute(Board board)
        {
            // Obtém a cor do jogador a partir da peça na posição de origem.
            Player player = board[FromPos].Color;
            // Define a posição de pulada do peão no tabuleiro para o jogador atual.
            board.SetPawnSkipPosition(player, skippedPos);
            // Executa o movimento normal da posição de origem para a posição de destino.
            new NormalMove(FromPos, ToPos).Execute(board);
        }
    }
}
