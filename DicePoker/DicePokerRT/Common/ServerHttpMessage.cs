﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanet.Network
{
    public class ServerHttpMessage
    {
        public ServerHttpMessage()
        { }

        public string Message { get; set; }
        public int Code { get; set; }
    }
}
