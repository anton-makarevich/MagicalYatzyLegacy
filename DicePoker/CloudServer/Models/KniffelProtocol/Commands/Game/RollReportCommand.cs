using Sanet.Models;
using Sanet.Network.Protocol.Commands;
using System;
using System.Collections.Generic;
using System.Text;


namespace Sanet.Kniffel.Protocol.Commands.Game
{
    public class RollReportCommand :PlayerCommand
    {
        protected override string CommandName
        {
            get { return COMMAND_NAME; }
        }
        public static string COMMAND_NAME = "gamePLAYER_ROLL_REPORT";

        private readonly List<int> m_LastResult;

        public List<int> LastResult
        {
            get { return m_LastResult; }
        }

        public RollReportCommand(StringTokenizer argsToken)
            :base(argsToken)
        {
            m_LastResult=new List<int>();
            for (int i=0;i<5;i++)
                m_LastResult.Add(int.Parse(argsToken.NextToken()));
        }

        public RollReportCommand(string name, List<int> results)
            :base(name)
        {
            m_LastResult = results;
        }

        public override void Encode(StringBuilder sb)
        {
            base.Encode(sb);
            foreach(int r in m_LastResult)
                Append(sb, r);
        }
    }
}
