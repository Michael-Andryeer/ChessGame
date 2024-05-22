// Define um namespace chamado ChessLogic para organizar as classes relacionadas à lógica do jogo de xadrez.
namespace ChessLogic
{
    // Define uma classe chamada Direction que representa direções no tabuleiro de xadrez.
    public class Direction
    {
        // Define direções estáticas predefinidas para o tabuleiro de xadrez.
        public readonly static Direction North = new Direction(-1, 0);  // Norte: Move uma linha para cima.
        public readonly static Direction South = new Direction(1, 0);   // Sul: Move uma linha para baixo.
        public readonly static Direction East = new Direction(0, 1);    // Leste: Move uma coluna para a direita.
        public readonly static Direction West = new Direction(0, -1);   // Oeste: Move uma coluna para a esquerda.
        public readonly static Direction NorthEast = North + East;      // Nordeste: Combinação de Norte e Leste.
        public readonly static Direction NorthWest = North + West;      // Noroeste: Combinação de Norte e Oeste.
        public readonly static Direction SouthEast = South + East;      // Sudeste: Combinação de Sul e Leste.
        public readonly static Direction SouthWest = South + West;      // Sudoeste: Combinação de Sul e Oeste.

        // Propriedades para armazenar as mudanças de linha e coluna.
        public int RowDelta { get; }     // Variação de linhas (vertical).
        public int ColumnDelta { get; }  // Variação de colunas (horizontal).

        // Construtor da classe Direction que inicializa as propriedades RowDelta e ColumnDelta.
        public Direction(int rowDelta, int columnDelta)
        {
            RowDelta = rowDelta;
            ColumnDelta = columnDelta;
        }

        // Sobrecarga do operador + para adicionar duas direções.
        public static Direction operator +(Direction dir1, Direction dir2)
        {
            // Retorna uma nova direção resultante da soma das variações de linhas e colunas das direções fornecidas.
            return new Direction(dir1.RowDelta + dir2.RowDelta, dir1.ColumnDelta + dir2.ColumnDelta);
        }

        // Sobrecarga do operador * para multiplicar uma direção por um escalar.
        public static Direction operator *(int scalar, Direction dir)
        {
            // Retorna uma nova direção resultante da multiplicação das variações de linhas e colunas pela constante fornecida.
            return new Direction(scalar * dir.RowDelta, scalar * dir.ColumnDelta);
        }
    }
}
