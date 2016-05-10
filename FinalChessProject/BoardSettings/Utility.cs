using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FinalChessProject.PiecesSettings;
using FinalChessProject.BoardSettings;

using FinalChessProject.PlayersSettings;
using FinalChessProject;

namespace FinalChessProject.BoardSettings
{

    public static class Utility
    {

        public static bool isValidMove(int row, int col)
        {
            return row >= 0 && col >= 0 && row <= 7 && col <= 7;
        }

        public static List<Move> getPlayerLegalMove(Board board, pieceColor color)
        {
            List<Move> legalMoves = new List<Move>();
            for (int i = 0; i < 8; ++i)
                for (int j = 0; j < 8; ++j)
                    if (!board.isEmptyCell(i, j) && board.getBoardPieces()[i, j].getPieceColor() == color)
                        legalMoves.AddRange(board.getBoardPieces()[i, j].getLegalMovesWithoutCheck(board));
            return legalMoves;
        }

        public static bool kingStillSafe(Board board, Tuple<int, int> currentPosition, Tuple<int, int> destinationPosition)
        {
            if (board.isEmptyCell(currentPosition.Item1, currentPosition.Item2))
                return true;
            if (board.getBoardPieces()[currentPosition.Item1, currentPosition.Item2].getPieceColor() == pieceColor.White)
            {

                Board tmpBoard = new Board(board);
                Piece tmpPiece = tmpBoard.getBoardPieces()[currentPosition.Item1, currentPosition.Item2];
                Utility.swapPieces(tmpBoard, tmpPiece, currentPosition, destinationPosition);
               
                Tuple<int, int> activePlayerKingPosition = tmpBoard.getPlayerKingPosition(pieceColor.White);
                List<Move> opponentLegalMoves = getPlayerLegalMove(tmpBoard, pieceColor.BLack);
                foreach (Move m in opponentLegalMoves)
                {
                    if (m.getMovePosition().Item1 == activePlayerKingPosition.Item1 &&
                        m.getMovePosition().Item2 == activePlayerKingPosition.Item2)
                        return false;
                }
            }
            else if (board.getBoardPieces()[currentPosition.Item1, currentPosition.Item2].getPieceColor() == pieceColor.BLack)
            {

                Board tmpBoard = new Board(board);
                Piece tmpPiece = tmpBoard.getBoardPieces()[currentPosition.Item1, currentPosition.Item2];
                Utility.swapPieces(tmpBoard, tmpPiece, currentPosition, destinationPosition);
              
                Tuple<int, int> activePlayerKingPosition = tmpBoard.getPlayerKingPosition(pieceColor.BLack);
                List<Move> opponentLegalMoves = getPlayerLegalMove(tmpBoard, pieceColor.White);
                foreach (Move m in opponentLegalMoves)
                {
                    if (m.getMovePosition().Item1 == activePlayerKingPosition.Item1 &&
                        m.getMovePosition().Item2 == activePlayerKingPosition.Item2)
                        return false;
                }
            }
            return true;
        }

        public static bool canPerformShortCastling(Board board, Piece king)
        {
            if (king.getPieceColor() == pieceColor.White)
            {
                List<Move> enemyLegalMoves = Utility.getPlayerLegalMove(board, pieceColor.BLack);
                foreach (Move m in enemyLegalMoves)
                    if (m.getMovePosition().Item1 == king.getPiecePosition().Item1 &&
                        m.getMovePosition().Item2 == king.getPiecePosition().Item2)
                        return false;
               
                if (board.isEmptyCell(7, 7))
                    return false;
                if (board.getBoardPieces()[7, 7].getPieceType() != (int)pieceType.whiteRook)
                    return false;
                if (!king.isFirstMove() ||
                   !board.isEmptyCell(7, 5) || !board.isEmptyCell(7, 6) ||
                   !board.getBoardPieces()[7, 7].isFirstMove() ||
                   !Utility.kingStillSafe(board, Tuple.Create(7, 4), Tuple.Create(7, 5)) ||
                   !Utility.kingStillSafe(board, Tuple.Create(7, 4), Tuple.Create(7, 6)))
                    return false;
            }
            else
            {
                List<Move> enemyLegalMoves = Utility.getPlayerLegalMove(board, pieceColor.White);
                foreach (Move m in enemyLegalMoves)
                    if (m.getMovePosition().Item1 == king.getPiecePosition().Item1 &&
                      m.getMovePosition().Item2 == king.getPiecePosition().Item2)
                        return false;
                if (board.isEmptyCell(0, 7))
                    return false;
                if (board.getBoardPieces()[0, 7].getPieceType() != (int)pieceType.blackRook)
                    return false;
                if (!king.isFirstMove() ||
                   !board.isEmptyCell(0, 5) || !board.isEmptyCell(0, 6) ||
                   !board.getBoardPieces()[0, 7].isFirstMove() ||
                   !Utility.kingStillSafe(board, Tuple.Create(0, 4), Tuple.Create(0, 5)) ||
                   !Utility.kingStillSafe(board, Tuple.Create(0, 4), Tuple.Create(0, 6)))
                    return false;
            }
            return true;
        }

