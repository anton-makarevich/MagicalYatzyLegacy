using Newtonsoft.Json;
using Sanet.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace Sanet.Kniffel.WebApi
{
    /// <summary>
    /// Communicates with web api service 
    /// </summary>
    public class InitService:AbstractHttpService
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
        public async Task<ServerHttpMessage> InitPlayer(string userid,string language="en")
        {
            var nameHelper = new AssemblyName(Assembly.GetExecutingAssembly().FullName);

            string versionString = string.Format("{0}.{1}{2}{3}", nameHelper.Version.Major, nameHelper.Version.Minor, nameHelper.Version.Build, nameHelper.Version.Revision);

            //var response = await _client.GetAsync(string.Format("{0}/{1}?version={2}&lang={3}",
            //    serviceUrl, userid.ToString(),versionString,language));
            var jsonString = await _client.DownloadStringTaskAsync(string.Format("{0}/{1}?version={2}&lang={3}",
                serviceUrl, userid.ToString(),versionString,language)); // response.Content.ReadAsStringAsync();
            ServerHttpMessage message=JsonConvert.DeserializeObject<ServerHttpMessage>(jsonString);
            return message;
        }
        
    }
}
