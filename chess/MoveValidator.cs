using System;
using System.Collections.Generic;
using System.Data.Common;
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
                result = PawnMechanics.PawnMove(field, row, column);
            }
            else if (field[row, column].Piece.Type == "Rook")
            {
                result = RookMechanics.RookMove(field, row, column);
            }
            else if (field[row, column].Piece.Type == "Bishop")
            {
                result = BishopMechanics.BishopMove(field, row, column);
            }
            else if (field[row, column].Piece.Type == "Knight")
            {
                result = KnightMechanics.KnightMove(field, row, column);
            }
            else if (field[row, column].Piece.Type == "Queen")
            {
                result = QueenMechanics.QueenMove(field, row, column);
            }
            else if (field[row, column].Piece.Type == "King")
            {
                result = KingMechanics.KingMove(field, row, column);
            }
            return result;
        }
    }
}
