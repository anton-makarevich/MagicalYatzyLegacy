﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanet.Kniffel
{
    public static class Config
    {
        public static string GetHostName()
        {
#if !DEBUG
       // http://....
            return "yatzy.azurewebsites.net/";
#else
            return "localhost:57584/";
#endif
        }
    }
}