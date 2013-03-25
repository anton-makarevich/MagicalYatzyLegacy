using Sanet.Models;
using Sanet.Network.Protocol.Commands;
using System.Text;


namespace Sanet.Kniffel.Protocol.Commands.Game
{
    public class PlayerPingCommand : AbstractCommand
    {
        protected override string CommandName
        {
            get { return COMMAND_NAME; }
        }
        public static string COMMAND_NAME = "gamePLAYER_PING";

        private readonly int m_PlayerPos;
        
        public int PlayerPos
        {
            get { return m_PlayerPos; }
        }
        
        public PlayerPingCommand(StringTokenizer argsToken)
        {
            m_PlayerPos = int.Parse(argsToken.NextToken());
        }

        public PlayerPingCommand(int pos)
        {
            m_PlayerPos = pos;
        }

        public override void Encode(StringBuilder sb)
        {
            Append(sb, m_PlayerPos);
        }
    }
}
