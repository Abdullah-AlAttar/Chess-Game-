using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalChessProject.BoardSettings;
namespace FinalChessProject.PiecesSettings
{
    class Rook : Piece
    {
        private readonly int[] rookMovement = { 0, -1, -1, 0, 0, 1, 1, 0 };
        private readonly int[,] whiteRookTable = new int[,]
      {
        {0,  0,  0,  0,  0,  0,  0,  0},
        { 5, 10, 10, 10, 10, 10, 10,  5},
        {-5,  0,  0,  0,  0,  0,  0, -5},
        {-5,  0,  0,  0,  0,  0,  0, -5},
        {-5,  0,  0,  0,  0,  0,  0, -5},
        {-5,  0,  0,  0,  0,  0,  0, -5},
        {-5,  0,  0,  0,  0,  0,  0, -5},
        {0,  0,  0,  5,  5,  0,  0,  0},
      };
        private readonly int[,] blackRookTable = new int[,]
     {
        {0,  0,  0,  5,  5,  0,  0,  0},
        {-5,  0,  0,  0,  0,  0,  0, -5},
        {-5,  0,  0,  0,  0,  0,  0, -5},
        {-5,  0,  0,  0,  0,  0,  0, -5},
        {-5,  0,  0,  0,  0,  0,  0, -5},
        {-5,  0,  0,  0,  0,  0,  0, -5},
        {5, 10, 10, 10, 10, 10, 10,  5},
        {0,  0,  0,  0,  0,  0,  0,  0},
     };
        public Rook(Tuple<int, int> piecePosition, pieceType type, pieceColor color) : base(piecePosition, type, color)
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
            List<Move> rookMoves = new List<Move>();
            pieceColor currentPieceColor = board.getBoardPieces()[this.getPiecePosition().Item1, this.getPiecePosition().Item2].getPieceColor();
       
            for (int i = 0; i < rookMovement.Length; i += 2)
            {
                int DestinationRow = this.getPiecePosition().Item1;
                int DestinationCol = this.getPiecePosition().Item2;
               
                while (Utility.isValidMove(DestinationRow, DestinationCol) )
                {
                    DestinationRow += rookMovement[i];
                    DestinationCol += rookMovement[i + 1];
                    if (!Utility.isValidMove(DestinationRow, DestinationCol)) break;

                    if (board.isEmptyCell(DestinationRow, DestinationCol))
                    {
                        if (Utility.kingStillSafe(board, this.getPiecePosition(), Tuple.Create(DestinationRow, DestinationCol)))
                            rookMoves.Add(new NormalMove(Tuple.Create(DestinationRow, DestinationCol)));
                    }
                    else
                    {

                        pieceColor destinationPieceColor = board.getBoardPieces()[DestinationRow, DestinationCol].getPieceColor();
                        if (currentPieceColor != destinationPieceColor)
                        {
                            if (Utility.kingStillSafe(board, this.getPiecePosition(), Tuple.Create(DestinationRow, DestinationCol)))
                                rookMoves.Add(new AttackMove(Tuple.Create(DestinationRow, DestinationCol)));
                            break;
                        }
                        else
                            break;
                    }
                }
            }
            return rookMoves;
        }
        public override List<Move> getLegalMovesWithoutCheck(Board board)
        {
            List<Move> rookMoves = new List<Move>();
            pieceColor currentPieceColor = board.getBoardPieces()[this.getPiecePosition().Item1, this.getPiecePosition().Item2].getPieceColor();
            for (int i = 0; i < rookMovement.Length; i += 2)
            {
                int DestinationRow = this.getPiecePosition().Item1;
                int DestinationCol = this.getPiecePosition().Item2;
                
                while (Utility.isValidMove(DestinationRow, DestinationCol))
                {
                    DestinationRow += rookMovement[i];
                    DestinationCol += rookMovement[i + 1];

                    if (Utility.isValidMove(DestinationRow, DestinationCol))
                    {
                        if (board.isEmptyCell(DestinationRow, DestinationCol))
                        {
                            rookMoves.Add(new NormalMove(Tuple.Create(DestinationRow, DestinationCol)));
                        }
                        else
                        {

                            pieceColor destinationPieceColor = board.getBoardPieces()[DestinationRow, DestinationCol].getPieceColor();
                            if (currentPieceColor != destinationPieceColor)
                            {
                                rookMoves.Add(new AttackMove(Tuple.Create(DestinationRow, DestinationCol)));
                                break;
                            }
                            else
                                break;
                        }

                    }
                    else break;
                }
            }
            return rookMoves;
        }

        public override int getPositionalValue()
        {
            return this.getPieceColor() == pieceColor.White ? whiteRookTable[getPiecePosition().Item1, getPiecePosition().Item2] :
                                                          blackRookTable[getPiecePosition().Item1, getPiecePosition().Item2];
        }
        public override bool isFirstMove()
        {
            return firstMove == false;
        }
    }
}
