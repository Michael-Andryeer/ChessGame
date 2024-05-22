// Define o namespace ChessLogic, que contém todas as classes relacionadas à lógica do jogo de xadrez
namespace ChessLogic
{
    // Define a classe Queen que herda da classe base Piece
    public class Queen : Piece
    {
        // Sobrescreve a propriedade Type da classe base para retornar o tipo da peça como Queen
        public override PieceType Type => PieceType.Queen;

        // Sobrescreve a propriedade Color da classe base para retornar a cor do jogador
        // Esta propriedade é apenas para leitura (somente get)
        public override Player Color { get; }

        // Define um array de direções possíveis para o movimento da Rainha
        private static readonly Direction[] dirs = new Direction[]
        {
            Direction.North,        // Movimento na direção norte
            Direction.South,        // Movimento na direção sul
            Direction.East,         // Movimento na direção leste
            Direction.West,         // Movimento na direção oeste
            Direction.NorthEast,    // Movimento na direção nordeste
            Direction.NorthWest,    // Movimento na direção noroeste
            Direction.SouthEast,    // Movimento na direção sudeste
            Direction.SouthWest     // Movimento na direção sudoeste
        };

        // Construtor da classe Queen que inicializa a propriedade Color
        // Parâmetro: color - a cor do jogador (branco ou preto)
        public Queen(Player color)
        {
            Color = color;  // Inicializa a propriedade Color com o valor fornecido
        }

        // Sobrescreve o método Copy da classe base para criar uma cópia da peça Queen
        // Retorna: uma nova instância de Queen com a mesma cor e estado de movimento
        public override Piece Copy()
        {
            // Cria uma nova instância de Queen com a mesma cor
            Queen copy = new Queen(Color);

            // Copia o estado de HasMoved da peça original para a cópia
            copy.HasMoved = HasMoved;

            // Retorna a cópia da peça
            return copy;
        }

        // Sobrescreve o método GetMoves da classe base para obter todos os movimentos possíveis para a Rainha a partir de uma posição específica
        // Parâmetros:
        // from - a posição atual da Rainha
        // board - o tabuleiro atual do jogo
        // Retorna: uma coleção de movimentos possíveis
        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            // Usa o método MovePositionsInDirs para obter todas as posições possíveis para a Rainha nas direções definidas
            // Transforma essas posições em movimentos normais (NormalMove)
            return MovePositionsInDirs(from, board, dirs).Select(to => new NormalMove(from, to));
        }
    }
}
