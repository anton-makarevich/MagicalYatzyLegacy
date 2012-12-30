using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanet.Kniffel.Models
{
    public class Player
    {
        /// <summary>
        /// Player display name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Player Password
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Player ID (GUID?)
        /// </summary>
        public string ID { get; set; }

        public bool IsMoving { get; set; }
        //Public Property GamePlatform As KniffelGamePlatform
        /// <summary>
        /// Avatar URI
        /// </summary>
        public string PicUrl { get; set; }

        /// <summary>
        /// Player type (human, network or AI controlled)
        /// </summary>
        public PlayerType Type { get; set; }

        /// <summary>
        /// If to remember pass (works only for human )
        /// </summary>
        

        public bool IsLocalHuman
        {
            get
            {
                return Type == PlayerType.Local;
            }
        }

        
    }
}
