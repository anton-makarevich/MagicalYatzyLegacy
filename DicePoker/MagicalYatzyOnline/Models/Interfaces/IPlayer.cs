using System;
namespace Sanet.Kniffel.Models.Interfaces
{
    public interface IPlayer
    {
        bool AllNumericFilled { get; }
        
        bool CanBuy { get; }
        
        Sanet.Kniffel.Models.Enums.ClientType Client { get; }
        
        
        Sanet.Kniffel.Models.Interfaces.IKniffelGame Game { get; set; }
        
        
        bool HasPassword { get; }
        
        void Init();
        bool IsBot { get; set; }
        bool IsDefaultName { get; }
        
        
        bool IsHuman { get; set; }
        
        bool IsMoving { get; set; }
        
        bool IsReady { get; set; }
        
        string Language { get; set; }
        
                
        int MaxRemainingNumeric { get; }
        string Name { get; set; }
        
        string Password { get; set; }
        
        
        System.Collections.Generic.List<Sanet.Kniffel.Models.RollResult> Results { get; set; }
        int Roll { get; set; }
        
        int SeatNo { get;  }
        
        
        int Total { get; }
        int TotalNumeric { get; }
        Sanet.Kniffel.Models.PlayerType Type { get; set; }
        
    }
}
