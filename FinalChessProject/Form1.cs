
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FinalChessProject.BoardSettings;
using FinalChessProject.PiecesSettings;
using FinalChessProject.PlayersSettings;

namespace FinalChessProject
{
    public partial class Form1 : Form
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void DisableProcessWindowsGhosting();
        private Board board = new Board();
        private Player player;
        private List<Move> whitePlayerLegalMoves, blackPlayerLegalMoves;
        private List<Piece> whitePlayerActivePieces, blackPlayerActivePieces;
        private List<Move> tmp = new List<Move>();
        private int mouseClickCounter = 0, bCol = 0, bRow = 0, wCol = 0, wRow = 0;
        private Tuple<int, int> originalPiecePosition;
        private Bitmap piecesImage, killedImageFromBlack, killedImageFromWhite;
        private Graphics graphics, killedGraphFromWhite, killedGraphFromBlack;
        private bool whiteMoveOccurred, blackMoveOccurred, twoPlayers;
        private int gameDifficulty = 2;
        Tuple<int, int, int, int> nextMove = Tuple.Create(-1, -1, -1, -1);
        private Dictionary<int, Bitmap> pieceImage = new Dictionary<int, Bitmap>
                  {
                    {11, new Bitmap(Properties.Resources.bPawn)},
                    {1, new Bitmap(Properties.Resources.bRock)},
                    {3, new Bitmap(Properties.Resources.bKnight)},
                    {5, new Bitmap(Properties.Resources.bBishop)},
                    {7, new Bitmap(Properties.Resources.bQueen)},
                    {9, new Bitmap(Properties.Resources.bKing)},
                    {12, new Bitmap(Properties.Resources.wPawn)},
                    {2, new Bitmap(Properties.Resources.wRock)},
                    {4, new Bitmap(Properties.Resources.wKnight)},
                    {6, new Bitmap(Properties.Resources.wBishop)},
                    {8, new Bitmap(Properties.Resources.wQueen)},
                    {10, new Bitmap(Properties.Resources.wKing)}
                };


        public Form1()
        {
            InitializeComponent();
            newGameToolStripMenuItem.Enabled = false;
            pictureBox1.Parent = boardBox;
        }

