using Sanet.Kniffel.Models.Enums;
using Sanet.Models;
using Sanet.Network.Protocol.Commands;
using System;
using System.Text;

namespace Sanet.Kniffel.Protocol.Commands.Game
{
    public class PlayerJoinedCommand : AbstractCommand
    {
        protected override string CommandName
        {
            get { return COMMAND_NAME; }
        }
        public static string COMMAND_NAME = "gamePLAYER_JOINED";

        private readonly int m_PlayerPos;
        private readonly string m_PlayerName;
        private readonly ClientType m_PlayerClient;
        private readonly string m_PlayerLanguage;

        public int PlayerPos
        {
            get { return m_PlayerPos; }
        }
       
        public string PlayerName
        {
            get { return m_PlayerName; }
        }

        public string PlayerLanguage
        {
            get { return m_PlayerLanguage; }
        }

        public ClientType PlayerClient
        {
            get { return m_PlayerClient; }
        }

        public PlayerJoinedCommand(StringTokenizer argsToken)
        {
            m_PlayerPos = int.Parse(argsToken.NextToken());
            m_PlayerName = argsToken.NextToken();
            m_PlayerClient = (ClientType)Enum.Parse(typeof(ClientType), argsToken.NextToken());
            m_PlayerLanguage = argsToken.NextToken();
        }

        public PlayerJoinedCommand(int pos, string name, ClientType client, string language)
        {
            m_PlayerPos = pos;
            m_PlayerName = name;
            m_PlayerClient = client;
            m_PlayerLanguage=language;
        }

        public override void Encode(StringBuilder sb)
        {
            Append(sb, m_PlayerPos);
            Append(sb, m_PlayerName);
            Append(sb, m_PlayerClient);
            Append(sb, m_PlayerLanguage);
        }
    }
}
