using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chess
{
    public static class PawnMechanics
    {
        //Pohyb pěšce
        public static List<(int, int)> PawnMove(Field[,] field, int row, int column)
        {
            var result = new List<(int, int)>();
            Field currentField = field[row, column];
            if (currentField.Piece != null)
            {
                //v závislosti na barvě se může pohybovat nahoru nebo dolu
                if (currentField.Piece.Color)
                {
                    //pokud je na výchozí pozici, tak může jít o dva
                    if (row == 6)
                    {
                        //pokud je cílové pole prázdné, tak ho přidáme do listu možných pohybů
                        if (field[5, column].Piece == null)
                        {
                            Field[,] temporaryChessBoard = ObjectExtensions.DeepCopyFieldArray(field);
                            temporaryChessBoard[5, column].Piece = temporaryChessBoard[row, column].Piece;
                            temporaryChessBoard[row, column].Piece = null;
                            if (!KingMechanics.IsKingChecked(temporaryChessBoard, currentField.Piece.Color))
                            {
                                result.Add((5, column));
                            }
                            if (field[4, column].Piece == null)
                            {
                                temporaryChessBoard = ObjectExtensions.DeepCopyFieldArray(field);
                                temporaryChessBoard[4, column].Piece = temporaryChessBoard[row, column].Piece;
                                temporaryChessBoard[row, column].Piece = null;
                                if (!KingMechanics.IsKingChecked(temporaryChessBoard, currentField.Piece.Color))
                                {
                                    result.Add((4, column));
                                }
                            }
                        }
                    }
                    else if (row > 0 && field[row - 1, column].Piece == null)
                    {
                        Field[,] temporaryChessBoard = ObjectExtensions.DeepCopyFieldArray(field);
                        temporaryChessBoard[row - 1, column].Piece = temporaryChessBoard[row, column].Piece;
                        temporaryChessBoard[row, column].Piece = null;
                        if (!KingMechanics.IsKingChecked(temporaryChessBoard, currentField.Piece.Color))
                        {
                            result.Add((row - 1, column));
                        }
                    }
                }
                else
                {
                    //stejně jako výše, pokud je na výchozí pozici (tentokrát druhá barva tedy opačná strana šachovnice a postupuje opačným směrem
                    if (row == 1)
                    {
                        if (field[2, column].Piece == null)
                        {
                            Field[,] temporaryChessBoard = ObjectExtensions.DeepCopyFieldArray(field);
                            temporaryChessBoard[2, column].Piece = temporaryChessBoard[row, column].Piece;
                            temporaryChessBoard[row, column].Piece = null;
                            if (!KingMechanics.IsKingChecked(temporaryChessBoard, currentField.Piece.Color))
                            {
                                result.Add((2, column));
                            }
                            if (field[3, column].Piece == null)
                            {
                                temporaryChessBoard = ObjectExtensions.DeepCopyFieldArray(field);
                                temporaryChessBoard[3, column].Piece = temporaryChessBoard[row, column].Piece;
                                temporaryChessBoard[row, column].Piece = null;
                                if (!KingMechanics.IsKingChecked(temporaryChessBoard, currentField.Piece.Color))
                                {
                                    result.Add((3, column));
                                }
                            }
                        }


                    }
                    else if (field.GetLength(0) - 1 != row && field[row + 1, column].Piece == null)
                    {
                        Field[,] temporaryChessBoard = ObjectExtensions.DeepCopyFieldArray(field);
                        temporaryChessBoard[row + 1, column].Piece = temporaryChessBoard[row, column].Piece;
                        temporaryChessBoard[row, column].Piece = null;
                        if (!KingMechanics.IsKingChecked(temporaryChessBoard, currentField.Piece.Color))
                        {
                            result.Add((row + 1, column));
                        }
                    }
                }
            }
            result.AddRange(PawnAttack(field, row, column));
            return result;
        }

        //jednoduše zkontroluje jestli může vzít nějakou figurku
        public static List<(int, int)> PawnAttack(Field[,] field, int row, int column)
        {
            var result = new List<(int, int)>();
            Field currentField = field[row, column];
            if (currentField.Piece != null)
            {
                if (currentField.Piece.Color)
                {
                    if (row > 0)
                    {
                        if (column > 0 && field[row - 1, column - 1].Piece != null && field[row - 1, column - 1].Piece.Color != currentField.Piece.Color)
                        {

                            Field[,] temporaryChessBoard = ObjectExtensions.DeepCopyFieldArray(field);
                            temporaryChessBoard[row - 1, column - 1].Piece = temporaryChessBoard[row, column].Piece;
                            temporaryChessBoard[row, column].Piece = null;
                            if (!KingMechanics.IsKingChecked(temporaryChessBoard, currentField.Piece.Color))
                            {
                                result.Add((row - 1, column - 1));
                            }
                        }
                        if (column < field.GetLength(1) - 1 && field[row - 1, column + 1].Piece != null && field[row - 1, column + 1].Piece.Color != currentField.Piece.Color)
                        {

                            Field[,] temporaryChessBoard = ObjectExtensions.DeepCopyFieldArray(field);
                            temporaryChessBoard[row - 1, column + 1].Piece = temporaryChessBoard[row, column].Piece;
                            temporaryChessBoard[row, column].Piece = null;
                            if (!KingMechanics.IsKingChecked(temporaryChessBoard, currentField.Piece.Color))
                            {
                                result.Add((row - 1, column + 1));
                            }
                        }
                    }
                }
                else
                {
                    if (row < 7)
                    {
                        if (column > 0 && field[row + 1, column - 1].Piece != null && field[row + 1, column - 1].Piece.Color != currentField.Piece.Color)
                        {
                            Field[,] temporaryChessBoard = ObjectExtensions.DeepCopyFieldArray(field);
                            temporaryChessBoard[row + 1, column - 1].Piece = temporaryChessBoard[row, column].Piece;
                            temporaryChessBoard[row, column].Piece = null;
                            if (!KingMechanics.IsKingChecked(temporaryChessBoard, currentField.Piece.Color))
                            {
                                result.Add((row + 1, column - 1));
                            }
                        }
                        if (column < field.GetLength(1) - 1 && field[row + 1, column + 1].Piece != null && field[row + 1, column + 1].Piece.Color != currentField.Piece.Color)
                        {
                            Field[,] temporaryChessBoard = ObjectExtensions.DeepCopyFieldArray(field);
                            temporaryChessBoard[row + 1, column + 1].Piece = temporaryChessBoard[row, column].Piece;
                            temporaryChessBoard[row, column].Piece = null;
                            if (!KingMechanics.IsKingChecked(temporaryChessBoard, currentField.Piece.Color))
                            {
                                result.Add((row + 1, column + 1));
                            }
                        }
                    }
                }
            }
            return result;
        }

        //hlídá na které políčka nemuže vstoupit protivníkův král (v podstate PawnAttact akorát hlídá i políčka na kterých nejsou protivníkovy figurky a ty na kterých jsou jeho vlastní figurky, protože by je teoreticky protivníkův král mohl vzít)
        public static List<(int, int)> PawnCoverage(Field[,] field, int row, int column)
        {
            var result = new List<(int, int)>();
            Field currentField = field[row, column];
            if (currentField.Piece != null)
            {
                if (currentField.Piece.Color)
                {
                    if (row > 0)
                    {
                        result.Add((row - 1, column - 1));
                        result.Add((row - 1, column + 1));
                    }
                }
                else
                {
                    if (row < 7)
                    {
                        result.Add((row + 1, column - 1));
                        result.Add((row + 1, column + 1));
                    }
                }
            }
            return result;
        }

        public static void PawnPromotion((Field, string) promotionInfo, Control.ControlCollection controls)
        {
            string image = "";
            string piece = "";
            bool pieceColor = true;
            switch (promotionInfo.Item2)
            {
                case "BR":
                    image = "blackRook.png";
                    piece = "Rook";
                    pieceColor = false;
                    break;
                case "WR":
                    image = "whiteRook.png";
                    piece = "Rook";
                    pieceColor = true;
                    break;
                case "BN":
                    image = "blackKnight.png";
                    piece = "Knight";
                    pieceColor = false;
                    break;
                case "WN":
                    image = "whiteKnight.png";
                    piece = "Knight";
                    pieceColor = true;
                    break;
                case "BB":
                    image = "blackBishop.png";
                    piece = "Bishop";
                    pieceColor = false;
                    break;
                case "WB":
                    image = "whiteBishop.png";
                    piece = "Bishop";
                    pieceColor = true;
                    break;
                case "BQ":
                    image = "blackQueen.png";
                    piece = "Queen";
                    pieceColor = false;
                    break;
                case "WQ":
                    image = "whiteQueen.png";
                    piece = "Queen";
                    pieceColor = true;
                    break;
                default:
                    break;
            }

            if (image != "" && piece != "" && promotionInfo.Item1.Piece != null)
            {
                    controls.Remove(promotionInfo.Item1.Piece.pictureBox);

                    PictureBox pictureBox = new PictureBox();
                    pictureBox.Width = 100;
                    pictureBox.Height = 100;
                    pictureBox.Image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", image));
                    pictureBox.Left = promotionInfo.Item1.Column * 100;
                    pictureBox.Top = promotionInfo.Item1.Row * 100;
                    pictureBox.BackColor = promotionInfo.Item1.Color ? Color.White : Color.DarkGray;
                    pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
                    pictureBox.Enabled = false;
                    controls.Add(pictureBox);
                    controls.SetChildIndex(pictureBox, 0);
                    promotionInfo.Item1.Piece = new Piece(piece, pieceColor, pictureBox);
                }
        }
    }
}
