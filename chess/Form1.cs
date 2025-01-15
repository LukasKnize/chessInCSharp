using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace chess
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        ChessBoardManager chessBoardManager = new ChessBoardManager();

        //vykreselní nové hry
        private void DrawGame(object sender, EventArgs e)
        {
            chessBoardManager.DrawChessBoard(Controls, checkLabel);
            chessBoardManager.CreatePieces(GameConstants.initialBoard, Controls);
            chessBoardManager.SetTurn(true);
            checkLabel.Text = "";
        }

        private void imageSource_Click(object sender, EventArgs e)
        {
            OpenUrl("https://commons.wikimedia.org/wiki/Category:PNG_chess_pieces/Standard_transparent");
        }

        private void openGitHub_Click(object sender, EventArgs e)
        {
            OpenUrl("https://github.com/LukasKnize/chessInCSharp");
        }

        //funkce pro otevøení url v prohlížeèi
        public static void OpenUrl(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch (Exception ex)
            {

            }
        }

        private void newGameButton_Click(object sender, EventArgs e)
        {
            chessBoardManager.DrawChessBoard(Controls, checkLabel);
            chessBoardManager.CreatePieces(GameConstants.initialBoard, Controls);
            chessBoardManager.SetTurn(true);
            checkLabel.Text = "";
        }

        private void loadGameButton_Click(object sender, EventArgs e)
        {
            GameSave.GameState? gameState = GameSave.LoadGame();
            if (gameState != null)
            {
                chessBoardManager.DrawChessBoard(Controls, checkLabel);
                chessBoardManager.CreatePieces(gameState.Board, Controls);
                chessBoardManager.SetTurn(gameState.Turn);
                checkLabel.Text = "";
            }
        }
    }
}
