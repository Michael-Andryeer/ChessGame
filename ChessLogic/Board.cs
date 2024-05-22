// Importa o namespace ChessLogic.Pieces que provavelmente contém as classes das peças de xadrez (Rook, Knight, Bishop, etc.).
using ChessLogic.Pieces;

// Define um namespace chamado ChessLogic para organizar as classes relacionadas à lógica do jogo de xadrez.
namespace ChessLogic
{
    // Define uma classe chamada Board que representa o tabuleiro de xadrez.
    public class Board
    {
        // Declara um campo somente leitura de uma matriz 8x8 de peças (Piece), representando o tabuleiro de xadrez.
        private readonly Piece[,] pieces = new Piece[8, 8];

        // Define um indexador para acessar e modificar as peças na matriz usando coordenadas de linha e coluna.
        public Piece this[int row, int col]
        {
            // Obtém a peça na posição especificada.
            get { return pieces[row, col]; }
            // Define a peça na posição especificada.
            set { pieces[row, col] = value; }
        }

        // Define um indexador para acessar e modificar as peças na matriz usando um objeto Position.
        public Piece this[Position pos]
        {
            // Obtém a peça na posição especificada por um objeto Position.
            get { return this[pos.Row, pos.Column]; }
            // Define a peça na posição especificada por um objeto Position.
            set { this[pos.Row, pos.Column] = value; }
        }

        // Método estático que cria e inicializa um novo tabuleiro de xadrez com as peças na posição inicial.
        public static Board Initial()
        {
            // Cria uma nova instância de Board.
            Board board = new Board();
            // Adiciona as peças iniciais ao tabuleiro.
            board.AddStartPieces();
            // Retorna o tabuleiro inicializado.
            return board;
        }

        // Método privado que adiciona as peças iniciais ao tabuleiro.
        private void AddStartPieces()
        {
            // Adiciona as peças pretas na primeira linha do tabuleiro.
            this[0, 0] = new Rook(Player.Black);
            this[0, 1] = new Knight(Player.Black);
            this[0, 2] = new Bishop(Player.Black);
            this[0, 3] = new Queen(Player.Black);
            this[0, 4] = new King(Player.Black);
            this[0, 5] = new Bishop(Player.Black);
            this[0, 6] = new Knight(Player.Black);
            this[0, 7] = new Rook(Player.Black);

            // Adiciona as peças brancas na última linha do tabuleiro.
            this[7, 0] = new Rook(Player.White);
            this[7, 1] = new Knight(Player.White);
            this[7, 2] = new Bishop(Player.White);
            this[7, 3] = new Queen(Player.White);
            this[7, 4] = new King(Player.White);
            this[7, 5] = new Bishop(Player.White);
            this[7, 6] = new Knight(Player.White);
            this[7, 7] = new Rook(Player.White);

            // Adiciona os peões pretos na segunda linha do tabuleiro.
            for (int i = 0; i < 8; i++)
            {
                this[1, i] = new Pawn(Player.Black);
                // Adiciona os peões brancos na penúltima linha do tabuleiro.
                this[6, i] = new Pawn(Player.White);
            }
        }

        // Método estático que verifica se uma posição está dentro dos limites do tabuleiro.
        public static bool IsInside(Position pos)
        {
            // Retorna verdadeiro se a posição estiver dentro dos limites (0-7 para linhas e colunas).
            return pos.Row >= 0 && pos.Row < 8 && pos.Column >= 0 && pos.Column < 8;
        }

        // Método que verifica se uma posição está vazia (sem peça).
        public bool IsEmpty(Position pos)
        {
            // Retorna verdadeiro se a posição especificada não contiver uma peça.
            return this[pos] == null;
        }
    }
}
