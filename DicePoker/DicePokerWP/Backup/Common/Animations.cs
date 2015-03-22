using System;
using System.Net;
using System.Windows;
#if WinRT
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;

#else
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
#endif
namespace Sanet
{
    public static class Animations
    {
        public static void RotateProjection(PlaneProjection projection, RotationAxis axis, double from, double to, double time, EmptyHandler callback = null )
        {
            Storyboard RotAnimation = new Storyboard();

            //Animations
            DoubleAnimation daRot = new DoubleAnimation();
            daRot.Duration = TimeSpan.FromSeconds(time);
            switch (axis)
            {
#if WinRT
                case RotationAxis.X:
                    Storyboard.SetTargetProperty(daRot, "RotationX");
                    break;
                case RotationAxis.Y:
                    Storyboard.SetTargetProperty(daRot, "RotationY");
                    break;
                case RotationAxis.Z:
                    Storyboard.SetTargetProperty(daRot, "RotationZ");
                    break;
#else
                case RotationAxis.X:
                    Storyboard.SetTargetProperty(daRot, new PropertyPath(PlaneProjection.RotationXProperty));
                    break;
                case RotationAxis.Y:
                    Storyboard.SetTargetProperty(daRot, new PropertyPath(PlaneProjection.RotationYProperty));
                    break;
                case RotationAxis.Z:
                    Storyboard.SetTargetProperty(daRot, new PropertyPath(PlaneProjection.RotationZProperty));
                    break;
#endif
            }

            RotAnimation.Children.Add(daRot);

            Storyboard.SetTarget(daRot, projection);

            daRot.From = 0;
            daRot.To = 90;
            RotAnimation.Completed += (sender, e) =>
            {
                switch (axis)
                {
                    case RotationAxis.X:
                        projection.RotationX=to;
                        break;
                    case RotationAxis.Y:
                        projection.RotationY = to;
                        break;
                    case RotationAxis.Z:
                        projection.RotationZ = to;
                        break;
                }
                if (callback != null) callback();
            };
            RotAnimation.Begin();
        }
        public static void ChangeControlOpacity(UIElement control, double from, double to, double time, EmptyHandler callback = null)
        {
            if (to > 1) to = 1;
            if (to < 0) to = 0;
            if (from > 1) from = 1;
            if (from < 0) from = 0;

            Storyboard EffectAnimation = new Storyboard();

            //Animations
            DoubleAnimation daExposure = new DoubleAnimation();
            
#if SILVERLIGHT
            daExposure.Duration = TimeSpan.FromSeconds(time) ;
            EffectAnimation.Duration = TimeSpan.FromSeconds(time) ;
            Storyboard.SetTargetProperty(daExposure, new PropertyPath(UIElement.OpacityProperty));
#else
            daExposure.Duration = new Duration(TimeSpan.FromSeconds(time));
            EffectAnimation.Duration = new Duration(TimeSpan.FromSeconds(time));
            Storyboard.SetTargetProperty(daExposure, "UIElement.Opacity");
#endif
            EffectAnimation.Children.Add(daExposure);
            Storyboard.SetTarget(daExposure, control);

            daExposure.From = from;
            daExposure.To = to;

            control.Opacity = from;
            EffectAnimation.Completed += (sender, e) =>
            {
                control.Opacity = to;
                if (callback != null) callback();
            };
            EffectAnimation.Begin();
        }
        
    }
    public delegate void EmptyHandler();
    public enum RotationAxis
    {X,Y,Z}
}
