// Define um namespace chamado ChessLogic para organizar as classes relacionadas à lógica do jogo de xadrez.
namespace ChessLogic
{
    // Define uma classe chamada GameState que representa o estado atual de um jogo de xadrez.
    public class GameState
    {
        // Campos privados que representam o jogador branco e o tabuleiro de xadrez.
        private Player white;
        private Board board;

        // Propriedade somente leitura que representa o tabuleiro de xadrez.
        public Board Board { get; }

        // Propriedade que representa o jogador atual. Pode ser alterada apenas dentro desta classe.
        public Player CurrentPlayer { get; private set; }

        public Result Result { get; private set; } = null;

        // Construtor da classe GameState que inicializa o tabuleiro e o jogador atual.
        public GameState(Board board, Player player)
        {
            // Inicializa a propriedade Board com o tabuleiro fornecido.
            Board = board;

            // Inicializa a propriedade CurrentPlayer com o jogador fornecido.
            CurrentPlayer = player;
        }

        // Método que retorna os movimentos legais para uma peça em uma determinada posição.
        public IEnumerable<Move> LegalMovesForPiece(Position pos)
        {
            // Verifica se a posição está vazia ou se a peça na posição pertence ao jogador atual.
            if (Board.IsEmpty(pos) || Board[pos].Color != CurrentPlayer)
            {
                return Enumerable.Empty<Move>(); // Retorna uma lista vazia de movimentos.
            }

            // Obtém a peça na posição especificada.
            Piece piece = Board[pos];

            // Obtém todos os movimentos possíveis para a peça na posição atual
            IEnumerable<Move> moveCandidates = piece.GetMoves(pos, Board);

            // Filtra os movimentos candidatos para incluir apenas aqueles que são legais no tabuleiro atual
            return moveCandidates.Where(move => move.IsLegal(Board));

        }

        // Método que executa um movimento e altera o jogador atual para o próximo.
        public void MakeMove(Move move)
        {
            // Define a posição de peão pulada para nula antes de executar o movimento.
            Board.SetPawnSkipPosition(CurrentPlayer, null);
            
            // Executa o movimento no tabuleiro.
            move.Execute(Board);

            // Altera o jogador atual para o próximo jogador.
            CurrentPlayer = CurrentPlayer.Opponent();

            CheckForGameOver();
        }

        // Construtor alternativo da classe GameState que inicializa o jogador branco e o tabuleiro de xadrez.
        // Este construtor chama o outro construtor passando o tabuleiro e o jogador branco.
        public GameState(Player white, Board board) : this(board, white)
        {
            // Inicializa o campo privado 'white' com o jogador branco fornecido.
            this.white = white;
        }
        // Método público que retorna todos os movimentos legais para todas as peças de um jogador.
        public IEnumerable<Move> AllLegalMovesFor(Player player)
        {
            // Obtém todas as posições das peças do jogador.
            IEnumerable<Move> moveCandidates = Board.PiecesPositionsFor(player).SelectMany(pos =>
            {
                // Para cada posição de peça, obtém a peça naquela posição.
                Piece piece = Board[pos];
                // Para cada peça, obtém todos os movimentos possíveis.
                return piece.GetMoves(pos, Board);
            });

            // Filtra os movimentos candidatos para incluir apenas aqueles que são legais no tabuleiro atual.
            return moveCandidates.Where(Move => Move.IsLegal(Board));
        }

        // Método privado que verifica se o jogo acabou e define o resultado com base nas condições atuais do jogo.
        private void CheckForGameOver()
        {
            // Verifica se não há movimentos legais para o jogador atual e se o jogador atual está em xeque.
            if (!AllLegalMovesFor(CurrentPlayer).Any())
            {
                // Se não houver movimentos legais para o jogador atual:
                if (Board.IsInCheck(CurrentPlayer))
                {
                    // Se o jogador atual estiver em xeque, define o resultado como vitória para o oponente.
                    Result = Result.Win(CurrentPlayer.Opponent());
                }
                else
                {
                    // Senão, define o resultado como empate.
                    Result = Result.Draw(EndReason.Stelemate);
                }
            }
        }

        // Método público que verifica se o jogo acabou.
        public bool IsGameOver()
        {
            // Retorna verdadeiro (true) se a propriedade Result não for nula, indicando que há um resultado definido para o jogo.
            // Caso contrário, retorna falso (false), indicando que o jogo ainda está em andamento.
            return Result != null;
        }
    }
}
