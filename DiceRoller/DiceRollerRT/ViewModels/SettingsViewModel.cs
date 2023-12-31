﻿using Sanet.Kniffel.DicePanel;
using Sanet.Kniffel.Models;
using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if WinRT
using Windows.UI.Xaml.Media.Imaging;
#else
using System.Windows.Media.Imaging;
#endif

namespace Sanet.Kniffel.ViewModels
{
    public class SettingsViewModel : AdBasedViewModel
    {
        public SettingsViewModel()
        {
            FillAppActions();
        }

        #region bind props
        /// <summary>
        /// Page title
        /// </summary>
        public string Title
        {
            get
            {
                return "SettingsCaptionText".Localize();
            }
        }

        public string SettingsStyleCaption
        {
            get
            {
                return "SettingsStyleCaptionText".Localize();
            }
        }

        public string AngLowText
        {
            get
            {
                return "AngLowText".Localize();
            }
        }
        public string AngHighText
        {
            get
            {
                return "AngHighText".Localize();
            }
        }
        public string AngVeryHighText
        {
            get
            {
                return "AngVeryHighText".Localize();
            }
        }
        public string SettingsAngleCaption
        {
            get
            {
                return "SettingsAngleCaptionText".Localize();
            }
        }

        public string SpeedSlow
        {
            get
            {
                return "SpeedSlowText".Localize();
            }
        }
        public string SpeedVerySlow
        {
            get
            {
                return "SpeedVerySlowText".Localize();
            }
        }
        public string SpeedFast
        {
            get
            {
                return "SpeedFastText".Localize();
            }
        }
        public string SpeedVeryFast
        {
            get
            {
                return "SpeedVeryFastText".Localize();
            }
        }
        public string SettingsSpeedCaption
        {
            get
            {
                return "SettingsSpeedCaptionText".Localize();
            }
        }

        //main props from model
        public int DiceNumber
        {
            get 
            {
                return RoamingSettings.DiceNumber;
            }
            set
            {
                RoamingSettings.DiceNumber = value;
                
                NotifyPropertyChanged("DiceNumber");
                NotifyPropertyChanged("IsNum1");
                NotifyPropertyChanged("IsNum2");
                NotifyPropertyChanged("IsNum3");
                NotifyPropertyChanged("IsNum4");
                NotifyPropertyChanged("IsNum5");
                NotifyPropertyChanged("IsNum6");
            }
        }
        public int DiceAngle
        {
            get
            {
                return RoamingSettings.DiceAngle;
            }
            set
            {
                RoamingSettings.DiceAngle = value;
                NotifyPropertyChanged("SettingsPanelAngle");
                NotifyPropertyChanged("DiceAngle");
                NotifyPropertyChanged("IsAngLow");
                NotifyPropertyChanged("IsAngHigh");
                NotifyPropertyChanged("IsAngVeryHigh");
            }
        }
        public int DiceSpeed
        {
            get
            {
                return RoamingSettings.DiceSpeed;
            }
            set
            {
                RoamingSettings.DiceSpeed = value;
                NotifyPropertyChanged("SettingsPanelSpeed");
                NotifyPropertyChanged("DiceSpeed");
                NotifyPropertyChanged("IsSpeedVerySlow");
                NotifyPropertyChanged("IsSpeedSlow");
                NotifyPropertyChanged("IsSpeedFast");
                NotifyPropertyChanged("IsSpeedVeryFast");
            }
        }
        public DiceStyle DiceStyle
        {
            get
            {
                return RoamingSettings.DiceStyle;
            }
            set
            {
                RoamingSettings.DiceStyle = value;
                NotifyPropertyChanged("SettingsPanelStyle");
                NotifyPropertyChanged("DiceStyle");
                NotifyPropertyChanged("IsStyleBlue");
                NotifyPropertyChanged("IsStyleRed");
                NotifyPropertyChanged("IsStyleWhite");
            }
        }
        //for toggles
        public bool IsNum1
        {
            get
            {
                return DiceNumber == 1;
            }
            set
            {
                if (value)
                {
                    DiceNumber = 1;
                }
            }
        }
        public bool IsNum2
        {
            get
            {
                return DiceNumber == 2;
            }
            set
            {
                if (value)
                {
                    DiceNumber = 2;
                }
            }
        }
        public bool IsNum3
        {
            get
            {
                return DiceNumber == 3;
            }
            set
            {
                if (value)
                {
                    DiceNumber = 3;
                }
            }
        }
        public bool IsNum4
        {
            get
            {
                return DiceNumber == 4;
            }
            set
            {
                if (value)
                {
                    DiceNumber = 4;
                }
            }
        }
        public bool IsNum5
        {
            get
            {
                return DiceNumber == 5;
            }
            set
            {
                if (value)
                {
                    DiceNumber = 5;
                }
            }
        }
        public bool IsNum6
        {
            get
            {
                return DiceNumber == 6;
            }
            set
            {
                if (value)
                {
                    DiceNumber = 6;
                }
            }
        }

