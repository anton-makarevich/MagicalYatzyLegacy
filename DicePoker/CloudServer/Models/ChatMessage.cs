using System;
using System.Collections.Generic;
using System.Linq;

namespace Sanet.Kniffel.Models
{
    public class ChatMessage
    {
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        public bool IsPrivate { get; set; }
        public string Message { get; set; }
    }
}