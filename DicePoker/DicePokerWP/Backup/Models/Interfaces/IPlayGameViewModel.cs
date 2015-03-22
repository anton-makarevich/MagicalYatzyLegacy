using Sanet.Kniffel.ViewModels;
using System;
namespace Sanet.Kniffel.Models.Interfaces
{
    public interface IPlayGameView
    {
        bool CanFix { get; }
        bool CanRoll { get; }
        string ClearLabel { get; }
        string ForthRollLabel { get; }
        global::Sanet.Kniffel.Models.Interfaces.IKniffelGame Game { get; set; }
        bool IsControlsVisible { get; set; }
        bool IsForthRollEnabled { get; }
        bool IsForthRollVisible { get; }
        bool IsMagicRollEnabled { get; }
        bool IsMagicRollVisible { get; }
        bool IsManualSetEnabled { get; }
        bool IsManualSetVisible { get; }
        bool IsPlayerSelected { get; }
        string MagicRollLabel { get; }
        string ManualSetLabel { get; }
        void OnRollEnd();
        void PlayAgain();
        string PlayAgainLabel { get; }
        global::System.Collections.ObjectModel.ObservableCollection<PlayerWrapper> Players { get; }
        string PlayersLabel { get; }
        string RemoveAdLabel { get; }
        void RemoveGameHandlers();
        string RollLabel { get; }
        global::System.Collections.Generic.List<IRollResult> RollResults { get; set; }
        string RulesLabel { get; }
        string RulesNameLabel { get; }
        global::System.Collections.Generic.List<IRollResult> SampleResults { get; }
        global::System.Threading.Tasks.Task SaveResults();
        PlayerWrapper SelectedPlayer { get; }
        string Title { get; set; }
        string TotalLabel { get; }
        void UpdateDPWidth();
    }
}
