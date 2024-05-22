using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ChessLogic;

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

        // Declara um campo para armazenar o estado do jogo.
        private GameState gameState;

        // Construtor da classe MainWindow que é chamado quando a janela é inicializada.
        public MainWindow()
        {
                InitializeComponent();
                InitializeBoard();
                gameState = new GameState(Player.White, Board.Initial());
                DrawBoard(gameState.Board);
            
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
                }
            }
        }

        // Método privado que desenha o tabuleiro de xadrez na interface gráfica com base no estado atual do tabuleiro.
        private void DrawBoard(Board board)
        {
            // Loop para percorrer todas as posições do tabuleiro (8x8).
            for (int r= 0; r < 8; r++)
            {
                for (int c = 0; c<8; c++)
                {
                    // Obtém a peça na posição atual do tabuleiro.
                    Piece piece = board[r, c];
                    // Define a imagem da peça na posição correspondente no tabuleiro da interface gráfica.
                    pieceImages[r, c].Source = Images.GetImage(piece);
                }
            }
        }

       
    }
}
