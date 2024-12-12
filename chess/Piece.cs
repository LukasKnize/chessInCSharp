using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess
{
    public class Piece
    {
        public string Type { get; set; }
        public bool Color { get; set; }
        public PictureBox pictureBox { get; set; }
        public Piece(string type, bool color, PictureBox pictureBox)
        {
            Type = type;
            Color = color;
            this.pictureBox = pictureBox;
        }
    }
}
