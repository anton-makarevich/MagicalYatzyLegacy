using Sanet.Kniffel.DicePanel;
using Sanet.Kniffel.Models;
using Sanet.Kniffel.Models.Enums;
using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Text;


namespace Sanet.Kniffel.Protocol.Commands.Lobby
{
    public class JoinCommand : AbstractLobbyCommand
    {
        protected override string CommandName
        {
            get { return COMMAND_NAME; }
        }
        public static string COMMAND_NAME = "lobbyJOIN_TABLE";

        private readonly int m_TableID;
        private readonly string m_PlayerName;
        private readonly string m_PlayerPass;
        private readonly ClientType m_PlayerClient;
        private readonly string m_PlayerLanguage;
        private readonly Rules m_GameRule;

        public int TableID
        {
            get { return m_TableID; }
        } 

        public string PlayerName
        {
            get { return m_PlayerName; }
        }

        public string PlayerPass
        {
            get { return m_PlayerPass; }
        }

        public string PlayerLanguage
        {
            get { return m_PlayerLanguage; }
        }

        public Rules GameRule
        {
            get { return m_GameRule; }
        }

        public ClientType PlayerClient
        {
            get { return m_PlayerClient; }
        }

        public DiceStyle SelectedStyle { get;private set; }

        public string PicUrl { get; private set; }

        public JoinCommand(StringTokenizer argsToken)
        {
            m_TableID = int.Parse(argsToken.NextToken());
            m_PlayerName = argsToken.NextToken();
            m_GameRule = (Rules)Enum.Parse(typeof(Rules), argsToken.NextToken()
#if SILVERLIGHT
, false
#endif
                );
            m_PlayerClient = (ClientType)Enum.Parse(typeof(ClientType), argsToken.NextToken()
#if SILVERLIGHT
, false
#endif
                );
            m_PlayerLanguage = argsToken.NextToken();
            m_PlayerPass = argsToken.NextToken();
            SelectedStyle = (DiceStyle)Enum.Parse(typeof(DiceStyle), argsToken.NextToken()
#if SILVERLIGHT
, false
#endif
                );
            if (argsToken.HasMoreTokens())
                PicUrl = argsToken.NextToken();
        }

        public JoinCommand(int tableid, string name, Rules rule, ClientType client,
            string language,string pass,DiceStyle style,string picurl)
        {
            m_TableID = tableid;
            m_PlayerName = name;
            m_GameRule = rule;
            m_PlayerClient = client;
            m_PlayerLanguage = language;
            m_PlayerPass = pass;
            SelectedStyle = style;
            PicUrl = picurl;
        }

        public override void Encode(StringBuilder sb)
        {
            Append(sb, m_TableID);
            Append(sb, m_PlayerName);
            Append(sb, m_GameRule);
            Append(sb, m_PlayerClient);
            Append(sb, m_PlayerLanguage);
            Append(sb, m_PlayerPass);
            Append(sb, SelectedStyle);
            Append(sb, PicUrl);
        }

        public string EncodeResponse(int seat, int gameid)
        {
            return new JoinResponse((JoinCommand)this, seat,gameid).Encode();
        }

        public string EncodeErrorResponse()
        {
            return new JoinResponse((JoinCommand)this, -1, -1).Encode();
        }
    }
}
