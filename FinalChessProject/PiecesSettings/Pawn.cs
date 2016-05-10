using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalChessProject.BoardSettings;
namespace FinalChessProject.PiecesSettings
{
    class Pawn : Piece
    {
        private readonly int[] pawnMovement = { 1, 0, 2, 0, 1, -1, 1, 1 };
        private readonly int[,] whitePawnTable = new int[,]
     {
         {60,  80,  80,  80,  80,  80,  80,  60},
         {50, 50, 50, 50, 50, 50, 50, 50},
         {10, 10, 20, 30, 30, 20, 10, 10},
         {5,  5, 10, 25, 25, 10,  5,  5},
         {0,  0,  0, 20, 20,  0,  0,  0},
         {5, -5,-10,  0,  0,-10, -5,  5},
         {5, 10, 10,-20,-20, 10, 10,  5},
         {0,  0,  0,  0,  0,  0,  0,  0}
     };
        private readonly int[,] blackPawnTable = new int[,]
     {
         {0,  0,  0,  0,  0,  0,  0,  0},
        {5, 10, 10,-20,-20, 10, 10,  5},
        {5, -5,-10,  0,  0,-10, -5,  5},
        {0,  0,  0, 20, 20,  0,  0,  0},
        {5,  5, 10, 25, 25, 10,  5,  5},
        {10, 10, 20, 30, 30, 20, 10, 10},
        {50, 50, 50, 50, 50, 50, 50, 50},
        {60,  80,  80,  80,  80, 80,  80,  60}
     };
        public Pawn(Tuple<int, int> piecePosition, pieceType type, pieceColor color) : base(piecePosition, type, color)
        {
            this.firstMove = true;
        }

        public override void firstMoveOccurred()
        {
            this.firstMove = false;
        }

        public override List<Move> getLegalMovesWithCheck(Board board)
        {
            List<Move> pawnMoves = new List<Move>();
            pieceColor currentPieceColor = board.getBoardPieces()[this.getPiecePosition().Item1, this.getPiecePosition().Item2].getPieceColor();
            for (int i = 0; i < pawnMovement.Length; i += 2)
            {
                int DestinationRow = this.getPiecePosition().Item1 + (int)this.getPieceColor() * pawnMovement[i];
                int DestinationCol = this.getPiecePosition().Item2 + pawnMovement[i + 1];
                if (!Utility.isValidMove(DestinationRow, DestinationCol) || 
                        !Utility.kingStillSafe(board, this.getPiecePosition(), Tuple.Create(DestinationRow, DestinationCol))) continue;

                if (i == 2 && this.firstMove)
                {
                    //checking if the next row is empty or not 
                    int nextRow = this.getPiecePosition().Item1 + (int)this.getPieceColor();
                    int nextCol = this.getPiecePosition().Item2 + pawnMovement[i + 1]; ;
                    if (board.isEmptyCell(nextRow, nextCol) && board.isEmptyCell(DestinationRow, DestinationCol))
                    {
                        pawnMoves.Add(new NormalMove(Tuple.Create(DestinationRow, DestinationCol))); 
                    }
                }
                else if (i == 0 && board.isEmptyCell(DestinationRow, DestinationCol))
                {
                    pawnMoves.Add(new NormalMove(Tuple.Create(DestinationRow, DestinationCol)));
                }
              
                else if (i == 4 || i == 6)
                {
                    if (board.isEmptyCell(DestinationRow, DestinationCol)) continue;
                    else
                    {
                        pieceColor destinationPieceColor = board.getBoardPieces()[DestinationRow, DestinationCol].getPieceColor();
                        if (currentPieceColor != destinationPieceColor)
                            pawnMoves.Add(new AttackMove(Tuple.Create(DestinationRow, DestinationCol)));
                        else continue;
                    }

                }
            }
            return pawnMoves;
        }
        public override List<Move> getLegalMovesWithoutCheck(Board board)
        {
            List<Move> pawnMoves = new List<Move>();
            pieceColor currentPieceColor = board.getBoardPieces()[this.getPiecePosition().Item1, this.getPiecePosition().Item2].getPieceColor();
            for (int i = 0; i < pawnMovement.Length; i += 2)
            {
                int DestinationRow = this.getPiecePosition().Item1 + (int)this.getPieceColor() * pawnMovement[i];
                int DestinationCol = this.getPiecePosition().Item2 + pawnMovement[i + 1];
                if (!Utility.isValidMove(DestinationRow, DestinationCol))continue;// ||
                     

                if (i == 2 && this.firstMove)
                {
                    //checking if the next row is empty or not 
                    int nextRow = this.getPiecePosition().Item1 + (int)this.getPieceColor();
                    int nextCol = this.getPiecePosition().Item2 + pawnMovement[i + 1]; ;
                    if (board.isEmptyCell(nextRow, nextCol) && board.isEmptyCell(DestinationRow, DestinationCol))
                    {
                        pawnMoves.Add(new NormalMove(Tuple.Create(DestinationRow, DestinationCol)));
                    }
                }
                else if (i == 0 && board.isEmptyCell(DestinationRow, DestinationCol))
                {
                    pawnMoves.Add(new NormalMove(Tuple.Create(DestinationRow, DestinationCol)));
                }
                else if (i == 4 || i == 6)
                {
                    if (board.isEmptyCell(DestinationRow, DestinationCol)) continue;
                    else
                    {
                        pieceColor destinationPieceColor = board.getBoardPieces()[DestinationRow, DestinationCol].getPieceColor();
                        if (currentPieceColor != destinationPieceColor)
                            pawnMoves.Add(new AttackMove(Tuple.Create(DestinationRow, DestinationCol)));
                        else continue;
                    }

                }
            }
            return pawnMoves;
        }


        public override void setPiecePosition(int row, int col)
        {
            this.piecePosition = Tuple.Create(row, col);
        }
        public override int getPositionalValue()
        {
            return this.getPieceColor() == pieceColor.White ? whitePawnTable[getPiecePosition().Item1, getPiecePosition().Item2] :
                                                          blackPawnTable[getPiecePosition().Item1, getPiecePosition().Item2];
        }
        public override bool isFirstMove()
        {
            return firstMove == false;
        }
    }
}
