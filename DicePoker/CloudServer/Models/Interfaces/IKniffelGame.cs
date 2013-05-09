using Sanet.Kniffel.Models.Events;
using System;
namespace Sanet.Kniffel.Models.Interfaces
{
    public interface IKniffelGame
    {
        event EventHandler<PlayerEventArgs> PlayerLeft;
        void ApplyScore(RollResult result);
        Player CurrentPlayer { get; set; }
        event EventHandler<RollEventArgs> DiceChanged;
        event EventHandler<FixDiceEventArgs> DiceFixed;
        event EventHandler<RollEventArgs> DiceRolled;
        event EventHandler<PlayerEventArgs> PlayerReady;
        void DoMove();
        void FixAllDices(int value, bool isfixed);
        void FixDice(int value, bool isfixed);
        int FixedDicesCount { get; }
        event EventHandler GameFinished;
        int GameId { get; set; }
        //int Roll { get; }
        bool IsDiceFixed(int value);
        bool IsPlaying { get; set; }
        void JoinGame(Player player);
        DieResult LastDiceResult { get; }
        event EventHandler<PlayerEventArgs> MagicRollUsed;
        void ManualChange(bool isfixed, int oldvalue, int newvalue);
        int Move { get; set; }
        event EventHandler<MoveEventArgs> MoveChanged;
        event EventHandler<ChatMessageEventArgs> OnChatMessage;
        void NextMove();
        string Password { get; set; }
        event EventHandler<PlayerEventArgs> PlayerJoined;
        global::System.Collections.Generic.List<Player> Players { get; set; }
        int PlayersNumber { get; }
        void ReporMagictRoll();
        void ReportRoll();
        bool RerollMode { get; set; }
        void RestartGame();
        event EventHandler<ResultEventArgs> ResultApplied;
        KniffelRule Rules { get; set; }
        
        void LeaveGame(Player player);
        void SetPlayerReady(Player player, bool isready);
        void SetPlayerReady(bool isready);
        void SendChatMessage(ChatMessage message);
    }
}
