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
        public bool Dirty { get; set; } = false; //používáme k zjištění zda se král nebo věže pohly nebo ne (kvůli rošáďě) 
        public PictureBox pictureBox { get; set; }
        public Piece(string type, bool color, PictureBox pictureBox)
        {
            Type = type;
            Color = color;
            this.pictureBox = pictureBox;
        }

        public Piece DeepCopy()
        {
            return new Piece(Type, Color, new PictureBox());
        }
    }
}
