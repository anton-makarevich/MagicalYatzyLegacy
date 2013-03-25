using Sanet.Models;
using Sanet.Network.Protocol.Commands;
using System.Text;

namespace Sanet.Kniffel.Protocol.Commands.Game
{
    public class PlayerChatMessageCommand : AbstractCommand
    {
        protected override string CommandName
        {
            get { return COMMAND_NAME; }
        }
        public static string COMMAND_NAME = "gamePLAYER_CHAT_MESSAGE";

        private readonly int m_PlayerPos;
        //private readonly int m_ReceiverPos;
        //private readonly bool m_IsPrivate;
        private readonly string m_Message;

        public int PlayerPos
        {
            get { return m_PlayerPos; }
        }
        //public int ReceiverPos
        //{
        //    get { return m_ReceiverPos; }
        //}
        //public bool IsPrivate
        //{
        //    get { return m_IsPrivate; }
        //}
        public string Message
        {
            get { return m_Message; }
        }

        public PlayerChatMessageCommand(StringTokenizer argsToken)
        {
            m_PlayerPos = int.Parse(argsToken.NextToken());
            //m_ReceiverPos = int.Parse(argsToken.NextToken());
            //m_IsPrivate = bool.Parse(argsToken.NextToken());
            m_Message=argsToken.NextToken();
        }

        public PlayerChatMessageCommand(int senderpos,string message)
        {
            m_PlayerPos = senderpos;
            //m_ReceiverPos = receiverpos;
            //m_IsPrivate = isprivate;
            m_Message = message;
        }

        public override void Encode(StringBuilder sb)
        {
            Append(sb, m_PlayerPos);
            //Append(sb, m_ReceiverPos);
            //Append(sb, m_IsPrivate);
            Append(sb, m_Message);
        }
    }
}
