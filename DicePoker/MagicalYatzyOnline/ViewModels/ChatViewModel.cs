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

namespace Sanet.Kniffel.ViewModels
{
    public class ChatViewModel:BaseViewModel
    {
        
        string _Name;
        IKniffelGame m_Game;

        public ChatViewModel(IKniffelGame game, string name)
        {
            createCommands();
            m_Game = game;
            _Name = name;
            m_Game.OnChatMessage += m_Game_OnChatMessage;
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

        List<ChatMessage> _Messages=new List<ChatMessage>();
        public ObservableCollection<ChatMessage> Messages
        {
            get
            {
                return new ObservableCollection<ChatMessage>( _Messages);
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
                    _Messages.Add(msg);
                    if (msg.Sender.Name != _Name)
                        Utilities.ShowToastNotification(string.Format("{0}: {1}",msg.Sender.Name,msg.Message));
                    NotifyPropertyChanged("Messages");
                }
            });
        }


        void SendHandler()
        {
            var msg =new ChatMessage();
            
            msg.Message=CurrentMessage;
            msg.Sender = m_Game.Players.FirstOrDefault(f => f.Name == _Name);
            m_Game.SendChatMessage(msg);
            //clear textbox
            CurrentMessage = string.Empty;
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
}
