// Importa o namespace ChessLogic.Pieces que provavelmente contém as classes das peças de xadrez (Rook, Knight, Bishop, etc.).
using ChessLogic.Pieces;
using System.Security.Cryptography.X509Certificates;

// Define um namespace chamado ChessLogic para organizar as classes relacionadas à lógica do jogo de xadrez.
namespace ChessLogic
{
    // Define uma classe chamada Board que representa o tabuleiro de xadrez.
    public class Board
    {
        // Declara um campo somente leitura de uma matriz 8x8 de peças (Piece), representando o tabuleiro de xadrez.
        private readonly Piece[,] pieces = new Piece[8, 8];

        // Declara um dicionário que armazena a posição do peão que pulou duas casas no turno anterior para cada jogador.
        private readonly Dictionary<Player, Position> pawnSkipPositions = new Dictionary<Player, Position>
        {
            {Player.White, null },
            {Player.Black, null }
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

        // Método que obtém a posição do peão que pulou duas casas para um jogador específico.
        public Position GetPawnSkipPosition(Player player)
        {
            return pawnSkipPositions[player];
        }

        // Método que define a posição do peão que pulou duas casas para um jogador específico.
        public void SetPawnSkipPosition(Player player, Position pos)
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
            return PiecesPositionsFor(player.Opponent()).Any(pos =>
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

        // Método que conta as peças no tabuleiro e retorna um objeto Counting com a contagem
        public Counting CountPieces()
        {
            // Cria uma nova instância de Counting
            Counting counting = new Counting();

            // Itera sobre todas as posições ocupadas no tabuleiro
            foreach (Position pos in PiecePositions())
            {
                // Obtém a peça na posição atual
                Piece piece = this[pos];
                // Incrementa a contagem da peça no objeto Counting
                counting.Increment(piece.Color, piece.Type);
            }

            // Retorna o objeto Counting com a contagem das peças
            return counting;
        }

        // Método que verifica se há material insuficiente para dar xeque-mate
        public bool InsufficientMaterial()
        {
            // Conta as peças no tabuleiro
            Counting counting = CountPieces();

            // Verifica diferentes cenários de material insuficiente
            return IsKingVKing(counting) || IsKingBishopVKing(counting) || IsKingKnightVKing(counting) || IsKingBishopVKingBishop(counting);
        }

        // Método estático que verifica se o jogo é apenas rei contra rei
        private static bool IsKingVKing(Counting counting)
        {
            // Verifica se há apenas dois reis no tabuleiro
            return counting.TotalCount == 2;
        }

        // Método estático que verifica se o jogo é rei e bispo contra rei
        private static bool IsKingBishopVKing(Counting counting)
        {
            // Verifica se há apenas três peças no tabuleiro e um dos jogadores tem um bispo
            return counting.TotalCount == 3 && (counting.White(PieceType.Bishop) == 1 || counting.Black(PieceType.Bishop) == 1);
        }

        // Método estático que verifica se o jogo é rei e cavalo contra rei
        private static bool IsKingKnightVKing(Counting counting)
        {
            // Verifica se há apenas três peças no tabuleiro e um dos jogadores tem um cavalo
            return counting.TotalCount == 3 && (counting.White(PieceType.Knight) == 1 || counting.Black(PieceType.Knight) == 1);
        }

        // Método que verifica se o jogo é rei e bispo contra rei e bispo, ambos os bispos em casas da mesma cor
        private bool IsKingBishopVKingBishop(Counting counting)
        {
            // Verifica se há exatamente quatro peças no tabuleiro
            if (counting.TotalCount != 4)
            {
                return false;
            }

            // Verifica se ambos os jogadores têm exatamente um bispo
            if (counting.White(PieceType.Bishop) != 1 || counting.Black(PieceType.Bishop) != 1)
            {
                return false;
            }

            // Encontra a posição do bispo branco
            Position wBishopPos = FindPiece(Player.White, PieceType.Bishop);
            // Encontra a posição do bispo preto
            Position BBishopPos = FindPiece(Player.Black, PieceType.Bishop);

            // Verifica se ambos os bispos estão em casas da mesma cor
            return wBishopPos.SquareColor() == BBishopPos.SquareColor();
        }

        // Método que encontra a posição de uma peça específica de um jogador específico
        private Position FindPiece(Player color, PieceType type)
        {
            // Retorna a primeira posição que contém a peça especificada
            return PiecesPositionsFor(color).First(pos => this[pos].Type == type);
        }

        // Método privado que verifica se o rei e a torre em posições específicas não se moveram.
        private bool IsUnmovedKingAndRook(Position kingPos, Position rookPos)
        {
            // Verifica se a posição do rei ou da torre está vazia.
            if (IsEmpty(kingPos) || IsEmpty(rookPos))
            {
                return false; // Retorna falso se qualquer posição estiver vazia.
            }

            // Obtém a peça na posição do rei e na posição da torre.
            Piece king = this[kingPos];
            Piece rook = this[rookPos];

            // Retorna verdadeiro se a peça na posição do rei for um rei, a peça na posição da torre for uma torre,
            // e ambas as peças não tiverem se movido ainda.
            return king.Type == PieceType.King && rook.Type == PieceType.Rook &&
                   !king.HasMoved && !rook.HasMoved;
        }

        // Método público que verifica o direito de roque do lado do rei (curto) para um jogador.
        public bool CastleRightKS(Player player)
        {
            // Usa uma expressão switch para verificar se o jogador tem o direito de roque curto,
            // com base nas posições iniciais do rei e da torre para branco e preto.
            return player switch
            {
                Player.White => IsUnmovedKingAndRook(new Position(7, 4), new Position(7, 7)),
                Player.Black => IsUnmovedKingAndRook(new Position(0, 4), new Position(0, 7)),
                _ => false // Retorna falso para qualquer outro valor de jogador (inválido).
            };
        }

        // Método privado que verifica se um peão está em uma das posições específicas e pode capturar en passant.
        private bool HasPawnInPosition(Player player, Position[] pawnPositions, Position skipPos)
        {
            // Itera sobre as posições dos peões fornecidas que estão dentro dos limites do tabuleiro.
            foreach (Position pos in pawnPositions.Where(IsInside))
            {
                // Obtém a peça na posição atual.
                Piece piece = this[pos];
                // Continua para a próxima posição se a peça for nula, não pertencer ao jogador, ou não for um peão.
                if (piece == null || piece.Color != player || piece.Type != PieceType.Pawn)
                {
                    continue;
                }

                // Cria um movimento en passant com a posição do peão e a posição do peão que pulou duas casas.
                EnPassant move = new EnPassant(pos, skipPos);

                // Retorna verdadeiro se o movimento en passant for legal.
                if (move.IsLegal(this))
                {
                    return true;
                }
            }

            // Retorna falso se nenhum peão puder capturar en passant.
            return false;
        }

        // Método público que verifica o direito de roque do lado da dama (longo) para um jogador.
        public bool CastleRightQS(Player player)
        {
            // Usa uma expressão switch para verificar se o jogador tem o direito de roque longo,
            // com base nas posições iniciais do rei e da torre para branco e preto.
            return player switch
            {
                Player.White => IsUnmovedKingAndRook(new Position(7, 4), new Position(7, 0)),
                Player.Black => IsUnmovedKingAndRook(new Position(0, 4), new Position(0, 0)),
                _ => false // Retorna falso para qualquer outro valor de jogador (inválido).
            };
        }

        // Método público que verifica se um jogador pode capturar en passant.
        public bool CanCaptureEnPassant(Player player)
        {
            // Obtém a posição do peão que pulou duas casas no último movimento do oponente.
            Position skipPos = GetPawnSkipPosition(player.Opponent());

            // Retorna falso se não houver peão que pulou duas casas.
            if (skipPos == null)
            {
                return false;
            }

            // Determina as posições potenciais dos peões que podem capturar en passant com base no jogador.
            Position[] pawnPositions = player switch
            {
                Player.White => new Position[] { skipPos + Direction.SouthWest, skipPos + Direction.SouthEast },
                Player.Black => new Position[] { skipPos + Direction.NorthEast, skipPos + Direction.NorthWest },
                _ => Array.Empty<Position>() // Retorna um array vazio para qualquer outro valor de jogador (inválido).
            };

            // Verifica se há um peão na posição que pode capturar en passant.
            return HasPawnInPosition(player, pawnPositions, skipPos);
        }

    }
}
