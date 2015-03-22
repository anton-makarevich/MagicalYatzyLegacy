using Sanet.Kniffel.Models;
using Sanet.Kniffel.Models.Events;
using Sanet.Kniffel.Models.Interfaces;
using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls;

namespace Sanet.Kniffel.ViewModels
{
    public class ChatViewModel:BaseViewModel
    {
        IKniffelGame m_Game;


        public ChatViewModel(IKniffelGame game)
        {
            createCommands();
            m_Game = game;
            m_Game.OnChatMessage += m_Game_OnChatMessage;
            m_Game.PlayerLeft+=m_Game_PlayerLeft;
        }

      

        #region Properties
            
        
        public string SendLabel
        {
            get
            { 
                return "SendLabel".Localize();
            }
            
        }

        
        public ObservableCollection<Player> Players
        {
            get
            {
                return new ObservableCollection<Player>(m_Game.Players);
            }
        }

        Player _SelectedPlayer;
        public Player SelectedPlayer
        {
            get
            { return _SelectedPlayer; }
            set
            {
                _SelectedPlayer = value;
                NotifyPropertyChanged("SelectedPlayer");
            }
        }

        ObservableCollection<CustomMessage> _Messages = new ObservableCollection<CustomMessage>();
        public ObservableCollection<CustomMessage> Messages
        {
            get
            {
                return  _Messages;
            }
            
        }

        string _CurrentMessage;
        public string CurrentMessage
        {
            get
            { return _CurrentMessage; }
            set
            {
                _CurrentMessage = value;
                NotifyPropertyChanged("CurrentMessage");
            }
        }



        #endregion

        #region Methods
        /// <summary>
        /// Chat message received from server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_Game_OnChatMessage(object sender, ChatMessageEventArgs e)
        {
            SmartDispatcher.BeginInvoke(() =>
            {
                var msg = e.Message;
                if (msg != null)
                {
                    CustomMessage conversationMessage = new CustomMessage(msg.Message, DateTime.Now, (msg.SenderName == m_Game.MyName) ? ConversationViewMessageType.Outgoing : ConversationViewMessageType.Incoming);
                    conversationMessage.SenderName = msg.SenderName;
                    _Messages.Add(conversationMessage);
                    //if (msg.SenderName != m_Game.MyName)
                    //    Utilities.ShowToastNotification(string.Format("{0}: {1}",msg.SenderName,msg.Message));

                    NotifyPropertyChanged("Messages");
                    NotifyPropertyChanged("Players");
                }
            });
        }

        void m_Game_PlayerLeft(object sender, PlayerEventArgs e)
        {
            NotifyPropertyChanged("Players");
        }

        public void SendHandler()
        {
            SmartDispatcher.BeginInvoke(() =>
            {
                if (string.IsNullOrEmpty(CurrentMessage))
                    return;
                var msg = new ChatMessage();

                msg.Message = CurrentMessage;
                msg.SenderName = m_Game.MyName;
                m_Game.SendChatMessage(msg);
                //clear textbox
                CurrentMessage = string.Empty;
            });
        }

        public void Refresh()
        {
            NotifyPropertyChanged("Players");
            NotifyPropertyChanged("Messages");
        }
        #endregion

        #region Commands
        public RelayCommand SendCommand { get; set; }
        void createCommands()
        {
            SendCommand = new RelayCommand(o => SendHandler(), () => true);
        }
        #endregion 
    }
    public class CustomMessage : ConversationViewMessage
    {
        public CustomMessage(string text, DateTime timeStamp, ConversationViewMessageType type)
            : base(text, timeStamp, type)
        {
        }
        string _SenderName;
        public string SenderName
        {
            get
            {
                return _SenderName + ", " + this.TimeStamp.ToShortTimeString(); 
            }
            set
            {
                _SenderName = value;
            }
        }
    }
}
