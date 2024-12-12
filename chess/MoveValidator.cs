using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess
{
    public static class MoveValidator
    {
        public static List<(int, int)> GetValidMoves(Field[,] field, int row, int column)
        {
            var result = new List<(int, int)>();
            if (field[row, column].Piece.Type == "Pawn")
            {
                result = PawnMove(field, row, column);
            }

            return result;
        }

        private static List<(int, int)> PawnMove(Field[,] field, int row, int column)
        {
            var result = new List<(int, int)>();
            Field currentField = field[row, column];
            if (currentField.Piece != null)
            {
                if (currentField.Piece.Color)
                {
                    if (row == 6)
                    {
                        if (field[5, column].Piece == null)
                        {
                            result.Add((5, column));
                        }

                        if (field[4, column].Piece == null)
                        {
                            result.Add((4, column));
                        }
                    }
                    else if (row > 0 && field[row - 1, column].Piece == null) 
                    {
                        result.Add((row -1, column));
                    }

                    if (row > 0)
                    {
                        if (column > 0 && field[row - 1, column - 1].Piece != null && field[row - 1, column - 1].Piece.Color != currentField.Piece.Color)
                        {
                            result.Add((row - 1, column - 1));
                        }
                        if (column < field.GetLength(1) -1 && field[row - 1, column + 1].Piece != null && field[row - 1, column + 1].Piece.Color != currentField.Piece.Color)
                        {
                            result.Add((row - 1, column + 1));
                        }
                    }
                }
                else
                {
                    if (row == 1)
                    {
                        if (field[2, column].Piece == null)
                        {
                            result.Add((2, column));
                        }

                        if (field[3, column].Piece == null)
                        {
                            result.Add((3, column));
                        }
                    }
                    else if (field.GetLength(0) - 1 != row && field[row + 1, column].Piece == null)
                    {
                        result.Add((row + 1, column));
                    }

                    if (row < 7)
                    {
                        if (column > 0 && field[row + 1, column - 1].Piece != null && field[row + 1, column - 1].Piece.Color != currentField.Piece.Color)
                        {
                            result.Add((row + 1, column - 1));
                        }
                        if (column < field.GetLength(1) - 1 && field[row + 1, column + 1].Piece != null && field[row + 1, column + 1].Piece.Color != currentField.Piece.Color)
                        {
                            result.Add((row + 1, column + 1));
                        }
                    }
                }
            }
            return result;
        }
    }
}
