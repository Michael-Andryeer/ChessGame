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

        // Propriedade que representa o resultado do jogo. Inicialmente é nula.
        public Result Result { get; private set; } = null;

        // Contador para rastrear o número de movimentos sem captura ou movimento de peão.
        private int noCaptureOrPawnMoves = 0;

        // String que representa o estado atual do jogo.
        private string stateString;

        // Dicionário para rastrear a história do estado do jogo e contagem de repetições de estado.
        private readonly Dictionary<string, int> StateHistory = new Dictionary<string, int>();

        // Construtor da classe GameState que inicializa o tabuleiro e o jogador atual.
        public GameState(Board board, Player player)
        {
            // Inicializa a propriedade Board com o tabuleiro fornecido.
            Board = board;

            // Inicializa a propriedade CurrentPlayer com o jogador fornecido.
            CurrentPlayer = player;

            // Gera a string do estado atual e adiciona ao histórico de estados.
            stateString = new StateString(CurrentPlayer, board).ToString();
            StateHistory[stateString] = 1;
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

            // Obtém todos os movimentos possíveis para a peça na posição atual.
            IEnumerable<Move> moveCandidates = piece.GetMoves(pos, Board);

            // Filtra os movimentos candidatos para incluir apenas aqueles que são legais no tabuleiro atual.
            return moveCandidates.Where(move => move.IsLegal(Board));
        }

        // Método que executa um movimento e altera o jogador atual para o próximo.
        public void MakeMove(Move move)
        {
            // Define a posição de peão pulada para nula antes de executar o movimento.
            Board.SetPawnSkipPosition(CurrentPlayer, null);

            // Executa o movimento no tabuleiro.
            bool captureOrPawn = move.Execute(Board);

            // Reseta o contador de movimentos sem captura ou movimento de peão se houve uma captura ou movimento de peão.
            if (captureOrPawn)
            {
                noCaptureOrPawnMoves = 0;
                StateHistory.Clear(); // Limpa o histórico de estados.
            }
            else
            {
                noCaptureOrPawnMoves++;
            }

            // Altera o jogador atual para o próximo jogador.
            CurrentPlayer = CurrentPlayer.Opponent();

            // Atualiza a string do estado e verifica se o jogo acabou.
            UpdateStateString();
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
            else if (Board.InsufficientMaterial())
            {
                // Verifica se há material insuficiente para dar xeque-mate e, se sim, define o resultado como empate.
                Result = Result.Draw(EndReason.InsufficienteMaterial);
            }
            else if (FiftyMoveRule())
            {
                // Verifica se a regra dos cinquenta movimentos se aplica e, se sim, define o resultado como empate.
                Result = Result.Draw(EndReason.FiftyMoveRule);
            }
            else if (ThreefoldRepetition())
            {
                // Verifica se houve repetição tripla de posição e, se sim, define o resultado como empate.
                Result = Result.Draw(EndReason.ThreefoldRepetition);
            }
        }

        // Método público que verifica se o jogo acabou.
        public bool IsGameOver()
        {
            // Retorna verdadeiro (true) se a propriedade Result não for nula, indicando que há um resultado definido para o jogo.
            // Caso contrário, retorna falso (false), indicando que o jogo ainda está em andamento.
            return Result != null;
        }

        // Método privado que verifica se a regra dos cinquenta movimentos se aplica.
        private bool FiftyMoveRule()
        {
            // Divide o número de movimentos sem captura ou movimento de peão por 2 para obter o número de movimentos completos.
            int fullMoves = noCaptureOrPawnMoves / 2;

            // Retorna verdadeiro se houver 50 movimentos completos sem captura ou movimento de peão.
            return fullMoves == 50;
        }

        // Método privado que atualiza a string do estado atual do jogo.
        private void UpdateStateString()
        {
            // Atualiza a string do estado atual do jogo.
            stateString = new StateString(CurrentPlayer, Board).ToString();

            // Adiciona a string do estado ao histórico, incrementando o contador de ocorrências.
            if (!StateHistory.ContainsKey(stateString))
            {
                StateHistory[stateString] = 1;
            }
            else
            {
                StateHistory[stateString]++;
            }
        }

        // Método privado que verifica se houve repetição tripla de posição.
        private bool ThreefoldRepetition()
        {
            // Retorna verdadeiro se o estado atual apareceu três vezes no histórico de estados.
            return StateHistory[stateString] == 3;
        }
    }
}
