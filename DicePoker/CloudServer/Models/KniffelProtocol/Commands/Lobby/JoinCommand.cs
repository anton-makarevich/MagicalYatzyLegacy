﻿using Sanet.Models;
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

        public int TableID
        {
            get { return m_TableID; }
        } 

        public string PlayerName
        {
            get { return m_PlayerName; }
        } 

        public JoinCommand(StringTokenizer argsToken)
        {
            m_PlayerName = argsToken.NextToken();
            m_TableID = int.Parse(argsToken.NextToken());
        }

        public JoinCommand(int p_tableID, string p_playerName)
        {
            m_TableID = p_tableID;
            m_PlayerName = p_playerName;
        }

        public override void Encode(StringBuilder sb)
        {
            Append(sb, m_PlayerName);
            Append(sb, m_TableID);
        }

        public string EncodeResponse(int seat)
        {
            return new JoinResponse(this, seat).Encode();
        }

        public string EncodeErrorResponse()
        {
            return new JoinResponse(this, -1).Encode();
        }
    }
}
