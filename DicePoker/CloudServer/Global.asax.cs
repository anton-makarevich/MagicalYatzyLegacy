﻿using Sanet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CloudServer
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        KniffelService.KniffelServiceSoapClient ks;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
           // BundleConfig.RegisterBundles(BundleTable.Bundles);
            LogManager.MessageLogged += LogManager_MessageLogged;
            ks=new KniffelService.KniffelServiceSoapClient();
        }

        void LogManager_MessageLogged(string from, string message, int level)
        {
            ks.PutOnlineServerLog(from, message);
        }
    }
}