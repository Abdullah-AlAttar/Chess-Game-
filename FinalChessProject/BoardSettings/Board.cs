using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FinalChessProject.PiecesSettings;
namespace FinalChessProject.BoardSettings
{
    public class Board
    {
        private Piece[,] boardPieces;

        public Board()
        {
            this.boardPieces = new Piece[8, 8];
            setStartingBoard();
        }
        public Board(Board board)
        {
           
            this.boardPieces = new Piece[8, 8];
            Piece[,] tmpPieces = board.getBoardPieces();
            for (int i = 0; i < 8; ++i)
                for (int j = 0; j < 8; ++j)
                {
                    if (tmpPieces[i, j] != null)
                    {
                        switch ((int)tmpPieces[i, j].getPieceType())
                        {
                            case 1:
                                boardPieces[i, j] = new Rook(Tuple.Create(i, j), pieceType.blackRook, pieceColor.BLack);
                                break;
                            case 2:
                                boardPieces[i, j] = new Rook(Tuple.Create(i, j), pieceType.whiteRook, pieceColor.White);
                                break;
                            case 3:
                                boardPieces[i, j] = new Knight(Tuple.Create(i, j), pieceType.blackKnight, pieceColor.BLack);
                                break;
                            case 4:
                                boardPieces[i, j] = new Knight(Tuple.Create(i, j), pieceType.whiteKnight, pieceColor.White);
                                break;
                            case 5:
                                boardPieces[i, j] = new Bishop(Tuple.Create(i, j), pieceType.blackBishop, pieceColor.BLack);
                                break;
                            case 6:
                                boardPieces[i, j] = new Bishop(Tuple.Create(i, j), pieceType.whiteBishop, pieceColor.White);
                                break;
                            case 7:
                                boardPieces[i, j] = new Queen(Tuple.Create(i, j), pieceType.blackQueen, pieceColor.BLack);
                                break;
                            case 8:
                                boardPieces[i, j] = new Queen(Tuple.Create(i, j), pieceType.whiteQueen, pieceColor.White);
                                break;
                            case 9:
                                boardPieces[i, j] = new King(Tuple.Create(i, j), pieceType.blackKing, pieceColor.BLack);
                                break;
                            case 10:
                                boardPieces[i, j] = new King(Tuple.Create(i, j), pieceType.whiteKing, pieceColor.White);

                                break;
                            case 11:
                                boardPieces[i, j] = new Pawn(Tuple.Create(i, j), pieceType.blackPawn, pieceColor.BLack);
                                break;
                            case 12:
                                boardPieces[i, j] = new Pawn(Tuple.Create(i, j), pieceType.whitePawn, pieceColor.White);
                                break;
                        }
                    }
                    else
                        boardPieces[i, j] = null;
                }

        }
        public Piece[,] getBoardPieces()
        {
            return this.boardPieces;
        }
        public Tuple<int, int> getPlayerKingPosition(pieceColor kingColor)
        {
            for (int i = 0; i < 8; ++i)
                for (int j = 0; j < 8; ++j)
                {
                    if (!isEmptyCell(i, j) && kingColor == pieceColor.White && boardPieces[i, j].getPieceType() == (int)pieceType.whiteKing)
                        return Tuple.Create(i, j);
                    else if (!isEmptyCell(i, j) && kingColor == pieceColor.BLack && boardPieces[i, j].getPieceType() == (int)pieceType.blackKing)
                        return Tuple.Create(i, j);
                }
            return Tuple.Create(-1, -1);
        }
        public Board setStartingBoard()
        {
            Piece[,] boardPieces = new Piece[8, 8];
            boardPieces[0, 0] = new Rook(Tuple.Create(0, 0), pieceType.blackRook, pieceColor.BLack);
            boardPieces[0, 1] = new Knight(Tuple.Create(0, 1), pieceType.blackKnight, pieceColor.BLack);
            boardPieces[0, 2] = new Bishop(Tuple.Create(0, 2), pieceType.blackBishop, pieceColor.BLack);
            boardPieces[0, 3] = new Queen(Tuple.Create(0, 3), pieceType.blackQueen, pieceColor.BLack);
            boardPieces[0, 4] = new King(Tuple.Create(0, 4), pieceType.blackKing, pieceColor.BLack);
            boardPieces[0, 5] = new Bishop(Tuple.Create(0, 5), pieceType.blackBishop, pieceColor.BLack);
            boardPieces[0, 6] = new Knight(Tuple.Create(0, 6), pieceType.blackKnight, pieceColor.BLack);
            boardPieces[0, 7] = new Rook(Tuple.Create(0, 7), pieceType.blackRook, pieceColor.BLack);

            boardPieces[1, 0] = new Pawn(Tuple.Create(1, 0), pieceType.blackPawn, pieceColor.BLack);
            boardPieces[1, 1] = new Pawn(Tuple.Create(1, 1), pieceType.blackPawn, pieceColor.BLack);
            boardPieces[1, 2] = new Pawn(Tuple.Create(1, 2), pieceType.blackPawn, pieceColor.BLack);
            boardPieces[1, 3] = new Pawn(Tuple.Create(1, 3), pieceType.blackPawn, pieceColor.BLack);
            boardPieces[1, 4] = new Pawn(Tuple.Create(1, 4), pieceType.blackPawn, pieceColor.BLack);
            boardPieces[1, 5] = new Pawn(Tuple.Create(1, 5), pieceType.blackPawn, pieceColor.BLack);
            boardPieces[1, 6] = new Pawn(Tuple.Create(1, 6), pieceType.blackPawn, pieceColor.BLack);
            boardPieces[1, 7] = new Pawn(Tuple.Create(1, 7), pieceType.blackPawn, pieceColor.BLack);

            boardPieces[7, 0] = new Rook(Tuple.Create(7, 0), pieceType.whiteRook, pieceColor.White);
            boardPieces[7, 1] = new Knight(Tuple.Create(7, 1), pieceType.whiteKnight, pieceColor.White);
            boardPieces[7, 2] = new Bishop(Tuple.Create(7, 2), pieceType.whiteBishop, pieceColor.White);
            boardPieces[7, 3] = new Queen(Tuple.Create(7, 3), pieceType.whiteQueen, pieceColor.White);
            boardPieces[7, 4] = new King(Tuple.Create(7, 4), pieceType.whiteKing, pieceColor.White);
            boardPieces[7, 5] = new Bishop(Tuple.Create(7, 5), pieceType.whiteBishop, pieceColor.White);
            boardPieces[7, 6] = new Knight(Tuple.Create(7, 6), pieceType.whiteKnight, pieceColor.White);
            boardPieces[7, 7] = new Rook(Tuple.Create(7, 7), pieceType.whiteRook, pieceColor.White);

            boardPieces[6, 0] = new Pawn(Tuple.Create(6, 0), pieceType.whitePawn, pieceColor.White);
            boardPieces[6, 1] = new Pawn(Tuple.Create(6, 1), pieceType.whitePawn, pieceColor.White);
            boardPieces[6, 2] = new Pawn(Tuple.Create(6, 2), pieceType.whitePawn, pieceColor.White);
            boardPieces[6, 3] = new Pawn(Tuple.Create(6, 3), pieceType.whitePawn, pieceColor.White);
            boardPieces[6, 4] = new Pawn(Tuple.Create(6, 4), pieceType.whitePawn, pieceColor.White);
            boardPieces[6, 5] = new Pawn(Tuple.Create(6, 5), pieceType.whitePawn, pieceColor.White);
            boardPieces[6, 6] = new Pawn(Tuple.Create(6, 6), pieceType.whitePawn, pieceColor.White);
            boardPieces[6, 7] = new Pawn(Tuple.Create(6, 7), pieceType.whitePawn, pieceColor.White);
            this.boardPieces = boardPieces;
            return this;
        }

        public int[,] getBoard()
        {
            int[,] board = new int[8, 8];
            for (int i = 0; i < 8; ++i)
                for (int j = 0; j < 8; ++j)
                {
                    board[i, j] = (boardPieces[i, j] == null ? 0 : (int)boardPieces[i, j].getPieceType());
                }
            return board;
        }
        public bool isEmptyCell(int row, int col)
        {
            return this.boardPieces[row, col] == null;
        }
       

    }
}


