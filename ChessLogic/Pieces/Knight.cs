// Define o namespace ChessLogic, que contém todas as classes relacionadas à lógica do jogo de xadrez
using System.Security.Cryptography.X509Certificates;

namespace ChessLogic
{
    // Define a classe Knight que herda da classe base Piece
    public class Knight : Piece
    {
        // Sobrescreve a propriedade Type da classe base para retornar o tipo da peça como Knight
        public override PieceType Type => PieceType.Knight;

        // Sobrescreve a propriedade Color da classe base para retornar a cor do jogador
        // Esta propriedade é apenas para leitura (somente get)
        public override Player Color { get; }

        // Construtor da classe Knight que inicializa a propriedade Color
        // Parâmetro: color - a cor do jogador (branco ou preto)
        public Knight(Player color)
        {
            Color = color;  // Inicializa a propriedade Color com o valor fornecido
        }

        // Sobrescreve o método Copy da classe base para criar uma cópia da peça Knight
        // Retorna: uma nova instância de Knight com a mesma cor e estado de movimento
        public override Piece Copy()
        {
            // Cria uma nova instância de Knight com a mesma cor
            Knight copy = new Knight(Color);

            // Copia o estado de HasMoved da peça original para a cópia
            copy.HasMoved = HasMoved;

            // Retorna a cópia da peça
            return copy;
        }

        // Gera as posições potenciais para onde o cavalo pode se mover a partir de uma posição específica
        private static IEnumerable<Position> PotentialToPositions(Position from)
        {
            // Loop através das direções vertical e horizontal para gerar todas as posições potenciais
            foreach (Direction vDir in new Direction[] { Direction.North, Direction.South })
            {
                foreach (Direction hDir in new Direction[] { Direction.West, Direction.East })
                {
                    // Adiciona as posições potenciais para onde o cavalo pode se mover
                    yield return from + 2 * vDir + hDir;
                    yield return from + 2 * hDir + vDir;
                }
            }
        }

        // Obtém as posições válidas para onde o cavalo pode se mover a partir de uma posição específica no tabuleiro
        private IEnumerable<Position> MovePositions(Position from, Board board)
        {
            // Filtra as posições potenciais para obter apenas as posições válidas
            return PotentialToPositions(from).Where(pos => Board.IsInside(pos)
            && (board.IsEmpty(pos) || board[pos].Color != Color));
        }

        // Obtém todos os movimentos possíveis para o cavalo a partir de uma posição específica no tabuleiro
        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            // Retorna os movimentos possíveis do cavalo como movimentos normais (NormalMove)
            return MovePositions(from, board).Select(to => new NormalMove(from, to));
        }
    }
}
