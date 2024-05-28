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

        private readonly Dictionary<Player, Position> pawnSkipPositions = new Dictionary<Player, Position>
        {
            {Player.White,null },
            {Player.Black,null }

        };

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

        public Position GetPawnSkipPosition(Player player)
        {
            return pawnSkipPositions[player];
        }

        public void SetPawnSkipPosition(Player player,Position pos)
        {
            pawnSkipPositions[player] = pos;
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

        // Método público que retorna um IEnumerable de posições ocupadas no tabuleiro
        public IEnumerable<Position> PiecePositions()
        {
            // Loop externo para iterar sobre as linhas do tabuleiro (0 a 7)
            for (int r = 0; r < 8; r++)
            {
                // Loop interno para iterar sobre as colunas do tabuleiro (0 a 7)
                for (int c = 0; c < 8; c++)
                {
                    // Cria um objeto Position representando a posição atual no tabuleiro
                    Position pos = new Position(r, c);

                    // Verifica se a posição não está vazia (ou seja, contém uma peça)
                    if (!IsEmpty(pos))
                    {
                        // Usa yield para retornar a posição atual ao chamador do método
                        yield return pos; //  usado para retornar elementos de uma coleção um a um, sem a necessidade de criar uma coleção intermediária completa na memória. 
                    }
                }
            }
        }

        // Método público que retorna um IEnumerable de posições ocupadas por peças de um jogador específico
        public IEnumerable<Position> PiecesPositionsFor(Player player)
        {
            // Retorna as posições filtradas, onde a cor da peça na posição é igual à cor do jogador fornecido
            return PiecePositions().Where(pos => this[pos].Color == player);
        }

        // Método público que verifica se um jogador está em xeque
        public bool IsInCheck(Player player)
        {
            // Verifica se alguma peça do oponente pode capturar o rei do jogador especificado
            return PiecesPositionsFor(player.Opponent()).Any(pos => // Obtém todas as posições das peças do oponente e verifica se alguma pode capturar o rei do jogador
            {
                // Obtém a peça na posição atual do oponente
                Piece piece = this[pos];

                // Retorna verdadeiro se a peça puder capturar o rei do jogador a partir da posição atual
                return piece.CanCaptureOpponentKing(pos, this);
            });
        }

        // Método público que cria uma cópia do tabuleiro
        public Board Copy()
        {
            // Cria uma nova instância do objeto Board
            Board copy = new Board();

            // Itera sobre todas as posições ocupadas no tabuleiro atual
            foreach (Position pos in PiecePositions())
            {
                // Copia a peça da posição atual no tabuleiro original para a mesma posição no tabuleiro copiado
                copy[pos] = this[pos].Copy();
            }

            // Retorna a cópia do tabuleiro
            return copy;
        }

    }
}


