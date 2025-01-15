using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess
{
    public static class GameConstants
    {
        //výchozí šachovnice
        public static readonly string[][] initialBoard =
        {
            new string[] { "BR", "BN", "BB", "BQ", "BK", "BB", "BN", "BR" },
            new string[] { "BP", "BP", "BP", "BP", "BP", "BP", "BP", "BP" },
            new string[] { "", "", "", "", "", "", "", "" },
            new string[] { "", "", "", "", "", "", "", "" },
            new string[] { "", "", "", "", "", "", "", "" },
            new string[] { "", "", "", "", "", "", "", "" },
            new string[] { "WP", "WP", "WP", "WP", "WP", "WP", "WP", "WP" },
            new string[] { "WR", "WN", "WB", "WQ", "WK", "WB", "WN", "WR" }
        };
    }
}
