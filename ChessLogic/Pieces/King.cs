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
