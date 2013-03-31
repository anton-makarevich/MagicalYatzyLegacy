using Sanet.Kniffel.Models;
using Sanet.Kniffel.Models.Enums;
using Sanet.Models;
using Sanet.Network.Protocol.Commands;
using System;
using System.Collections.Generic;
using System.Text;


namespace Sanet.Kniffel.Protocol.Commands.Game
{
    public class ApplyScoreCommand :PlayerCommand
    {
        protected override string CommandName
        {
            get { return COMMAND_NAME; }
        }
        public static string COMMAND_NAME = "gamePLAYER_APPLY_SCORE";

        public KniffelScores ScoreType { get; set; }
        public int PossibleValue { get; set; }
        public bool HasBonus { get; set; }

        public ApplyScoreCommand(StringTokenizer argsToken)
            :base(argsToken)
        {
            PossibleValue = int.Parse(argsToken.NextToken());
            HasBonus = bool.Parse(argsToken.NextToken());
            ScoreType=(KniffelScores)Enum.Parse(typeof(KniffelScores),argsToken.NextToken());
        }

        public ApplyScoreCommand(int pos,RollResult result)
            :base(pos)
        {
            PossibleValue = result.PossibleValue;
            HasBonus = result.HasBonus;
            ScoreType = result.ScoreType;
        }

        public override void Encode(StringBuilder sb)
        {
            base.Encode(sb);
            Append(sb, PossibleValue);
            Append(sb, HasBonus);
            Append(sb, ScoreType);
        }
                
    }
}
