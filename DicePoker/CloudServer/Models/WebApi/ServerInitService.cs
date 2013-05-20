using Newtonsoft.Json;
using Sanet.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sanet.Kniffel.WebApi
{
    /// <summary>
    /// Communicates with web api service 
    /// </summary>
    public class ServerInitService:AbstractHttpService
    {
        protected override string serviceName
        {
            get
            {
                return "Init";//used as last part of service url
            }

        }
        
        /// <summary>
        /// Player enter lobby - let's server know his info
        /// </summary>
        /// <returns></returns>
        public async Task<ServerHttpMessage> InitPlayer(string userid , string version, string language)
        {

            var response = await _client.GetAsync(string.Format("{0}/{1}?version={2}&lang={3}",
                serviceUrl, userid.ToString(), version,language));
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ServerHttpMessage>(jsonString);
        }
        
    }
}
