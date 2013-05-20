using Sanet.Models;
using Sanet.Network.Protocol.Commands;
using System.Text;

namespace Sanet.Kniffel.Protocol.Commands.Game
{
    public class PlayerRerolledCommand :PlayerCommand
    {
        protected override string CommandName
        {
            get { return COMMAND_NAME; }
        }
        public static string COMMAND_NAME = "gamePLAYER_ROLL_RESET";



        public PlayerRerolledCommand(StringTokenizer argsToken)
            :base(argsToken)
        {
            
        }

        public PlayerRerolledCommand(string name)
            :base(name)
        {
            
        }

        public override void Encode(StringBuilder sb)
        {
            base.Encode(sb);
            
        }
    }
}
