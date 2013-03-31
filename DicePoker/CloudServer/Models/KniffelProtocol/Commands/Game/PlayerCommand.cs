using Sanet.Models;
using Sanet.Network.Protocol.Commands;
using System;
using System.Collections.Generic;
using System.Text;


namespace Sanet.Kniffel.Protocol.Commands.Game
{
    public class PlayerCommand : AbstractCommand
    {
        protected override string CommandName
        {
            get { return COMMAND_NAME; }
        }
        public static string COMMAND_NAME = "gamePLAYER";

        private readonly int m_PlayerPos;

        public int PlayerPos
        {
            get { return m_PlayerPos; }
        }

        public PlayerCommand(StringTokenizer argsToken)
        {
            m_PlayerPos = int.Parse(argsToken.NextToken());
        }

        public PlayerCommand(int pos)
        {
            m_PlayerPos = pos;
        }

        public override void Encode(StringBuilder sb)
        {
            Append(sb, m_PlayerPos);
        }
    }
}
