using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess
{
    public static class BishopMechanics
    {
        public static List<(int, int)> BishopMove(Field[,] fields, int row, int column)
        {
            var result = new List<(int, int)>();
            Field currentField = fields[row, column];

            // vektory směrů kterými může postava postupovat
            var directions = new (int, int)[] { (1, 1), (1, -1), (-1, 1), (-1, -1) };

            //projdememe všechny směry
            foreach (var (dRow, dColumn) in directions)
            {
                int currentRow = row;
                int currentColumn = column;

                while (true)
                {
                    currentRow += dRow;
                    currentColumn += dColumn;

                    // kontrola že jsme stále na šachovnici
                    if (currentRow < 0 || currentRow >= fields.GetLength(0) ||
                        currentColumn < 0 || currentColumn >= fields.GetLength(1))
                    {
                        break;
                    }

                    var targetField = fields[currentRow, currentColumn];
                    var targetPiece = targetField.Piece;

                    if (targetPiece == null)
                    {
                        // pokud je pole prázdné a nevznikne tak šach na našho krále, tak můžeme přidat
                            Field[,] temporaryChessBoard = ObjectExtensions.DeepCopyFieldArray(fields);
                            temporaryChessBoard[currentRow, currentColumn].Piece = temporaryChessBoard[row, column].Piece;
                            temporaryChessBoard[row, column].Piece = null;
                            if (!KingMechanics.IsKingChecked(temporaryChessBoard, currentField.Piece.Color))
                            {
                                result.Add((currentRow, currentColumn));
                            }
                    }
                    else
                    {
                        // pokud je v cílovém poli protivník a nevznikne tak šach na našho krále, tak můžeme přidat
                        if (targetPiece.Color != currentField.Piece.Color)
                        {
                                Field[,] temporaryChessBoard = ObjectExtensions.DeepCopyFieldArray(fields);
                                temporaryChessBoard[currentRow, currentColumn].Piece = temporaryChessBoard[row, column].Piece;
                                temporaryChessBoard[row, column].Piece = null;
                                if (!KingMechanics.IsKingChecked(temporaryChessBoard, currentField.Piece.Color))
                                {
                                    result.Add((currentRow, currentColumn));
                                }
                        }
                        break; //ukončíme pohyb v daném směru 
                    }
                }
            }

            return result;
        }

        public static List<(int, int)> BishopCoverage(Field[,] fields, int row, int column)
        {
            var result = new List<(int, int)>();
            Field currentField = fields[row, column];

            // vektory směrů kterými může postava postupovat
            var directions = new (int, int)[] { (1, 1), (1, -1), (-1, 1), (-1, -1) };

            //projdememe všechny směry
            foreach (var (dRow, dColumn) in directions)
            {
                int currentRow = row;
                int currentColumn = column;

                while (true)
                {
                    currentRow += dRow;
                    currentColumn += dColumn;

                    // kontrola že jsme stále na šachovnici
                    if (currentRow < 0 || currentRow >= fields.GetLength(0) ||
                        currentColumn < 0 || currentColumn >= fields.GetLength(1))
                    {
                        break;
                    }

                    var targetField = fields[currentRow, currentColumn];
                    var targetPiece = targetField.Piece;

                    if (targetPiece == null)
                    {
                        // pokud je pole prázdné, tak můžeme přidat
                        result.Add((currentRow, currentColumn));
                    }
                    else
                    {
                        // pokud je v cílovém poli protivník, tak můžeme přidat
                        if (currentField.Piece != null && targetPiece.Color != currentField.Piece.Color)
                        {
                            result.Add((currentRow, currentColumn));
                            //aby král nemohl postoupit o jedno pole od této postavy na které ale stále postava míří
                            if (targetPiece.Type != "King")
                            {
                                break;
                            }

                        }
                        //pokud míří na některou naši figurku, tak ji král nemůže vzít
                        else if (currentField.Piece != null && targetField.Color == currentField.Piece.Color)
                        {
                                result.Add((currentRow, currentColumn));
                                break; //ukončíme pohyb v daném směru
                            
                        }
                    }
                }
            }

            return result;
        }

    }
}
