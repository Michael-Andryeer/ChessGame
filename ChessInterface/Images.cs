using ChessLogic;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

// Define um namespace chamado ChessInterface para organizar as classes relacionadas à interface gráfica do jogo de xadrez.
namespace ChessInterface
{
    // Define uma classe estática chamada Images, responsável por carregar e fornecer as imagens das peças de xadrez.
    public static class Images
    {
        // Dicionário privado que mapeia cada tipo de peça (PieceType) para sua respectiva imagem para peças brancas.
        private static readonly Dictionary<PieceType, ImageSource> whiteSources = new()
        {
            // Mapeia o tipo de peça para sua imagem correspondente.
            {PieceType.Pawn, LoadImage("Assets/PawnW.png") },
            {PieceType.Knight, LoadImage("Assets/KnightW.png") },
            {PieceType.Rook, LoadImage("Assets/RookW.png") },
            {PieceType.Queen, LoadImage("Assets/QueenW.png") },
            {PieceType.King, LoadImage("Assets/KingW.png") },
        };

        // Dicionário privado que mapeia cada tipo de peça (PieceType) para sua respectiva imagem para peças pretas.
        private static readonly Dictionary<PieceType, ImageSource> blackSources = new()
        {
            // Mapeia o tipo de peça para sua imagem correspondente.
            {PieceType.Pawn, LoadImage("Assets/PawnB.png") },
            {PieceType.Knight, LoadImage("Assets/KnightB.png") },
            {PieceType.Rook, LoadImage("Assets/RookB.png") },
            {PieceType.Queen, LoadImage("Assets/QueenB.png") },
            {PieceType.King, LoadImage("Assets/KingB.png") },
        };

        // Método privado que carrega uma imagem a partir de um arquivo e retorna a imagem carregada como um ImageSource.
        private static ImageSource LoadImage(string filePath)
        {
            return new BitmapImage(new Uri(filePath, UriKind.Relative));
        }

        // Método estático que retorna a imagem correspondente a uma peça específica com base na cor e tipo da peça.
        public static ImageSource GetImage(Player color, PieceType type)
        {
            // Retorna a imagem correspondente à peça com base na cor e tipo fornecidos.
            return color switch
            {
                Player.White => whiteSources[type], // Retorna a imagem para peças brancas.
                Player.Black => blackSources[type], // Retorna a imagem para peças pretas.
                _ => null // Retorna null se a cor da peça não for nem branca nem preta.
            };
        }

        // Sobrecarga do método GetImage que retorna a imagem correspondente a uma peça específica com base no objeto Piece fornecido.
        public static ImageSource GetImage(Piece piece)
        {
            // Verifica se a peça fornecida não é nula.
            if (piece == null)
            {
                return null; // Retorna null se a peça fornecida for nula.
            }

            // Retorna a imagem correspondente à peça com base na cor e tipo da peça.
            return GetImage(piece.Color, piece.Type);
        }
    }
}