        private void updateBoard(Board board)
        {
            if (piecesImage == null)
                piecesImage = new Bitmap(512, 512);
            if (graphics == null)
                graphics = Graphics.FromImage(piecesImage);
            else
                graphics.Clear(Color.Transparent);

            int[,] boardTypes = board.getBoard();
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    if (boardTypes[r, c] != 0)
                        graphics.DrawImage(pieceImage[boardTypes[r, c]], new Rectangle(c * 64, r * 64, 64, 64));
                }
            }

            pictureBox1.Image = piecesImage;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            newGameToolStripMenuItem.Enabled = true;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            boardBox.Visible = true;
            pictureBox1.Visible = true;
            panel1.Visible = true;
            killedpiecesofwhiteplayer.Visible = true;
            killedpiecesofblackplayer.Visible = true;
            twoPlayers = false;
            blackPlayerLegalMoves = initBlackPlayerMoves();
            whitePlayerLegalMoves = initWhitePlayerMoves();
            player = new WhitePlayer();
            updateBoard(board);
        }

      

        private void KilledFromWhite(int ptype)
        {
            if (killedImageFromWhite == null)
                killedImageFromWhite = new Bitmap(192, 448);
            if (killedGraphFromWhite == null)
                killedGraphFromWhite = Graphics.FromImage(killedImageFromWhite);
            killedGraphFromWhite.DrawImage(pieceImage[ptype], new Rectangle(wCol * 64, wRow * 64, 64, 64));
            killedpiecesofwhiteplayer.Image = killedImageFromWhite;
            ++wCol;
            if (wCol == 3)
            { wCol = 0; ++wRow; }
        }

        private void KilledFromBlack(int ptype)
        {
            if (killedImageFromBlack == null)
                killedImageFromBlack = new Bitmap(192, 448);
            if (killedGraphFromBlack == null)
                killedGraphFromBlack = Graphics.FromImage(killedImageFromBlack);
            killedGraphFromBlack.DrawImage(pieceImage[ptype], new Rectangle(bCol * 64, bRow * 64, 64, 64));
            killedpiecesofblackplayer.Image = killedImageFromBlack;
            ++bCol;
            if (bCol == 3)
            { bCol = 0; ++bRow; }
        }

        private void hightlightLegalMoves(List<Move> legalMoves)
        {
            foreach (Move m in legalMoves)
            {
                int r = m.getMovePosition().Item1, c = m.getMovePosition().Item2;
                graphics.DrawImage(Properties.Resources.Select, new Rectangle(c * 64, r * 64, 64, 64));
            }
            pictureBox1.Image = piecesImage;
        }

        private async void pictureBox1_MouseClick_1(object sender, MouseEventArgs e)
        {
            int x = e.Y / 64, y = e.X / 64;
            if (twoPlayers)
            {
                if (player.getPlayerColor() == playerColor.White)
                {
                    if (mouseClickCounter == 0 && !board.isEmptyCell(x, y) && board.getBoardPieces()[x, y].getPieceColor() == pieceColor.BLack) ;

                    else if (!board.isEmptyCell(x, y) && mouseClickCounter == 0 && e.Button == MouseButtons.Left)
                    {
                        ++mouseClickCounter;
                        tmp = board.getBoardPieces()[x, y].getLegalMovesWithCheck(board);
                        originalPiecePosition = Tuple.Create(x, y);
                        hightlightLegalMoves(tmp);

                    }
                    else if (mouseClickCounter == 1 && e.Button == MouseButtons.Left)
                    {
                        foreach (Move m in tmp)
                        {
                            if (m.getMovePosition().Item1 == x && m.getMovePosition().Item2 == y)
                            {
                                Piece tmpPiece = board.getBoardPieces()[originalPiecePosition.Item1, originalPiecePosition.Item2];
                                if (m.isAttackMove())
                                    KilledFromBlack(board.getBoardPieces()[x, y].getPieceType());
                                
                                if (tmpPiece.getPieceType() == (int)pieceType.whitePawn)
                                    Utility.handleWhitePawnMove(board, tmpPiece, m);
                                else
                                {
                                    if (tmpPiece.getPieceType() == (int)pieceType.whiteKing)
                                    {
                                        if (Utility.isCastlingMove(tmpPiece, m))
                                            Utility.handleWhiteKingCastling(board, tmpPiece, m);
                                        else
                                            Utility.handlePieceMove(board, tmpPiece, m);
                                    }
                                    else
                                        Utility.handlePieceMove(board, tmpPiece, m);
                                }
                                whiteMoveOccurred = true;
                                break;
                            }
                        }
                        updateBoard(board);
                        if (whiteMoveOccurred)
                        {
                            Utility.checkBlackKingStatus(board, player, whitePlayerActivePieces, whitePlayerLegalMoves, blackPlayerActivePieces, blackPlayerLegalMoves);
                            player = new BlackPlayer();
                            whiteMoveOccurred = false;
                        }
                        mouseClickCounter = 0;
                    }
                }
                else
                {
                    if (mouseClickCounter == 0 && !board.isEmptyCell(x, y) && board.getBoardPieces()[x, y].getPieceColor() == pieceColor.White) ;

                    else if (!board.isEmptyCell(x, y) && mouseClickCounter == 0 && e.Button == MouseButtons.Left)
                    {
                        ++mouseClickCounter;
                        tmp = board.getBoardPieces()[x, y].getLegalMovesWithCheck(board);
                        originalPiecePosition = Tuple.Create(x, y);
                        hightlightLegalMoves(tmp);
                    }
                    else if (mouseClickCounter == 1 && e.Button == MouseButtons.Left)
                    {
                        foreach (Move m in tmp)
                        {
                            if (m.getMovePosition().Item1 == x && m.getMovePosition().Item2 == y)
                            {
                                Piece tmpPiece = board.getBoardPieces()[originalPiecePosition.Item1, originalPiecePosition.Item2];
                                if (m.isAttackMove())
                                    KilledFromWhite(board.getBoardPieces()[x, y].getPieceType());
                                if (tmpPiece.getPieceType() == (int)pieceType.blackPawn)
                                    Utility.handleBlackPawnMove(board, tmpPiece, m);
                                else
                                {
                                    if (tmpPiece.getPieceType() == (int)pieceType.blackKing)
                                    {
                                        if (Utility.isCastlingMove(tmpPiece, m))
                                            Utility.handleblackKingCastling(board, tmpPiece, m);
                                        else
                                            Utility.handlePieceMove(board, tmpPiece, m);
                                    }
                                    else
                                        Utility.handlePieceMove(board, tmpPiece, m);
                                }
                                blackMoveOccurred = true;
                                break;
                            }
                        }
                        updateBoard(board);
                        if (blackMoveOccurred)
                        {
                            Utility.checkWhiteKingStatus(board, player, blackPlayerActivePieces, blackPlayerLegalMoves, whitePlayerActivePieces, whitePlayerLegalMoves);
                            player = new WhitePlayer();
                            blackMoveOccurred = false;
                        }
                        mouseClickCounter = 0;
                    }
                }
            }
            else if (!twoPlayers)
            {
                
                if (player.getPlayerColor() == playerColor.White)
                {
                    if (mouseClickCounter == 0 && !board.isEmptyCell(x, y) && board.getBoardPieces()[x, y].getPieceColor() == pieceColor.BLack) ;

                    else if (!board.isEmptyCell(x, y) && mouseClickCounter == 0 && e.Button == MouseButtons.Left)
                    {
                        ++mouseClickCounter;
                        tmp = board.getBoardPieces()[x, y].getLegalMovesWithCheck(board);
                        originalPiecePosition = Tuple.Create(x, y);
                        hightlightLegalMoves(tmp);
                       // if (nextMove.Item1 != -1)
                         //   highlightComputerLastMove(Tuple.Create(nextMove.Item1, nextMove.Item2), Tuple.Create(nextMove.Item3, nextMove.Item4));

                    }
                    else if (mouseClickCounter == 1 && e.Button == MouseButtons.Left)
                    {
                        foreach (Move m in tmp)
                        {
                            if (m.getMovePosition().Item1 == x && m.getMovePosition().Item2 == y)
                            {
                                Piece tmpPiece = board.getBoardPieces()[originalPiecePosition.Item1, originalPiecePosition.Item2];
                                if (m.isAttackMove())
                                    KilledFromBlack(board.getBoardPieces()[x, y].getPieceType());
                                if (tmpPiece.getPieceType() == (int)pieceType.whitePawn)
                                    Utility.handleWhitePawnMove(board, tmpPiece, m);
                                else
                                {
                                    if (tmpPiece.getPieceType() == (int)pieceType.whiteKing)
                                    {
                                        if (Utility.isCastlingMove(tmpPiece, m))
                                            Utility.handleWhiteKingCastling(board, tmpPiece, m);
                                        else
                                            Utility.handlePieceMove(board, tmpPiece, m);
                                    }
                                    else
                                        Utility.handlePieceMove(board, tmpPiece, m);
                                }
                                whiteMoveOccurred = true;
                                break;
                            }
                        }
                       
                        updateBoard(board);

                        if (whiteMoveOccurred)
                        {
                            Utility.checkBlackKingStatus(board, player, whitePlayerActivePieces, whitePlayerLegalMoves, blackPlayerActivePieces, blackPlayerLegalMoves);
                            player = new BlackPlayer();
                            whiteMoveOccurred = false;
                            await Task.Delay(100);
                            // computer move
                            DisableProcessWindowsGhosting();
                            nextMove = Computer.getBestMove(board, gameDifficulty);
                            
                            if (nextMove.Item1 == -1)
                            { MessageBox.Show("THE GAME IS OVER YOU WIN"); return; }
                            Move computerMove;
                            if (board.isEmptyCell(nextMove.Item3, nextMove.Item4))
                                computerMove = new NormalMove(Tuple.Create(nextMove.Item3, nextMove.Item4));
                            else
                                computerMove = new AttackMove(Tuple.Create(nextMove.Item3, nextMove.Item4));



                            Piece tmpPiece1 = board.getBoardPieces()[nextMove.Item1, nextMove.Item2];

                            if (computerMove.isAttackMove())
                                KilledFromWhite(board.getBoardPieces()[nextMove.Item3, nextMove.Item4].getPieceType());

                            if (tmpPiece1.getPieceType() == (int)pieceType.blackPawn)
                                Utility.handleBlackPawnMove(board, tmpPiece1, computerMove);
                            else
                            {
                                if (tmpPiece1.getPieceType() == (int)pieceType.blackKing)
                                {
                                    if (Utility.isCastlingMove(tmpPiece1, computerMove))
                                        Utility.handleblackKingCastling(board, tmpPiece1, computerMove);
                                    else
                                        Utility.handlePieceMove(board, tmpPiece1, computerMove);
                                }
                                else
                                    Utility.handlePieceMove(board, tmpPiece1, computerMove);
                            }
                            updateBoard(board);
                            highlightComputerLastMove(Tuple.Create(nextMove.Item1, nextMove.Item2), Tuple.Create(nextMove.Item3, nextMove.Item4));
                            Utility.checkWhiteKingStatus(board, player, blackPlayerActivePieces, blackPlayerLegalMoves, whitePlayerActivePieces, whitePlayerLegalMoves);
                            player = new WhitePlayer();
                        }
                        else
                             if (nextMove.Item1 != -1)
                            highlightComputerLastMove(Tuple.Create(nextMove.Item1, nextMove.Item2), Tuple.Create(nextMove.Item3, nextMove.Item4));
                        mouseClickCounter = 0;
                    }
                }
          
            }
        }

        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Run(new Form1());
        } 

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button1.Visible = false;
            button2.Visible = false;
            boardBox.Visible = true;
            pictureBox1.Visible = true;
            panel1.Visible = true;
            killedpiecesofwhiteplayer.Visible = true;
            killedpiecesofblackplayer.Visible = true;

            board = new Board();
            player = new WhitePlayer();
            bCol = bRow = wRow = wCol = 0;
            killedImageFromBlack = new Bitmap(192, 448);

            killedGraphFromBlack = Graphics.FromImage(killedImageFromBlack);
            killedpiecesofblackplayer.Image = killedImageFromBlack;
            killedImageFromWhite = new Bitmap(192, 448);
            killedGraphFromWhite = Graphics.FromImage(killedImageFromWhite);
            killedpiecesofwhiteplayer.Image = killedImageFromWhite;
            updateBoard(board);
        }

        private void highlightComputerLastMove(Tuple<int,int> from,Tuple<int,int> to)
        {
           
                graphics.DrawImage(Properties.Resources.blueSquare, new Rectangle(from.Item2 * 64,from.Item1  * 64, 64, 64));
                graphics.DrawImage(Properties.Resources.blueSquare, new Rectangle(to.Item2 * 64, to.Item1 * 64, 64, 64));

                pictureBox1.Image = piecesImage;
        }
        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void usToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("      version 0.0        Chess Game developed by team el2sater ");
        }
        private List<Move> initBlackPlayerMoves()
        {
            List<Move> tmp = new List<Move>();
            tmp.Add(new NormalMove(Tuple.Create(2, 0)));
            tmp.Add(new NormalMove(Tuple.Create(3, 0)));
            tmp.Add(new NormalMove(Tuple.Create(2, 1)));
            tmp.Add(new NormalMove(Tuple.Create(3, 1)));
            tmp.Add(new NormalMove(Tuple.Create(2, 2)));
            tmp.Add(new NormalMove(Tuple.Create(3, 2)));
            tmp.Add(new NormalMove(Tuple.Create(2, 3)));
            tmp.Add(new NormalMove(Tuple.Create(3, 3)));
            tmp.Add(new NormalMove(Tuple.Create(2, 4)));
            tmp.Add(new NormalMove(Tuple.Create(3, 4)));
            tmp.Add(new NormalMove(Tuple.Create(2, 5)));
            tmp.Add(new NormalMove(Tuple.Create(3, 5)));
            tmp.Add(new NormalMove(Tuple.Create(2, 6)));
            tmp.Add(new NormalMove(Tuple.Create(3, 6)));
            tmp.Add(new NormalMove(Tuple.Create(2, 7)));
            tmp.Add(new NormalMove(Tuple.Create(3, 7)));
            tmp.Add(new NormalMove(Tuple.Create(2, 0)));
            tmp.Add(new NormalMove(Tuple.Create(2, 2)));
            tmp.Add(new NormalMove(Tuple.Create(2, 7)));
            tmp.Add(new NormalMove(Tuple.Create(2, 5)));
            return tmp;
        }
        private List<Move> initWhitePlayerMoves()
        {
            List<Move> tmp = new List<Move>();
            tmp.Add(new NormalMove(Tuple.Create(4, 0)));
            tmp.Add(new NormalMove(Tuple.Create(5, 0)));
            tmp.Add(new NormalMove(Tuple.Create(4, 1)));
            tmp.Add(new NormalMove(Tuple.Create(5, 1)));
            tmp.Add(new NormalMove(Tuple.Create(4, 2)));
            tmp.Add(new NormalMove(Tuple.Create(5, 2)));
            tmp.Add(new NormalMove(Tuple.Create(4, 3)));
            tmp.Add(new NormalMove(Tuple.Create(5, 3)));
            tmp.Add(new NormalMove(Tuple.Create(4, 4)));
            tmp.Add(new NormalMove(Tuple.Create(5, 4)));
            tmp.Add(new NormalMove(Tuple.Create(4, 5)));
            tmp.Add(new NormalMove(Tuple.Create(5, 5)));
            tmp.Add(new NormalMove(Tuple.Create(4, 6)));
            tmp.Add(new NormalMove(Tuple.Create(5, 6)));
            tmp.Add(new NormalMove(Tuple.Create(4, 7)));
            tmp.Add(new NormalMove(Tuple.Create(5, 7)));
            tmp.Add(new NormalMove(Tuple.Create(5, 0)));
            tmp.Add(new NormalMove(Tuple.Create(5, 2)));
            tmp.Add(new NormalMove(Tuple.Create(5, 5)));
            tmp.Add(new NormalMove(Tuple.Create(5, 7)));

      
            return tmp;

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            newGameToolStripMenuItem.Enabled = true;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            boardBox.Visible = true;
            pictureBox1.Visible = true;
            panel1.Visible = true;
            killedpiecesofwhiteplayer.Visible = true;
            killedpiecesofblackplayer.Visible = true;
            twoPlayers = true;
            blackPlayerLegalMoves = initBlackPlayerMoves();
            whitePlayerLegalMoves = initWhitePlayerMoves();
            player = new WhitePlayer();
            updateBoard(board);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
