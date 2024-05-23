
using System;
using System.IO; // Importa o namespace para trabalhar com streams
using System.Windows; // Importa o namespace para acessar a classe Application
using System.Windows.Input; // Importa o namespace para acessar a classe Cursor

namespace ChessInterface
{
    public static class ChessCursors
    {
        // Define dois cursores estáticos para representar o cursor branco e preto do xadrez
        public static readonly Cursor WhiteCursor = LoadCursor("Assets/CursorW.cur");
        public static readonly Cursor BlackCursor = LoadCursor("Assets/CursorB.cur");

        // Método privado para carregar o cursor a partir de um arquivo
        private static Cursor LoadCursor(string filePath)
        {
            // Obtém um stream para o arquivo do cursor
            Stream stream = Application.GetResourceStream(new Uri(filePath, UriKind.Relative)).Stream;

            // Retorna um novo objeto Cursor usando o stream carregado do arquivo, marcando-o para fechamento após o uso
            return new Cursor(stream, true);
        }
    }
}
