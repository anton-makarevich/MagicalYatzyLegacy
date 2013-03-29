﻿using Sanet.Kniffel.Models.Enums;
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

        public ClientType PlayerClient
        {
            get { return m_PlayerClient; }
        }

        public JoinCommand(StringTokenizer argsToken)
        {
            m_TableID = int.Parse(argsToken.NextToken());
            m_PlayerName = argsToken.NextToken();
            m_PlayerPass = argsToken.NextToken();
            m_PlayerClient = (ClientType)Enum.Parse(typeof(ClientType), argsToken.NextToken());
            m_PlayerLanguage = argsToken.NextToken();
        }

        public JoinCommand(int tableid, string name, string pass, ClientType client, string language)
        {
            m_TableID = tableid;
            m_PlayerName = name;
            m_PlayerPass = pass;
            m_PlayerClient = client;
            m_PlayerLanguage = language;
        }

        public override void Encode(StringBuilder sb)
        {
            Append(sb, m_TableID);
            Append(sb, m_PlayerName);
            Append(sb, m_PlayerPass);
            Append(sb, m_PlayerClient);
            Append(sb, m_PlayerLanguage);
        }

        public string EncodeResponse(int seat, int gameid)
        {
            return new JoinResponse(this, seat,gameid).Encode();
        }

        public string EncodeErrorResponse()
        {
            return new JoinResponse(this, -1,-1).Encode();
        }
    }
}
