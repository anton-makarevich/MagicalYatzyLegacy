using Sanet.Kniffel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace BiblePronto.Roaming
{
    public static class RoamingSettings
    {
        static Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

        
        /// <summary>
        /// Get current subitem name from roaming
        /// </summary>
        public static Player GetLastPlayer(int index)
        {
            var valueKey = "LastPlayer" + index.ToString();
            Player player;
            if (roamingSettings.Values.ContainsKey(valueKey))
            {
                ApplicationDataCompositeValue value = (ApplicationDataCompositeValue)roamingSettings.Values[valueKey];
                player = new Player();
                if (value != null && value.ContainsKey("strName"))
                    player.Name=  (string)value["strName"];
                if (value != null && value.ContainsKey("strPass"))
                    player.Name = (string)value["strName"];
                if (value != null && value.ContainsKey("strType"))
                    player.Name = (string)value["strName"];
            }
            return string.Empty;
        }
         
        /// <summary>
        /// SaveSettings to roaming folder
        /// </summary>
        public static void SaveReadingPlanSettings(string readingplan, int day, string scripture)
        {
            var valueKey = "ReadingPlan" + readingplan;
            ApplicationDataCompositeValue value = new ApplicationDataCompositeValue();
            value["intDay"]=day;
            value["strScripture"] = scripture;
            roamingSettings.Values[valueKey] = value;
        }


        

    }

}
