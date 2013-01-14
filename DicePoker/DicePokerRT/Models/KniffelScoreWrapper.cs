using DicePokerRT.KniffelLeaderBoardService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Sanet.Kniffel.Models
{
    /// <summary>
    /// W8 wrapper over KniffelScore class to bind to image
    /// </summary>
    public class KniffelScoreWrapper
    {
        KniffelScore _score;

        public KniffelScoreWrapper(KniffelScore score)
        {
            _score = score;
        }

        public ImageSource Image
        {
            get
            {
                try
                {
                    if (_score.PicUrl.Length > 2)
                        return new BitmapImage(new Uri(_score.PicUrl));
                }
                catch { }
                return null;
            }
        }

        public string Name
        {
            get
            {
                return _score.Player;
            }
        }

        public int Score
        {
            get
            {
                return _score.HighScore;
            }
        }

        public int Games
        {
            get
            {
                return _score.Games;
            }
        }

        public int Total
        {
            get
            {
                return _score.Total;
            }
        }
    }
}
