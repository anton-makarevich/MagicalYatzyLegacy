using Sanet.Kniffel.Models;
using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sanet.Kniffel.Protocol
{
    public class TupleTableInfo
    {
        public int Id { get; set; }
        public List<string> Players { get; set; }
        public Rules Rule { get; set; }
        public string Name { get; set; }
        
        public TupleTableInfo() { }    
    
        public TupleTableInfo(int id, List<string> players,Rules rule)
        {
            Id=id;
            Players=players;
            Rule=rule;
        }
                

        public TupleTableInfo(StringTokenizer argsToken)
        {
            Players = new List<string>();
            Id = int.Parse(argsToken.NextToken());
            Rule=(Rules)Enum.Parse(typeof(Rules),argsToken.NextToken()
#if WINDOWS_PHONE
                ,false
#endif
                );
            for (int i=0;i<4;i++)
                if (argsToken.HasMoreTokens())
                    Players.Add(argsToken.NextToken());
                       
        }
        public string ToString(char p_delimiter)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Id);
            sb.Append(p_delimiter);

            sb.Append(Rule);
            sb.Append(p_delimiter);

            foreach (string name in Players)
            {
                sb.Append(name);
                sb.Append(p_delimiter);
            }

            return sb.ToString();
        }
    }
}
