
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Sanet.Controls
{
    public sealed partial class BlurbControl 
    {
        public BlurbControl()
        {
            this.InitializeComponent();
            fadeoutTimer.Tick += fadeoutTimer_Tick;
        }

        void fadeoutTimer_Tick(object sender, object e)
        {
            fadeoutTimer.Stop();
            Animations.ChangeControlOpacity(this, 1.0, 0.0, 0.5,HideMe);
        }

        void HideMe()
        {
            this.Visibility =Visibility.Collapsed;
        }

        DispatcherTimer fadeoutTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(4) };

        public string Message
        {
            get
            {
                return (string)GetValue(MessageProperty);
            }
            set
            {
                SetValue(MessageProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(BlurbControl), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnTextChanged)));

        static void OnTextChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            // Get reference to self
            if (args.NewValue==null ||string.IsNullOrEmpty(args.NewValue.ToString()))
                return;
            BlurbControl  source = (BlurbControl )sender;

            source.MessageText.Text = args.NewValue.ToString();
            source.Visibility = Visibility.Visible;
            Animations.ChangeControlOpacity(source, 0, 1.0, 0.5);
            source.fadeoutTimer.Start();
        }
      
    }
}
