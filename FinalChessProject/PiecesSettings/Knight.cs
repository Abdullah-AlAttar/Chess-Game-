using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FinalChessProject.BoardSettings;
namespace FinalChessProject.PiecesSettings
{
    class Knight : Piece
    {

        private readonly int[] knightMovment = { -1, -2, -2, -1, -2, 1, -1, 2, 1, 2, 2, 1, 2, -1, 1, -2 };
        private readonly int[,] whiteKnightTable = new int[,]
        {
            {-50,-40,-30,-30,-30,-30,-40,-50},
            {-40,-20,  0,  0,  0,  0,-20,-40},
            {-30,  0, 10, 15, 15, 10,  0,-30},
            {-30,  5, 15, 20, 20, 15,  5,-30},
            {-30,  0, 15, 20, 20, 15,  0,-30},
            {-30,  5, 10, 15, 15, 10,  5,-30},
            {-40,-20,  0,  5,  5,  0,-20,-40},
            {-50,-40,-30,-30,-30,-30,-40,-50}
        };
        private readonly int[,] blackKnightTable = new int[,]
      {
            {-50,-40,-30,-30,-30,-30,-40,-50},
            {-40,-20,  0,  5,  5,  0,-20,-40},
            {-30,  5, 10, 15, 15, 10,  5,-30},
            {-30,  0, 15, 20, 20, 15,  0,-30},
            {-30,  5, 15, 20, 20, 15,  5,-30},
            {-30,  0, 10, 15, 15, 10,  0,-30},
            {-40,-20,  0,  0,  0,  0,-20,-40},
            {-50,-40,-30,-30,-30,-30,-40,-50}
      };
        public Knight(Tuple<int, int> piecePosition, pieceType type, pieceColor color) : base(piecePosition, type, color)
        {

        }
        public override void setPiecePosition(int row, int col)
        {
            this.piecePosition = Tuple.Create(row, col);
        }
        public override void firstMoveOccurred()
        {
            this.firstMove = true;
        }
        public override List<Move> getLegalMovesWithCheck(Board board)
        {

            List<Move> knightMoves = new List<Move>();
            pieceColor currentPieceColor = board.getBoardPieces()[this.getPiecePosition().Item1, this.getPiecePosition().Item2].getPieceColor();

            for (int i = 0; i < knightMovment.Length; i += 2)
            {
                int DestinationRow = this.getPiecePosition().Item1 + knightMovment[i];
                int DestinationCol = this.getPiecePosition().Item2 + knightMovment[i + 1];
         
                if (Utility.isValidMove(DestinationRow, DestinationCol) &&
                 Utility.kingStillSafe(board, this.getPiecePosition(), Tuple.Create(DestinationRow, DestinationCol)))
                {
                    if (board.isEmptyCell(DestinationRow, DestinationCol))
                    {
                        knightMoves.Add(new NormalMove(Tuple.Create(DestinationRow, DestinationCol)));
                    }
                    else
                    {
                        pieceColor destinationPieceColor = board.getBoardPieces()[DestinationRow, DestinationCol].getPieceColor();
                        if (currentPieceColor != destinationPieceColor)
                            knightMoves.Add(new AttackMove(Tuple.Create(DestinationRow, DestinationCol)));
                    }
                }
            }
            return knightMoves;
        }

        public override List<Move> getLegalMovesWithoutCheck(Board board)
        {

            List<Move> knightMoves = new List<Move>();
            pieceColor currentPieceColor = board.getBoardPieces()[this.getPiecePosition().Item1, this.getPiecePosition().Item2].getPieceColor();
            for (int i = 0; i < knightMovment.Length; i += 2)
            {
                int DestinationRow = this.getPiecePosition().Item1 + knightMovment[i];
                int DestinationCol = this.getPiecePosition().Item2 + knightMovment[i + 1];
         
                if (Utility.isValidMove(DestinationRow, DestinationCol))
                {
                    if (board.isEmptyCell(DestinationRow, DestinationCol))
                    {
                        knightMoves.Add(new NormalMove(Tuple.Create(DestinationRow, DestinationCol)));
                    }
                    else
                    {
                        pieceColor destinationPieceColor = board.getBoardPieces()[DestinationRow, DestinationCol].getPieceColor();
                        if (currentPieceColor != destinationPieceColor)
                            knightMoves.Add(new AttackMove(Tuple.Create(DestinationRow, DestinationCol)));
                    }
                }
            }
            return knightMoves;
        }

        public override int getPositionalValue()
        {
            return this.getPieceColor() == pieceColor.White ? whiteKnightTable[getPiecePosition().Item1, getPiecePosition().Item2] :
                                                      blackKnightTable[getPiecePosition().Item1, getPiecePosition().Item2];
        }
        public override bool isFirstMove()
        {
            return firstMove == false;
        }
    }
}
