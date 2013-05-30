using Sanet.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Sanet.Models;
using Sanet.Kniffel.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Sanet.Kniffel.Controls
{
    public sealed partial class ChatControl :UserControl
    {
        PlayGameViewModel playViewModel;

        public ChatControl()
            :base()
        {
            this.InitializeComponent();
            playViewModel= ViewModelProvider.GetViewModel<PlayGameViewModel>();
            if (playViewModel.ChatModel != null)
            {
                playViewModel.ChatModel.Messages.CollectionChanged += (s, args) => ScrollToBottom();
                playViewModel.PropertyChanged += ChatControl_PropertyChanged;
            }
            _scrollTimer.Tick += _scrollTimer_Tick;
            
        }

        
        void _scrollTimer_Tick(object sender, object e)
        {
            _scrollTimer.Stop();
            var scrollViewer = messagesList.GetFirstDescendantOfType<ScrollViewer>();
            scrollViewer.ScrollToVerticalOffset(scrollViewer.ScrollableHeight);
        }

        void ChatControl_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName=="ChatModel")
                playViewModel.ChatModel.Messages.CollectionChanged += (s, args) => ScrollToBottom();
        }

        DispatcherTimer _scrollTimer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(500) };
        
        private void ScrollToBottom()
        {
            _scrollTimer.Start();
        }

        private void Grid_KeyUp_1(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                playViewModel.ChatModel.CurrentMessage = chatTextField.Text;
                playViewModel.ChatModel.SendHandler();
                e.Handled = true;
            }
        }

        private void UserControl_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (chatTextField.FocusState == Windows.UI.Xaml.FocusState.Unfocused)
                chatTextField.Focus(Windows.UI.Xaml.FocusState.Programmatic);
        }
                
    }
}
