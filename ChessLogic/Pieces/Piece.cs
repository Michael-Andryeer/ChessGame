// Define um namespace chamado ChessLogic para organizar as classes relacionadas à lógica do jogo de xadrez.
namespace ChessLogic
{
    // Define uma classe abstrata chamada Piece que serve como base para diferentes tipos de peças no xadrez.
    public abstract class Piece
    {
        // Propriedade abstrata que deve ser implementada nas classes derivadas para indicar o tipo da peça.
        public abstract PieceType Type { get; }

        // Propriedade abstrata que deve ser implementada nas classes derivadas para representar a cor do jogador dono da peça.
        public abstract Player Color { get; }

        // Propriedade que indica se a peça já foi movida. Pode ser lida e modificada.
        public bool HasMoved { get; set; } = false;

        // Método abstrato que deve ser implementado nas classes derivadas para criar uma cópia da peça.
        public abstract Piece Copy();

        // Método abstrato que deve ser implementado nas classes derivadas para obter os movimentos possíveis da peça a partir de uma posição no tabuleiro.
        public abstract IEnumerable<Move> GetMoves(Position from, Board board);

        // Método protegido que retorna as posições para onde a peça pode se mover em uma direção específica.
        protected IEnumerable<Position> MovePositionsInDir(Position from, Board board, Direction dir)
        {
            // Itera sobre as posições na direção especificada.
            for (Position pos = from + dir; Board.IsInside(pos); pos += dir)
            {
                // Se a posição estiver vazia, a peça pode se mover para lá.
                if (board.IsEmpty(pos))
                {
                    yield return pos;
                    continue;
                }

                // Se a posição não estiver vazia, verifica se a peça na posição é do adversário.
                Piece piece = board[pos];

                if (piece.Color != Color)
                {
                    // Se for uma peça do adversário, a peça pode se mover para lá.
                    yield return pos;
                }

                // Interrompe a iteração, pois a peça não pode pular sobre outras peças.
                yield break;
            }
        }

        // Método protegido que retorna as posições para onde a peça pode se mover em várias direções.
        protected IEnumerable<Position> MovePositionsInDirs(Position from, Board board, Direction[] dirs)
        {
            // Usa LINQ para chamar MovePositionsInDir para cada direção e concatenar os resultados.
            return dirs.SelectMany(dir => MovePositionsInDir(from, board, dir));
        }

        // Método público e virtual que verifica se é possível capturar o rei adversário a partir de uma posição dada no tabuleiro
        public virtual bool CanCaptureOpponentKing(Position from, Board board)
        {
            // Obtém todos os movimentos possíveis a partir da posição fornecida e verifica se algum desses movimentos resulta na captura do rei adversário
            return GetMoves(from, board).Any(move => // Verifica se existe algum movimento na lista de movimentos possíveis que atenda a condição especificada no bloco de código
            {
                // Obtém a peça na posição de destino do movimento atual
                Piece piece = board[move.ToPos];

                // Retorna verdadeiro se a peça não for nula e se o tipo da peça for Rei
                return piece != null && piece.Type == PieceType.King;
            });
        }

    }
}
