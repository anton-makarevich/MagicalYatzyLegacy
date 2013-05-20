using Sanet.Models;
using Sanet.Network.Protocol.Commands;
using System.Text;


namespace Sanet.Kniffel.Protocol.Commands.Game
{
    public class PlayerPingCommand :PlayerCommand
    {
        protected override string CommandName
        {
            get { return COMMAND_NAME; }
        }
        public static string COMMAND_NAME = "gamePLAYER_PING";

        
        
        public PlayerPingCommand(StringTokenizer argsToken)
            :base(argsToken)
        {
            
        }

        public PlayerPingCommand(string name)
            :base (name)
        {
            
        }

        public override void Encode(StringBuilder sb)
        {
            base.Encode(sb);
        }
    }
}
