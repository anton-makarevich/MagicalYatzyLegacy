using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanet.Models;

namespace Sanet.Kniffel.Models
{
    public class Player
    {
        #region Prperties
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
        
        private PlayerType _Type = PlayerType.Local;
        public PlayerType Type
        {
            get { return _Type; }
            set
            {
                if (_Type != value)
                {
                    _Type = value;
                }
            }
        }

            
        /// <summary>
        /// If to remember pass (works only for human )
        /// </summary>
        private bool _RememberPass;
        public bool RememberPass
        {
            get 
            { 
                if (IsLocalHuman)
                    return _RememberPass;
                return false;
            }
            set
            {
                if (_RememberPass != value && IsLocalHuman)
                {
                    _RememberPass = value;
                }
                else
                    _RememberPass = false;
            }
        }

        /// <summary>
        /// Returns if current player is human
        /// </summary>
        public bool IsLocalHuman
        {
            get
            {
                return Type == PlayerType.Local;
            }
        }

        /// <summary>
        /// Label for user name
        /// </summary>
        public string PlayerNameLabelLocalized
        {
            get
            {
                return Messages.PLAYER_NAME.Localize();
            }
        }
        /// <summary>
        /// Label for user password
        /// </summary>
        public string PlayerPasswordLabelLocalized
        {
            get
            {
                return Messages.PLAYER_PASSWORD.Localize();
            }
        }
        /// <summary>
        /// Label for user type
        /// </summary>
        public string PlayerTypeLabelLocalized
        {
            get
            {
                return Messages.PLAYER_TYPE.Localize();
            }
        }
        #endregion
    }
}
