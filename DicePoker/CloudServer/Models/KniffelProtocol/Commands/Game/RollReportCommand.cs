using Sanet.Models;
using Sanet.Network.Protocol.Commands;
using System;
using System.Collections.Generic;
using System.Text;


namespace Sanet.Kniffel.Protocol.Commands.Game
{
    public class RollReportCommand : AbstractCommand
    {
        protected override string CommandName
        {
            get { return COMMAND_NAME; }
        }
        public static string COMMAND_NAME = "gamePLAYER_ROLL_REPORT";

        private readonly int m_PlayerPos;
        private readonly List<int> m_LastResult;

        public int PlayerPos
        {
            get { return m_PlayerPos; }
        }

        public List<int> LastResult
        {
            get { return m_LastResult; }
        }

        public RollReportCommand(StringTokenizer argsToken)
        {
            m_LastResult=new List<int>();
            m_PlayerPos = int.Parse(argsToken.NextToken());
            for (int i=0;i<5;i++)
                m_LastResult.Add(int.Parse(argsToken.NextToken()));
        }

        public RollReportCommand(int pos, List<int> results)
        {
            m_PlayerPos = pos;
            m_LastResult = results;
        }

        public override void Encode(StringBuilder sb)
        {
            Append(sb, m_PlayerPos);
            foreach(int r in m_LastResult)
                Append(sb, r);
        }
    }
}
