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
                    if (float.TryParse(versionStr, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out version) && version >= 2.02f)
                    {

                        return new ServerHttpMessage()
                        {
                            Code = -2,//we can defined different codes here
                            IsClientUpdated = true,
                            ServerRestartDate = new DateTime(2013, 5, 1, 17, 0, 0),
                            Message = GetMessage(language)
                            //"ServerMaintananceMessage" //"App version is outdated, some features may not work. Please upgrade from Windows Store"
                        };
                    }
                    else
                    {
                        return new ServerHttpMessage()
                        {
                            Code = -1,//we can defined different codes here
                            IsClientUpdated =false,
                            ServerRestartDate = new DateTime(2013, 5, 1, 17, 0, 0),
                            Message = ""
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
                //case "ru":
                //    return "Приветствуем в игре \"Магический Yatzy Online!\". Внимание данная версия содержит серьезную ошибку. Патч уже готов и находится на сертификации!";
                //case "de":
                //    return "Willkommen in \"Magical Yatzy Online\"! Das Multiplayer-Online-Spiel ist im Beta. Leider können noch Fehler auftreten. Wir sind dankbar für jede Information darüber. Danke!";
                //case "by":
                //    return "Вітаем у гульні \"Магічны Yatzy Online!\". Анлайн гульня пакуль у бэце. Калі ласка паведамляйце аб любых праблемах. Длякуй!";
                //default:
                //    return "Welcome to \"Magical Yatzy Online\" Game! Warning! Current version has a serious bug. Patch is ready and in certification now!";
                //case "ru":
                //    return "Приветствуем в игре \"Магический Yatzy Online!\". Сетевая игра пока еще в бета стадии. Мы будем признательны за информацию о любых ошибках. Спасибо!";
                //case "de":
                //    return "Willkommen in \"Magical Yatzy Online\"! Das Multiplayer-Online-Spiel ist im Beta. Leider können noch Fehler auftreten. Wir sind dankbar für jede Information darüber. Danke!";
                //case "by":
                //    return "Вітаем у гульні \"Магічны Yatzy Online!\". Анлайн гульня пакуль у бэце. Калі ласка паведамляйце аб любых праблемах. Длякуй!";
                //default:
                //    return "Welcome to \"Magical Yatzy Online\" Game! Multiplayer game is still in beta. We appriciate your help in reporting any issues with the game. Thank you!";
                case "ru":
                    return "Приветствуем в игре \"Магический Yatzy Online!\"";
                case "de":
                    return "Willkommen in \"Magical Yatzy Online\" Game!";
                case "by":
                    return "Вітаем у гульні \"Магічны Yatzy Online!\"";
                default:
                    return "Welcome to \"Magical Yatzy Online\" Game!";
                //case "ru":
                //    return "Приветствуем в игре \"Магический Yatzy Online!\". Чат отключен до следующего обновления, так как содержит критическую ошибку. Извините :(";
                //case "de":
                //    return "Willkommen in \"Magical Yatzy Online\"! Der Chat ist deaktiviert, da er einen kritischen Fehler  enthält. Wir entschuldigen uns  für die entstandenen Unannehmlichkeiten.";
                //case "by":
                //    return "Вітаем у гульні \"Магічны Yatzy Online!\". Анлайн гульня пакуль у бэце. Калі ласка паведамляйце аб любых праблемах. Длякуй!";
                //default:
                //    return "Welcome to \"Magical Yatzy Online\" Game! Chat is disabled till next update as it has a critical bug. Sorry :(";
            }
        }
        
    }


}