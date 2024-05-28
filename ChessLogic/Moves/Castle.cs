using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    // Define a classe Castle que herda da classe Move e representa o movimento de roque no xadrez.
    public class Castle : Move
    {
        // Propriedade sobrescrita que indica o tipo de movimento.
        public override MoveType Type { get; }

        // Propriedade sobrescrita que indica a posição de origem do movimento.
        public override Position FromPos { get; }

        // Propriedade sobrescrita que indica a posição de destino do movimento.
        public override Position ToPos { get; }

        // Campo somente leitura que indica a direção do movimento do rei.
        private readonly Direction kingMoveDir;

        // Campo somente leitura que armazena a posição de origem da torre.
        private readonly Position rookFromPos;

        // Campo somente leitura que armazena a posição de destino da torre.
        private readonly Position rookToPos;

        // Construtor da classe Castle que inicializa um movimento de roque.
        public Castle(MoveType type, Position kingPos)
        {
            Type = type; // Inicializa a propriedade Type com o tipo de movimento fornecido.
            FromPos = kingPos; // Inicializa a propriedade FromPos com a posição do rei.

            // Verifica se o tipo de movimento é roque pelo lado do rei (kingside).
            if (type == MoveType.CastleKS)
            {
                kingMoveDir = Direction.East; // Define a direção do movimento do rei.
                ToPos = new Position(kingPos.Row, 6); // Define a posição de destino do rei.
                rookFromPos = new Position(kingPos.Row, 7); // Define a posição de origem da torre.
                rookToPos = new Position(kingPos.Row, 5); // Define a posição de destino da torre.
            }
            // Verifica se o tipo de movimento é roque pelo lado da dama (queenside).
            else if (type == MoveType.CastleQS)
            {
                kingMoveDir = Direction.West; // Define a direção do movimento do rei.
                ToPos = new Position(kingPos.Row, 2); // Define a posição de destino do rei.
                rookFromPos = new Position(kingPos.Row, 0); // Define a posição de origem da torre.
                rookToPos = new Position(kingPos.Row, 3); // Define a posição de destino da torre.
            }
        }

        // Método sobrescrito que executa o movimento de roque no tabuleiro.
        public override void Execute(Board board)
        {
            new NormalMove(FromPos, ToPos).Execute(board); // Executa o movimento do rei.
            new NormalMove(rookFromPos, rookToPos).Execute(board); // Executa o movimento da torre.
        }

        // Método sobrescrito que verifica se o movimento de roque é legal.
        public override bool IsLegal(Board board)
        {
            Player player = board[FromPos].Color; // Obtém o jogador atual com base na posição de origem.

            // Verifica se o rei do jogador está em xeque.
            if (board.IsInCheck(player))
            {
                return false; // Retorna false se o rei estiver em xeque.
            }

            Board copy = board.Copy(); // Cria uma cópia do tabuleiro.
            Position kingPosIncopy = FromPos; // Inicializa a posição do rei na cópia.

            // Loop para verificar as duas posições intermediárias do movimento do rei.
            for (int i = 0; i < 2; i++)
            {
                new NormalMove(kingPosIncopy, kingPosIncopy + kingMoveDir).Execute(copy); // Executa o movimento do rei na cópia.
                kingPosIncopy += kingMoveDir; // Atualiza a posição do rei na cópia.

                // Verifica se o rei está em xeque na nova posição.
                if (copy.IsInCheck(player))
                {
                    return false; // Retorna false se o rei estiver em xeque.
                }
            }

            return true; // Retorna true se o movimento de roque for legal.
        }
    }
}
