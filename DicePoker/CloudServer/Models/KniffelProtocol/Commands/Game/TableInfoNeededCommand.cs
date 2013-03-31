using Sanet.Models;
using Sanet.Network.Protocol.Commands;
using System;
using System.Collections.Generic;
using System.Text;


namespace Sanet.Kniffel.Protocol.Commands.Game
{
    public class TableInfoNeededCommand : AbstractCommand
    {
        protected override string CommandName
        {
            get { return COMMAND_NAME; }
        }
        public static string COMMAND_NAME = "gameTABLE_INFO_NEEDED";

        private readonly int m_PlayerPos;

        public int PlayerPos
        {
            get { return m_PlayerPos; }
        }

        public TableInfoNeededCommand(StringTokenizer argsToken)
        {
            m_PlayerPos = int.Parse(argsToken.NextToken());
        }

        public TableInfoNeededCommand(int pos)
        {
            m_PlayerPos = pos;
        }

        public override void Encode(StringBuilder sb)
        {
            Append(sb, m_PlayerPos);
        }
    }
}
