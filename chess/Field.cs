using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess
{
    public class Field
    {
        public int Column { get; set; }
        public int Row { get; set; }
        public bool Color { get; set; }
        public Piece? Piece { get; set; }

        public Panel Panel { get; set; }
        public Field(int column, int row, bool color, Panel panel, Piece? piece)
        {
            Column = column;
            Row = row;
            Color = color;
            Panel = panel;
            Piece = piece;
        }
    }
}
