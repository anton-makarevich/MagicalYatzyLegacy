using Sanet.Kniffel.Models;
using Sanet.Kniffel.Models.Interfaces;
using Sanet.Models;
using Sanet.Network.Protocol.Commands;
using System.Collections.Generic;
using System.Text;
using System.Linq;


namespace Sanet.Kniffel.Protocol.Commands.Game
{
    public class GameEndedCommand : AbstractCommand
    {
        protected override string CommandName
        {
            get { return COMMAND_NAME; }
        }
        public static string COMMAND_NAME = "gameTABLE_ENDED";

               

        public GameEndedCommand()
        {
            
        }
           
        public override void Encode(StringBuilder sb)
        {
                        
        }
    }
}
