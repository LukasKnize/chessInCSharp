using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace chess
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void DrawGame(object sender, EventArgs e)
        {
            DrawChessBoard();
            CreatePieces(GameConstants.initialBoard);
        }

        Field[,] Fields = new Field[8, 8];

        private void DrawChessBoard()
        {
            bool lastColor = false;

            for (int rowIndex = 0; rowIndex < 8; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < 8; columnIndex++)
                {
                    Panel panel = new Panel();
                    panel.Width = 100;
                    panel.Height = 100;
                    panel.BackColor = lastColor == true ? Color.DarkGray : Color.White;
                    panel.Left = columnIndex * 100;
                    panel.Top = rowIndex * 100;
                    Controls.Add(panel);
                    lastColor = columnIndex == 7 ? lastColor : !lastColor;
                    Fields[rowIndex, columnIndex] = new Field(columnIndex, rowIndex, columnIndex == 7 ? !lastColor : lastColor, panel, null);
                    int currentRow = rowIndex;
                    int currentColumn = columnIndex;
                    panel.Click += (sender, e) => { fieldClick(Fields[currentRow, currentColumn], panel); };
                }
            }
        }
        List<Piece> piecesList = new List<Piece>();

        private bool turn = true;

        private void CreatePieces(string[,] initialValues)
        {
            for (int rowIndex = 0; rowIndex < initialValues.GetLength(0); rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < initialValues.GetLength(1); columnIndex++)
                {
                    string image = "";
                    string piece = "";
                    bool pieceColor = true;
                    switch (initialValues[rowIndex, columnIndex])
                    {
                        case "BP":
                            image = "blackPawn.png";
                            piece = "Pawn";
                            pieceColor = false;
                            break;
                        case "WP":
                            image = "whitePawn.png";
                            piece = "Pawn";
                            pieceColor = true;
                            break;
                        case "BR":
                            image = "blackRook.png";
                            piece = "Rook";
                            pieceColor = false;
                            break;
                        case "WR":
                            image = "whiteRook.png";
                            piece = "Rook";
                            pieceColor = true;
                            break;
                        case "BN":
                            image = "blackKnight.png";
                            piece = "Knight";
                            pieceColor = false;
                            break;
                        case "WN":
                            image = "whiteKnight.png";
                            piece = "Knight";
                            pieceColor = true;
                            break;
                        case "BB":
                            image = "blackBishop.png";
                            piece = "Bishop";
                            pieceColor = false;
                            break;
                        case "WB":
                            image = "whiteBishop.png";
                            piece = "Bishop";
                            pieceColor = true;
                            break;
                        case "BQ":
                            image = "blackQueen.png";
                            piece = "Queen";
                            pieceColor = false;
                            break;
                        case "WQ":
                            image = "whiteQueen.png";
                            piece = "Queen";
                            pieceColor = true;
                            break;
                        case "BK":
                            image = "blackKing.png";
                            piece = "King";
                            pieceColor = false;
                            break;
                        case "WK":
                            image = "whiteKing.png";
                            piece = "King";
                            pieceColor = true;
                            break;
                        default:
                            break;
                    }

                    if (image != "" && piece != "")
                    {
                        PictureBox pictureBox = new PictureBox();
                        pictureBox.Width = 100;
                        pictureBox.Height = 100;
                        pictureBox.Image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", image));
                        pictureBox.Left = columnIndex * 100;
                        pictureBox.Top = rowIndex * 100;
                        pictureBox.BackColor = Fields[rowIndex, columnIndex].Color ? Color.White : Color.DarkGray;
                        pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
                        pictureBox.Enabled = false;
                        Controls.Add(pictureBox);
                        Controls.SetChildIndex(pictureBox, 0);
                        Fields[rowIndex, columnIndex].Piece = new Piece(piece, pieceColor, pictureBox);
                    }
                }
            }
        }

        private Field? SelectedField;
        private List<(int, int)> HighLightedCoords = new List<(int, int)>();
        private void fieldClick(Field clickedField, Panel selectedPanel)
        {
            if (SelectedField == null)
            {
                if (clickedField.Piece != null && clickedField.Piece.Color == turn)
                {
                    SelectedField = clickedField;
                    selectedPanel.BackColor = Color.FromArgb(137, 189, 158);
                    clickedField.Piece.pictureBox.BackColor = Color.FromArgb(137, 189, 158);
                    HighLightedCoords = MoveValidator.GetValidMoves(Fields, clickedField.Row, clickedField.Column);
                    HighLightedCoords.ForEach(possibleMove =>
                    {
                        Field hightlightField = Fields[possibleMove.Item1, possibleMove.Item2];
                        if (hightlightField.Piece != null)
                        {
                            hightlightField.Piece.pictureBox.BackColor = Color.FromArgb(240, 201, 135);
                        }
                        else
                        {
                            hightlightField.Panel.BackColor = Color.FromArgb(240, 201, 135);
                        }
                    });
                }
            }
            else if (HighLightedCoords.Contains((clickedField.Row, clickedField.Column)))
            {

                if (clickedField.Piece != null && clickedField.Piece.pictureBox != null)
                {
                    Controls.Remove(clickedField.Piece.pictureBox);
                }
                clickedField.Piece = SelectedField.Piece;
                SelectedField.Piece = null;
                clickedField.Piece.pictureBox.Top = clickedField.Row * 100;
                clickedField.Piece.pictureBox.Left = clickedField.Column * 100;
                clickedField.Piece.pictureBox.BackColor = clickedField.Color == true ? Color.White : Color.DarkGray;
                if (turn && clickedField.Row == 0 && clickedField.Piece.Type == "Pawn")
                {
                    var promotionForm = new PieceSelect(pieceSelected =>
                    {
                        PawnMechanics.PawnPromotion(pieceSelected, Controls);
                    }, turn, clickedField);

                    promotionForm.ShowDialog();
                }
                else if (!turn && clickedField.Row == 7 && clickedField.Piece.Type == "Pawn")
                {
                    var promotionForm = new PieceSelect(pieceSelected =>
                    {
                        PawnMechanics.PawnPromotion(pieceSelected, Controls);
                    }, turn, clickedField);

                    promotionForm.ShowDialog();
                }

                ClearHightLight();
                if (KingMechanics.IsKingChecked(Fields, !turn))
                {
                    if (KingMechanics.IsKingCheckedmated(Fields, !turn))
                    {
                        Debug.Print("mate");
                    }
                }
                turn = !turn;


            }
            else if (clickedField.Column == SelectedField.Column && clickedField.Row == SelectedField.Row)
            {
                if (clickedField.Piece != null)
                {
                    clickedField.Piece.pictureBox.BackColor = clickedField.Color == true ? Color.White : Color.DarkGray;
                }
                else
                {
                    clickedField.Panel.BackColor = clickedField.Color == true ? Color.White : Color.DarkGray;
                }
                ClearHightLight();
            }

        }

        private void ClearHightLight()
        {
            foreach (var HighLightedCoord in HighLightedCoords)
            {
                Field field = Fields[HighLightedCoord.Item1, HighLightedCoord.Item2];
                if (field.Piece != null)
                {
                    field.Piece.pictureBox.BackColor = field.Color == true ? Color.White : Color.DarkGray;
                }
                else
                {
                    field.Panel.BackColor = field.Color == true ? Color.White : Color.DarkGray;
                }

            }
            SelectedField.Panel.BackColor = SelectedField.Color == true ? Color.White : Color.DarkGray;
            SelectedField = null;
            HighLightedCoords.Clear();
        }
    }
}
