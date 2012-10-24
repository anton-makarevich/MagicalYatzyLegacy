using Sanet.AllWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Sanet.Kniffel.Models
{
    public class RollResult:BaseViewModel
    {
        string _Text;
        public string Text
        {
            get { return _Text; }
        }
        int _Value;
        public int Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                NotifyPropertyChanged("Value");
            }
        }

        Brush _Foreground;
        public Brush Foreground
        {
            get 
            { 
                return _Foreground; }
        }
        Brush _Background;
        public Brush Background
        {
            get
            {
                return _Background;
            }
        }
        KniffelScores _ScoreType;
        public KniffelScores ScoreType
        {
            get
            {return _ScoreType;}
            set
            {
                _ScoreType = value;
                _Text = value.ToString();
                _Background = Brushes.SolidSanetBlue;
                _Foreground = new SolidColorBrush(Colors.White);
                NotifyPropertyChanged("ScoreType");
                NotifyPropertyChanged("Text");
                NotifyPropertyChanged("Background");
                NotifyPropertyChanged("Foreground");
            }
        }
    }
    public enum KniffelScores
    {
        Total,
        FullHouse,
        LargeStraight,
        SmallStraights,
        ThreeInRow,
        FourInRow,
        Kniffel
    }
}
