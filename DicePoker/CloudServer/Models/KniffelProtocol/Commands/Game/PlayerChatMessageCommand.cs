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

        
        //public int ReceiverPos
        //{
        //    get { return m_ReceiverPos; }
        //}
        //public bool IsPrivate
        //{
        //    get { return m_IsPrivate; }
        //}
        public string Message { get; set; }

        public PlayerChatMessageCommand(StringTokenizer argsToken)
            :base(argsToken)
        {
            //m_ReceiverPos = int.Parse(argsToken.NextToken());
            //m_IsPrivate = bool.Parse(argsToken.NextToken());
            Message=argsToken.NextToken();
        }

        public PlayerChatMessageCommand(string sendername,string message)
            :base(sendername)
        {
            //m_ReceiverPos = receiverpos;
            //m_IsPrivate = isprivate;
            Message = message;
        }

        public override void Encode(StringBuilder sb)
        {
            base.Encode(sb);//Append(sb, m_ReceiverPos);
            //Append(sb, m_IsPrivate);
            Append(sb, Message);
        }
    }
}
