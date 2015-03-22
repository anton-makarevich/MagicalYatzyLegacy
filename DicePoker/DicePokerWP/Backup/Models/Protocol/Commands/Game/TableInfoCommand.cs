using Sanet.Kniffel.Models;
using Sanet.Kniffel.Models.Interfaces;
using Sanet.Models;
using Sanet.Network.Protocol.Commands;
using System.Collections.Generic;
using System.Text;
using System.Linq;


namespace Sanet.Kniffel.Protocol.Commands.Game
{
    public class TableInfoCommand : AbstractCommand
    {
        protected override string CommandName
        {
            get { return COMMAND_NAME; }
        }
        public static string COMMAND_NAME = "gameTABLE_INFO";

        private readonly List<TuplePlayerInfo> m_Seats;
        private readonly int m_Round;
        private readonly int m_PlayersNb;
             
        public int Round
        {
            get { return m_Round; }
        }

        public int PlayersNb
        {
            get { return m_PlayersNb; }
        }
        
        public List<TuplePlayerInfo> Seats
        {
            get { return m_Seats; }
        }
        

        public TableInfoCommand(StringTokenizer argsToken)
        {
            m_Seats = new List<TuplePlayerInfo>();

            m_Round = int.Parse(argsToken.NextToken());
            m_PlayersNb = int.Parse(argsToken.NextToken());
            
            for (int i = 0; i < m_PlayersNb; ++i)
            {
                m_Seats.Add(new TuplePlayerInfo(argsToken));
            }
            
        }

        
        public TableInfoCommand(IKniffelGame game)
        {
            m_Seats = new List<TuplePlayerInfo>();

            m_PlayersNb = game.PlayersNumber;
            m_Round = game.Move;
        
            
            foreach(Player player in game.Players)
            {
                List<int> results = new List<int>();
                foreach (var result in player.Results)
                {
                    results.Add((result.HasValue)?result.Value:-1);
                    results.Add((result.HasBonus)?1:0);
                }
                TuplePlayerInfo seat = new TuplePlayerInfo(player.Name, 
                    results, 
                    player.SeatNo,
                    player.IsMoving,
                    player.IsReady,
                    player.Language,
                    player.PicUrl,
                    player.Client,
                    player.SelectedStyle);
                m_Seats.Add(seat);
            
            
            }
            
        }

        public override void Encode(StringBuilder sb)
        {
            Append(sb, m_Round);
            Append(sb, PlayersNb);
            
            for (int i = 0; i < m_Seats.Count; ++i)
            {
                Append(sb, m_Seats[i].ToString(AbstractCommand.Delimitter));
            }
            
        }
    }
}
