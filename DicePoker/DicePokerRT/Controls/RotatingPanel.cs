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
    public class RotatingPanel : Grid
    {
        /// <summary>
        /// Olane projection to simulate rotation
        /// </summary>
        PlaneProjection Rotator = new PlaneProjection();
        /// <summary>
        /// I have a problem with ui,xaml error
        /// noticed it happens only when control invisible on load
        /// so this is attempt to show control on load and hide on timer
        /// </summary>
        DispatcherTimer workAroundTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
        #region Constructor
        
        public RotatingPanel()
        {
            this.Projection = Rotator;
            this.Tapped += RotatingPanel_Tapped;

            workAroundTimer.Tick += workAroundTimer_Tick;
            workAroundTimer.Start();
            
        }

        void workAroundTimer_Tick(object sender, object e)
        {
            workAroundTimer.Tick -= workAroundTimer_Tick;
            workAroundTimer.Stop();
            if (!outsidechange)
                if (this.Tag==null)
                IsFace = true;
            
        }


        #endregion

        #region DependencyProperties

        bool outsidechange = false;
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
                
            }
        }
        public static readonly DependencyProperty IsFaceProperty =
            DependencyProperty.Register("IsFace",
            typeof(bool),
            typeof(RotatingPanel),
            new PropertyMetadata(false,new PropertyChangedCallback(OnSideChange)));
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

        static void OnSideChange(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = (RotatingPanel)sender;
            control.outsidechange=true;
            control.IsFace = (bool)e.NewValue;
        }

        #endregion
    }
}
