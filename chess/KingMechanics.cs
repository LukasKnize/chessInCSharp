using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess
{
    public static class KingMechanics
    {
        public static List<(int, int)> KingMove(Field[,] fields, int row, int column)
        {
            var result = new List<(int, int)>();
            Field currentField = fields[row, column];

            // vektory směrů kterými může postava postupovat
            var directions = new (int, int)[] { (1, 1), (1, -1), (-1, 1), (-1, -1), (0, 1), (0, -1), (1, 0), (-1, 0) };

            //projdememe všechny směry
            foreach (var (dRow, dColumn) in directions)
            {


                int currentRow = row + dRow;
                int currentColumn = column + dColumn;

                // kontrola že jsme stále na šachovnici
                if (currentRow >= 0 && currentRow < fields.GetLength(0) &&
                    currentColumn >= 0 && currentColumn < fields.GetLength(1))
                {
                    var targetField = fields[currentRow, currentColumn];
                    var targetPiece = targetField.Piece;
                    if (targetPiece == null)
                    {
                        // pokud je pole prázdné, tak můžeme přidat
                        if (!IsCheck(fields, currentRow, currentColumn, currentField.Piece.Color))
                        {
                            result.Add((currentRow, currentColumn));
                        }


                    }
                    else
                    {
                        // pokud je v cílovém poli protivník, tak můžeme přidat
                        if (targetPiece.Color != currentField.Piece.Color)
                        {
                            if (!IsCheck(fields, currentRow, currentColumn, currentField.Piece.Color))
                            {
                                result.Add((currentRow, currentColumn));
                            }

                        }
                    }
                }


            }
            return result;
        }

        public static List<(int, int)> KingCoverage(Field[,] fields, int row, int column)
        {
            var result = new List<(int, int)>();
            Field currentField = fields[row, column];

            // vektory směrů kterými může postava postupovat
            var directions = new (int, int)[] { (1, 1), (1, -1), (-1, 1), (-1, -1), (0, 1), (0, -1), (1, 0), (-1, 0) };

            //projdememe všechny směry
            foreach (var (dRow, dColumn) in directions)
            {


                int currentRow = row + dRow;
                int currentColumn = column + dColumn;

                // kontrola že jsme stále na šachovnici
                if (currentRow >= 0 && currentRow < fields.GetLength(0) &&
                    currentColumn >= 0 && currentColumn < fields.GetLength(1))
                {
                    var targetField = fields[currentRow, currentColumn];
                    var targetPiece = targetField.Piece;
                    result.Add((currentRow, currentColumn));
                }


            }
            return result;
        }

        public static bool IsCheck(Field[,] fields, int row, int column, bool color)
        {
            // Projde všechny pole šachovnice
            for (int i = 0; i < fields.GetLength(0); i++)
            {
                for (int j = 0; j < fields.GetLength(1); j++)
                {
                    var piece = fields[i, j].Piece;

                    // přeskočí prázdná pole, a pole s figurkou s naší barvou
                    if (piece == null || piece.Color == color)
                    {
                        continue;
                    }

                    //podle nalezené figurky spustí metodu
                    List<(int, int)> opponentMoves = piece.Type switch
                    {
                        "Rook" => RookMechanics.RookCoverage(fields, i, j),
                        "Bishop" => BishopMechanics.BishopCoverage(fields, i, j),
                        "Queen" => QueenMechanics.QueenCoverage(fields, i, j),
                        "Knight" => KnightMechanics.KnightCoverage(fields, i, j),
                        "Pawn" => PawnMechanics.PawnCoverage(fields, i, j),
                        _ => new List<(int, int)>()
                    };

                    // pokud je pozice šachována, tak vrátí true, jinak false
                    if (opponentMoves.Contains((row, column)))
                    {
                        return true; 
                    }
                }
            }

            return false;
        }
    }
}
