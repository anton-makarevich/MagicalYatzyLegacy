using Sanet.Kniffel.DicePanel;
using Sanet.Kniffel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanet.Kniffel.ViewModels
{
    class SettingsViewModel:BaseViewModel
    {
        public SettingsModel Settings = new SettingsModel();
        #region bind props
        //main props from model
        public int DiceNumber
        {
            get 
            {
                return Settings.DiceNumber;
            }
            set
            {
                Settings.DiceNumber = value;
                Settings.Save();
                NotifyPropertyChanged("DiceNumber");
            }
        }
        public int DiceAngle
        {
            get
            {
                return Settings.DiceAngle;
            }
            set
            {
                Settings.DiceAngle = value;
                Settings.Save();
                NotifyPropertyChanged("DiceAngle");
            }
        }
        public int DiceSpeed
        {
            get
            {
                return Settings.DiceSpeed;
            }
            set
            {
                Settings.DiceSpeed = value;
                Settings.Save();
                NotifyPropertyChanged("DiceSpeed");
            }
        }
        public dpStyle DiceStyle
        {
            get
            {
                return Settings.DiceStyle;
            }
            set
            {
                Settings.DiceStyle = value;
                Settings.Save();
                NotifyPropertyChanged("DiceStyle");
            }
        }
        //for toggles
        public bool IsNum1
        {
            get
            {
                return DiceNumber == 1;
            }
        }
        public bool IsNum2
        {
            get
            {
                return DiceNumber == 2;
            }
        }
        public bool IsNum3
        {
            get
            {
                return DiceNumber == 3;
            }
        }
        public bool IsNum4
        {
            get
            {
                return DiceNumber == 4;
            }
        }
        public bool IsNum5
        {
            get
            {
                return DiceNumber == 5;
            }
        }
        public bool IsNum6
        {
            get
            {
                return DiceNumber == 6;
            }
        }
        public bool IsStyleBlue
        {
            get
            {
                return DiceStyle == dpStyle.dpsBlue;
            }
        }
        public bool IsStyleRed
        {
            get
            {
                return DiceStyle == dpStyle.dpsBrutalRed;
            }
        }
        public bool IsStyleWhite
        {
            get
            {
                return DiceStyle == dpStyle.dpsClassic;
            }
        }
        public bool IsSpeedVerySlow
        {
            get
            {
                return DiceNumber == 25;
            }
        }
        public bool IsSpeedSlow
        {
            get
            {
                return DiceNumber == 15;
            }
        }
        public bool IsSpeedFast
        {
            get
            {
                return DiceNumber == 5;
            }
        }
        public bool IsSpeedVeryFast
        {
            get
            {
                return DiceNumber == 1;
            }
        }
        public bool IsAngLow
        {
            get 
            {
                return DiceAngle == 0;
            }
        }
        public bool IsAngHigh
        {
            get
            {
                return DiceAngle == 2;
            }
        }
        public bool IsAngVeryHigh
        {
            get
            {
                return DiceAngle == 4
            }
        }

#endregion
    }
}
