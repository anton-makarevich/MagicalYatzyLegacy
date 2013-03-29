using Newtonsoft.Json;
using Sanet.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;

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
        public async Task<ServerHttpMessage> InitPlayer(string userid)
        {
            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;
            string versionString = string.Format("{0}.{1}{2}{3}", version.Major, version.Minor, version.Build, version.Revision);

            var response = await _client.GetAsync(string.Format("{0}/{1}?version={2}", serviceUrl, userid.ToString(),versionString));
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ServerHttpMessage>(jsonString);
        }
        
    }
}
