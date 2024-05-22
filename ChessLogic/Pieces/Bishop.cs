
// Define o namespace ChessLogic, que provavelmente contém todas as classes relacionadas à lógica do jogo de xadrez
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
    }
}
