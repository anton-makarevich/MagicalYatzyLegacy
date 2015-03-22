using Mangopollo.Tiles;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanet.Models;
using Sanet;

namespace Sanet.Kniffel.Models
{
    /// <summary>
    /// Class to work with app tiles
    /// </summary>
    public static class TileHelper
    {
        /// <summary>
        /// Method to update the secondary tile content with given text
        /// </summary>
        /// <param name="tileID"></param>
        /// <param name="tileText"></param>
        public static void UpdateTileContent(string tileID, string tileText)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method to update the secondary tile content with given (template with block)
        /// </summary>
        public static void UpdateTileContent(string tileID, string tileText1, string tileText2,
            string tileText3, string tileText4, string block, string blockText)
        {

            throw new NotImplementedException();

        }

        /// <summary>
        /// Method to update the secondary tile content with given (template with 4 rows)
        /// </summary>
        public static void UpdateTileContent(string tileID, string tileText1, string tileText2,
            string tileText3, string tileText4)
        {

            if (tileID == "main")
            {
                string res = tileText2;
                if (string.IsNullOrEmpty(res))
                    res = tileText3;
                if (string.IsNullOrEmpty(res))
                    res = tileText4;

                if (!string.IsNullOrEmpty(res))
                    UpdateMainTile(tileText1, res);
            }

        }

                
        /// <summary>
        /// Uodate of main tile back side with current verse
        /// </summary>
        public static void UpdateMainTile(string title, string text)
        {
            if (Mangopollo.Utils.CanUseLiveTiles)
            {

                FlipTileData appTileData = GetTileFlipData(title, text, "AppNameLabel".Localize());
                ShellTile.ActiveTiles.First().Update(appTileData);
            }
            else
            {
                StandardTileData appTileData = GetTileData(title, text);
                ShellTile.ActiveTiles.First().Update(appTileData);
            }
        }

        public static Task PinVerseOfTheDay()
        {
            throw new NotImplementedException();
        }

        public static Task PinRandomVerse()
        {
            throw new NotImplementedException();
        }

        public static bool IsSecondaryTileExists(string tileid)
        {
            throw new NotImplementedException();
        }
        
        
        /// <summary>
        /// Tile data for wp7.8/wp8
        /// </summary>
        private static FlipTileData GetTileFlipData(string title, string text, string facetitle )
        {
            return new FlipTileData
            {
                Title = facetitle,
                BackTitle = title,
                BackContent = Utilities.TrimText(text, 39),
                WideBackContent = Utilities.TrimText(text, 81),
                SmallBackgroundImage = new Uri("/Assets/Tiles/FlipCycleTileSmall.png", UriKind.Relative),
                BackgroundImage = new Uri("/Assets/Tiles/FlipCycleTileMedium.png", UriKind.Relative),
                BackBackgroundImage = new Uri("/Assets/Tiles/FlipCycleTileMediumBack.png", UriKind.Relative),
                WideBackgroundImage = new Uri("/Assets/Tiles/FlipCycleTileLarge.png", UriKind.Relative),
                WideBackBackgroundImage = new Uri("/Assets/Tiles/FlipCycleTileLargeBack.png", UriKind.Relative),

            };
        }
        /// <summary>
        /// tiledata for wp7
        /// </summary>
        private static StandardTileData GetTileData(string title, string text, string facetitle = "Bible Pronto")
        {
            return new StandardTileData
            {
                Title = facetitle,
                BackTitle = title,
                BackContent = Utilities.TrimText(text, 39),
                BackgroundImage = new Uri("/Assets/Tiles/FlipCycleTileMedium.png", UriKind.Relative),
                BackBackgroundImage = new Uri("/Assets/Tiles/FlipCycleTileMediumBack.png", UriKind.Relative),

            };
        }

    }
}
