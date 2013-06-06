using Sanet.Kniffel.DicePanel;
using Sanet.Kniffel.Models;
using Sanet.Kniffel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Sanet.Kniffel.Models
{
    public static class RoamingSettings
    {
        
        /// <summary>
        /// Get player info from roaming
        /// </summary>
        public static PlayerWrapper GetLastPlayer(int index)
        {
            var valueKey = "LastPlayer" + index.ToString();
            Player player=new Player();
            if (!string.IsNullOrEmpty(LocalSettings.GetValue(valueKey +"_strProfile")))
                player.Profile = (ProfileType)Enum.Parse(typeof(ProfileType), LocalSettings.GetValue(valueKey + "_strProfile"),false);
            
            if (string.IsNullOrEmpty(LocalSettings.GetValue(valueKey + "_strName")))
                return null;
            else
               player.Name=  LocalSettings.GetValue(valueKey + "_strName");

            if (!string.IsNullOrEmpty(LocalSettings.GetValue(valueKey + "_strPass")))
                player.Password = LocalSettings.GetValue(valueKey + "_strPass");
            
            if (!string.IsNullOrEmpty(LocalSettings.GetValue(valueKey + "_strPicUrl")))
                player.PicUrl = LocalSettings.GetValue(valueKey + "_strPicUrl");
            
            if (!string.IsNullOrEmpty(LocalSettings.GetValue(valueKey + "_strType")))
                player.Type = (PlayerType)Enum.Parse(typeof(PlayerType), LocalSettings.GetValue(valueKey + "_strType"),false);
            
            if (!string.IsNullOrEmpty(LocalSettings.GetValue(valueKey + "_boolPass")))
                player.RememberPass = System.Convert.ToBoolean( LocalSettings.GetValue(valueKey + "_boolPass"));
            
            return new PlayerWrapper(player);
        }
         
        /// <summary>
        /// Save player info to roaming
        /// </summary>
        public static void SaveLastPlayer(Player player, int index)
        {
            var valueKey = "LastPlayer" + index.ToString();
            if (player != null)
            {
                LocalSettings.SetValue(valueKey+"_strName", player.Name);
                LocalSettings.SetValue(valueKey+"_strPass",  player.Password);
                LocalSettings.SetValue(valueKey+"_strType", player.Type.ToString());
                LocalSettings.SetValue(valueKey+"_boolPass",  player.RememberPass);
                LocalSettings.SetValue(valueKey + "_strProfile", player.Profile.ToString());
                LocalSettings.SetValue(valueKey + "_strPicUrl", player.PicUrl);
            }
            else
                LocalSettings.SetValue(valueKey+"_strName", "");
        }

        public static Rules LastRule
        {
            get
            {
                if (LocalSettings.GetValue("LastRule") == null)
                    LocalSettings.SetValue("LastRule", Rules.krStandard.ToString());
                return ((Rules)Enum.Parse(typeof(Rules), LocalSettings.GetValue("LastRule"),false));
            }
            set
            {
                LocalSettings.SetValue("LastRule", value.ToString());
            }
        }

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

            if (!string.IsNullOrEmpty(LocalSettings.GetValue(valueKey)))
                res = System.Convert.ToInt32(LocalSettings.GetValue(valueKey));
            
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

            if (!string.IsNullOrEmpty(LocalSettings.GetValue(valueKey)))
                res = System.Convert.ToInt32(LocalSettings.GetValue(valueKey));
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

            if (!string.IsNullOrEmpty(LocalSettings.GetValue(valueKey)))
                res = System.Convert.ToInt32(LocalSettings.GetValue(valueKey));
            return res;
        }

        /// <summary>
        /// Sets count of magic rolls for current player
        /// </summary>
        public static void SetMagicRollsCount(Player player, int count)
        {
            var valueKey = string.Format("MR_{0}_{1}", player.Name, player.Password);
            LocalSettings.SetValue(valueKey , count);

        }
        /// <summary>
        /// Sets count of manual sets for current player
        /// </summary>
        public static void SetManualSetsCount(Player player, int count)
        {
            var valueKey = string.Format("MS_{0}_{1}", player.Name, player.Password);
            LocalSettings.SetValue(valueKey, count);

        }
        /// <summary>
        /// Sets count of forth rolls for current player
        /// </summary>
        public static void SetForthRollsCount(Player player, int count)
        {
            var valueKey = string.Format("FR_{0}_{1}", player.Name, player.Password);
            LocalSettings.SetValue(valueKey, count);

        }

        #endregion

        #region setting section
        public static DiceStyle DiceStyle
        {
            get
            {
                if (LocalSettings.GetValue("DiceStyle") == null)
                    LocalSettings.SetValue("DiceStyle", DiceStyle.dpsClassic.ToString());
                return ((DiceStyle)Enum.Parse(typeof(DiceStyle), LocalSettings.GetValue("DiceStyle"), false));
            }
            set 
            {
                LocalSettings.SetValue("DiceStyle",value.ToString());
            }
        }
        public static int DiceSpeed
        {
            get
            {
                if (LocalSettings.GetValue("DiceSpeed") == null)
                    LocalSettings.SetValue("DiceSpeed",5);
                return int.Parse(LocalSettings.GetValue("DiceSpeed"));
            }
            set
            {
                LocalSettings.SetValue("DiceSpeed", value);
            }
        }
        public static int DiceAngle 
        {
            get
            {
                if (LocalSettings.GetValue("DiceAngle") == null )
                    LocalSettings.SetValue("DiceAngle",0);
                return int.Parse(LocalSettings.GetValue("DiceAngle"));
            }
            set
            {
                LocalSettings.SetValue("DiceAngle", value);
            }
        }
        public static int DiceNumber
        {
            get
            {
                if (LocalSettings.GetValue("DiceNumber") == null)
                    LocalSettings.SetValue("DiceNumber",5);
                return int.Parse(LocalSettings.GetValue("DiceNumber"));
            }
            set
            {
                LocalSettings.SetValue("DiceNumber", value);
            }
        }

        public const int _NumOfRuns = 0;
        public static int NumOfRuns
        {
            get
            {
                if (LocalSettings.GetValue("NumOfRuns") == null)
                {
                    LocalSettings.SetValue("NumOfRuns",_NumOfRuns);
                    return _NumOfRuns;
                }
                return Convert.ToInt32(LocalSettings.GetValue("NumOfRuns"));
            }
            set
            {
                LocalSettings.SetValue("NumOfRuns", value);

            }
        }

        public static bool IsSoundEnabled
        {
            get
            {
                if (LocalSettings.GetValue("IsSoundEnabled") == null)
                    LocalSettings.SetValue("IsSoundEnabled", 0);
                return bool.Parse(LocalSettings.GetValue("IsSoundEnabled"));
            }
            set
            {
                LocalSettings.SetValue("IsSoundEnabled", value);
            }
        }
        #endregion

        #region records
        public static int LocalBabyRecord
        {
            get
            {
                if (LocalSettings.GetValue("LocalBabyRecord") == null)
                    LocalSettings.SetValue("LocalBabyRecord", 0);
                return int.Parse(LocalSettings.GetValue("LocalBabyRecord"));
            }
            set
            {
                LocalSettings.SetValue("LocalBabyRecord",value);
            }
        }
        public static int LocalSimpleRecord
        {
            get
            {
                if (LocalSettings.GetValue("LocalSimpleRecord") == null)
                    LocalSettings.SetValue("LocalSimpleRecord", 0);
                return int.Parse(LocalSettings.GetValue("LocalSimpleRecord"));
            }
            set
            {
                LocalSettings.SetValue("LocalSimpleRecord", value);
            }
        }
        public static int LocalStandardRecord
        {
            get
            {
                if (LocalSettings.GetValue("LocalStandardRecord") == null)
                    LocalSettings.SetValue("LocalStandardRecord", 0);
                return int.Parse(LocalSettings.GetValue("LocalStandardRecord"));
            }
            set
            {
                LocalSettings.SetValue("LocalStandardRecord", value);
            }
        }
        public static int LocalExtendedRecord
        {
            get
            {
                if (LocalSettings.GetValue("LocalExtendedRecord") == null)
                    LocalSettings.SetValue("LocalExtendedRecord",0);
                return int.Parse(LocalSettings.GetValue("LocalExtendedRecord"));
            }
            set
            {
                LocalSettings.SetValue("LocalExtendedRecord", value);
            }
        }
        #endregion

        public static string AccessToken
        {
            get
            {
                if (LocalSettings.GetValue("AccessToken") == null)
                    LocalSettings.SetValue("AccessToken", 5);
                return LocalSettings.GetValue("AccessToken");
            }
            set
            {
                LocalSettings.SetValue(AccessToken", value);
            }
        }
    }

}
