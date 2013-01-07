using Sanet.AllWrite;
using Sanet.Models;
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
        string _Value;
        public string Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                NotifyPropertyChanged("Value");
            }
        }

        //need to move to extensionn to keep core code platform independent

        //Brush _Foreground;
        //public Brush Foreground
        //{
        //    get 
        //    { 
        //        return _Foreground; }
        //}
        ////Brush _Background;
        //public Brush Background
        //{
        //    get
        //    {
        //        //return _Background;
        //        switch (ScoreType)
        //        {
        //            case KniffelScores.Total:
        //                return Brushes.SolidSanetBlue;
        //            case KniffelScores.FourInRow:
        //            case KniffelScores.ThreeInRow:
        //            case KniffelScores.OfAKind:
        //            case KniffelScores.FullHouse:
        //            case KniffelScores.Pairs:
        //                return Brushes.SolidBronzeBackColor;
        //            case KniffelScores.SmallStraight:
        //            case KniffelScores.LargeStraight:
        //                return Brushes.SolidSilverBackColor;
        //        }
        //        return new SolidColorBrush(Colors.Gray);
        //    }
        //}
        //KniffelScores _ScoreType;
        //public KniffelScores ScoreType
        //{
        //    get
        //    {return _ScoreType;}
        //    set
        //    {
        //        _ScoreType = value;
        //        //add use of resource here
        //        _Text = RModel.GetString(value.ToString());
        //        //_Background = Brushes.SolidSanetBlue;
        //        _Foreground = new SolidColorBrush(Colors.White);
        //        NotifyPropertyChanged("ScoreType");
        //        NotifyPropertyChanged("Text");
        //        NotifyPropertyChanged("Background");
        //        NotifyPropertyChanged("Foreground");
        //    }
        //}
        
    }
    public enum KniffelScores
    {
        Total,
        FullHouse,
        LargeStraight,
        SmallStraight,
        ThreeInRow,
        FourInRow,
        OfAKind,
        Pairs
    }
}
