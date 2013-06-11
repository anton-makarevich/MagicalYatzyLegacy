using Sanet.Kniffel.DicePanel;
using Sanet.Kniffel.Models.Enums;
using Sanet.Models;
using Sanet.Network.Protocol.Commands;
using System;
using System.Text;

namespace Sanet.Kniffel.Protocol.Commands.Game
{
    public class PlayerJoinedCommand :PlayerCommand
    {
        protected override string CommandName
        {
            get { return COMMAND_NAME; }
        }
        public static string COMMAND_NAME = "gamePLAYER_JOINED";

        private readonly ClientType m_PlayerClient;
        private readonly string m_PlayerLanguage;

        public int SeatNo { get; set; }
       
        public string PlayerLanguage
        {
            get { return m_PlayerLanguage; }
        }

        public ClientType PlayerClient
        {
            get { return m_PlayerClient; }
        }

        public DiceStyle SelectedStyle { get; private set; }

        public PlayerJoinedCommand(StringTokenizer argsToken)
            :base(argsToken)
        {
            SeatNo = int.Parse(argsToken.NextToken());
            m_PlayerClient = (ClientType)Enum.Parse(typeof(ClientType), argsToken.NextToken()
#if WINDOWS_PHONE
, false
#endif
                );
            m_PlayerLanguage = argsToken.NextToken();
            SelectedStyle = (DiceStyle)Enum.Parse(typeof(DiceStyle), argsToken.NextToken()
#if WINDOWS_PHONE
, false
#endif
                );
        }

        public PlayerJoinedCommand(string name,int seatno,
            ClientType client, string language, DiceStyle style)
            :base(name)
        {
            SeatNo = seatno;
            m_PlayerClient = client;
            m_PlayerLanguage=language;
            SelectedStyle = style;
        }

        public override void Encode(StringBuilder sb)
        {
            base.Encode(sb);
            Append(sb, SeatNo);
            Append(sb, m_PlayerClient);
            Append(sb, m_PlayerLanguage);
            Append(sb, SelectedStyle);
        }
    }
}
