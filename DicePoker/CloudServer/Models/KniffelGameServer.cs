﻿using Sanet.Kniffel.Models.Enums;
using Sanet.Kniffel.Models.Events;
using Sanet.Kniffel.Models.Interfaces;
using Sanet.Kniffel.Protocol.Commands.Game;
using Sanet.Kniffel.Protocol.Observer;
using Sanet.Network.Protocol.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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

            RemoveTimerDelay = 15;

            this.ReceivedSomething += KniffelGameServer_ReceivedSomething;
        }

        void KniffelGameServer_ReceivedSomething(object sender, KeyEventArgs<string> e)
        {
            if (Player != null)
                Player.LastTimeActive = DateTime.Now;
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


        public int RemoveTimerDelay { get; set; }
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
            if (_Game.CurrentPlayer != null && _Game.Move > 0)
            {
                Send(new RoundChangedCommand(_Game.CurrentPlayer.Name, _Game.Move));
                //if (Game.FixedDicesCount > 0)
                //{

                //    foreach (int i in Game.FixedDice)
                //    {
                //        Send(new FixDiceCommand(Game.CurrentPlayer.Name, i, false));
                //    }
                //    Thread.Sleep(100);
                //    foreach (int i in Game.FixedDice)
                //    {
                //        Send(new FixDiceCommand(Game.CurrentPlayer.Name, i, true));
                //    }
                //}
            }
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
            _Game.PlayerRerolled += _Game_PlayerRerolled;
            _Game.StyleChanged += _Game_StyleChanged;
        }

        void _Game_StyleChanged(object sender, PlayerEventArgs e)
        {
            Send(new ChangeStyleCommand(e.Player.Name,e.Player.SelectedStyle));
        }

        void _Game_PlayerRerolled(object sender, PlayerEventArgs e)
        {
            Send(new PlayerRerolledCommand(e.Player.Name));
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
            try
            {
                Send(new PlayerLeftCommand(e.Player.Name));
            }
            catch (Exception ex)
            {
                LogManager.Log("GameServer.OnPlayerLeft", ex);
            }
            
            if (Player.Name == e.Player.Name && LeftTable != null)
                    LeftTable(this, new KeyEventArgs<int>(e.Player.SeatNo));
            
        }


        #region GameHandlers
        void _Game_ResultApplied(object sender, ResultEventArgs e)
        {
            Send(new ApplyScoreCommand(e.Player.Name,e.Result));
            
        }

        void _Game_PlayerJoined(object sender, PlayerEventArgs e)
        {
            Send(new PlayerJoinedCommand(e.Player.Name,e.Player.SeatNo,
                e.Player.Client, e.Player.Language, e.Player.SelectedStyle));
            Send(new PlayerJoinedCommandV2(e.Player.Name, e.Player.SeatNo,
                e.Player.Client, e.Player.Language, e.Player.SelectedStyle,e.Player.PicUrl));
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
            Send(new DiceChangedCommand(e.Player.Name,e.Value.ToList()));
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
            m_CommandObserver.ManualChangeCommandReceived += m_CommandObserver_ManualChangeCommandReceived;
            m_CommandObserver.PlayerRerolledCommandReceived += m_CommandObserver_PlayerRerolledCommandReceived;
            m_CommandObserver.ChangeStyleCommandReceived += m_CommandObserver_ChangeStyleCommandReceived;
            m_CommandObserver.PlayerDeactivatedCommandReceived += m_CommandObserver_PlayerDeactivatedCommandReceived;
        }

        void m_CommandObserver_PlayerDeactivatedCommandReceived(object sender, CommandEventArgs<PlayerDeactivatedCommand> e)
        {
            LogManager.Log(LogLevel.Message, "GameServer", "{0} deactivated", e.Command.Name);
            RemoveTimerDelay = 180;
        }

        void m_CommandObserver_ChangeStyleCommandReceived(object sender, CommandEventArgs<ChangeStyleCommand> e)
        {
            Game.ChangeStyle(Player, e.Command.SelectedStyle);
        }

        void m_CommandObserver_PlayerRerolledCommandReceived(object sender, CommandEventArgs<PlayerRerolledCommand> e)
        {
            if (e.Command.Name != _Game.CurrentPlayer.Name)
            {
                LogManager.Log(LogLevel.Error, "GameServer.CommandReceived", "Player {0} want to act, but its {1}'s turn", e.Command.Name, _Game.CurrentPlayer.Name);
                return;
            }
            Game.ResetRolls();
        }

        void m_CommandObserver_ManualChangeCommandReceived(object sender, CommandEventArgs<ManualChangeCommand> e)
        {
            if (e.Command.Name != _Game.CurrentPlayer.Name)
            {
                LogManager.Log(LogLevel.Error, "GameServer.CommandReceived", "Player {0} want to act, but its {1}'s turn", e.Command.Name, _Game.CurrentPlayer.Name);
                return;
            }
            Game.ManualChange(e.Command.IsFixed, e.Command.Value, e.Command.NewValue);
        }

        void m_CommandObserver_MagicRollCommandReceived(object sender, CommandEventArgs<MagicRollCommand> e)
        {
            if (e.Command.Name != _Game.CurrentPlayer.Name)
            {
                LogManager.Log(LogLevel.Error, "GameServer.CommandReceived", "Player {0} want to act, but its {1}'s turn", e.Command.Name, _Game.CurrentPlayer.Name);
                return;
            }
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
            if (e.Command.Name != _Game.CurrentPlayer.Name)
            {
                LogManager.Log(LogLevel.Error, "GameServer.CommandReceived", "Player {0} want to act, but its {1}'s turn", e.Command.Name, _Game.CurrentPlayer.Name);
                return;
            }
            var result = Player.Results.Find(f => f.ScoreType == e.Command.ScoreType);
            result.PossibleValue = e.Command.PossibleValue;
            result.HasBonus = e.Command.HasBonus;
            _Game.ApplyScore(result);
        }

        void m_CommandObserver_FixDiceCommandReceived(object sender, CommandEventArgs<FixDiceCommand> e)
        {
            if (e.Command.Name != _Game.CurrentPlayer.Name)
            {
                LogManager.Log(LogLevel.Error, "GameServer.CommandReceived", "Player {0} want to act, but its {1}'s turn", e.Command.Name, _Game.CurrentPlayer.Name);
                return;
            }
            _Game.FixDice(e.Command.Value, e.Command.IsFixed);
        }

        void m_CommandObserver_RollReportCommandReceived(object sender, CommandEventArgs<RollReportCommand> e)
        {
            if (e.Command.Name != _Game.CurrentPlayer.Name)
            {
                LogManager.Log(LogLevel.Error, "GameServer.CommandReceived", "Player {0} want to act, but its {1}'s turn", e.Command.Name, _Game.CurrentPlayer.Name);
                return;
            }
            _Game.ReportRoll();
        }

        void m_CommandObserver_TableInfoNeededCommandReceived(object sender, CommandEventArgs<TableInfoNeededCommand> e)
        {
            RemoveTimerDelay = 15;
            SendTableInfo();
        }

        void m_CommandObserver_PlayerPingCommandReceived(object sender, CommandEventArgs<Protocol.Commands.Game.PlayerPingCommand> e)
        {
            RemoveTimerDelay = 15;
        }

        void m_CommandObserver_ChatMessageCommandReceived(object sender, CommandEventArgs<Protocol.Commands.Game.PlayerChatMessageCommand> e)
        {
            var msg = new ChatMessage();
            msg.SenderName = _Player.Name;
            msg.Message = e.Command.Message;
            msg.ReceiverName = "na";
            msg.IsPrivate = false;
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
            m_CommandObserver.TableInfoNeededCommandReceived -= m_CommandObserver_TableInfoNeededCommandReceived;
            m_CommandObserver.RollReportCommandReceived -= m_CommandObserver_RollReportCommandReceived;
            m_CommandObserver.FixDiceCommandReceived -= m_CommandObserver_FixDiceCommandReceived;
            m_CommandObserver.ApplyScoreCommandReceived -= m_CommandObserver_ApplyScoreCommandReceived;
            m_CommandObserver.PlayerLeftCommandReceived -= m_CommandObserver_PlayerLeftCommandReceived;
            m_CommandObserver.PlayerReadyCommandReceived -= m_CommandObserver_PlayerReadyCommandReceived;
            m_CommandObserver.MagicRollCommandReceived -= m_CommandObserver_MagicRollCommandReceived;
            m_CommandObserver.ManualChangeCommandReceived -= m_CommandObserver_ManualChangeCommandReceived;
            m_CommandObserver.PlayerRerolledCommandReceived -= m_CommandObserver_PlayerRerolledCommandReceived;
            m_CommandObserver.ChangeStyleCommandReceived -= m_CommandObserver_ChangeStyleCommandReceived;
            m_CommandObserver.PlayerDeactivatedCommandReceived -= m_CommandObserver_PlayerDeactivatedCommandReceived;

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
            _Game.PlayerRerolled -= _Game_PlayerRerolled;
            _Game.StyleChanged -= _Game_StyleChanged;

            _Player = null;
            _Game = null;

            base.Dispose();
        }
    }
}
