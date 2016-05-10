using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalChessProject.BoardSettings;
namespace FinalChessProject.PiecesSettings
{
    public class Bishop : Piece
    {
        private readonly int[] bishopMovement = { 1, 1, -1, -1, -1, 1, 1, -1 };
        private readonly int[,] whiteBishopTable = new int[,]
        {
            {-20,-10,-10,-10,-10,-10,-10,-20},
            {-10,  0,  0,  0,  0,  0,  0,-10},
            {-10,  0,  5, 10, 10,  5,  0,-10},
            {-10,  5,  5, 10, 10,  5,  5,-10},
            {-10,  0, 10, 10, 10, 10,  0,-10},
            {-10, 10, 10, 10, 10, 10, 10,-10},
            {-10,  5,  0,  0,  0,  0,  5,-10},
            {-20,-10,-10,-10,-10,-10,-10,-20},
        };
        private readonly int[,] blackBishopTable = new int[,]
      {
            {-20,-10,-10,-10,-10,-10,-10,-20},
            {-10,  5,  0,  0,  0,  0,  5,-10},
            {-10, 10, 10, 10, 10, 10, 10,-10},
            {-10,  0, 10, 10, 10, 10,  0,-10},
            {-10,  5,  5, 10, 10,  5,  5,-10},
            {-10,  0,  5, 10, 10,  5,  0,-10},
            {-10,  0,  0,  0,  0,  0,  0,-10},
            {-20,-10,-10,-10,-10,-10,-10,-20}
      };

        public Bishop(Tuple<int, int> piecePosition, pieceType type, pieceColor color) : base(piecePosition, type, color)
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
            List<Move> bishopMoves = new List<Move>();
            pieceColor currentPieceColor = board.getBoardPieces()[this.getPiecePosition().Item1, this.getPiecePosition().Item2].getPieceColor();
            for (int i = 0; i < bishopMovement.Length; i += 2)
            {
                int DestinationRow = this.getPiecePosition().Item1;
                int DestinationCol = this.getPiecePosition().Item2;
                while (Utility.isValidMove(DestinationRow, DestinationCol))
                {
                    DestinationRow += bishopMovement[i];
                    DestinationCol += bishopMovement[i + 1];
                    if (!Utility.isValidMove(DestinationRow, DestinationCol)) break;

                    if (board.isEmptyCell(DestinationRow, DestinationCol))
                    {
                        if (Utility.kingStillSafe(board, this.getPiecePosition(), Tuple.Create(DestinationRow, DestinationCol)))
                            bishopMoves.Add(new NormalMove(Tuple.Create(DestinationRow, DestinationCol)));
                    }
                    else
                    {
                        pieceColor destinationPieceColor = board.getBoardPieces()[DestinationRow, DestinationCol].getPieceColor();
                        if (currentPieceColor != destinationPieceColor)
                        {
                            if (Utility.kingStillSafe(board, this.getPiecePosition(), Tuple.Create(DestinationRow, DestinationCol)))
                                bishopMoves.Add(new AttackMove(Tuple.Create(DestinationRow, DestinationCol)));
                            break;
                        }
                        else
                            break;
                    }
                }
            }
            return bishopMoves;
        }


        public override List<Move> getLegalMovesWithoutCheck(Board board)
        {
            List<Move> bishopMoves = new List<Move>();
            pieceColor currentPieceColor = board.getBoardPieces()[this.getPiecePosition().Item1, this.getPiecePosition().Item2].getPieceColor();
            for (int i = 0; i < bishopMovement.Length; i += 2)
            {
                int DestinationRow = this.getPiecePosition().Item1;
                int DestinationCol = this.getPiecePosition().Item2;
                while (Utility.isValidMove(DestinationRow, DestinationCol))
                {
                    DestinationRow += bishopMovement[i];
                    DestinationCol += bishopMovement[i + 1];

                    if (Utility.isValidMove(DestinationRow, DestinationCol))// &&
                                                                            //  Utility.kingStillSafe(board, this.getPiecePosition(), Tuple.Create(DestinationRow, DestinationCol)))
                    {

                        if (board.isEmptyCell(DestinationRow, DestinationCol))
                        {
                            bishopMoves.Add(new NormalMove(Tuple.Create(DestinationRow, DestinationCol)));
                        }
                        else
                        {
                            pieceColor destinationPieceColor = board.getBoardPieces()[DestinationRow, DestinationCol].getPieceColor();
                            if (currentPieceColor != destinationPieceColor)
                            {
                                bishopMoves.Add(new AttackMove(Tuple.Create(DestinationRow, DestinationCol)));
                                break;
                            }
                            else
                                break;
                        }

                    }
                    else break;
                }
            }
            return bishopMoves;
        }

        public override int getPositionalValue()
        {
            return this.getPieceColor() == pieceColor.White ? whiteBishopTable[getPiecePosition().Item1, getPiecePosition().Item2] :
                                                        blackBishopTable[getPiecePosition().Item1, getPiecePosition().Item2];
        }
        public override bool isFirstMove()
        {
            return firstMove == false;
        }
    }
}
