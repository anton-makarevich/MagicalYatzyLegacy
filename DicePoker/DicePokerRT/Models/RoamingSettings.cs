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
using Sanet.Models;

namespace Sanet.Kniffel.Models
{
    public static class RoamingSettings
    {
        static Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

#if !ROLLER
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
#endif
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
                    roamingSettings.Values["DiceStyle"] = dpStyle.dpsClassic.ToString();
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
        public static int DiceNumber
        {
            get
            {
                if (roamingSettings.Values["DiceNumber"] == null)
                    roamingSettings.Values["DiceNumber"] = 5;
                return (int)roamingSettings.Values["DiceNumber"];
            }
            set
            {
                roamingSettings.Values["DiceNumber"] = value;
            }
        }
        public static bool IsSoundEnabled
        {
            get
            {
                if (roamingSettings.Values["IsSoundEnabled"] == null)
                    roamingSettings.Values["IsSoundEnabled"] = true;
                return (bool)(roamingSettings.Values["IsSoundEnabled"]);
            }
            set
            {
                roamingSettings.Values["IsSoundEnabled"] = value;
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

        #region records
        public static int LocalBabyRecord
        {
            get
            {
                if (roamingSettings.Values["LocalBabyRecord"] == null)
                    roamingSettings.Values["LocalBabyRecord"] = 0;
                return (int)roamingSettings.Values["LocalBabyRecord"];
            }
            set
            {
                roamingSettings.Values["LocalBabyRecord"] = value;
            }
        }
        public static int LocalSimpleRecord
        {
            get
            {
                if (roamingSettings.Values["LocalSimpleRecord"] == null)
                    roamingSettings.Values["LocalSimpleRecord"] = 0;
                return (int)roamingSettings.Values["LocalSimpleRecord"];
            }
            set
            {
                roamingSettings.Values["LocalSimpleRecord"] = value;
            }
        }
        public static int LocalStandardRecord
        {
            get
            {
                if (roamingSettings.Values["LocalStandardRecord"] == null)
                    roamingSettings.Values["LocalStandardRecord"] = 0;
                return (int)roamingSettings.Values["LocalStandardRecord"];
            }
            set
            {
                roamingSettings.Values["LocalStandardRecord"] = value;
            }
        }
        public static int LocalExtendedRecord
        {
            get
            {
                if (roamingSettings.Values["LocalExtendedRecord"] == null)
                    roamingSettings.Values["LocalExtendedRecord"] = 0;
                return (int)roamingSettings.Values["LocalExtendedRecord"];
            }
            set
            {
                roamingSettings.Values["LocalExtendedRecord"] = value;
            }
        }
        public static int LocalMagicRecord
        {
            get
            {
                if (roamingSettings.Values["LocalMagicRecord"] == null)
                    roamingSettings.Values["LocalMagicRecord"] = 0;
                return (int)roamingSettings.Values["LocalMagicRecord"];
            }
            set
            {
                roamingSettings.Values["LocalMagicRecord"] = value;
            }
        }
        #endregion

        #region Magic artifacts
        /// <summary>
        /// Returns count of magic rolls for current player
        /// </summary>
        public static int GetMagicRollsCount(Player player)
        {
            if (AdminModule.IsAdmin(player))
                return 1;
            int res = 0;
            var valueKey = string.Format("MR_{0}_{1}", player.Name, player.Password);
            if (roamingSettings.Values.ContainsKey(valueKey))
            {
                if (int.TryParse(roamingSettings.Values[valueKey].ToString().Decrypt(32),out res)){};
            }
            return res;
        }

        /// <summary>
        /// Returns count of manual sets for current player
        /// </summary>
        public static int GetManualSetsCount(Player player)
        {
            if (AdminModule.IsAdmin(player))
                return 1;
            int res = 0;
            var valueKey = string.Format("MS_{0}_{1}", player.Name, player.Password);
            if (roamingSettings.Values.ContainsKey(valueKey))
            {
                if (int.TryParse(roamingSettings.Values[valueKey].ToString().Decrypt(32), out res)) { };
            }
            return res;
        }

        /// <summary>
        /// Returns count of manual sets for current player
        /// </summary>
        public static int GetForthRollsCount(Player player)
        {
            if (AdminModule.IsAdmin(player))
                return 1;
            int res = 0;
            var valueKey = string.Format("FR_{0}_{1}", player.Name, player.Password);
            if (roamingSettings.Values.ContainsKey(valueKey))
            {
                if (int.TryParse(roamingSettings.Values[valueKey].ToString().Decrypt(32), out res)) { };
            }
            return res;
        }

        /// <summary>
        /// Sets count of magic rolls for current player
        /// </summary>
        public static void SetMagicRollsCount(Player player, int count)
        {
            var valueKey = string.Format("MR_{0}_{1}", player.Name, player.Password);
            roamingSettings.Values[valueKey] = count.ToString().Encrypt(32);
            
        }
        /// <summary>
        /// Sets count of manual sets for current player
        /// </summary>
        public static void SetManualSetsCount(Player player, int count)
        {
            var valueKey = string.Format("MS_{0}_{1}", player.Name, player.Password);
            roamingSettings.Values[valueKey] = count.ToString().Encrypt(32);

        }
        /// <summary>
        /// Sets count of forth rolls for current player
        /// </summary>
        public static void SetForthRollsCount(Player player, int count)
        {
            var valueKey = string.Format("FR_{0}_{1}", player.Name, player.Password);
            roamingSettings.Values[valueKey] = count.ToString().Encrypt(32);

        }

        #endregion
    }

}