        public static bool canPerformLongCastling(Board board, Piece king)
        {
            if (king.getPieceColor() == pieceColor.White)
            {
                List<Move> enemyLegalMoves = Utility.getPlayerLegalMove(board, pieceColor.BLack);
                foreach (Move m in enemyLegalMoves)
                    if (m.getMovePosition().Item1 == king.getPiecePosition().Item1 &&
                      m.getMovePosition().Item2 == king.getPiecePosition().Item2)
                        return false;

                if (board.isEmptyCell(7, 0))
                    return false;
                if (board.getBoardPieces()[7, 0].getPieceType() != (int)pieceType.whiteRook)
                    return false;
                if (!king.isFirstMove() ||
                   !board.isEmptyCell(7, 1) || !board.isEmptyCell(7, 2) || !board.isEmptyCell(7, 3) ||
                   !board.getBoardPieces()[7, 0].isFirstMove()||
                   !Utility.kingStillSafe(board,Tuple.Create(7,4),Tuple.Create(7,3)) ||
                   !Utility.kingStillSafe(board, Tuple.Create(7, 4), Tuple.Create(7, 2)))
                    return false;
            }
            else
            {
                List<Move> enemyLegalMoves = Utility.getPlayerLegalMove(board, pieceColor.White);
                foreach (Move m in enemyLegalMoves)
                    if (m.getMovePosition().Item1 == king.getPiecePosition().Item1 &&
                       m.getMovePosition().Item2 == king.getPiecePosition().Item2)
                        return false;

                if (board.isEmptyCell(0, 0))
                    return false;
                if (board.getBoardPieces()[0, 0].getPieceType() != (int)pieceType.blackRook)
                    return false;
                if (!king.isFirstMove() ||
                   !board.isEmptyCell(0, 1) || !board.isEmptyCell(0, 2) || !board.isEmptyCell(0, 3) ||
                   !board.getBoardPieces()[0, 0].isFirstMove() ||
                   !Utility.kingStillSafe(board, Tuple.Create(0, 4), Tuple.Create(0, 3)) ||
                   !Utility.kingStillSafe(board, Tuple.Create(0, 4), Tuple.Create(0, 2)))
                    return false;
            }
            return true;
        }

        public static void handleWhitePawnMove(Board board, Piece tmpPiece, Move m)
        {
            tmpPiece.firstMoveOccurred();
            //this part handles pawn promotion 
            if (m.getMovePosition().Item1 == 0)
            {
                if (m.isAttackMove())
                {
                    System.Media.SoundPlayer playersound = new System.Media.SoundPlayer(Properties.Resources.record20160425102455PM_mp3cut);
                    playersound.Play();
                }
                else
                {
                    System.Media.SoundPlayer playersound = new System.Media.SoundPlayer(Properties.Resources.recordM_mp3cut);
                    playersound.Play();
                }
                board.getBoardPieces()[m.getMovePosition().Item1, m.getMovePosition().Item2] =
                new Queen(Tuple.Create(m.getMovePosition().Item1, m.getMovePosition().Item2), pieceType.whiteQueen, pieceColor.White);
                board.getBoardPieces()[tmpPiece.getPiecePosition().Item1, tmpPiece.getPiecePosition().Item2] = null;
             
            }
            //this part handles  pawn move
            else
            {
                if (m.isAttackMove())
                {
                    System.Media.SoundPlayer playersound = new System.Media.SoundPlayer(Properties.Resources.record20160425102455PM_mp3cut);
                    playersound.Play();
                }
                else
                {
                    System.Media.SoundPlayer playersound = new System.Media.SoundPlayer(Properties.Resources.recordM_mp3cut);
                    playersound.Play();
                }
                Utility.swapPieces(board, tmpPiece, tmpPiece.getPiecePosition(), m.getMovePosition());
            }

        }

