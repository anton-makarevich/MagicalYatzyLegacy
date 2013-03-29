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
        private readonly int m_GameId;
        public int GameId
        {
            get { return m_GameId; }
        }

        public JoinResponse(StringTokenizer argsToken)
            : base(new JoinCommand(argsToken))
        {
            m_NoSeat = int.Parse(argsToken.NextToken());
            m_GameId = int.Parse(argsToken.NextToken());
        }

        public JoinResponse(JoinCommand command, int seat,int gameid)
            : base(command)
        {
            m_NoSeat = seat;
            m_GameId = GameId;
        }

        public override void Encode(StringBuilder sb)
        {
            base.Encode(sb);
            Append(sb, m_NoSeat);
            Append(sb, m_GameId);
        }
    }
}
