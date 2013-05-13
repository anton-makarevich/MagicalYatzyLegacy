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
        public void JoinGame(Player player)
        {
            if (_Player == player)
                return;
            InitGameObserver();
            _Player = player;
            _Game.JoinGame(player);
        }

        public void SendTableInfo()
        {
            Send(new TableInfoCommand(_Game));
        }

        public void LeaveGame()
        {
            _Game.LeaveGame(Player);
        }

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
            _Game.PlayerLeft += _Game_PlayerLeft;
            _Game.PlayerReady += _Game_PlayerReady;
            _Game.GameUpdated += _Game_GameUpdated;
            _Game.OnChatMessage += _Game_OnChatMessage;
            
        }

        void _Game_OnChatMessage(object sender, ChatMessageEventArgs e)
        {
            Send(new PlayerChatMessageCommand(e.Message.SenderName, e.Message.Message, e.Message.ReceiverName,e.Message.IsPrivate));
        }

        void _Game_GameUpdated(object sender, EventArgs e)
        {
            SendTableInfo();
        }

        void _Game_PlayerReady(object sender, PlayerEventArgs e)
        {
            Send(new PlayerReadyCommand(e.Player.Name,e.Player.IsReady));
        }

        void _Game_PlayerLeft(object sender, PlayerEventArgs e)
        {
            Send(new PlayerLeftCommand(e.Player.Name));
            if (Player.ID == e.Player.ID && LeftTable != null)
                LeftTable(this, new KeyEventArgs<int>(e.Player.SeatNo));
        }


        #region GameHandlers
        void _Game_ResultApplied(object sender, ResultEventArgs e)
        {
            Send(new ApplyScoreCommand(e.Player.Name,e.Result));
            
        }

        void _Game_PlayerJoined(object sender, PlayerEventArgs e)
        {
            Send(new PlayerJoinedCommand(e.Player.Name,e.Player.SeatNo, e.Player.Client, e.Player.Language));
            //if (e.Player.Name == _Player.Name)
            //{
            //    foreach (Player p in Game.Players)
            //    {
            //        _Game_PlayerReady(null, new PlayerEventArgs(p));
            //    }
            //}
        }

        void _Game_MoveChanged(object sender, MoveEventArgs e)
        {
            Send(new RoundChangedCommand(e.Player.Name, e.Move));
        }

        void _Game_MagicRollUsed(object sender, PlayerEventArgs e)
        {
            Send(new MagicRollCommand(e.Player.Name));
        }

        void _Game_GameFinished(object sender, EventArgs e)
        {
            Send(new GameEndedCommand());
        }

        void _Game_DiceRolled(object sender, RollEventArgs e)
        {
            Send(new RollReportCommand(e.Player.Name,e.Value.ToList()));
        }

        void _Game_DiceFixed(object sender, FixDiceEventArgs e)
        {
            Send(new FixDiceCommand(e.Player.Name,e.Value,e.Isfixed));
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
            m_CommandObserver.ChatMessageCommandReceived += m_CommandObserver_ChatMessageCommandReceived;
            m_CommandObserver.PlayerPingCommandReceived += m_CommandObserver_PlayerPingCommandReceived;
            m_CommandObserver.TableInfoNeededCommandReceived += m_CommandObserver_TableInfoNeededCommandReceived;
            m_CommandObserver.RollReportCommandReceived += m_CommandObserver_RollReportCommandReceived;
            m_CommandObserver.FixDiceCommandReceived += m_CommandObserver_FixDiceCommandReceived;
            m_CommandObserver.ApplyScoreCommandReceived += m_CommandObserver_ApplyScoreCommandReceived;
            m_CommandObserver.PlayerLeftCommandReceived += m_CommandObserver_PlayerLeftCommandReceived;
            m_CommandObserver.PlayerReadyCommandReceived += m_CommandObserver_PlayerReadyCommandReceived;
            m_CommandObserver.MagicRollCommandReceived += m_CommandObserver_MagicRollCommandReceived;
        }

        void m_CommandObserver_MagicRollCommandReceived(object sender, CommandEventArgs<MagicRollCommand> e)
        {
            Game.ReporMagictRoll();
        }

        void m_CommandObserver_PlayerReadyCommandReceived(object sender, CommandEventArgs<PlayerReadyCommand> e)
        {
            Game.SetPlayerReady(Player, e.Command.IsReady);
        }

        void m_CommandObserver_PlayerLeftCommandReceived(object sender, CommandEventArgs<PlayerLeftCommand> e)
        {
            LeaveGame();
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
            var msg = new ChatMessage();
            msg.SenderName = _Player.Name;
            msg.Message = e.Command.Message;
            msg.ReceiverName = e.Command.ReceiverName;
            msg.IsPrivate = e.Command.IsPrivate;
            _Game.SendChatMessage(msg);
        }

        
        void m_CommandObserver_DisconnectCommandReceived(object sender, CommandEventArgs<DisconnectCommand> e)
        {
            LeaveGame();
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
            m_CommandObserver.ChatMessageCommandReceived -= m_CommandObserver_ChatMessageCommandReceived;
            m_CommandObserver.PlayerPingCommandReceived -= m_CommandObserver_PlayerPingCommandReceived;
            m_CommandObserver.RollReportCommandReceived -= m_CommandObserver_RollReportCommandReceived;
            m_CommandObserver.FixDiceCommandReceived -= m_CommandObserver_FixDiceCommandReceived;
            m_CommandObserver.ApplyScoreCommandReceived -= m_CommandObserver_ApplyScoreCommandReceived;
            m_CommandObserver.PlayerLeftCommandReceived -= m_CommandObserver_PlayerLeftCommandReceived;
            m_CommandObserver.PlayerReadyCommandReceived -= m_CommandObserver_PlayerReadyCommandReceived;
            m_CommandObserver.MagicRollCommandReceived -= m_CommandObserver_MagicRollCommandReceived;

            _Game.DiceChanged -= _Game_DiceChanged;
            _Game.DiceFixed -= _Game_DiceFixed;
            _Game.DiceRolled -= _Game_DiceRolled;
            _Game.GameFinished -= _Game_GameFinished;
            _Game.MagicRollUsed -= _Game_MagicRollUsed;
            _Game.MoveChanged -= _Game_MoveChanged;
            _Game.PlayerJoined -= _Game_PlayerJoined;
            _Game.ResultApplied -= _Game_ResultApplied;
            _Game.PlayerLeft -= _Game_PlayerLeft;
            _Game.PlayerReady -= _Game_PlayerReady;
            _Game.GameUpdated -= _Game_GameUpdated;
            _Game.OnChatMessage -= _Game_OnChatMessage;

            _Player = null;

            base.Dispose();
        }
    }
}
