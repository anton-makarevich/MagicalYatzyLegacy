using Sanet.Kniffel.Models.Enums;
using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sanet.Kniffel.Protocol
{
    public class TuplePlayerInfo
    {
        public string Name { get; set; }
        public List<int> Results { get; set; }
        public int SeatNo { get; set; }
        public bool IsPlaying { get; set; }
        public bool IsReady { get; set; }
        public ClientType ClientType { get; set; }
        public string Language { get; set; }
        public string PhotoUri { get; set; }
                
        public TuplePlayerInfo(string name, List<int> results,
            int seatno,bool isplaying,
            bool isready, 
            string lang, 
            string photouri,
            ClientType client)
        {
            Name = name;
            Results = results;
            SeatNo = seatno;
            IsPlaying = isplaying;
            IsReady = isready;
            Language = lang;
            PhotoUri = photouri;
            ClientType = client;
        }
                

        public TuplePlayerInfo(StringTokenizer argsToken)
        {
            Results = new List<int>();
            Name=argsToken.NextToken();
            SeatNo = int.Parse(argsToken.NextToken());
            IsPlaying = bool.Parse(argsToken.NextToken());
            IsReady = bool.Parse(argsToken.NextToken());
            Language=argsToken.NextToken();
            PhotoUri=argsToken.NextToken();
            ClientType=(ClientType)Enum.Parse(typeof(ClientType),argsToken.NextToken());
            for (int i=0;i<28;i++)
                Results.Add(int.Parse(argsToken.NextToken()));
                       
        }
        public string ToString(char p_delimiter)
        {
            while (Results.Count < 28)
                Results.Add(0);

            StringBuilder sb = new StringBuilder();
            sb.Append(Name);
            sb.Append(p_delimiter);

            sb.Append(SeatNo);
            sb.Append(p_delimiter);

            sb.Append(IsPlaying);
            sb.Append(p_delimiter);

            sb.Append(IsReady);
            sb.Append(p_delimiter);

            sb.Append(Language);
            sb.Append(p_delimiter);

            sb.Append(PhotoUri);
            sb.Append(p_delimiter);

            sb.Append(ClientType);
            sb.Append(p_delimiter);

            foreach (int i in Results)
            {
                sb.Append(i);
                sb.Append(p_delimiter);
            }

            return sb.ToString();
        }
    }
}
