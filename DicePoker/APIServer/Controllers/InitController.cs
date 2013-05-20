using Sanet.Network;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace PokerServerService.Controllers
{
    /// <summary>
    /// Controller to init player
    /// </summary>
    public class InitController : ApiController
    {
        /// <summary>
        /// Basic bonus,requested on game start
        /// </summary>
        public ServerHttpMessage Get(string id)
        {
            //first check version virst
            var queryArgs = Request.RequestUri.ParseQueryString();
            if (queryArgs.Count == 2)
            {
                string versionStr = Request.RequestUri.ParseQueryString()[0];
                string language = Request.RequestUri.ParseQueryString()[1];
                if (string.IsNullOrEmpty(language))
                    language = "en";
                float version;
                if (!string.IsNullOrEmpty(versionStr))
                {
                    if (float.TryParse(versionStr,NumberStyles.AllowDecimalPoint,CultureInfo.InvariantCulture, out version) && version < 2.0f)
                    {

                        return new ServerHttpMessage()
                        {
                            Code = -2,//we can defined different codes here
                            IsClientUpdated=true,
                            ServerRestartDate=new DateTime(2013,5,1,17,0,0),
                            Message = GetMessage(language)
                            //"ServerMaintananceMessage" //"App version is outdated, some features may not work. Please upgrade from Windows Store"
                        };
                    }
                }
            }
            return null;
        }

        private string GetMessage(string language)
        {
            switch (language)
            {
                case "ru":
                    return "Приветствуем в игре \"Магический Yatzy Online!\"";
                case "de":
                    return "Welcome to \"Magical Yatzy Online\" Game!";
                case "by":
                    return "Вітаем у гульні \"Магічны Yatzy Online!\"";
                default:
                    return "Welcome to \"Magical Yatzy Online\" Game!";
            }
        }
        
    }


}