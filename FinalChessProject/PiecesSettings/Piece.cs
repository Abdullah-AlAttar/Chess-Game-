using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalChessProject.BoardSettings;
namespace FinalChessProject.PiecesSettings
{
    public enum pieceType
    {
        blackRook = 1, blackKnight = 3, blackBishop = 5, blackQueen = 7, blackKing = 9, blackPawn = 11,
        whiteRook = 2, whiteKnight = 4, whiteBishop = 6, whiteQueen = 8, whiteKing = 10, whitePawn = 12
    }
    public enum pieceColor { White = -1, BLack = 1 }
    public abstract class Piece
    {
        protected Tuple<int, int> piecePosition;
        protected bool firstMove;
        protected pieceType type;
        protected pieceColor color;

        public Piece() { }
       
        public Piece(Tuple<int, int> piecePosition, pieceType type, pieceColor color)
        {
            this.piecePosition = piecePosition;
            this.type = type;
            this.color = color;
        }
        public Tuple<int, int> getPiecePosition()
        {
            return this.piecePosition;
        }
        public int getPieceType()
        {
            return (int)this.type;
        }

        public pieceColor getPieceColor()
        {
            return this.color;
        }
        public abstract void setPiecePosition(int row, int col);
        public abstract List<Move> getLegalMovesWithCheck(Board board);
        public abstract List<Move> getLegalMovesWithoutCheck(Board board);
        public abstract int getPositionalValue();
        public abstract void firstMoveOccurred();
        public abstract bool isFirstMove();
    }
}