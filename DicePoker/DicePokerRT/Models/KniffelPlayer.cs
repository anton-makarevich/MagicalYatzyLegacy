﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanet.Models;

namespace Sanet.Kniffel.Models
{
    public class Player:BaseViewModel
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
                    NotifyPropertyChanged("Type");
                    NotifyPropertyChanged("IsBot");
                    NotifyPropertyChanged("IsHuman");
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
                if (IsHuman)
                    return _RememberPass;
                return false;
            }
            set
            {
                if (_RememberPass != value && IsHuman)
                {
                    _RememberPass = value;
                }
                else
                    _RememberPass = false;
            }
        }

        /// <summary>
        /// Returns if current player is bot
        /// </summary>
        public bool IsBot
        {
            get
            {
                return Type == PlayerType.AI;
            }
            set
            {
                if (value)
                    Type = PlayerType.AI;
            }
        }
        /// <summary>
        /// Returns if current player is human
        /// </summary>
        public bool IsHuman
        {
            get
            {
                return Type == PlayerType.Local;
            }
            set
            {
                if (value)
                    Type = PlayerType.Local;
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

        /// <summary>
        /// Label for 'Human'
        /// </summary>
        public string PlayerHumanLabelLocalized
        {
            get
            {
                return Messages.PLAYER_HUMAN.Localize();
            }
        }
        /// <summary>
        /// Label for 'Bot'
        /// </summary>
        public string PlayerBotLabelLocalized
        {
            get
            {
                return Messages.PLAYER_BOT.Localize();
            }
        }
        /// <summary>
        /// Label for 'Remember password'
        /// </summary>
        public string PlayerRememberLabelLocalized
        {
            get
            {
                return Messages.PLAYER_PASSWORD_REMEMBER.Localize();
            }
        }

        #endregion
    }
}
