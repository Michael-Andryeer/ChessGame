
// Define um namespace chamado ChessLogic para organizar as classes relacionadas à lógica do jogo de xadrez.
namespace ChessLogic
{
    // Define uma classe chamada Position que representa uma posição no tabuleiro de xadrez.
    public class Position
    {
        // Propriedades somente leitura que representam a linha e a coluna de uma posição no tabuleiro.
        public int Row { get; }
        public int Column { get; }

        // Construtor da classe Position que inicializa as propriedades Row e Column com os valores fornecidos.
        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }

        // Método que determina a cor da casa do tabuleiro de xadrez com base na posição.
        public Player SquareColor()
        {
            // Se a soma de Row e Column for par, a cor é branca; caso contrário, é preta.
            if ((Row + Column) % 2 == 0)
            {
                return Player.White;
            }
            return Player.Black;
        }

        // Sobrescreve o método Equals para comparar duas instâncias da classe Position.
        public override bool Equals(object obj)
        {
            // Verifica se o objeto fornecido é uma instância de Position e se tem a mesma linha e coluna.
            return obj is Position position &&
                   Row == position.Row &&
                   Column == position.Column;
        }

        // Sobrescreve o método GetHashCode para fornecer um código hash para uma instância de Position.
        public override int GetHashCode()
        {
            // Combina as propriedades Row e Column para gerar um código hash único.
            return HashCode.Combine(Row, Column);
        }

        // Sobrecarga do operador == para comparar duas instâncias de Position.
        public static bool operator ==(Position left, Position right)
        {
            // Usa o EqualityComparer padrão para verificar a igualdade entre as duas instâncias.
            return EqualityComparer<Position>.Default.Equals(left, right);
        }

        // Sobrecarga do operador != para verificar se duas instâncias de Position são diferentes.
        public static bool operator !=(Position left, Position right)
        {
            // Retorna o oposto da comparação de igualdade.
            return !(left == right);
        }

        // Sobrecarga do operador + para adicionar uma direção a uma posição.
        public static Position operator +(Position pos, Direction dir)
        {
            // Retorna uma nova Position resultante da soma das variações de linha e coluna da direção fornecida.
            return new Position(pos.Row + dir.RowDelta, pos.Column + dir.ColumnDelta);
        }
    }
}
