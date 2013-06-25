using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Sanet.Models;
using Sanet.Kniffel.ViewModels;
using Microsoft.Phone.Shell;
using Sanet.Kniffel.Models;

namespace DicePokerWP
{
    public partial class MagicRoomPage : UserControl
    {
        // Constructor
        public MagicRoomPage()
        {
            InitializeComponent();
            SetViewModel<MagicRoomViewModel>();
            this.Loaded += MainPage_Loaded;
            
        }

                
        
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {

            
            try
            {
                if (StoreManager.IsAdVisible())
                    AdRotatorControl.Invalidate();
            }
            catch (Exception ex)
            {
                var t = ex.Message;
            }
            
        }

        
        
        #region ViewModel
        public void SetViewModel<T>() where T : BaseViewModel
        {
            DataContext = ViewModelProvider.GetViewModel<T>();

        }

        public T GetViewModel<T>() where T : BaseViewModel
        {
            return (T)DataContext;
        }

        #endregion

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                OfferAction item = (OfferAction)(e.AddedItems[0]);
                item.MenuAction();
                ((ListBox)sender).SelectedItem = null;
            }
        }
    }
}