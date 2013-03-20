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
                    case KniffelScores.FullHouse:
                    case KniffelScores.FourOfAKind:
                    case KniffelScores.ThreeOfAKind:
                    case KniffelScores.SmallStraight:
                    case KniffelScores.LargeStraight:
                        return Brushes.SolidBronzeBackColor;
                    case KniffelScores.Kniffel:
                        return Brushes.SolidGoldBackColor;
                    default:
                        return Brushes.SolidSanetBlue;
                }
            }

            catch (Exception ex) 
            {
                LogManager.Log("ScoreTypeToColorConverter", ex);
            }

            return new SolidColorBrush(Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
