using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess
{
    public static class KnightMechanics
    {
        public static List<(int, int)> KnightMove(Field[,] fields, int row, int column)
        {
            var result = new List<(int, int)>();
            Field currentField = fields[row, column];

            // vektory směrů kterými může postava postupovat
            var knightMoves = new (int, int)[]
            {
            (-2, -1), (-2, 1), (2, -1), (2, 1),
            (-1, -2), (-1, 2), (1, -2), (1, 2)
            };

            //projdememe všechny směry
            foreach (var (dRow, dColumn) in knightMoves)
            {
                int targetRow = row + dRow;
                int targetColumn = column + dColumn;

                // kontrola že jsme stále na šachovnici
                if (targetRow >= 0 && targetRow < fields.GetLength(0) &&
                    targetColumn >= 0 && targetColumn < fields.GetLength(1))
                {
                    var targetField = fields[targetRow, targetColumn];
                    var targetPiece = targetField.Piece;

                    if (targetPiece == null || (currentField.Piece != null && targetPiece.Color != currentField.Piece.Color))
                    {
                        // pokud je pole prázdné, nebo je na něm protivník, tak můžeme přidat
                        Field[,] temporaryChessBoard = ObjectExtensions.DeepCopyFieldArray(fields);
                        temporaryChessBoard[targetRow, targetColumn].Piece = temporaryChessBoard[row, column].Piece;
                        temporaryChessBoard[row, column].Piece = null;
                        if (currentField.Piece != null && !KingMechanics.IsKingChecked(temporaryChessBoard, currentField.Piece.Color))
                        {
                            result.Add((targetRow, targetColumn));
                        }
                    }
                }
            }

            return result;
        }

        public static List<(int, int)> KnightCoverage(Field[,] fields, int row, int column)
        {
            var result = new List<(int, int)>();
            Field currentField = fields[row, column];

            // vektory směrů kterými může postava postupovat
            var directions = new (int, int)[]
            {
            (-2, -1), (-2, 1), (2, -1), (2, 1),
            (-1, -2), (-1, 2), (1, -2), (1, 2)
            };

            //projdememe všechny směry
            foreach (var (dRow, dColumn) in directions)
            {
                int targetRow = row + dRow;
                int targetColumn = column + dColumn;

                // kontrola že jsme stále na šachovnici
                if (targetRow >= 0 && targetRow < fields.GetLength(0) &&
                    targetColumn >= 0 && targetColumn < fields.GetLength(1))
                {
                    var targetField = fields[targetRow, targetColumn];
                    var targetPiece = targetField.Piece;

                        result.Add((targetRow, targetColumn));
                    
                }
            }

            return result;
        }
    }
}
