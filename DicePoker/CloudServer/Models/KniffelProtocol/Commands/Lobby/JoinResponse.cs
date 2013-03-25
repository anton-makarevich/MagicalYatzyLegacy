using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sanet.Kniffel.Protocol.Commands.Lobby
{
    public class JoinResponse : AbstractLobbyResponse<JoinCommand>
    {

        protected override string CommandName
        {
            get { return COMMAND_NAME; }
        }
        public static string COMMAND_NAME = "lobbyJOIN_TABLE_RESPONSE";
        private readonly int m_NoSeat;
        public int NoSeat
        {
            get { return m_NoSeat; }
        } 


        public JoinResponse(StringTokenizer argsToken)
            : base(new JoinCommand(argsToken))
        {
            m_NoSeat = int.Parse(argsToken.NextToken());
        }

        public JoinResponse(JoinCommand command, int seat)
            : base(command)
        {
            m_NoSeat = seat;
        }

        public override void Encode(StringBuilder sb)
        {
            base.Encode(sb);
            Append(sb, m_NoSeat);
        }
    }
}
