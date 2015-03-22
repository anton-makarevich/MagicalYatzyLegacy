using System;
namespace Sanet.Kniffel.Models.Interfaces
{
    public interface IRollResult
    {
        bool HasBonus { get; set; }
        bool HasValue { get; set; }
        bool IsNumeric { get; }
        bool IsZeroValue { get; }
        int MaxValue { get; }
        int PossibleValue { get; set; }
        Sanet.Kniffel.Models.Enums.KniffelScores ScoreType { get; set; }
        
        int Value { get; set; }
    }
}
