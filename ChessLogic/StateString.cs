using System.Text;

namespace ChessLogic
{
    // Classe que representa o estado do jogo de xadrez como uma string.
    public class StateString
    {
        // StringBuilder utilizado para construir a string do estado.
        private readonly StringBuilder sb = new StringBuilder();

        // Construtor que inicializa o estado do jogo com o jogador atual e o tabuleiro.
        public StateString(Player currentPlayer, Board board)
        {
            // Adiciona a disposição das peças ao StringBuilder.
            AddPiecePlacement(board);

            // Adiciona um espaço separador.
            sb.Append(' ');

            // Adiciona o jogador atual ao StringBuilder.
            AddCurrentPlayer(currentPlayer);

            // Adiciona um espaço separador.
            sb.Append(' ');

            // Adiciona os direitos de roque ao StringBuilder.
            AddCastlingRights(board);

            // Adiciona um espaço separador.
            sb.Append(' ');

            // Adiciona a possibilidade de captura en passant ao StringBuilder.
            AddEnPassant(board, currentPlayer);
        }

        // Override do método ToString para retornar a string construída.
        public override string ToString()
        {
            return sb.ToString();
        }

        // Método estático que retorna o caractere correspondente à peça fornecida.
        private static char PieceChar(Piece piece)
        {
            // Determina o caractere com base no tipo da peça.
            char c = piece.Type switch
            {
                PieceType.Pawn => 'p',
                PieceType.Knight => 'n',
                PieceType.Rook => 'r',
                PieceType.Bishop => 'b',
                PieceType.Queen => 'q',
                PieceType.King => 'k',
                _ => ' ' // Valor padrão.
            };

            // Se a peça for branca, retorna a letra maiúscula; caso contrário, retorna o caractere original.
            if (piece.Color == Player.White)
            {
                return char.ToUpper(c);
            }

            return c;
        }

        // Método que adiciona os dados de uma linha do tabuleiro ao StringBuilder.
        private void AddRowData(Board board, int row)
        {
            int empty = 0; // Contador de casas vazias.

            // Itera sobre as colunas da linha fornecida.
            for (int c = 0; c < 8; c++)
            {
                // Se a posição estiver vazia, incrementa o contador de casas vazias.
                if (board[row, c] == null)
                {
                    empty++;
                    continue;
                }

                // Se houve casas vazias anteriormente, adiciona o número ao StringBuilder.
                if (empty > 0)
                {
                    sb.Append(empty);
                    empty = 0; // Reseta o contador de casas vazias.
                }

                // Adiciona o caractere da peça ao StringBuilder.
                sb.Append(PieceChar(board[row, c]));
            }

            // Se houver casas vazias restantes no final da linha, adiciona o número ao StringBuilder.
            if (empty > 0)
            {
                sb.Append(empty);
            }
        }

        // Método que adiciona a disposição das peças no tabuleiro ao StringBuilder.
        private void AddPiecePlacement(Board board)
        {
            // Itera sobre as linhas do tabuleiro.
            for (int r = 0; r < 8; r++)
            {
                // Adiciona uma barra de separação entre as linhas, exceto na primeira linha.
                if (r != 0)
                {
                    sb.Append('/');
                }

                // Adiciona os dados da linha atual ao StringBuilder.
                AddRowData(board, r);
            }
        }

        // Método que adiciona o jogador atual ao StringBuilder.
        private void AddCurrentPlayer(Player currentPlayer)
        {
            // Adiciona 'w' para branco e 'b' para preto.
            if (currentPlayer == Player.White)
            {
                sb.Append('w');
            }
            else
            {
                sb.Append('b');
            }
        }

        // Método que adiciona os direitos de roque ao StringBuilder.
        private void AddCastlingRights(Board board)
        {
            // Verifica os direitos de roque para ambos os jogadores.
            bool castleWKS = board.CastleRightKS(Player.White);
            bool castleWQS = board.CastleRightQS(Player.White);
            bool castleBKS = board.CastleRightKS(Player.Black);
            bool castleBQS = board.CastleRightQS(Player.Black);

            // Se nenhum direito de roque estiver disponível, adiciona '-'.
            if (!(castleWKS || castleWQS || castleBKS || castleBQS))
            {
                sb.Append('-');
                return;
            }

            // Adiciona os caracteres correspondentes aos direitos de roque disponíveis.
            if (castleWKS)
            {
                sb.Append('K');
            }

            if (castleWQS)
            {
                sb.Append('Q');
            }

            if (castleBKS)
            {
                sb.Append('k');
            }

            if (castleBQS)
            {
                sb.Append('q');
            }
        }

        // Método que adiciona a possibilidade de captura en passant ao StringBuilder.
        private void AddEnPassant(Board board, Player currentPlayer)
        {
            // Se não houver possibilidade de captura en passant, adiciona '-'.
            if (!board.CanCaptureEnPassant(currentPlayer))
            {
                sb.Append('-');
                return;
            }

            // Obtém a posição do peão que pulou duas casas no último movimento do oponente.
            Position pos = board.GetPawnSkipPosition(currentPlayer.Opponent());

            // Converte a coluna e a linha da posição para notação de xadrez.
            char file = (char)('a' + pos.Column);
            int rank = 8 - pos.Row;

            // Adiciona a posição de captura en passant ao StringBuilder.
            sb.Append(file);
            sb.Append(rank);
        }
    }
}
