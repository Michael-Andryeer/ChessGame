using ChessLogic.Pieces; // Importa o namespace ChessLogic.Pieces que contém definições de peças de xadrez.

namespace ChessLogic
{
    // Define uma classe chamada PawnPromotion que herda da classe Move.
    public class PawnPromotion : Move
    {
        // Sobrescreve a propriedade Type para retornar MoveType.PawnPromotion.
        public override MoveType Type => MoveType.PawnPromotion;

        // Sobrescreve a propriedade FromPos para obter a posição inicial do movimento.
        public override Position FromPos { get; }

        // Sobrescreve a propriedade ToPos para obter a posição final do movimento.
        public override Position ToPos { get; }

        // Declara um campo readonly para armazenar o tipo da nova peça após a promoção.
        private readonly PieceType newType;

        // Construtor da classe PawnPromotion que inicializa as propriedades FromPos, ToPos e newType.
        public PawnPromotion(Position from, Position to, PieceType newType)
        {
            FromPos = from; // Inicializa a posição inicial.
            ToPos = to; // Inicializa a posição final.
            this.newType = newType; // Inicializa o tipo da nova peça.
        }

        // Método privado que cria uma nova peça de promoção com base no tipo e na cor do jogador.
        private Piece CreatePromotionPiece(Player color)
        {
            // Usa uma expressão switch para retornar a nova peça com base no tipo.
            return newType switch
            {
                PieceType.Knight => new Knight(color), // Retorna um Cavalo se o tipo for Knight.
                PieceType.Bishop => new Bishop(color), // Retorna um Bispo se o tipo for Bishop.
                PieceType.Rook => new Rook(color), // Retorna uma Torre se o tipo for Rook.
                _ => new Queen(color) // Retorna uma Rainha por padrão.
            };
        }

        // Sobrescreve o método Execute para executar o movimento de promoção no tabuleiro.
        public override void Execute(Board board)
        {
            // Obtém o peão na posição inicial.
            Piece pawn = board[FromPos];
            // Remove o peão da posição inicial.
            board[FromPos] = null;

            // Cria a peça de promoção com base na cor do peão.
            Piece promotionPiece = CreatePromotionPiece(pawn.Color);
            // Define que a peça de promoção já se moveu.
            promotionPiece.HasMoved = true;
            // Coloca a peça de promoção na posição final.
            board[ToPos] = promotionPiece;
        }
    }
}
