using System;
using System.Net;
using System.Windows;

#if WinRT
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.Foundation;

#else
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
#endif

namespace Sanet.AllWrite
{
    public static class Brushes
    {
        public static LinearGradientBrush UnviewedBackColor = SetupLinearGradientBrush(Color.FromArgb(0XFF, 0X18, 0X14, 0X15), Color.FromArgb(0XFF, 0X2C, 0X2A, 0X2B), Color.FromArgb(0XFF, 0X18, 0X14, 0X15));
        public static LinearGradientBrush ViewedBackColor = SetupLinearGradientBrush(Color.FromArgb(0XFF, 0X8B, 0X10, 0X10), Color.FromArgb(0XFF, 0XD4, 0X10, 0X10), Color.FromArgb(0XFF, 0X8B, 0X10, 0X10));
        public static LinearGradientBrush NextBackColor = SetupLinearGradientBrush(Color.FromArgb(0XFF, 0X10, 0X6E, 0X10), Color.FromArgb(0XFF, 0X10, 0XAA, 0X10), Color.FromArgb(0XFF, 0X10, 0X6E, 0X10));

        public static LinearGradientBrush GoldBackColor = SetupLinearGradientBrush(Color.FromArgb(0XFF, 0XF7, 0XA3, 0X0D), Color.FromArgb(0XFF, 0XFA, 0XBC, 0X4B), Color.FromArgb(0XFF, 0XF7, 0XA3, 0X0D));
        public static LinearGradientBrush SilverBackColor = SetupLinearGradientBrush(Color.FromArgb(0XFF, 0X8B, 0X8B, 0X8B), Color.FromArgb(0XFF, 0XB5, 0XB5, 0XB5), Color.FromArgb(0XFF, 0X8B, 0X8B, 0X8B));
        public static LinearGradientBrush BronzeBackColor = SetupLinearGradientBrush(Color.FromArgb(0XFF, 0X9D, 0X55, 0X09), Color.FromArgb(0XFF, 0XDF, 0X79, 0X0B), Color.FromArgb(0XFF, 0X9D, 0X55, 0X09));

        //solid colors
        public static SolidColorBrush SolidViewedBackColor = new SolidColorBrush(Color.FromArgb(0XFF, 0X8B, 0X10, 0X10));

        public static SolidColorBrush SolidGoldBackColor = new SolidColorBrush(Color.FromArgb(0XFF, 0XF7, 0XA3, 0X0D));
        public static SolidColorBrush SolidSilverBackColor = new SolidColorBrush(Color.FromArgb(0XFF, 0X8B, 0X8B, 0X8B));
        public static SolidColorBrush SolidBronzeBackColor = new SolidColorBrush(Color.FromArgb(0XFF, 0X9D, 0X55, 0X09));

        public static LinearGradientBrush GrayBackColor = SetupLinearGradientBrush(Color.FromArgb(0XFF, 0X4c, 0X4c, 0X4c), Color.FromArgb(0XFF, 0X8c, 0X8c, 0X8c), Color.FromArgb(0XFF, 0X4c, 0X4c, 0X4c));

        public static SolidColorBrush SolidSanetBlue = new SolidColorBrush(Color.FromArgb(255,0,156,214));
        
        public static LinearGradientBrush SetupLinearGradientBrush(Color c1, Color c2, Color c3)
        {
            LinearGradientBrush lgb = new LinearGradientBrush();
            GradientStop gs = null;
            lgb.StartPoint = new Point(0, 1);
            lgb.EndPoint = new Point(1, 0);

            gs = new GradientStop();
            gs.Color = c1;
            gs.Offset = 0;
            lgb.GradientStops.Add(gs);
            gs = new GradientStop();
            gs.Color = c2;
            gs.Offset = 0.5;
            lgb.GradientStops.Add(gs);
            gs = new GradientStop();
            gs.Color = c3;
            gs.Offset = 1;
            lgb.GradientStops.Add(gs);

            return lgb;
        }

        public static LinearGradientBrush SetupLinearGradientBrush(Color c1, Color c2)
        {
            LinearGradientBrush lgb = new LinearGradientBrush();
            GradientStop gs = null;
            lgb.StartPoint = new Point(0, 0);
            lgb.EndPoint = new Point(0, 1);

            gs = new GradientStop();
            gs.Color = c1;
            gs.Offset = 0;
            lgb.GradientStops.Add(gs);
            gs = new GradientStop();
            gs.Color = c2;
            gs.Offset = 1;
            lgb.GradientStops.Add(gs);

            return lgb;
        }

        public static SolidColorBrush TransparentBrush = new SolidColorBrush(Colors.Transparent);
    }
}
