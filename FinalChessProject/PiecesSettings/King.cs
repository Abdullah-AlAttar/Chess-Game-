using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalChessProject.BoardSettings;
using System.Windows.Forms;
namespace FinalChessProject.PiecesSettings
{
    class King : Piece
    {
        private readonly int[] kingMovement = { 0, -2, 0, 2,  1, -1, 0, -1, -1, -1, -1, 0, -1, 1, 0, 1, 1, 1, 1, 0 };
        private readonly int[,] whiteKingTable = new int[,]
        {
              {-30,-40,-40,-50,-50,-40,-40,-30},
              {-30,-40,-40,-50,-50,-40,-40,-30},
              {-30,-40,-40,-50,-50,-40,-40,-30},
              {-30,-40,-40,-50,-50,-40,-40,-30},
              {-20,-30,-30,-40,-40,-30,-30,-20},
              {-10,-20,-20,-20,-20,-20,-20,-10},
              {20, 20,  0,  0,  0,  0, 20, 20},
              {20, 20, 10,  0,  0, 10, 20, 20}
        };
        private readonly int[,] blackKingTable = new int[,]
        {
            {20, 20, 10,  0,  0, 10, 20, 20},
            {20, 20,  0,  0,  0,  0, 20, 20},
            {-10,-20,-20,-20,-20,-20,-20,-10},
            {-20,-30,-30,-40,-40,-30,-30,-20},
            {-30,-40,-40,-50,-50,-40,-40,-30},
            {-30,-40,-40,-50,-50,-40,-40,-30},
            {-30,-40,-40,-50,-50,-40,-40,-30},
            {-30,-40,-40,-50,-50,-40,-40,-30}
        };
        public King(Tuple<int, int> piecePosition, pieceType type, pieceColor color) : base(piecePosition, type, color)
        {

        }
        public override void firstMoveOccurred()
        {
            this.firstMove = true;
        }
        public override void setPiecePosition(int row, int col)
        {
            this.piecePosition = Tuple.Create(row, col);
        }
        public override List<Move> getLegalMovesWithCheck(Board board)
        {
            List<Move> kingMoves = new List<Move>();
            pieceColor currentPieceColor = board.getBoardPieces()[this.getPiecePosition().Item1, this.getPiecePosition().Item2].getPieceColor();

            for (int i = 0; i < kingMovement.Length; i += 2)
            {
                int DestinationRow = this.getPiecePosition().Item1 + kingMovement[i];
                int DestinationCol = this.getPiecePosition().Item2 + kingMovement[i + 1];
                if (i == 0)
                {
                     if (Utility.canPerformLongCastling(board, this))
                        kingMoves.Add(new NormalMove(Tuple.Create(DestinationRow, DestinationCol)));

                }
                else if (i == 2)
                {
                    if(Utility.canPerformShortCastling(board, this))
                        kingMoves.Add(new NormalMove(Tuple.Create(DestinationRow, DestinationCol)));
                }
                else
                {
                    if (Utility.isValidMove(DestinationRow, DestinationCol) &&
                            Utility.kingStillSafe(board, this.getPiecePosition(), Tuple.Create(DestinationRow, DestinationCol)))
                    {

                        if (board.isEmptyCell(DestinationRow, DestinationCol))
                        {
                            kingMoves.Add(new NormalMove(Tuple.Create(DestinationRow, DestinationCol)));
                        }
                        else
                        {
                            pieceColor destinationPieceColor = board.getBoardPieces()[DestinationRow, DestinationCol].getPieceColor();
                            if (currentPieceColor != destinationPieceColor)
                                kingMoves.Add(new AttackMove(Tuple.Create(DestinationRow, DestinationCol)));

                        }
                    }
                }
            }
            return kingMoves;
        }
        public override List<Move> getLegalMovesWithoutCheck(Board board)
        {
            List<Move> kingMoves = new List<Move>();
            pieceColor currentPieceColor = board.getBoardPieces()[this.getPiecePosition().Item1, this.getPiecePosition().Item2].getPieceColor();
            for (int i = 0; i < kingMovement.Length; i += 2)
            {
                int DestinationRow = this.getPiecePosition().Item1 + kingMovement[i];
                int DestinationCol = this.getPiecePosition().Item2 + kingMovement[i + 1];
                if (Utility.isValidMove(DestinationRow, DestinationCol) )//&&
                {
                    if (board.isEmptyCell(DestinationRow, DestinationCol))
                    {
                        kingMoves.Add(new NormalMove(Tuple.Create(DestinationRow, DestinationCol)));
                    }
                    else
                    {
                        pieceColor destinationPieceColor = board.getBoardPieces()[DestinationRow, DestinationCol].getPieceColor();
                        if (currentPieceColor != destinationPieceColor)
                            kingMoves.Add(new AttackMove(Tuple.Create(DestinationRow, DestinationCol)));
                    }
                }
            }
            return kingMoves;
        }

        public override int getPositionalValue()
        {
            return this.getPieceColor() == pieceColor.White ? whiteKingTable[getPiecePosition().Item1, getPiecePosition().Item2] :
                                                          blackKingTable[getPiecePosition().Item1, getPiecePosition().Item2];
        }

        public override bool isFirstMove()
        {
            return firstMove == false;
        }
    }
}