        public static bool isCastlingMove(Piece tmpPiece, Move m)
        {
            return (Math.Abs(tmpPiece.getPiecePosition().Item2 - m.getMovePosition().Item2) == 2);
        }

        public static void handleWhiteKingCastling(Board board, Piece tmpPiece, Move m)
        {
          
            Piece associatedRooK;
            if (tmpPiece.getPiecePosition().Item2 > m.getMovePosition().Item2)
            {
                Utility.swapPieces(board, tmpPiece, tmpPiece.getPiecePosition(), m.getMovePosition());
                associatedRooK = board.getBoardPieces()[7, 0];
                Utility.swapPieces(board, associatedRooK, associatedRooK.getPiecePosition(), Tuple.Create(7, 3));
            }
            else
            {
                Utility.swapPieces(board, tmpPiece, tmpPiece.getPiecePosition(), m.getMovePosition());
                associatedRooK = board.getBoardPieces()[7, 7];
                Utility.swapPieces(board, associatedRooK, associatedRooK.getPiecePosition(), Tuple.Create(7, 5));
            }
            associatedRooK.firstMoveOccurred();
            tmpPiece.firstMoveOccurred();
            System.Media.SoundPlayer playersound = new System.Media.SoundPlayer(Properties.Resources.recordM_mp3cut);
            playersound.Play();
        }

        public static void swapPieces(Board board, Piece tmpPiece, Tuple<int, int> from, Tuple<int, int> to)
        {
            tmpPiece.setPiecePosition(to.Item1, to.Item2);
            board.getBoardPieces()[to.Item1, to.Item2] = tmpPiece;
            board.getBoardPieces()[from.Item1, from.Item2] = null;
        }

        public static void handlePieceMove(Board board, Piece tmpPiece, Move m)
        {
            if (!tmpPiece.isFirstMove())
                tmpPiece.firstMoveOccurred();
            if (m.isAttackMove())
            {
                System.Media.SoundPlayer playersound = new System.Media.SoundPlayer(Properties.Resources.record20160425102455PM_mp3cut);
                playersound.Play();
            }
            else {
                System.Media.SoundPlayer playersound = new System.Media.SoundPlayer(Properties.Resources.recordM_mp3cut);
                playersound.Play();
            }
            Utility.swapPieces(board, tmpPiece, tmpPiece.getPiecePosition(), m.getMovePosition());
        }

        public static void handleblackKingCastling(Board board, Piece tmpPiece, Move m)
        {
             Piece associatedRooK;
            if (tmpPiece.getPiecePosition().Item2 > m.getMovePosition().Item2)
            {
                Utility.swapPieces(board, tmpPiece, tmpPiece.getPiecePosition(), m.getMovePosition());
                associatedRooK = board.getBoardPieces()[0, 0];
                Utility.swapPieces(board, associatedRooK, associatedRooK.getPiecePosition(), Tuple.Create(0, 3));
            }
            else
            {
                Utility.swapPieces(board, tmpPiece, tmpPiece.getPiecePosition(), m.getMovePosition());
                associatedRooK = board.getBoardPieces()[0, 7];
                Utility.swapPieces(board, associatedRooK, associatedRooK.getPiecePosition(), Tuple.Create(0, 5));
            }
            associatedRooK.firstMoveOccurred();
            tmpPiece.firstMoveOccurred();
            System.Media.SoundPlayer playersound = new System.Media.SoundPlayer(Properties.Resources.recordM_mp3cut);
            playersound.Play();
        }

