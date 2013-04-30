using Sanet.Kniffel.Server;
using Sanet.Kniffel.WebApi;
using Sanet.Network;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
        public async Task<ServerHttpMessage> Get(string id)
        {
            //first check version virst
            var queryArgs = Request.RequestUri.ParseQueryString();
            if (queryArgs.Count == 1)
            {
                string versionStr = Request.RequestUri.ParseQueryString()[0];

                var service = new ServerInitService();
                var respond = await service.InitPlayer(id, versionStr);
                respond.IsServerOnline = true;
                respond.OnlinePlayersCount = ClientRequestHandler.ClientsCount;
                return respond;
                
            }
            return null;
        }

        
    }


}