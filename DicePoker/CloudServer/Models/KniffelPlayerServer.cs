using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanet.Models;
using Sanet.Kniffel.Models.Enums;
using Sanet.Kniffel.Models.Interfaces;

namespace Sanet.Kniffel.Models
{
    public partial class Player:BaseViewModel
    {
        

        #region Prperties

        public bool IsManualSetlAvailable { get; set; }
        public bool IsMagicRollAvailable { get; set; }
        public bool IsForthRollAvailable { get; set; }

        public bool HadStartupMagic { get; set; }

        public string ArtifactsInfoMessage { get; set; }

        #endregion

        #region Methods
        
        

        public void RefreshArtifactsInfo(bool aftersync=false, bool forcesync=false)
        {
            
            
            
        }

        
        #endregion

 

    }
}
