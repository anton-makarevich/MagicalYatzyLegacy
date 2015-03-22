using Sanet.Kniffel.Models;
using Sanet.Kniffel.Models.Enums;
using Sanet.Kniffel.Models.Interfaces;
using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanet.Kniffel.ViewModels
{
    public class RollResultWrapper:BaseViewModel,IRollResult
    {
        RollResult _Result;

        public RollResult Result
        {
            get
            {
                return _Result;
            }
        }

        public RollResultWrapper(RollResult result)
        {
            _Result = result;
        }


        public string Text
        {
            get { return ScoreType.ToString().Localize(); }
        }

        public string TextShort
        {
            get { return (ScoreType.ToString() + "Short").Localize(); }
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
        
        public int Value
        {
            get { return _Result.Value; }
            set
            {
                HasValue = false;
                _Result.Value = value;
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
                return _Result.MaxValue;
            }

        }

        ///summary>
        /// possible value after roll
        /// </summary>
        public int PossibleValue
        {
            get { return _Result.PossibleValue; }
            set
            {
                if (_Result.PossibleValue != value)
                {
                    _Result.PossibleValue = value;
                    NotifyPropertyChanged("PossibleValue");
                }
            }
        }

        /// <summary>
        /// Wheather this result has value (already filled)
        /// </summary>
        public bool HasValue
        {
            get 
            {
                return _Result.HasValue;
            }
            set
            {
                if (_Result.HasValue != value)
                {
                    _Result.HasValue = value;
                    NotifyPropertyChanged("HasValue");
                }
                else if (value)
                    NotifyPropertyChanged("HasValue");
            }
        }

        /// <summary>
        /// Wheather this result has 100 point bonus for second kniffel (for extended rules only)
        /// </summary>
        public bool HasBonus
        {
            get { return _Result.HasBonus; }
            set
            {
                if (_Result.HasBonus != value)
                {
                    _Result.HasBonus = value;
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
                return _Result.IsNumeric;
            }
        }

        /// <summary>
        /// wheather value is null
        /// </summary>
        public bool IsZeroValue
        {
            get
            {
                return _Result.IsZeroValue;
            }
        }

        public KniffelScores ScoreType
        {
            get
            { return _Result.ScoreType; }
            set
            {
                _Result.ScoreType = value;
                NotifyPropertyChanged("ScoreType");
                NotifyPropertyChanged("Text");

            }
        }

        public void Refresh()
        {
            NotifyPropertyChanged("HasBonus");
            //if (HasValue)
            //    NotifyPropertyChanged("HasValue");
            NotifyPropertyChanged("Value");
        }
    }
}
