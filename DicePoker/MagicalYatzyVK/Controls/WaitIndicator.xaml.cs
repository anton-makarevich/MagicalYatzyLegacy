using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace Sanet.Controls
{
    public partial class WaitIndicator : UserControl
    {
        
        #region Member Variables
        private Ellipse[] m_ellipseArray = null;
        private Storyboard m_indicatorStoryboard = null;
        #endregion

        #region Constants
        private const byte ALPHA_LEVEL_1 = 30;
        private const byte ALPHA_LEVEL_2 = 30;
        private const byte ALPHA_LEVEL_3 = 30;
        private const byte ALPHA_LEVEL_4 = 46;
        private const byte ALPHA_LEVEL_5 = 62;
        private const byte ALPHA_LEVEL_6 = 109;
        private const byte ALPHA_LEVEL_7 = 156;
        private const byte ALPHA_LEVEL_8 = 204;

        private const int INTERVAL_MS = 150;
        private const int ELLIPSE_COUNT = 8;
        #endregion

        #region DependencyProperties

        
        /// <summary>
        /// Returns if visible side is face
        /// </summary>
        public bool IsActive
        {
            get
            {
                return (bool)GetValue(IsActiveProperty);
            }
            set
            {
                SetValue(IsActiveProperty, value);

            }
        }
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive",
            typeof(bool),
            typeof(WaitIndicator),
            new PropertyMetadata(false, new PropertyChangedCallback(OnActiveChange)));
        #endregion

        #region Constructor
        public WaitIndicator()
        {
            InitializeComponent();

            // Create a control array that allows use to easy enumerate the ellipses
            // without resorting to reflection
            m_ellipseArray = new Ellipse[ELLIPSE_COUNT];
            m_ellipseArray[0] = Ellipse1;
            m_ellipseArray[1] = Ellipse2;
            m_ellipseArray[2] = Ellipse3;
            m_ellipseArray[3] = Ellipse4;
            m_ellipseArray[4] = Ellipse5;
            m_ellipseArray[5] = Ellipse6;
            m_ellipseArray[6] = Ellipse7;
            m_ellipseArray[7] = Ellipse8;

            DefineStoryboard();
            
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
                LayoutRoot.Visibility = Visibility.Collapsed;
        } 
        #endregion

        #region Public Functions
        public void Start()
        {
            LayoutRoot.Visibility = Visibility.Visible;
            m_indicatorStoryboard.Begin();
        }

        public void Stop()
        {
            LayoutRoot.Visibility = Visibility.Collapsed;
            m_indicatorStoryboard.Stop();
        } 
        #endregion

        #region Private Functions
        private void DefineStoryboard()
        {
            // An array double the size of the number of ellipses which makes the
            // calculations a little easier so we don't need to handle circling
            // the array (starting in the middle, taking values from the end, then
            // more back from the beginning).  This way we can just shift our 
            // starting position
            byte[] alphaLevelsArray = new byte[16] { ALPHA_LEVEL_1, ALPHA_LEVEL_2, ALPHA_LEVEL_3, ALPHA_LEVEL_4, ALPHA_LEVEL_5, ALPHA_LEVEL_6, ALPHA_LEVEL_7, ALPHA_LEVEL_8, 
                                                    ALPHA_LEVEL_1, ALPHA_LEVEL_2, ALPHA_LEVEL_3, ALPHA_LEVEL_4, ALPHA_LEVEL_5, ALPHA_LEVEL_6, ALPHA_LEVEL_7, ALPHA_LEVEL_8 };

            m_indicatorStoryboard = new Storyboard();

            for (int ellipseIndex = 0; ellipseIndex < ELLIPSE_COUNT; ellipseIndex++)
            {
                Ellipse animateEllipse = m_ellipseArray[ellipseIndex];

                ColorAnimationUsingKeyFrames animation = new ColorAnimationUsingKeyFrames();
                animation.RepeatBehavior = RepeatBehavior.Forever;

                Storyboard.SetTarget(animation, animateEllipse);
                Storyboard.SetTargetProperty(animation, new PropertyPath("(Fill).(SolidBrush.Color)"));

                for (int frameIndex = 0; frameIndex <= 7; frameIndex++)
                {
                    LinearColorKeyFrame keyFrame = new LinearColorKeyFrame();
                    keyFrame.Value = Color.FromArgb(alphaLevelsArray[ellipseIndex + ELLIPSE_COUNT - frameIndex], 0, 156, 214);
                    keyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(frameIndex * INTERVAL_MS));

                    animation.KeyFrames.Add(keyFrame);
                }

                m_indicatorStoryboard.Children.Add(animation);
            }

            this.Resources.Add("IndicatorStoryboard", m_indicatorStoryboard);
        }

        static void OnActiveChange(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = (WaitIndicator)sender;
            if ((bool)e.NewValue)
                control.Start();
            else
                control.Stop();
        }
        #endregion
    }
}
