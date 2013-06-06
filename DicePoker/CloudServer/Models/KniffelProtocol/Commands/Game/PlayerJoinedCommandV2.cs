using Sanet.Kniffel.DicePanel;
using Sanet.Kniffel.Models.Enums;
using Sanet.Models;
using Sanet.Network.Protocol.Commands;
using System;
using System.Text;

namespace Sanet.Kniffel.Protocol.Commands.Game
{
    public class PlayerJoinedCommandV2 : PlayerJoinedCommand
    {
        protected override string CommandName
        {
            get { return COMMAND_NAME; }
        }
        public static string COMMAND_NAME = "gamePLAYER_JOINED_V2";

        public string PicUrl{get;private set;}

        public PlayerJoinedCommandV2(StringTokenizer argsToken)
            :base(argsToken)
        {
            PicUrl=argsToken.NextToken();
        }

        public PlayerJoinedCommandV2(string name,int seatno,
            ClientType client, string language, 
            DiceStyle style, string picurl)
            :base(name,seatno,client,language,style)
        {
            PicUrl = picurl;
        }

        public override void Encode(StringBuilder sb)
        {
            base.Encode(sb);
            Append(sb, PicUrl);
        }
    }
}
