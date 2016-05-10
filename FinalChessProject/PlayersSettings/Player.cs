using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalChessProject.PiecesSettings;
using FinalChessProject.BoardSettings;
using System.Windows.Forms;
namespace FinalChessProject.PlayersSettings
{
    public enum playerColor { White, Black }
    public abstract class Player
    {
        protected List<Move> opponentLegalMoves;

        public Player()
        {
        }
        public abstract playerColor getPlayerColor();
        public abstract List<Piece> findActivePlayerPieces(Board board);
        public abstract List<Move>  calculateActivePlayerMoves(Board board, List<Piece> playerPieces);
        public abstract bool kingInCheck(Tuple<int,int> currentPlayerKingPosition,List<Move> enemyLegalMoves);
    }
    public class WhitePlayer : Player
    {
        public WhitePlayer()
        {
           
        }

        public override List<Move> calculateActivePlayerMoves(Board board, List<Piece> playerPieces)
        {
            List<Move> legalPlayerMoves = new List<Move>();

            foreach (Piece piece in playerPieces)
            {
                legalPlayerMoves.AddRange(piece.getLegalMovesWithCheck(board));
            }
            return legalPlayerMoves;
        }

        public override List<Piece> findActivePlayerPieces(Board board)
        {
            List<Piece> tmp = new List<Piece>();
            for (int i = 0; i < 8; ++i)
                for (int j = 0; j < 8; ++j)
                    if (!board.isEmptyCell(i,j)&& board.getBoardPieces()[i, j].getPieceColor() == pieceColor.White)
                        tmp.Add(board.getBoardPieces()[i, j]);
            return tmp;
        }

        public override playerColor getPlayerColor()
        {
            return playerColor.White;
        }

        public override bool kingInCheck (Tuple<int,int> currentPlayerKingPosition, List<Move> enemyLegalMoves)
        {
                foreach (Move enemyMove in enemyLegalMoves)
                    if(enemyMove.getMovePosition().Item1==currentPlayerKingPosition.Item1 &&
                       enemyMove.getMovePosition().Item2==currentPlayerKingPosition.Item2)
                        return true;
            return false;
        }

    }
    public class BlackPlayer : Player
    {
        public BlackPlayer()
        {

        }
        public override List<Move> calculateActivePlayerMoves(Board board, List<Piece> playerPieces)
        {
            List<Move> legalPlayerMoves = new List<Move>();

            foreach (Piece piece in playerPieces)
            {

                legalPlayerMoves.AddRange(piece.getLegalMovesWithCheck(board));
            }
            return legalPlayerMoves;
        }

        public override List<Piece> findActivePlayerPieces(Board board)
        {
            List<Piece> tmp = new List<Piece>();
            for (int i = 0; i < 8; ++i)
                for (int j = 0; j < 8; ++j)
                    if (!board.isEmptyCell(i,j) && board.getBoardPieces()[i, j].getPieceColor() == pieceColor.BLack)
                        tmp.Add(board.getBoardPieces()[i, j]);
            return tmp;
        }


        public override playerColor getPlayerColor()
        {
            return playerColor.Black;
        }

        public override bool kingInCheck(Tuple<int, int> currentPlayerKingPosition, List<Move> enemyLegalMoves)
        {
            foreach (Move enemyMove in enemyLegalMoves)
                if (enemyMove.getMovePosition().Item1 == currentPlayerKingPosition.Item1 &&
                   enemyMove.getMovePosition().Item2 == currentPlayerKingPosition.Item2)
                    return true;
            return false;
        }

    }
}
