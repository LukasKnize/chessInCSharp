using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chess
{
    public partial class PieceSelect : Form
    {
        private bool _color;
        private Action<(Field, string)> _onPieceSelected;
        private Field _field;
        public PieceSelect(Action<(Field, string)> onPieceSelected, bool color, Field field)
        {
            InitializeComponent();
            _color = color;
            _onPieceSelected = onPieceSelected;
            _field = field;
        }

        private void PieceSelect_Load(object sender, EventArgs e)
        {
            if (_color)
            {
                pictureBox1.Image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "whiteBishop.png"));
                pictureBox2.Image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "whiteKnight.png"));
                pictureBox3.Image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "whiteRook.png"));
                pictureBox4.Image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "whiteQueen.png"));
            }
            else
            {
                pictureBox1.Image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "blackBishop.png"));
                pictureBox2.Image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "blackKnight.png"));
                pictureBox3.Image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "blackRook.png"));
                pictureBox4.Image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "blackQueen.png"));
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            _onPieceSelected.Invoke((_field, _color ? "WB" : "BB"));
            this.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            _onPieceSelected.Invoke((_field, _color ? "WN" : "BN"));
            this.Close();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            _onPieceSelected.Invoke((_field, _color ? "WR" : "BR"));
            this.Close();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            _onPieceSelected.Invoke((_field, _color ? "WQ" : "BQ"));
            this.Close();
        }
    }
}
