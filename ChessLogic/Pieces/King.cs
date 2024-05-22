// Define a classe King que herda da classe base Piece
using ChessLogic;

public class King : Piece
{
    // Sobrescreve a propriedade Type da classe base para retornar o tipo da peça como King
    public override PieceType Type => PieceType.King;

    // Sobrescreve a propriedade Color da classe base para retornar a cor do jogador
    // Esta propriedade é apenas para leitura (somente get)
    public override Player Color { get; }

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
}
