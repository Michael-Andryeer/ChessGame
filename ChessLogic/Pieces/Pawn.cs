
// Define o namespace ChessLogic, que contém todas as classes relacionadas à lógica do jogo de xadrez
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

        // Construtor da classe Pawn que inicializa a propriedade Color
        // Parâmetro: color - a cor do jogador (branco ou preto)
        public Pawn(Player color)
        {
            Color = color;  // Inicializa a propriedade Color com o valor fornecido
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
    }
}
