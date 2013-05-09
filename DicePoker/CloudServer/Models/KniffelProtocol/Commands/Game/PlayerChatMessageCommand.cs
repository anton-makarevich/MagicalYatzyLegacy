using Sanet.Models;
using Sanet.Network.Protocol.Commands;
using System.Text;

namespace Sanet.Kniffel.Protocol.Commands.Game
{
    public class PlayerChatMessageCommand :PlayerCommand
    {
        protected override string CommandName
        {
            get { return COMMAND_NAME; }
        }
        public static string COMMAND_NAME = "gamePLAYER_CHAT_MESSAGE";


        public string ReceiverName
        {
            get;private set;
        }
        public bool IsPrivate
        {
            get;private set;
        }
        public string Message { get; set; }

        public PlayerChatMessageCommand(StringTokenizer argsToken)
            :base(argsToken)
        {
            Message=argsToken.NextToken();
            IsPrivate = bool.Parse(argsToken.NextToken());
            ReceiverName=argsToken.NextToken();
        }

        public PlayerChatMessageCommand(string sendername,string message, string receiver, bool isPrivate)
            :base(sendername)
        {
            ReceiverName=receiver;
            IsPrivate = isPrivate;
            Message = message;
        }

        public override void Encode(StringBuilder sb)
        {
            base.Encode(sb);
            Append(sb, Message);
            Append(sb, IsPrivate);
            Append(sb, ReceiverName);
            
            
        }
    }
}
