using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Diagnostics;


namespace chess
{
    public static class GameSave
    {
        public class GameState
        {
            public bool Turn { get; set; }
            public string[][] Board { get; set; }
        }
        //uložení aktuálního stavu hry
        public static void SaveGame(bool turn, string[][] board)
        {
            try
            {
                var gameState = new GameState
                {
                    Turn = turn,
                    Board = board
                };

                //json serializace
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(gameState, options);

                File.WriteAllText("./chess.json", json);
            }
            catch (Exception ex)
            {
            }
        }

        //načtení dat
        public static GameState? LoadGame()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "JSON files (*.json)|*.json";
                openFileDialog.Title = "Načíst hru";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string json = File.ReadAllText(openFileDialog.FileName);
                        var gameState = JsonSerializer.Deserialize<GameState>(json);

                        //validace formátu
                        if (gameState?.Board?.GetLength(0) == 8 && gameState.Board.All(x => x.GetLength(0) == 8))
                        {
                            return gameState;
                        }
                        else
                        {
                            MessageBox.Show("Data nejsou ve správném formátu.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Došlo k neočekávané vyjímce: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            return null;
        }

        //konverze Field[,] na 2d array pro uložní
        public static string[][] BoardConvertor(Field[,] fields)
        {
            string[][] convertedBoard = new string[fields.GetLength(0)][];

            for (int i = 0; i < fields.GetLength(0); i++)
            {
                convertedBoard[i] = new string[fields.GetLength(1)];
                for (int j = 0; j < fields.GetLength(1); j++)
                {
                    if (fields[i,j].Piece != null)
                    {
                        switch (fields[i, j].Piece.Type)
                        {
                            case "King":
                                convertedBoard[i][j] = fields[i, j].Piece.Color ? "WK" : "BK";
                                break;
                            case "Queen":
                                convertedBoard[i][j] = fields[i, j].Piece.Color ? "WQ" : "BQ";
                                break;
                            case "Rook":
                                convertedBoard[i][j] = fields[i, j].Piece.Color ? "WR" : "BR";
                                break;
                            case "Bishop":
                                convertedBoard[i][j] = fields[i, j].Piece.Color ? "WB" : "BB";
                                break;
                            case "Knight":
                                convertedBoard[i][j] = fields[i, j].Piece.Color ? "WN" : "BN";
                                break;
                            case "Pawn":
                                convertedBoard[i][j] = fields[i, j].Piece.Color ? "WP" : "BP";
                                break;
                            default:
                                convertedBoard[i][j] = "";
                                break;
                        }
                    }
                    else
                    {
                        convertedBoard[i][j] = "";
                    }
                }
            }
            return convertedBoard;
        }
    }
}
