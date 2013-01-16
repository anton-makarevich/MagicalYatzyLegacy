using System;
using System.Net;
using System.Windows;

using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media;

namespace Sanet.Controls
{
    public partial class TipsProvider :StackPanel
    {
        public event EventHandler Vanished;
        public TipsProvider()
        {
            //InitializeComponent();
        }



        public string Text
        {
            get {
                return (string)GetValue(TextProperty); }
            set {
                SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TipsProvider), new PropertyMetadata(string.Empty,new PropertyChangedCallback(OnOrientationChanged)));

       static void OnOrientationChanged(object sender, DependencyPropertyChangedEventArgs args)
       {
           // Get reference to self
           TipsProvider source = (TipsProvider)sender;

           source.ShowText((string)args.NewValue,Colors.Blue);
       }

            

        //private dictionaries
        //Dictionary<TextBlock, Storyboard> storyboards = new Dictionary<TextBlock, Storyboard>();
        Dictionary<Storyboard, TextBlock> textblocks = new Dictionary<Storyboard, TextBlock>();

        Random r = new Random();

        public void ShowText(string text, Color color)
        {
            if (string.IsNullOrEmpty(text)) return;
            if (OnlyOneItem)
            {
                foreach (UIElement ui in this.Children)
                    ui.Visibility = Visibility.Collapsed;
            }

            TextBlock tb = new TextBlock { TextAlignment = TextAlignment.Center, TextWrapping = TextWrapping.Wrap, Margin=new Thickness(10,0,10,0) };
            
            tb.Text = text;
            LastText = text;
            tb.Foreground = new SolidColorBrush(color);
            tb.Opacity = 1;
            tb.FontSize = FontSize;
            this.Children.Insert(0, tb);
            if (IsFading)
            {
                Storyboard sb = new Storyboard();
                sb.Completed += barinfoEndAnimation;

                sb.Children.Clear();

                //opacity animation to "remove" tip
                Duration dur = TimeSpan.FromSeconds(LifeTime - VisibleTime);
                DoubleAnimation OpacityAnimation = new DoubleAnimation();
                OpacityAnimation.BeginTime = TimeSpan.FromSeconds(VisibleTime);
                OpacityAnimation.Duration = dur;
                sb.Duration = TimeSpan.FromSeconds(LifeTime);
                sb.Children.Add(OpacityAnimation);
                Storyboard.SetTarget(OpacityAnimation, tb);
                Storyboard.SetTargetProperty(OpacityAnimation, "UIElement.Opacity");
                OpacityAnimation.From = 1;
                OpacityAnimation.To = 0;
                //storyboards.Add(tb, sb);
                textblocks.Add(sb, tb);
                

                sb.Begin();
            }
        }
        public void ShowText(string[] texts, Color color)
        {
            ShowText(texts[r.Next(0, texts.Length)], color);
        }
        //in the end
        public void barinfoEndAnimation(object sender, object e)
        {
            Storyboard sb = (Storyboard)sender;
            TextBlock tb = textblocks[sb];
            if (Vanished != null && tb.Visibility == Visibility.Visible) Vanished(this, null);
            this.Children.Remove(tb);
            textblocks.Remove(sb);

        }
        public string LastText { get; set; }

        public bool OnlyOneItem { get; set; }

        private double lifetime = 6;
        public double LifeTime
        {
            get
            { return lifetime; }
            set
            { lifetime = value; }
        }

        private double vistime = 0;
        public double VisibleTime
        {
            get
            { return vistime; }
            set
            { vistime = value; }
        }
        double _fontsize = 20;
        public double FontSize
        {
            get { return _fontsize; }
            set { _fontsize = value; }
        }

        bool _isFading = true;
        public bool IsFading
        {
            get { return _isFading; }
            set { _isFading = value; }
        }

    }
}
