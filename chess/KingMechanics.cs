using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
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
                        if (currentField.Piece != null && !IsCheck(fields, currentRow, currentColumn, currentField.Piece.Color))
                        {
                            result.Add((currentRow, currentColumn));
                        }


                    }
                    else
                    {
                        // pokud je v cílovém poli protivník, tak můžeme přidat
                        if (currentField.Piece != null && targetPiece.Color != currentField.Piece.Color)
                        {
                            if (!IsCheck(fields, currentRow, currentColumn, currentField.Piece.Color))
                            {
                                result.Add((currentRow, currentColumn));
                            }

                        }
                    }
                }


            }
            result.AddRange(Castle(fields, row, column));
            return result;
        }

        public static List<(int, int)> Castle(Field[,] fields, int row, int column)
        {
            var result = new List<(int, int)>();
            Field currentField = fields[row, column];
            //pokud je na políčku král, král se nepohl, a král není šachován
            if (currentField.Piece != null && !currentField.Piece.Dirty && currentField.Piece.Type == "King" && !IsCheck(fields, row, column, currentField.Piece.Color))
            {
                //tyhle ošklivý podmínky by měli zajistit že políčka mezi králem a věži jsou prázdná, nešachovaná a že s věží nebylo pohnuto (a teda hlavně jestli tam je)
                if(row == 0 && fields[row, 1].Piece == null && fields[row, 2].Piece == null && fields[row, 3].Piece == null && fields[row, 0].Piece != null && fields[row, 0].Piece.Type == "Rook" && !fields[row, 0].Piece.Dirty)
                {
                    if (!IsCheck(fields, row, 1, currentField.Piece.Color) && !IsCheck(fields, row, 2, currentField.Piece.Color) && !IsCheck(fields, row, 3, currentField.Piece.Color))
                    {
                        result.Add((row, 1));
                    }
                }
                
                if(row == 0 && fields[row, 5].Piece == null && fields[row, 6].Piece == null && fields[row, 7].Piece != null && fields[row, 7].Piece.Type == "Rook" && !fields[row, 7].Piece.Dirty)
                {
                    if (!IsCheck(fields, row, 5, currentField.Piece.Color) && !IsCheck(fields, row, 6, currentField.Piece.Color))
                    {
                        result.Add((row, 6));
                    }
                }

                if (row == 7 && fields[row, 1].Piece == null && fields[row, 2].Piece == null && fields[row, 3].Piece == null && fields[row, 0].Piece != null && fields[row, 0].Piece.Type == "Rook" && !fields[row, 0].Piece.Dirty)
                {
                    if (!IsCheck(fields, row, 1, currentField.Piece.Color) && !IsCheck(fields, row, 2, currentField.Piece.Color) && !IsCheck(fields, row, 3, currentField.Piece.Color))
                    {
                        result.Add((row, 1));
                    }
                }

                if (row == 7 && fields[row, 5].Piece == null && fields[row, 6].Piece == null && fields[row, 7].Piece != null && fields[row, 7].Piece.Type == "Rook" && !fields[row, 7].Piece.Dirty)
                {
                    if (!IsCheck(fields, row, 5, currentField.Piece.Color) && !IsCheck(fields, row, 6, currentField.Piece.Color))
                    {
                        result.Add((row, 6));
                    }
                }
            }
            return result;
        }

        public static List<(int, int)> KingCoverage(Field[,] fields, int row, int column)
        {
            var result = new List<(int, int)>();

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
                        "King" => PawnMechanics.PawnCoverage(fields, i, j),
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

        //zjistí zda král dané barvy je šachován (funkce která najde krále dané barvy a pak spustí IsCheck, v podstatě pouze schování prohledávání šachovnice do funkce)
        public static bool IsKingChecked(Field[,] fields, bool color)
        {
            // Projde všechny pole šachovnice
            for (int i = 0; i < fields.GetLength(0); i++)
            {
                for (int j = 0; j < fields.GetLength(1); j++)
                {
                    var piece = fields[i, j].Piece;

                    // najde krále dané barvy a zkontroluje jestli je šachován
                    if (piece != null && piece.Color == color && piece.Type == "King")
                    {
                        if (IsCheck(fields, i, j, color))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static bool IsKingCheckedmated(Field[,] fields, bool color)
        {
            // Projde všechny pole šachovnice
            for (int i = 0; i < fields.GetLength(0); i++)
            {
                for (int j = 0; j < fields.GetLength(1); j++)
                {
                    var piece = fields[i, j].Piece;

                    // najde krále dané barvy a zkontroluje jestli je šachován
                    if (piece != null && piece.Color == color && piece.Type == "King")
                    {
                        if (IsCheckmate(fields, i, j, color))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static bool IsCheckmate(Field[,] fields, int row, int column, bool color)
        {
            //prvně zkontrolujeme krále, pokud on se může hýbat, tak není šach mat
            var result = KingMove(fields, row, column);
            if (result.Count > 0)
            {
                return false;
            }

            //následně zkontrolujeme zda můžeme vyhodit šachující figurku, a zda nám to v něčem pomůže
            if (CanCaptureToGetOutOfCheck(fields, row, column, color))
            {
                return false;
            }

            //naposledy projdeme všechny tahy, zda neexistuje nějaký který by hráči pomohl
            if(CanMoveToGetOutOfCheck(fields, row, column, color))
            {
                return false;
            }
            return true;
        }

        internal static bool CanCaptureToGetOutOfCheck(Field[,] fields, int row, int column, bool color)
        {
            //nejdřív najdeme šachující figurku:
            int? checkingRow = null;
            int? checkingColumn = null;
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

                    //podle nalezené figurky spustí metodu, počítáme hru s jedním králem, a tedy zde krále vynecháme abychom snížili počet duplicitních tahů (krále kontrolujeme už předtím)
                    List<(int, int)> opponentMoves = piece.Type switch
                    {
                        "Rook" => RookMechanics.RookCoverage(fields, i, j),
                        "Bishop" => BishopMechanics.BishopCoverage(fields, i, j),
                        "Queen" => QueenMechanics.QueenCoverage(fields, i, j),
                        "Knight" => KnightMechanics.KnightCoverage(fields, i, j),
                        "Pawn" => PawnMechanics.PawnCoverage(fields, i, j),
                        _ => new List<(int, int)>()
                    };

                    // pokud je pozice šachována, tak vrátí pozici šachující figurky
                    if (opponentMoves.Contains((row, column)))
                    {
                        checkingRow = i;
                        checkingColumn = j;
                        break;
                    }
                }
                if (checkingRow != null)
                {
                    break;
                }
            }

            //pokud najdeme šachující figurku tak se pokusíme najít zda jí můžeme sebrat
            //(ano, nedostatkem tohoto algoritmu je že najdeme jen tu první, pokud by jich existovalo více, ale jedním pohybem stejně můžeme vzít jen jednu z nich a v případě že nás šachují dvě, tak vzít jednu není řešením, do budoucna možnost optimalizace)
            // Projde všechny pole šachovnice
            if (checkingColumn != null && checkingRow != null)
            {
                for (int i = 0; i < fields.GetLength(0); i++)
                {
                    for (int j = 0; j < fields.GetLength(1); j++)
                    {
                        var piece = fields[i, j].Piece;

                        // přeskočí prázdná pole, a pole s figurkou protivníka
                        if (piece == null || piece.Color != color)
                        {
                            continue;
                        }

                        //podle nalezené figurky spustí metodu
                        List<(int, int)> ourMoves = piece.Type switch
                        {
                            "Rook" => RookMechanics.RookCoverage(fields, i, j),
                            "Bishop" => BishopMechanics.BishopCoverage(fields, i, j),
                            "Queen" => QueenMechanics.QueenCoverage(fields, i, j),
                            "Knight" => KnightMechanics.KnightCoverage(fields, i, j),
                            "Pawn" => PawnMechanics.PawnCoverage(fields, i, j),
                            _ => new List<(int, int)>()
                        };

                        //pokud můžeme figurku sebrat, zkusíme ji sebrat a pak zkontolovat zda jsme stále v šachu
                        if (checkingColumn.HasValue && checkingRow.HasValue && ourMoves.Contains((checkingRow.Value, checkingColumn.Value)))
                        {
                            Field[,] temporaryChessBoard = ObjectExtensions.DeepCopyFieldArray(fields);
                            temporaryChessBoard[checkingRow.Value, checkingColumn.Value].Piece = piece;
                            //pokud tím že vezmeme figurku král nebude v šachu, vrátíme true
                            if (!IsCheck(temporaryChessBoard, row, column, color))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        //zjistí jestli existuje nějaký tah který by hráče dostaal z šachu
        internal static bool CanMoveToGetOutOfCheck(Field[,] fields, int row, int column, bool color) {
            // Projde všechny pole šachovnice
            for (int i = 0; i < fields.GetLength(0); i++)
            {
                for (int j = 0; j < fields.GetLength(1); j++)
                {
                    var piece = fields[i, j].Piece;

                    // přeskočí prázdná pole, a pole s protivníkovou figurkou
                    if (piece == null || piece.Color != color)
                    {
                        continue;
                    }

                    //podle nalezené figurky spustí metodu
                    List<(int, int)> possibleMoves = piece.Type switch
                    {
                        "Rook" => RookMechanics.RookMove(fields, i, j),
                        "Bishop" => BishopMechanics.BishopMove(fields, i, j),
                        "Queen" => QueenMechanics.QueenMove(fields, i, j),
                        "Knight" => KnightMechanics.KnightCoverage(fields, i, j),
                        "Pawn" => PawnMechanics.PawnMove(fields, i, j),
                        _ => new List<(int, int)>()
                    };

                    // projdeme všechny možné kroky a zkontrolujeme, zda je stále šach
                    foreach(var possibleMove in possibleMoves)
                    {
                        Field[,] temporaryChessBoard = ObjectExtensions.DeepCopyFieldArray(fields);
                        temporaryChessBoard[possibleMove.Item1, possibleMove.Item2].Piece = temporaryChessBoard[i, j].Piece;
                        temporaryChessBoard[i, j].Piece = null;
                        if (!IsCheck(temporaryChessBoard, row, column, color))
                        {
                            return true;
                        }
                    };
                }
            }
            return false;
        }
    }
}
