using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

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
                DoubleAnimation DoubleAnimationO = new DoubleAnimation();
                DoubleAnimationO.BeginTime = TimeSpan.FromSeconds(VisibleTime);
                DoubleAnimationO.Duration = dur;
                sb.Duration = TimeSpan.FromSeconds(LifeTime);
                sb.Children.Add(DoubleAnimationO);
                Storyboard.SetTarget(DoubleAnimationO, tb);
                Storyboard.SetTargetProperty(DoubleAnimationO, new PropertyPath(TextBlock.OpacityProperty));
                DoubleAnimationO.From = 1;
                DoubleAnimationO.To = 0;
                //storyboards.Add(tb, sb);
                textblocks.Add(sb, tb);
                

                sb.Begin();
            }
        }
        public void ShowText(string[] texts, Color color)
        {
            ShowText(texts[r.Next(0, texts.Length)], color);
        }
        //подчиска
        public void barinfoEndAnimation(object sender, EventArgs e)
        {
            Storyboard sb = (Storyboard)sender;
            TextBlock tb = textblocks[sb];
            if (Vanished != null && tb.Visibility == Visibility.Visible) Vanished(this, e);
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
