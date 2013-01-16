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
            get
            {
                if (ScoreType == KniffelScores.Total)
                    return "Chance".Localize();
                return ScoreType.ToString().Localize();
            }
        }
        //Aplied value
        int _Value;
        public int Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                HasValue = true;
                NotifyPropertyChanged("Value");
            }
        }

        
        /// <summary>
        /// maximum value for scoretype
        /// </summary>
        public int MaxValue
        {
            get 
            {
                switch (ScoreType)
                {
                    case KniffelScores.Ones:
                        return 5;
                    case KniffelScores.Twos:
                        return 10;
                    case KniffelScores.Threes:
                        return 15;
                    case KniffelScores.Fours:
                        return 20;
                    case KniffelScores.Fives:
                        return 25;
                    case KniffelScores.Sixs:
                        return 30;
                    case KniffelScores.ThreeOfAKind:
                        return 30;
                        
                    case KniffelScores.FourOfAKind:
                        return 30;
                        
                    case KniffelScores.FullHouse:
                        return 25;
                        
                    case KniffelScores.SmallStraight:
                        return 30;
                    case KniffelScores.LargeStraight:
                        return 40;
                       
                    case KniffelScores.Total:
                        return 30;
                        
                    case KniffelScores.Kniffel:
                        return 50;
                        
                }
                return 0;
            }
            
        }

        ///summary>
        /// possible value after roll
        /// </summary>
        private int _PossibleValue;
        public int PossibleValue
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

        /// <summary>
        /// Wheather this result has value (already filled)
        /// </summary>
        private bool _HasValue;
        public bool HasValue
        {
            get { return _HasValue; }
            set
            {
                if (_HasValue != value)
                {
                    _HasValue = value;
                    NotifyPropertyChanged("HasValue");
                }
            }
        }

        /// <summary>
        /// Wheather this result has 100 point bonus for second kniffel (for extended rules only)
        /// </summary>
        private bool _HasBonus;
        public bool HasBonus
        {
            get { return _HasBonus; }
            set
            {
                if (_HasBonus != value)
                {
                    _HasBonus = value;
                    NotifyPropertyChanged("HasBonus");
                }
            }
        }

        /// <summary>
        /// Wheather it's a numeric result
        /// </summary>
        public bool IsNumeric
        {
            get
            {
                return (ScoreType == KniffelScores.Ones ||
                    ScoreType == KniffelScores.Twos ||
                    ScoreType == KniffelScores.Threes ||
                    ScoreType == KniffelScores.Fours ||
                    ScoreType == KniffelScores.Fives ||
                    ScoreType == KniffelScores.Sixs);
            }
        }

        /// <summary>
        /// wheather value is null
        /// </summary>
        public bool IsZeroValue
        {
            get
            {
                return Value == 0;
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
