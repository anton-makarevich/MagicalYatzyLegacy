using Sanet.Kniffel.Models.Enums;
using Sanet.Kniffel.Models.Events;
using Sanet.Kniffel.Models.Interfaces;
using Sanet.Kniffel.Protocol.Commands.Game;
using Sanet.Kniffel.Protocol.Observer;
using Sanet.Network.Protocol.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sanet.Kniffel.Models
{
    public class KniffelGameServer : CommandQueueCommunicator<GameServerCommandObserver>
    {
        KniffelGame _Game;
        private Player _Player;
        public event EventHandler<KeyEventArgs<int>> LeftTable = delegate { };


        public KniffelGameServer(KniffelGame game)
        {
            _Game = game;
            

            
            
        }

        public void JoinGame(Player player)
        {
            InitGameObserver();
            _Player = player;
            _Game.JoinGame(player);
        }

        public void SendTableInfo()
        {
            Send(new TableInfoCommand(_Game));
        }
        
        #region Events
        

        #endregion

        #region Properties

        public Player Player
        {
            get { return _Player; }
        }
        public KniffelGame Game
        {
            get { return _Game; }
        }
        
#endregion

#region Methods
        void InitGameObserver()
        {
            _Game.DiceChanged += _Game_DiceChanged;
            _Game.DiceFixed += _Game_DiceFixed;
            _Game.DiceRolled += _Game_DiceRolled;
            _Game.GameFinished += _Game_GameFinished;
            _Game.MagicRollUsed += _Game_MagicRollUsed;
            _Game.MoveChanged += _Game_MoveChanged;
            _Game.PlayerJoined += _Game_PlayerJoined;
            _Game.ResultApplied += _Game_ResultApplied;
        }


        #region GameHandlers
        void _Game_ResultApplied(object sender, ResultEventArgs e)
        {
            Send(new ApplyScoreCommand(e.Player.SeatNo,e.Result));
            
        }

        void _Game_PlayerJoined(object sender, PlayerEventArgs e)
        {
            Send(new PlayerJoinedCommand(e.Player.SeatNo, e.Player.Name, e.Player.Client, e.Player.Language));
        }

        void _Game_MoveChanged(object sender, MoveEventArgs e)
        {
            Send(new RoundChangedCommand(e.Player.SeatNo, e.Move));
        }

        void _Game_MagicRollUsed(object sender, PlayerEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void _Game_GameFinished(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        void _Game_DiceRolled(object sender, RollEventArgs e)
        {
            Send(new RollReportCommand(e.Player.SeatNo,e.Value.ToList()));
        }

        void _Game_DiceFixed(object sender, FixDiceEventArgs e)
        {
            Send(new FixDiceCommand(e.Player.SeatNo,e.Value,e.Isfixed));
        }

        void _Game_DiceChanged(object sender, RollEventArgs e)
        {
            //throw new NotImplementedException();
        }

        #endregion
        protected override void InitializeCommandObserver()
        {
            m_CommandObserver.CommandReceived += m_CommandObserver_CommandReceived;
            m_CommandObserver.DisconnectCommandReceived += m_CommandObserver_DisconnectCommandReceived;
            m_CommandObserver.SitOutChangedCommandReceived += m_CommandObserver_SitOutChangedCommandReceived;
            m_CommandObserver.ChatMessageCommandReceived += m_CommandObserver_ChatMessageCommandReceived;
            m_CommandObserver.PlayerPingCommandReceived += m_CommandObserver_PlayerPingCommandReceived;
            m_CommandObserver.TableInfoNeededCommandReceived += m_CommandObserver_TableInfoNeededCommandReceived;
            m_CommandObserver.RollReportCommandReceived += m_CommandObserver_RollReportCommandReceived;
            m_CommandObserver.FixDiceCommandReceived += m_CommandObserver_FixDiceCommandReceived;
            m_CommandObserver.ApplyScoreCommandReceived += m_CommandObserver_ApplyScoreCommandReceived;
        }

        void m_CommandObserver_ApplyScoreCommandReceived(object sender, CommandEventArgs<ApplyScoreCommand> e)
        {
            var result = Player.Results.Find(f => f.ScoreType == e.Command.ScoreType);
            result.PossibleValue = e.Command.PossibleValue;
            result.HasBonus = e.Command.HasBonus;
            _Game.ApplyScore(result);
        }

        void m_CommandObserver_FixDiceCommandReceived(object sender, CommandEventArgs<FixDiceCommand> e)
        {
            _Game.FixDice(e.Command.Value, e.Command.IsFixed);
        }

        void m_CommandObserver_RollReportCommandReceived(object sender, CommandEventArgs<RollReportCommand> e)
        {
            _Game.ReportRoll();
        }

        void m_CommandObserver_TableInfoNeededCommandReceived(object sender, CommandEventArgs<TableInfoNeededCommand> e)
        {
            SendTableInfo();
        }

        void m_CommandObserver_PlayerPingCommandReceived(object sender, CommandEventArgs<Protocol.Commands.Game.PlayerPingCommand> e)
        {
            //throw new NotImplementedException();
        }

        void m_CommandObserver_ChatMessageCommandReceived(object sender, CommandEventArgs<Protocol.Commands.Game.PlayerChatMessageCommand> e)
        {
            //throw new NotImplementedException();
        }

        void m_CommandObserver_SitOutChangedCommandReceived(object sender, CommandEventArgs<Protocol.Commands.Game.PlayerSitOutChangedCommand> e)
        {
            //throw new NotImplementedException();
        }

        void m_CommandObserver_DisconnectCommandReceived(object sender, CommandEventArgs<DisconnectCommand> e)
        {
            //throw new NotImplementedException();
        }

        void m_CommandObserver_CommandReceived(object sender, StringEventArgs e)
        {
            //throw new NotImplementedException();
        }


#endregion

        public override void Dispose()
        {
            m_CommandObserver.CommandReceived -= m_CommandObserver_CommandReceived;
            m_CommandObserver.DisconnectCommandReceived -= m_CommandObserver_DisconnectCommandReceived;
            m_CommandObserver.SitOutChangedCommandReceived -= m_CommandObserver_SitOutChangedCommandReceived;
            m_CommandObserver.ChatMessageCommandReceived -= m_CommandObserver_ChatMessageCommandReceived;
            m_CommandObserver.PlayerPingCommandReceived -= m_CommandObserver_PlayerPingCommandReceived;
            m_CommandObserver.RollReportCommandReceived -= m_CommandObserver_RollReportCommandReceived;
            m_CommandObserver.FixDiceCommandReceived -= m_CommandObserver_FixDiceCommandReceived;
            m_CommandObserver.ApplyScoreCommandReceived -= m_CommandObserver_ApplyScoreCommandReceived;

            _Game.DiceChanged -= _Game_DiceChanged;
            _Game.DiceFixed -= _Game_DiceFixed;
            _Game.DiceRolled -= _Game_DiceRolled;
            _Game.GameFinished -= _Game_GameFinished;
            _Game.MagicRollUsed -= _Game_MagicRollUsed;
            _Game.MoveChanged -= _Game_MoveChanged;
            _Game.PlayerJoined -= _Game_PlayerJoined;
            _Game.ResultApplied -= _Game_ResultApplied;

            _Player = null;

            base.Dispose();
        }
    }
}
