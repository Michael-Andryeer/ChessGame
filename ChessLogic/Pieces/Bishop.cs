// Importa o namespace System.Collections.Generic, que contém interfaces e classes definidas para coleções genéricas
using System.Collections.Generic;

namespace ChessLogic
{
    // Define a classe Bishop que herda da classe base Piece
    public class Bishop : Piece
    {
        // Sobrescreve a propriedade Type da classe base para retornar o tipo da peça como Bishop
        public override PieceType Type => PieceType.Bishop;

        // Sobrescreve a propriedade Color da classe base para retornar a cor do jogador
        // Esta propriedade é apenas para leitura (somente get)
        public override Player Color { get; }

        // Define um array de direções possíveis para o movimento do bispo
        private static readonly Direction[] dirs = new Direction[]
        {
            Direction.NorthEast, // Movimento na direção nordeste
            Direction.NorthWest, // Movimento na direção noroeste
            Direction.SouthEast, // Movimento na direção sudeste
            Direction.SouthWest  // Movimento na direção sudoeste
        };

        // Construtor da classe Bishop que inicializa a propriedade Color
        // Parâmetro: color - a cor do jogador (branco ou preto)
        public Bishop(Player color)
        {
            Color = color;  // Inicializa a propriedade Color com o valor fornecido
        }

        // Sobrescreve o método Copy da classe base para criar uma cópia da peça Bishop
        // Retorna: uma nova instância de Bishop com a mesma cor e estado de movimento
        public override Piece Copy()
        {
            // Cria uma nova instância de Bishop com a mesma cor
            Bishop copy = new Bishop(Color);

            // Copia o estado de HasMoved da peça original para a cópia
            copy.HasMoved = HasMoved;

            // Retorna a cópia da peça
            return copy;
        }

        // Sobrescreve o método GetMoves da classe base para obter todos os movimentos possíveis para o bispo a partir de uma posição específica
        // Parâmetros:
        // from - a posição atual do bispo
        // board - o tabuleiro atual do jogo
        // Retorna: uma coleção de movimentos possíveis
        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            // Usa o método MovePositionsInDirs para obter todas as posições possíveis para o bispo nas direções definidas
            // Transforma essas posições em movimentos normais (NormalMove)
            return MovePositionsInDirs(from, board, dirs).Select(to => new NormalMove(from, to));
        }
    }
}
