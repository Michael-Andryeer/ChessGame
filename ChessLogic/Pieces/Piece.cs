

namespace ChessLogic
{
    public abstract class Piece
    {
        public abstract PieceType Type {get;}

        public abstract Player Color {get;}

        public bool HasMoved { get; set; } = false;

        public abstract Piece Copy();
    }
}
