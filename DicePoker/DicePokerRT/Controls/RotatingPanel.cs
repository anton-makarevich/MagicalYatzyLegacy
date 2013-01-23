using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Sanet.Controls
{
    public class RotatingPanel:Grid
    {
        PlaneProjection Rotator = new PlaneProjection();

        #region Constructor
        public RotatingPanel()
        {
            this.Projection= Rotator;
            this.Tapped += RotatingPanel_Tapped;
        }

       
        #endregion
        
        #region DependencyProperties
        /// <summary>
        /// Gets or sets content of face panel
        /// </summary>
        public Border FaceSide
        {
            get { return (Border)GetValue(FaceSideProperty); }
            set { SetValue(FaceSideProperty, value); }
        }

        public static readonly DependencyProperty FaceSideProperty =
            DependencyProperty.Register("FaceSide",
            typeof(Border),
            typeof(RotatingPanel),
            new PropertyMetadata(new Border(),new PropertyChangedCallback(OnSideChanged)));

        
        /// <summary>
        /// Gets or sets content of face panel
        /// </summary>
        public Border BackSide
        {
            get { return (Border)GetValue(BackSideProperty); }
            set { SetValue(BackSideProperty, value); }
        }

        public static readonly DependencyProperty BackSideProperty =
            DependencyProperty.Register("BackSide",
            typeof(Border),
            typeof(RotatingPanel),
            new PropertyMetadata(new Border(), new PropertyChangedCallback(OnSideChanged)));


        /// <summary>
        /// Returns if visible side is face
        /// </summary>
        public bool IsFace
        {
            get
            {
                return (bool)GetValue(IsFaceProperty);
            }
            set
            {
                SetValue(IsFaceProperty, value);
                if (value)
                {
                    FaceSide.Visibility = Visibility.Visible;
                    BackSide.Visibility = Visibility.Collapsed;
                }
                else
                {
                    FaceSide.Visibility = Visibility.Collapsed;
                    BackSide.Visibility = Visibility.Visible;
                }
            }
        }
        public static readonly DependencyProperty IsFaceProperty =
            DependencyProperty.Register("IsFace",
            typeof(bool),
            typeof(RotatingPanel),
            new PropertyMetadata(true, new PropertyChangedCallback(OnIsFaceChanged)));
        #endregion

        #region Properties
       

        
        #endregion

        #region Methods
        /// <summary>
        /// Start control rotating on tap
        /// </summary>
        void RotatingPanel_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            e.Handled = true;
            Animations.RotateProjection(Rotator, RotationAxis.X, 0, 90, 0.35, EndRotationStep);
        }

        void EndRotationStep()
        {
            IsFace = !IsFace;
            Animations.RotateProjection(Rotator, RotationAxis.X, 90, 0, 0.35);
        }


        /// <summary>
        /// Add face/back borders to visual tree
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void OnSideChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var grid = (RotatingPanel)sender;
            var border = (Border)e.NewValue;
            grid.Children.Add(border);
            grid.IsFace = grid.IsFace;
        }

        static void OnIsFaceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var grid = (RotatingPanel)sender;
            if ((bool)e.NewValue)
            {
                grid.FaceSide.Visibility = Visibility.Visible;
                grid.BackSide.Visibility = Visibility.Collapsed;
            }
            else
            {
                grid.FaceSide.Visibility = Visibility.Collapsed;
                grid.BackSide.Visibility = Visibility.Visible;
            }
        }
        #endregion
    }
}
