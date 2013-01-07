using Sanet.AllWrite;
using Sanet.Kniffel.Models.Enums;
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
       
        public string Text
        {
            get { return ScoreType.ToString().Localize(); }
        }

        public string TextShort
        {
            get { return (ScoreType.ToString()+"Short").Localize(); }
        }

        public string AltText
        {
            get { return ScoreType.ToString().Localize(); }
        }
        //Aplied value
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
        //possible value
        
        private string _PossibleValue;
        public string PossibleValue
        {
            get { return _PossibleValue; }
            set
            {
                if (_PossibleValue != value)
                {
                    _PossibleValue = value;
                    NotifyPropertyChanged("PossibleValue");
                }
            }
        }


        KniffelScores _ScoreType;
        public KniffelScores ScoreType
        {
            get
            { return _ScoreType; }
            set
            {
                _ScoreType = value;
                //add use of resource here
                //_Text = RModel.GetString(value.ToString());
                //_Background = Brushes.SolidSanetBlue;
                //_Foreground = new SolidColorBrush(Colors.White);
                NotifyPropertyChanged("ScoreType");
                NotifyPropertyChanged("Text");
                //NotifyPropertyChanged("Background");
                //NotifyPropertyChanged("Foreground");
            }
        }
        
        /*
        //need to move to extensions or use converter to keep core code platform independent
        //as this  contains xaml specific classes

        Brush _Foreground;
        public Brush Foreground
        {
            get 
            { 
                return _Foreground; }
        }
        //Brush _Background;
        public Brush Background
        {
            get
            {
                //return _Background;
                switch (ScoreType)
                {
                    case KniffelScores.Total:
                        return Brushes.SolidSanetBlue;
                    case KniffelScores.FourInRow:
                    case KniffelScores.ThreeInRow:
                    case KniffelScores.OfAKind:
                    case KniffelScores.FullHouse:
                    case KniffelScores.Pairs:
                        return Brushes.SolidBronzeBackColor;
                    case KniffelScores.SmallStraight:
                    case KniffelScores.LargeStraight:
                        return Brushes.SolidSilverBackColor;
                }
                return new SolidColorBrush(Colors.Gray);
            }
        }*/
        
    }
    
}
