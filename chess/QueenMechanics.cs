using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess
{
    public static class QueenMechanics
    {
        public static List<(int, int)> QueenMove(Field[,] fields, int row, int column)
        {
            var result = new List<(int, int)>();

            // královna je vlastně kombinace věže a střelce
            result.AddRange(RookMechanics.RookMove(fields, row, column));
            result.AddRange(BishopMechanics.BishopMove(fields, row, column));

            return result;
        }

        public static List<(int, int)> QueenCoverage(Field[,] fields, int row, int column)
        {
            var result = new List<(int, int)>();

            // královna je vlastně kombinace věže a střelce
            result.AddRange(RookMechanics.RookCoverage(fields, row, column));
            result.AddRange(BishopMechanics.BishopCoverage(fields, row, column));

            return result;
        }
    }
}
