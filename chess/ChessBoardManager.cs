using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chess
{
    public class ChessBoardManager
    {
        internal Field[,] Fields = new Field[8, 8];

        private bool turn = true;

        //funkce vykreslí šachovnici na formulář
        public void DrawChessBoard(Control.ControlCollection controls)
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
                    controls.Add(panel);
                    lastColor = columnIndex == 7 ? lastColor : !lastColor;
                    Fields[rowIndex, columnIndex] = new Field(columnIndex, rowIndex, columnIndex == 7 ? !lastColor : lastColor, panel, null);
                    int currentRow = rowIndex;
                    int currentColumn = columnIndex;
                    panel.Click += (sender, e) => { fieldClick(Fields[currentRow, currentColumn], panel, controls); };
                }
            }
        }

        //funkce vytvoří postavy na specifických polích
        public void CreatePieces(string[,] initialValues, Control.ControlCollection controls)
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
                        controls.Add(pictureBox);
                        controls.SetChildIndex(pictureBox, 0);
                        Fields[rowIndex, columnIndex].Piece = new Piece(piece, pieceColor, pictureBox);
                    }
                }
            }
        }

        private Field? SelectedField;

        private List<(int, int)> HighLightedCoords = new List<(int, int)>();

        //funkce která se stará o pohyb postav a vykreslování průběhu hry
        private void fieldClick(Field clickedField, Panel selectedPanel, Control.ControlCollection controls)
        {
            //pokud nemáme žádné pole vybrané
            if (SelectedField == null)
            {
                //zkontrolujeme že na políčku je figurka barvy, která je na tahu
                if (clickedField.Piece != null && clickedField.Piece.Color == turn)
                {
                    SelectedField = clickedField;
                    //zvýraznění vybrané figurky
                    selectedPanel.BackColor = Color.FromArgb(137, 189, 158);
                    clickedField.Piece.pictureBox.BackColor = Color.FromArgb(137, 189, 158);
                    //najdeme možné tahy dané figurky
                    HighLightedCoords = MoveValidator.GetValidMoves(Fields, clickedField.Row, clickedField.Column);
                    //zvýrazníme je
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
            //pokud už je pole vyznačené a zároveň se nachází ve vyznačených polích (polích možných tahů) tak provede tah
            else if (HighLightedCoords.Contains((clickedField.Row, clickedField.Column)))
            {
                //řešení rošády, asi by se dalo vyřešit lépe, kdybych to celé jinak navrhl, ale tohle mi teď přijde jako časově nejméně náročné řešení
                if (SelectedField.Piece != null && SelectedField.Piece.Type == "King")
                {
                    if (SelectedField.Row == 0 && clickedField.Row == 0 && SelectedField.Column == 4 && clickedField.Column == 1)
                    {
                        Field rookField = Fields[0, 2];
                        rookField.Piece = Fields[0, 0].Piece;
                        rookField.Piece = null;
                        if (rookField.Piece != null && rookField.Piece.pictureBox != null)
                        {
                            rookField.Piece.pictureBox.Top = rookField.Row * 100;
                            rookField.Piece.pictureBox.Left = rookField.Column * 100;
                            rookField.Piece.pictureBox.BackColor = rookField.Color == true ? Color.White : Color.DarkGray;
                        }
                    }
                    else if (SelectedField.Row == 0 && clickedField.Row == 0 && SelectedField.Column == 4 && clickedField.Column == 6)
                    {
                        Field rookField = Fields[0, 5];
                        rookField.Piece = Fields[0, 7].Piece;
                        Fields[0, 7].Piece = null;
                        if (rookField.Piece != null && rookField.Piece.pictureBox != null)
                        {
                            rookField.Piece.pictureBox.Top = rookField.Row * 100;
                            rookField.Piece.pictureBox.Left = rookField.Column * 100;
                            rookField.Piece.pictureBox.BackColor = rookField.Color == true ? Color.White : Color.DarkGray;
                        }

                    }
                    else if (SelectedField.Row == 7 && clickedField.Row == 7 && SelectedField.Column == 4 && clickedField.Column == 1)
                    {
                        Field rookField = Fields[7, 2];
                        rookField.Piece = Fields[7, 0].Piece;
                        Fields[7, 0].Piece = null;
                        if (rookField.Piece != null && rookField.Piece.pictureBox != null)
                        {
                            rookField.Piece.pictureBox.Top = rookField.Row * 100;
                            rookField.Piece.pictureBox.Left = rookField.Column * 100;
                            rookField.Piece.pictureBox.BackColor = rookField.Color == true ? Color.White : Color.DarkGray;
                        }
                    }
                    else if (SelectedField.Row == 7 && clickedField.Row == 7 && SelectedField.Column == 4 && clickedField.Column == 6)
                    {
                        Field rookField = Fields[7, 5];
                        rookField.Piece = Fields[7, 7].Piece;
                        Fields[7, 7].Piece = null;
                        if (rookField.Piece != null && rookField.Piece.pictureBox != null)
                        {
                            rookField.Piece.pictureBox.Top = rookField.Row * 100;
                            rookField.Piece.pictureBox.Left = rookField.Column * 100;
                            rookField.Piece.pictureBox.BackColor = rookField.Color == true ? Color.White : Color.DarkGray;
                        }
                    }
                }

                //pokud je na políčku protivník, tak ho to odebere
                if (clickedField.Piece != null && clickedField.Piece.pictureBox != null)
                {
                    controls.Remove(clickedField.Piece.pictureBox);
                }

                if (SelectedField.Piece != null)
                {
                    clickedField.Piece = SelectedField.Piece;
                    SelectedField.Piece = null;
                }

                if (clickedField.Piece != null && clickedField.Piece.pictureBox != null)
                {
                    clickedField.Piece.pictureBox.Top = clickedField.Row * 100;
                    clickedField.Piece.pictureBox.Left = clickedField.Column * 100;
                    clickedField.Piece.pictureBox.BackColor = clickedField.Color == true ? Color.White : Color.DarkGray;
                }
                //pokud pohneme s králem nebo věží, pak s ní ůž nemůžeme dělat rošádu
                if (clickedField.Piece != null && (clickedField.Piece.Type == "King" || clickedField.Piece.Type == "Rook"))
                {
                    clickedField.Piece.Dirty = true;
                }

                //v případě že se pěšák dostane na poslední pozici, tak je možné vybrat figurku místo něho
                if (turn && clickedField.Row == 0 && clickedField.Piece != null && clickedField.Piece.Type == "Pawn")
                {
                    var promotionForm = new PieceSelect(pieceSelected =>
                    {
                        PawnMechanics.PawnPromotion(pieceSelected, controls);
                    }, turn, clickedField);

                    promotionForm.ShowDialog();
                }
                else if (!turn && clickedField.Row == 7 && clickedField.Piece != null && clickedField.Piece.Type == "Pawn")
                {
                    var promotionForm = new PieceSelect(pieceSelected =>
                    {
                        PawnMechanics.PawnPromotion(pieceSelected, controls);
                    }, turn, clickedField);

                    promotionForm.ShowDialog();
                }

                //nakonec zruší všechno zvýraznění
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
            //pokud znovu klikneme na naši vybranou figurku, zvýraznění zrušíme a můžeme vybrat jinou figurku
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

        //vymazání zvýraznění
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
            if (SelectedField != null && SelectedField.Panel != null)
                SelectedField.Panel.BackColor = SelectedField.Color == true ? Color.White : Color.DarkGray;
            SelectedField = null;
            HighLightedCoords.Clear();
        }
    }
}