        public bool IsStyleBlue
        {
            get
            {
                return DiceStyle == DiceStyle.dpsBlue;
            }
            set
            {
                if (value)
                {
                    DiceStyle = DiceStyle.dpsBlue;
                }
            }
        }
        public bool IsStyleRed
        {
            get
            {
                return DiceStyle == DiceStyle.dpsBrutalRed;
            }
            set
            {
                if (value)
                {
                    DiceStyle = DiceStyle.dpsBrutalRed;
                }
            }
        }
        public bool IsStyleWhite
        {
            get
            {
                return DiceStyle == DiceStyle.dpsClassic;
            }
            set
            {
                if (value)
                {
                    DiceStyle = DiceStyle.dpsClassic;
                }
            }
        }

        public bool IsSpeedVerySlow
        {
            get
            {
                return DiceSpeed== 70;
            }
            set
            {
                if (value)
                {
                    DiceSpeed = 70;
                }
            }
        }
        public bool IsSpeedSlow
        {
            get
            {
                return DiceSpeed == 50;
            }
            set
            {
                if (value)
                {
                    DiceSpeed = 50;
                }
            }
        }
        public bool IsSpeedFast
        {
            get
            {
                return DiceSpeed == 30;
            }
            set
            {
                if (value)
                {
                    DiceSpeed = 30;
                }
            }
        }
        public bool IsSpeedVeryFast
        {
            get
            {
                return DiceSpeed == 15;
            }
            set
            {
                if (value)
                {
                    DiceSpeed = 15;
                }
            }
        }

        public bool IsAngLow
        {
            get 
            {
                return DiceAngle == 0;
            }
            set
            {
                if (value)
                {
                    DiceAngle = 0;
                }
            }
        }
        public bool IsAngHigh
        {
            get
            {
                return DiceAngle == 2;
            }
            set
            {
                if (value)
                {
                    DiceAngle = 2;
                }
            }
        }
        public bool IsAngVeryHigh
        {
            get
            {
                return DiceAngle == 4;
            }
            set
            {
                if (value)
                {
                    DiceAngle = 4;
                }
            }
        }

        public bool IsSoundEnabled
        {
            get
            {
                var rv= RoamingSettings.IsSoundEnabled;
                return rv;
            }
            set
            {
                RoamingSettings.IsSoundEnabled = value;
                NotifyPropertyChanged("IsSoundEnabled");
            }
        }
        public string SoundLabel
        {
            get
            {
                return "SoundLabel".Localize();
            }
        }
        public string OffContent
        {
            get
            {
                return "OffContent".Localize();
            }
        }
        public string OnContent
        {
            get
            {
                return "OnContent".Localize();
            }
        }
        private List<AboutAppAction> _AboutAppActions;
        public List<AboutAppAction> AboutAppActions
        {
            get { return _AboutAppActions; }
            set
            {
                if (_AboutAppActions != value)
                {
                    _AboutAppActions = value;
                    NotifyPropertyChanged("AboutAppActions");
                }
            }
        }
#endregion

        #region Methods

        public void FillAppActions()
        {
#if !ROLLER
            _AboutAppActions = new List<AboutAppAction>();
            
            if (StoreManager.IsAdVisible())
                _AboutAppActions.Add(
                    new AboutAppAction
                    {
                        Label = "RemoveAdAction",
                        MenuAction = new Action(
#if Win8
                            async 
#endif
                            () =>
                        {
#if Win8
                            await 
#endif
                            StoreManager.RemoveAd();
                            ViewModelProvider.GetViewModel<AboutPageViewModel>().NotifyAdChanged();
                            ViewModelProvider.GetViewModel<AboutPageViewModel>().FillAppActions();
                            ViewModelProvider.GetViewModel<SettingsViewModel>().NotifyAdChanged();
                            ViewModelProvider.GetViewModel<SettingsViewModel>().FillAppActions();
                            ViewModelProvider.GetViewModel<MainPageViewModel>().NotifyAdChanged();
                            ViewModelProvider.GetViewModel<MainPageViewModel>().FillSecondaryActions();
                        }),
                        Image = new BitmapImage(SanetImageProvider.GetAssetsImage("Unlock.png"))
                    });
            NotifyPropertyChanged("AboutAppActions");
#endif
        }

        #endregion
    }
}
