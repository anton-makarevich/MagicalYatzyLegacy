using Poker8.Common;
using Poker8.Services;
using Poker8.ViewModels;
using PokerWorld.Chips;
using PokerWorld.Data;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Poker8.Controls
{
    public sealed partial class ChatControl :PopupPaneBase
    {
        public event EventHandler ChipsBought;

        public ChatControl()
            :base()
        {
            this.InitializeComponent();
            
        }
        
      
    }
}
