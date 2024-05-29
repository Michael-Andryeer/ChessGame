using System.Collections.Generic; // Namespace para coleções genéricas.
using System.Linq; // Usado para o método LINQ Any().
using System.Windows; // Namespace para classes WPF básicas.
using System.Windows.Controls; // Namespace para controles WPF.
using System.Windows.Input; // Namespace para manipulação de entrada do usuário.
using System.Windows.Media; // Namespace para gráficos 2D.
using System.Windows.Shapes; // Namespace para formas gráficas.
using ChessLogic; // Namespace para a lógica do jogo de xadrez.

// Define um namespace chamado ChessInterface para organizar as classes relacionadas à interface gráfica do jogo de xadrez.
namespace ChessInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    // Define uma classe parcial chamada MainWindow que representa a janela principal da interface do usuário.
    public partial class MainWindow : Window
    {
        // Declara um campo somente leitura de uma matriz 8x8 de objetos Image para representar as imagens das peças no tabuleiro.
        private readonly Image[,] pieceImages = new Image[8, 8];

        // Declara um campo somente leitura de uma matriz 8x8 de objetos Rectangle para representar os destaques no tabuleiro.
        private readonly Rectangle[,] highLights = new Rectangle[8, 8];

        // Declara um campo para armazenar um dicionário de movimentos possíveis com a posição como chave.
        private readonly Dictionary<Position, Move> moveCache = new Dictionary<Position, Move>();

        // Declara um campo para armazenar o estado do jogo.
        private GameState gameState;

        // Declara um campo para armazenar a posição selecionada no tabuleiro.
        private Position selectedPos = null;

        // Construtor da classe MainWindow que é chamado quando a janela é inicializada.
        public MainWindow()
        {
            InitializeComponent(); // Inicializa os componentes da interface gráfica.
            InitializeBoard(); // Inicializa o tabuleiro de xadrez na interface gráfica.
            gameState = new GameState(Player.White, Board.Initial()); // Inicializa o estado do jogo.
            DrawBoard(gameState.Board); // Desenha o tabuleiro na interface gráfica.
            SetCursor(gameState.CurrentPlayer); // Define o cursor do mouse com base no jogador atual.
        }

        // Método privado para inicializar o tabuleiro de xadrez na interface gráfica.
        private void InitializeBoard()
        {
            // Loop para percorrer todas as posições do tabuleiro (8x8).
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    // Cria uma nova instância de Image para representar a imagem de uma peça.
                    Image image = new Image();
                    // Armazena a referência da imagem na matriz pieceImages na posição correspondente.
                    pieceImages[r, c] = image;
                    // Adiciona a imagem ao Painel (Grid) da interface gráfica para ser exibida na tela.
                    PieceGrid.Children.Add(image);

                    // Cria uma nova instância de Rectangle para representar o destaque de uma posição.
                    Rectangle highlight = new Rectangle();
                    // Armazena a referência do destaque na matriz highLights na posição correspondente.
                    highLights[r, c] = highlight;
                    // Adiciona o destaque ao Painel (Grid) da interface gráfica para ser exibido na tela.
                    HighlightGrid.Children.Add(highlight);
                }
            }
        }

        // Método privado que desenha o tabuleiro de xadrez na interface gráfica com base no estado atual do tabuleiro.
        private void DrawBoard(Board board)
        {
            // Loop para percorrer todas as posições do tabuleiro (8x8).
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    // Obtém a peça na posição atual do tabuleiro.
                    Piece piece = board[r, c];
                    // Define a imagem da peça na posição correspondente no tabuleiro da interface gráfica.
                    pieceImages[r, c].Source = Images.GetImage(piece);
                }
            }
        }

        // Manipulador de eventos para o clique do mouse no tabuleiro.
        private void BoardGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Verifica se o menu está visível na tela.
            if (IsMenuOnScreen())
            {
                return; // Retorna se o menu estiver visível.
            }

            // Obtém a posição do clique do mouse.
            Point point = e.GetPosition(BoardGrid);
            // Converte a posição do clique do mouse em uma posição do tabuleiro.
            Position pos = ToSquareposition(point);

            // Se nenhuma posição estiver selecionada, trata a posição como a posição inicial.
            if (selectedPos == null)
            {
                OnFromPositionSelected(pos);
            }
            // Se uma posição estiver selecionada, trata a posição como a posição de destino.
            else
            {
                OnToPositionSelected(pos);
            }
        }

        // Método privado que converte uma posição de clique do mouse em uma posição do tabuleiro.
        private Position ToSquareposition(Point point)
        {
            // Calcula o tamanho de cada quadrado do tabuleiro.
            double squareSize = BoardGrid.ActualWidth / 8;
            // Calcula a linha da posição do tabuleiro.
            int row = (int)(point.Y / squareSize);
            // Calcula a coluna da posição do tabuleiro.
            int col = (int)(point.X / squareSize);
            // Retorna a posição do tabuleiro.
            return new Position(row, col);
        }

        // Método privado que trata a seleção da posição inicial.
        private void OnFromPositionSelected(Position pos)
        {
            // Obtém os movimentos legais para a peça na posição selecionada.
            IEnumerable<Move> moves = gameState.LegalMovesForPiece(pos);

            // Se houver movimentos legais, armazena a posição e destaca os movimentos.
            if (moves.Any())
            {
                selectedPos = pos; // Armazena a posição selecionada.
                CacheMoves(moves); // Armazena os movimentos no cache.
                ShowHighlights(); // Destaca as posições possíveis no tabuleiro.
            }
        }

        // Método privado que trata a seleção da posição de destino.
        private void OnToPositionSelected(Position pos)
        {
            // Limpa a posição selecionada e oculta os destaques.
            selectedPos = null;
            HideHighlights();

            // Se a posição de destino estiver no cache de movimentos, executa o movimento.
            if (moveCache.TryGetValue(pos, out Move move))
            {
                // Verifica se o movimento é uma promoção de peão.
                if (move.Type == MoveType.PawnPromotion)
                {
                    HandlePromotion(move.FromPos, move.ToPos); // Trata a promoção do peão.
                }
                else
                {
                    HandleMove(move); // Executa o movimento.
                }
            }
        }

        // Método privado que trata a promoção de um peão.
        private void HandlePromotion(Position from, Position to)
        {
            // Define a imagem do peão na posição de destino.
            pieceImages[to.Row, to.Column].Source = Images.GetImage(gameState.CurrentPlayer, PieceType.Pawn);
            // Remove a imagem do peão na posição de origem.
            pieceImages[from.Row, from.Column].Source = null;

            // Cria um novo menu de promoção.
            PromotionMenu promMenu = new PromotionMenu(gameState.CurrentPlayer);
            // Define o conteúdo do MenuContainer como o menu de promoção.
            MenuContainer.Content = promMenu;

            // Adiciona um manipulador de eventos para a seleção de uma peça no menu de promoção.
            promMenu.PieceSelected += type =>
            {
                MenuContainer.Content = null; // Limpa o conteúdo do MenuContainer.
                Move promMove = new PawnPromotion(from, to, type); // Cria um movimento de promoção.
                HandleMove(promMove); // Executa o movimento de promoção.
            };
        }

        // Método privado que executa um movimento.
        private void HandleMove(Move move)
        {
            // Executa o movimento no estado do jogo.
            gameState.MakeMove(move);
            // Redesenha o tabuleiro na interface gráfica.
            DrawBoard(gameState.Board);
            // Define o cursor do mouse com base no jogador atual.
            SetCursor(gameState.CurrentPlayer);

            // Verifica se o jogo terminou.
            if (gameState.IsGameOver())
            {
                ShowGameOver(); // Exibe a tela de fim de jogo.
            }
        }

        // Método privado que armazena os movimentos possíveis no cache.
        private void CacheMoves(IEnumerable<Move> moves)
        {
            // Limpa o cache de movimentos.
            moveCache.Clear();

            // Armazena cada movimento no cache com a posição de destino como chave.
            foreach (Move move in moves)
            {
                moveCache[move.ToPos] = move;
            }
        }

        // Método privado que destaca as posições possíveis no tabuleiro.
        private void ShowHighlights()
        {
            // Define a cor do destaque com transparência.
            Color color = Color.FromArgb(150, 125, 255, 125);

            // Percorre todas as posições no cache de movimentos e aplica o destaque.
            foreach (Position to in moveCache.Keys)
            {
                highLights[to.Row, to.Column].Fill = new SolidColorBrush(color);
            }
        }

        // Método privado que oculta os destaques no tabuleiro.
        private void HideHighlights()
        {
            // Percorre todas as posições no cache de movimentos e remove o destaque.
            foreach (Position to in moveCache.Keys)
            {
                highLights[to.Row, to.Column].Fill = Brushes.Transparent;
            }
        }

        // Método privado que define o cursor do mouse com base no jogador atual.
        private void SetCursor(Player player)
        {
            // Verifica se o jogador atual é branco.
            if (player == Player.White)
            {
                // Define o cursor como o cursor branco.
                Cursor = ChessCursors.WhiteCursor;
            }
            else
            {
                // Define o cursor como o cursor preto.
                Cursor = ChessCursors.BlackCursor;
            }
        }

        // Método privado que verifica se o menu está visível na tela.
        private bool IsMenuOnScreen()
        {
            return MenuContainer.Content != null; // Retorna true se o menu estiver visível, caso contrário, false.
        }

        // Método privado que exibe a tela de fim de jogo.
        private void ShowGameOver()
        {
            // Cria uma nova instância do menu de fim de jogo.
            GameOverMenu gameOverMenu = new GameOverMenu(gameState);
            // Define o conteúdo do MenuContainer como o menu de fim de jogo.
            MenuContainer.Content = gameOverMenu;

            // Adiciona um manipulador de eventos para a seleção de uma opção no menu de fim de jogo.
            gameOverMenu.OptionSelected += option =>
            {
                // Verifica se a opção selecionada é reiniciar.
                if (option == Option.Restart)
                {
                    // Limpa o conteúdo do MenuContainer e reinicia o jogo.
                    MenuContainer.Content = null;
                    RestartGame();
                }
                else
                {
                    // Encerra o aplicativo.
                    Application.Current.Shutdown();
                }
            };
        }

        // Método privado que reinicia o jogo.
        private void RestartGame()
        {
            selectedPos = null;
            HideHighlights(); // Oculta os destaques no tabuleiro.
            moveCache.Clear(); // Limpa o cache de movimentos.
            gameState = new GameState(Player.White, Board.Initial()); // Reinicia o estado do jogo.
            DrawBoard(gameState.Board); // Redesenha o tabuleiro na interface gráfica.
            SetCursor(gameState.CurrentPlayer); // Define o cursor do mouse com base no jogador atual.
        }

        // Manipulador de eventos para a tecla pressionada na janela.
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // Se o menu não estiver visível e a tecla pressionada for Esc, exibe o menu de pausa.
            if (!IsMenuOnScreen() && e.Key == Key.Escape)
            {
                ShowPauseMenu();
            }
        }

        // Método privado que exibe o menu de pausa.
        private void ShowPauseMenu()
        {
            PauseMenu pauseMenu = new PauseMenu(); // Cria uma nova instância do menu de pausa.
            MenuContainer.Content = pauseMenu; // Define o conteúdo do MenuContainer como o menu de pausa.

            // Adiciona um manipulador de eventos para a seleção de uma opção no menu de pausa.
            pauseMenu.OptionSelected += option =>
            {
                MenuContainer.Content = null; // Limpa o conteúdo do MenuContainer.

                // Se a opção selecionada for reiniciar, reinicia o jogo.
                if (option == Option.Restart)
                {
                    RestartGame();
                }
            };
        }
    }
}
