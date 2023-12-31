﻿using Sanet.Kniffel.DicePanel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Sanet.Kniffel.Models
{
    public class SettingsModel
    {
        public int DiceNumber { get; set; }
        public DiceStyle DiceStyle { get; set; }
        public int DiceSpeed { get; set; }
        public int DiceAngle { get; set; }

        public SettingsModel()
        {
             var settings = ApplicationData.Current.LocalSettings;

             if (settings.Values.ContainsKey("DiceNumber"))
                 DiceNumber = (int)settings.Values["DiceNumber"];
             else
                 DiceNumber = 5;

             if (settings.Values.ContainsKey("DiceStyle"))
                 DiceStyle = (dpStyle)Enum.Parse(typeof(dpStyle), (string)settings.Values["DiceStyle"]);
             else
                 DiceStyle = dpStyle.dpsBlue;

             if (settings.Values.ContainsKey("DiceSpeed"))
                 DiceSpeed = (int)settings.Values["DiceSpeed"];
             else
                 DiceSpeed = 5;

             if (settings.Values.ContainsKey("DiceAngle"))
                 DiceAngle = (int)settings.Values["DiceAngle"];
             else
                 DiceAngle = 0;
        }

        public void Save()
        {
            try
            {
                var settings = ApplicationData.Current.LocalSettings;
                settings.Values["DiceNumber"] = DiceNumber;
                settings.Values["DiceStyle"] = DiceStyle.ToString();
                settings.Values["DiceSpeed"] = DiceSpeed;
                settings.Values["DiceAngle"] = DiceAngle;
            }
            catch { }
        }
    }
}