        public static void checkBlackKingStatus(Board board, Player player, List<Piece> whitePlayerActivePieces, List<Move> whitePlayerLegalMoves, List<Piece> blackPlayerActivePieces, List<Move> blackPlayerLegalMoves)
        {

            whitePlayerActivePieces = player.findActivePlayerPieces(board);
            whitePlayerLegalMoves = player.calculateActivePlayerMoves(board, whitePlayerActivePieces);
            player = new BlackPlayer();
            blackPlayerActivePieces = player.findActivePlayerPieces(board);
            blackPlayerLegalMoves = player.calculateActivePlayerMoves(board, blackPlayerActivePieces);

            Tuple<int, int> blackPlayerKingPosition = board.getPlayerKingPosition(pieceColor.BLack);

            if (player.kingInCheck(blackPlayerKingPosition, whitePlayerLegalMoves))
            {
                System.Media.SoundPlayer playersound = new System.Media.SoundPlayer(Properties.Resources.record2M_mp3cut);
                playersound.Play();
            }
            if (blackPlayerActivePieces.Count() == 0)
                MessageBox.Show("The game is over white player wins");

         
        }

        public static void handleBlackPawnMove(Board board, Piece tmpPiece, Move m)
        {
            tmpPiece.firstMoveOccurred();
            if (m.getMovePosition().Item1 == 7)
            {
                if (m.isAttackMove())
                {
                    //  KilledFromBlack(board.getBoardPieces()[x, y].getPieceType());
                    System.Media.SoundPlayer playersound = new System.Media.SoundPlayer(Properties.Resources.record20160425102455PM_mp3cut);
                    playersound.Play();
                }
                else
                {
                    System.Media.SoundPlayer playersound = new System.Media.SoundPlayer(Properties.Resources.recordM_mp3cut);
                    playersound.Play();
                }
                board.getBoardPieces()[m.getMovePosition().Item1, m.getMovePosition().Item2] =
                new Queen(Tuple.Create(m.getMovePosition().Item1, m.getMovePosition().Item2), pieceType.whiteQueen, pieceColor.White);
                board.getBoardPieces()[tmpPiece.getPiecePosition().Item1, tmpPiece.getPiecePosition().Item2] = null;

            }
            //this part handles  pawn move
            else
            {
                if (m.isAttackMove())
                {
                    System.Media.SoundPlayer playersound = new System.Media.SoundPlayer(Properties.Resources.record20160425102455PM_mp3cut);
                    playersound.Play();
                }
                else
                {
                    System.Media.SoundPlayer playersound = new System.Media.SoundPlayer(Properties.Resources.recordM_mp3cut);
                    playersound.Play();
                }
                Utility.swapPieces(board, tmpPiece, tmpPiece.getPiecePosition(), m.getMovePosition());
            }
        }

        public static void checkWhiteKingStatus(Board board, Player player, List<Piece> blackPlayerActivePieces, List<Move> blackPlayerLegalMoves, List<Piece> whitePlayerActivePieces, List<Move> whitePlayerLegalMoves)
        {
            blackPlayerActivePieces = player.findActivePlayerPieces(board);
            blackPlayerLegalMoves = player.calculateActivePlayerMoves(board, blackPlayerActivePieces);
            player = new WhitePlayer();
            whitePlayerActivePieces = player.findActivePlayerPieces(board);
            whitePlayerLegalMoves = player.calculateActivePlayerMoves(board, whitePlayerActivePieces);
            Tuple<int, int> whitePlayerKingPosition = board.getPlayerKingPosition(pieceColor.White);
            if (player.kingInCheck(whitePlayerKingPosition, blackPlayerLegalMoves))
            {
                System.Media.SoundPlayer playersound = new System.Media.SoundPlayer(Properties.Resources.record2M_mp3cut);
                playersound.Play();
            }
            if (whitePlayerLegalMoves.Count() == 0)
                MessageBox.Show("The game is over black player wins");

        }
    }
}
