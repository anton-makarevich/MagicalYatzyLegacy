using Sanet.Models;
using Sanet.Network.Protocol.Commands;
using System.Text;


namespace Sanet.Kniffel.Protocol.Commands.Game
{
    public class PlayerSitOutChangedCommand : AbstractCommand
    {
        protected override string CommandName
        {
            get { return COMMAND_NAME; }
        }
        public static string COMMAND_NAME = "gamePLAYER_SITOUT_CHANGED";

        private readonly int m_PlayerPos;
        private readonly bool m_SitOutState;

        public int PlayerPos
        {
            get { return m_PlayerPos; }
        }
        public bool SitOutState
        {
            get { return m_SitOutState; }
        }

        public PlayerSitOutChangedCommand(StringTokenizer argsToken)
        {
            m_PlayerPos = int.Parse(argsToken.NextToken());
            m_SitOutState = bool.Parse(argsToken.NextToken());
        }

        public PlayerSitOutChangedCommand(int pos, bool state)
        {
            m_PlayerPos = pos;
            m_SitOutState = state;
        }

        public override void Encode(StringBuilder sb)
        {
            Append(sb, m_PlayerPos);
            Append(sb, m_SitOutState);
        }
    }
}
