﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Sanet.Kniffel.WebApi
{
    /// <summary>
    /// Class to inherit by http services for webapi
    /// </summary>
    public abstract class AbstractHttpService
    {
        protected abstract string serviceName{get;}

        //uri to service
        protected string serviceUrl
        {
            get
            {
                return "http://" + Config.GetHostName() + "api/" + serviceName;
            }
        }

        //HttpClient for communication
        protected readonly HttpClient _client = new HttpClient();

        /// <summary>
        /// JSON serialization of object
        /// </summary>
        protected string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}