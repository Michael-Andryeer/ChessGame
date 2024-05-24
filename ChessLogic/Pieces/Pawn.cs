// Define o namespace ChessLogic, que contém todas as classes relacionadas à lógica do jogo de xadrez
using System.Security.Cryptography.X509Certificates;

namespace ChessLogic
{
    // Define a classe Pawn que herda da classe base Piece
    public class Pawn : Piece
    {
        // Sobrescreve a propriedade Type da classe base para retornar o tipo da peça como Pawn
        public override PieceType Type => PieceType.Pawn;

        // Sobrescreve a propriedade Color da classe base para retornar a cor do jogador
        // Esta propriedade é apenas para leitura (somente get)
        public override Player Color { get; }

        // Define a direção em que o peão avança com base na cor do jogador
        private readonly Direction forward;

        // Construtor da classe Pawn que inicializa a propriedade Color
        // Parâmetro: color - a cor do jogador (branco ou preto)
        public Pawn(Player color)
        {
            Color = color;  // Inicializa a propriedade Color com o valor fornecido

            // Define a direção em que o peão avança com base na cor do jogador
            if (color == Player.White)
            {
                forward = Direction.North;
            }
            else if (color == Player.Black)
            {
                forward = Direction.South;
            }
        }

        // Sobrescreve o método Copy da classe base para criar uma cópia da peça Pawn
        // Retorna: uma nova instância de Pawn com a mesma cor e estado de movimento
        public override Piece Copy()
        {
            // Cria uma nova instância de Pawn com a mesma cor
            Pawn copy = new Pawn(Color);

            // Copia o estado de HasMoved da peça original para a cópia
            copy.HasMoved = HasMoved;

            // Retorna a cópia da peça
            return copy;
        }

        // Verifica se uma posição é válida para o movimento de um peão
        private static bool CanMoveTo(Position pos, Board board)
        {
            return Board.IsInside(pos) && board.IsEmpty(pos);
        }

        // Verifica se uma posição pode ser capturada por um peão
        private bool CanCaptureAt(Position pos, Board board)
        {
            if (!Board.IsInside(pos) || board.IsEmpty(pos))
            {
                return false;
            }

            return board[pos].Color != Color;
        }

        // Obtém todos os movimentos possíveis para um peão na direção de avanço
        private IEnumerable<Move> ForwardMoves(Position from, Board board)
        {
            Position oneMovePos = from + forward;

            if (CanMoveTo(oneMovePos, board))
            {
                yield return new NormalMove(from, oneMovePos);

                Position twoMovesPos = oneMovePos + forward;

                if (!HasMoved && CanMoveTo(twoMovesPos, board))
                {
                    yield return new NormalMove(from, twoMovesPos);
                }
            }
        }

        // Obtém todos os movimentos possíveis para um peão nas diagonais
        private IEnumerable<Move> Diagonalmoves(Position from, Board board)
        {
            foreach (Direction dir in new Direction[] { Direction.West, Direction.East })
            {
                Position to = from + forward + dir;

                if (CanCaptureAt(to, board))
                {
                    yield return new NormalMove(from, to);
                }
            }
        }

        // Obtém todos os movimentos possíveis para um peão a partir de uma posição específica no tabuleiro
        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            // Retorna a concatenação dos movimentos possíveis na direção de avanço e nas diagonais
            return ForwardMoves(from, board).Concat(Diagonalmoves(from, board));
        }

        // Método público e virtual que verifica se é possível capturar o rei adversário a partir de uma posição dada no tabuleiro
        public override bool CanCaptureOpponentKing(Position from, Board board)
        {
            // Obtém todos os movimentos na diagonal possíveis a partir da posição fornecida e verifica se algum desses movimentos resulta na captura do rei adversário
            return Diagonalmoves(from, board).Any(move => // Verifica se existe algum movimento na lista de movimentos possíveis que atenda a condição especificada no bloco de código
            {
                // Obtém a peça na posição de destino do movimento atual
                Piece piece = board[move.ToPos];

                // Retorna verdadeiro se a peça não for nula e se o tipo da peça for Rei
                return piece != null && piece.Type == PieceType.King;
            });
        }

    }
}
