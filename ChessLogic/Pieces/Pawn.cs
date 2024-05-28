// Define o namespace ChessLogic, que contém todas as classes relacionadas à lógica do jogo de xadrez
using ChessLogic.Moves;
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
            // Verifica se a posição está dentro dos limites do tabuleiro e se está vazia
            return Board.IsInside(pos) && board.IsEmpty(pos);
        }

        // Verifica se uma posição pode ser capturada por um peão
        private bool CanCaptureAt(Position pos, Board board)
        {
            // Verifica se a posição está fora dos limites do tabuleiro ou se está vazia
            if (!Board.IsInside(pos) || board.IsEmpty(pos))
            {
                return false;
            }

            // Retorna verdadeiro se a peça na posição pode ser capturada (cor diferente)
            return board[pos].Color != Color;
        }

        // Gera movimentos de promoção para um peão que atinge o final do tabuleiro
        private static IEnumerable<Move> PromotionMoves(Position from, Position to)
        {
            yield return new PawnPromotion(from, to, PieceType.Knight);
            yield return new PawnPromotion(from, to, PieceType.Bishop);
            yield return new PawnPromotion(from, to, PieceType.Rook);
            yield return new PawnPromotion(from, to, PieceType.Queen);
        }

        // Obtém todos os movimentos possíveis para um peão na direção de avanço
        private IEnumerable<Move> ForwardMoves(Position from, Board board)
        {
            // Calcula a posição de um movimento para frente
            Position oneMovePos = from + forward;

            // Verifica se o peão pode mover-se para a posição calculada
            if (CanMoveTo(oneMovePos, board))
            {
                // Verifica se a posição alcançada é uma posição de promoção
                if (oneMovePos.Row == 0 || oneMovePos.Row == 7)
                {
                    // Gera movimentos de promoção
                    foreach (Move promMove in PromotionMoves(from, oneMovePos))
                    {
                        yield return promMove;
                    }
                }
                else
                {
                    // Retorna um movimento normal para a posição calculada
                    yield return new NormalMove(from, oneMovePos);

                    // Calcula a posição de dois movimentos para frente se o peão não tiver se movido
                    Position twoMovesPos = oneMovePos + forward;

                    // Verifica se o peão pode mover-se duas posições para frente
                    if (!HasMoved && CanMoveTo(twoMovesPos, board))
                    {
                        yield return new DoublePawn(from, twoMovesPos);
                    }
                }
            }
        }

        // Obtém todos os movimentos possíveis para um peão nas diagonais
        private IEnumerable<Move> Diagonalmoves(Position from, Board board)
        {
            // Itera sobre as direções leste e oeste para capturas diagonais
            foreach (Direction dir in new Direction[] { Direction.West, Direction.East })
            {
                // Calcula a posição de captura diagonal
                Position to = from + forward + dir;
                // Verifica se a posição de destino é igual à posição de peão pulada pelo oponente
                if (to == board.GetPawnSkipPosition(Color.Opponent()))
                {
                    // Se for, retorna um movimento EnPassant (captura em passagem) como uma possível jogada
                    yield return new EnPassant(from, to);
                }

                // Verifica se o peão pode capturar na posição calculada
                else if (CanCaptureAt(to, board))
                {
                    // Verifica se a posição alcançada é uma posição de promoção
                    if (to.Row == 0 || to.Row == 7)
                    {
                        // Gera movimentos de promoção
                        foreach (Move promMove in PromotionMoves(from, to))
                        {
                            yield return promMove;
                        }
                    }
                    else
                    {
                        // Retorna um movimento de captura normal
                        yield return new NormalMove(from, to);
                    }
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
            return Diagonalmoves(from, board).Any(move =>
            {
                // Obtém a peça na posição de destino do movimento atual
                Piece piece = board[move.ToPos];

                // Retorna verdadeiro se a peça não for nula e se o tipo da peça for Rei
                return piece != null && piece.Type == PieceType.King;
            });
        }
    }
}
