// Define o namespace ChessLogic.Pieces, que provavelmente contém classes específicas para as peças do jogo de xadrez
namespace ChessLogic.Pieces
{
    // Define a classe Rook que herda da classe base Piece
    public class Rook : Piece
    {
        // Sobrescreve a propriedade Type da classe base para retornar o tipo da peça como Rook
        public override PieceType Type => PieceType.Rook;

        // Sobrescreve a propriedade Color da classe base para retornar a cor do jogador
        // Esta propriedade é apenas para leitura (somente get)
        public override Player Color { get; }

        // Construtor da classe Rook que inicializa a propriedade Color
        // Parâmetro: color - a cor do jogador (branco ou preto)
        public Rook(Player color)
        {
            Color = color;  // Inicializa a propriedade Color com o valor fornecido
        }

        // Sobrescreve o método Copy da classe base para criar uma cópia da peça Rook
        // Retorna: uma nova instância de Rook com a mesma cor e estado de movimento
        public override Piece Copy()
        {
            // Cria uma nova instância de Rook com a mesma cor
            Rook copy = new Rook(Color);

            // Copia o estado de HasMoved da peça original para a cópia
            copy.HasMoved = HasMoved;

            // Retorna a cópia da peça
            return copy;
        }
    }
}
