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
            if (queryArgs.Count == 1)
            {
                string versionStr = Request.RequestUri.ParseQueryString()[0];
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
                            Message = "ServerMaintananceMessage" //"App version is outdated, some features may not work. Please upgrade from Windows Store"
                        };
                    }
                }
            }
            return null;
        }

        
    }


}