using Sanet.AllWrite;
using Sanet.Kniffel.Models.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;


namespace Sanet.Common
{
    public class ScoreTypeToColorConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
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
                        return Brushes.SolidSanetBlue;
                    case KniffelScores.Kniffel:
                        return Brushes.ViewedBackColor;//.GoldBackColor;
                    case KniffelScores.Total:
                        return Brushes.SilverBackColor;
                    default:
                        return Brushes.GrayBackColor;
                }
            }

            catch (Exception ex) 
            {
                LogManager.Log("ScoreTypeToColorConverter", ex);
            }

            return new SolidColorBrush(Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            throw new NotImplementedException();
        }
    }

}
