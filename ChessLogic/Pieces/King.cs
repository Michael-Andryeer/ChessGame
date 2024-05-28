// Define a classe King que herda da classe base Piece
using ChessLogic;
using System.Collections.Generic;

public class King : Piece
{
    // Sobrescreve a propriedade Type da classe base para retornar o tipo da peça como King
    public override PieceType Type => PieceType.King;

    // Sobrescreve a propriedade Color da classe base para retornar a cor do jogador
    // Esta propriedade é apenas para leitura (somente get)
    public override Player Color { get; }

    // Define as direções possíveis para o movimento do rei
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


    // Construtor da classe King que inicializa a propriedade Color
    // Parâmetro: color - a cor do jogador (branco ou preto)
    public King(Player color)
    {
        Color = color;  // Inicializa a propriedade Color com o valor fornecido
    }

    // Método privado que verifica se a torre na posição especificada ainda não se moveu
    private static bool IsUnmovedRook(Position pos, Board board)
    {
        if (board.IsEmpty(pos))
        {
            return false;
        }

        Piece piece = board[pos];
        return piece.Type == PieceType.Rook && !piece.HasMoved;
    }

    // Método privado que verifica se todas as posições especificadas estão vazias no tabuleiro
    private static bool AllEmpty(IEnumerable<Position> positions, Board board)
    {
        return positions.All(pos => board.IsEmpty(pos));
    }

    // Método privado que verifica se o roque do lado do rei é possível
    private bool CanCastleKingSide(Position from, Board board)
    {
        if (HasMoved)
        {
            return false;
        }

        Position rookPos = new Position(from.Row, 7);
        Position[] betweenPositions = new Position[] { new(from.Row, 5), new(from.Row, 6) };

        return IsUnmovedRook(rookPos, board) && AllEmpty(betweenPositions, board);
    }

    // Método privado que verifica se o roque do lado da rainha é possível
    private bool CanCastleQueenSide(Position from, Board board)
    {
        if (HasMoved)
        {
            return false;
        }

        Position rookPos = new Position(from.Row, 0);
        Position[] betweenPositions = new Position[] { new(from.Row, 1), new(from.Row, 3) };

        return IsUnmovedRook(rookPos, board) && AllEmpty(betweenPositions, board);
    }

    // Sobrescreve o método Copy da classe base para criar uma cópia da peça King
    // Retorna: uma nova instância de King com a mesma cor e estado de movimento
    public override Piece Copy()
    {
        // Cria uma nova instância de King com a mesma cor
        King copy = new King(Color);

        // Copia o estado de HasMoved da peça original para a cópia
        copy.HasMoved = HasMoved;

        // Retorna a cópia da peça
        return copy;
    }

    // Obtém as posições válidas para onde o rei pode se mover a partir de uma posição específica no tabuleiro
    private IEnumerable<Position> MovePositions(Position from, Board board)
    {
        // Loop através das direções possíveis para o movimento do rei
        foreach (Direction dir in dirs)
        {
            Position to = from + dir;

            // Verifica se a posição de destino está dentro dos limites do tabuleiro
            if (!Board.IsInside(to))
            {
                continue; // Passa para a próxima iteração se estiver fora do tabuleiro
            }

            // Verifica se a posição de destino está vazia ou contém uma peça de cor diferente
            if (board.IsEmpty(to) || board[to].Color != Color)
            {
                yield return to; // Retorna a posição como válida para o movimento
            }
        }
    }

    // Obtém todos os movimentos possíveis para o rei a partir de uma posição específica no tabuleiro
    public override IEnumerable<Move> GetMoves(Position from, Board board)
    {
        // Retorna os movimentos possíveis do rei como movimentos normais (NormalMove)
        foreach (Position to in MovePositions(from, board))
        {
            yield return new NormalMove(from, to);
        }

        // Verifica se o roque do lado do rei é possível e adiciona-o aos movimentos possíveis
        if (CanCastleKingSide(from, board))
        {
            yield return new Castle(MoveType.CastleKS, from);
        }

        // Verifica se o roque do lado da rainha é possível e adiciona-o aos movimentos possíveis
        if (CanCastleQueenSide(from, board))
        {
            yield return new Castle(MoveType.CastleQS, from);
        }
    }

    // Método público sobrescrito que verifica se é possível capturar o rei adversário a partir de uma posição dada no tabuleiro
    public override bool CanCaptureOpponentKing(Position from, Board board)
    {
        // Obtém todas as posições possíveis de movimento a partir da posição fornecida e verifica se algum desses movimentos resulta na captura do rei adversário
        return MovePositions(from, board).Any(to => // Verifica se existe alguma posição de movimento na lista de posições possíveis que atenda à condição especificada no bloco de código
        {
            // Obtém a peça na posição de destino do movimento atual
            Piece piece = board[to];

            // Retorna verdadeiro se a peça não for nula e se o tipo da peça for Rei
            return piece != null && piece.Type == PieceType.King;
        });
    }

}
