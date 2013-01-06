using Sanet.Kniffel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Sanet.Kniffel.Settings
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
            ApplicationDataCompositeValue value = new ApplicationDataCompositeValue();
            value["strName"] = player.Name;
            value["strPass"] = player.Password;
            value["strType"] = player.Type.ToString();
            value["boolPass"] = player.RememberPass;
            roamingSettings.Values[valueKey] = value;
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

        

    }

}
