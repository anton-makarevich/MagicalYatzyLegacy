using Sanet.Kniffel.DicePanel;
using Sanet.Models;
using Sanet.Network.Protocol.Commands;
using System;
using System.Collections.Generic;
using System.Text;


namespace Sanet.Kniffel.Protocol.Commands.Game
{
    public class ChangeStyleCommand :PlayerCommand
    {
        protected override string CommandName
        {
            get { return COMMAND_NAME; }
        }
        public static string COMMAND_NAME = "gamePLAYER_CHANGE_STYLE";

        public DiceStyle SelectedStyle {get;private set;}

        public ChangeStyleCommand(StringTokenizer argsToken)
            :base(argsToken)
        {
            SelectedStyle=(DiceStyle)Enum.Parse(typeof(DiceStyle),argsToken.NextToken()
#if SILVERLIGHT
                ,false
#endif
                );
        }

        public ChangeStyleCommand(string name, DiceStyle style)
            :base(name)
        {
            SelectedStyle = style;
        }

        public override void Encode(StringBuilder sb)
        {
            base.Encode(sb);
            Append(sb, SelectedStyle);
        }
                
    }
}
