
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
            return;
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
        
        
        
        
    }
}
