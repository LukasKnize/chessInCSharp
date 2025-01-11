using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess
{
    public static class RookMechanics
    {
        public static List<(int, int)> RookMove(Field[,] fields, int row, int column)
        {
            var result = new List<(int, int)>();
            Field currentField = fields[row, column];
            bool moveToGetFromCheckOnly = false;
            if (KingMechanics.IsKingChecked(fields, currentField.Piece.Color))
            {
                moveToGetFromCheckOnly = true;
            }

            // vektory směrů kterými může postava postupovat
            var directions = new (int, int)[] { (0, 1), (0, -1), (1, 0), (-1, 0) };

            //projdememe všechny směry
            foreach (var (dRow, dCol) in directions)
            {
                int currentRow = row;
                int currentColumn = column;

                while (true)
                {
                    currentRow += dRow;
                    currentColumn += dCol;

                    // kontrola že jsme stále na šachovnici
                    if (currentRow < 0 || currentRow >= 8 ||
                        currentColumn < 0 || currentColumn >= 8)
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

        public static List<(int, int)> RookCoverage(Field[,] fields, int row, int column)
        {
            var result = new List<(int, int)>();
            Field currentField = fields[row, column];

            // vektory směrů kterými může postava postupovat
            var directions = new (int, int)[] { (0, 1), (0, -1), (1, 0), (-1, 0) };

            //projdememe všechny směry
            foreach (var (dRow, dCol) in directions)
            {
                int currentRow = row;
                int currentColumn = column;
                while (true)
                {
                    currentRow += dRow;
                    currentColumn += dCol;

                    // kontrola že jsme stále na šachovnici
                    if (currentRow < 0 || currentRow >= 8 ||
                        currentColumn < 0 || currentColumn >= 8)
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
                        if (targetPiece.Color != currentField.Piece.Color)
                        {
                            result.Add((currentRow, currentColumn));
                            //aby král nemohl postoupit o jedno pole od této postavy na které ale stále postava míří
                            if (targetPiece.Type != "King")
                            {
                                break;
                            }

                        }
                        //pokud míří na některou naši figurku, tak ji král nemůže vzít
                        else if (targetField.Color == currentField.Piece.Color)
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
