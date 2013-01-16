using Sanet.AllWrite;
using Sanet.Kniffel.Models.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Sanet.Common
{
    public class ScoreTypeToColorConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var score = (KniffelScores)value  ;
                switch (score)
                {
                    case KniffelScores.Total:
                        return Brushes.SolidSanetBlue;
                    case KniffelScores.OfAKind:
                        return Brushes.SolidSilverBackColor;
                    case KniffelScores.Pairs:
                        return Brushes.SolidBronzeBackColor;
                    case KniffelScores.FullHouse:
                    case KniffelScores.SmallStraight:
                    case KniffelScores.LargeStraight:
                        return Brushes.SolidGoldBackColor;
                }
            }

            catch { }

            return new SolidColorBrush(Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
