using Sanet.Kniffel.DicePanel;
using Sanet.Kniffel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Sanet.Kniffel.Models
{
    public static class RoamingSettings
    {
        static Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

        
        /// <summary>
        /// Get player info from roaming
        /// </summary>
        public static Player GetLastPlayer(int index)
        {
            var valueKey = "LastPlayer" + index.ToString();
            Player player=null;
            if (roamingSettings.Values.ContainsKey(valueKey))
            {
                ApplicationDataCompositeValue value = (ApplicationDataCompositeValue)roamingSettings.Values[valueKey];
                player = new Player();
                if (value != null && value.ContainsKey("strName"))
                    player.Name=  (string)value["strName"];
                if (value != null && value.ContainsKey("strPass"))
                    player.Password = (string)value["strPass"];
                if (value != null && value.ContainsKey("strType"))
                    player.Type = (PlayerType)Enum.Parse(typeof(PlayerType), (string)value["strType"]);
                if (value != null && value.ContainsKey("boolPass"))
                    player.RememberPass = (bool)value["boolPass"];
            }
            return player;
        }
         
        /// <summary>
        /// Save player info to roaming
        /// </summary>
        public static void SaveLastPlayer(Player player, int index)
        {
            var valueKey = "LastPlayer" + index.ToString();
            if (player != null)
            {
                ApplicationDataCompositeValue value = new ApplicationDataCompositeValue();
                value["strName"] = player.Name;
                value["strPass"] = player.Password;
                value["strType"] = player.Type.ToString();
                value["boolPass"] = player.RememberPass;
                roamingSettings.Values[valueKey] = value;
            }
            else
                roamingSettings.Values[valueKey] = null;
        }

        public static Rules LastRule
        {
            get
            {
                if (roamingSettings.Values["LastRule"] == null)
                    roamingSettings.Values["LastRule"] = Rules.krStandard.ToString();
                return ((Rules)Enum.Parse(typeof(Rules), (string)roamingSettings.Values["LastRule"]));
            }
            set
            {
                roamingSettings.Values["LastRule"] = value.ToString();
            }
        }

        #region setting section
        public static dpStyle DiceStyle
        {
            get
            {
                if (roamingSettings.Values["DiceStyle"] == null)
                    roamingSettings.Values["DiceStyle"] = dpStyle.dpsBlue.ToString();
                return ((dpStyle)Enum.Parse(typeof(dpStyle), (string)roamingSettings.Values["DiceStyle"]));
            }
            set 
            {
                roamingSettings.Values["DiceStyle"] = value.ToString();
            }
        }
        public static int DiceSpeed
        {
            get
            {
                if (roamingSettings.Values["DiceSpeed"] == null)
                    roamingSettings.Values["DiceSpeed"] = 5;
                return (int)roamingSettings.Values["DiceSpeed"];
            }
            set
            {
                roamingSettings.Values["DiceSpeed"] = value;
            }
        }
        public static int DiceAngle 
        {
            get
            {
                if (roamingSettings.Values["DiceAngle"] == null )
                    roamingSettings.Values["DiceAngle"] = 0;
                return (int)roamingSettings.Values["DiceAngle"];
            }
            set
            {
                roamingSettings.Values["DiceAngle"] = value;
            }
        }

        public const int _NumOfRuns = 0;
        public static int NumOfRuns
        {
            get
            {
                if (roamingSettings.Values["NumOfRuns"] == null)
                {
                    roamingSettings.Values["NumOfRuns"] = _NumOfRuns;
                    return _NumOfRuns;
                }
                return Convert.ToInt32(roamingSettings.Values["NumOfRuns"]);
            }
            set
            {
                roamingSettings.Values["NumOfRuns"] = value;

            }
        }

        #endregion

    }

}
