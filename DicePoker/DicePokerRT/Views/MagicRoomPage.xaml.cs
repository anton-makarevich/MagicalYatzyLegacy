﻿using Sanet.Controls;
using Sanet.Kniffel.Models;
using Sanet.Kniffel.ViewModels;
using Sanet.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DicePokerRT
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MagicRoomPage : PopupPaneBase
    {
        public MagicRoomPage()
        {
            this.InitializeComponent();
            SetViewModel<MagicRoomViewModel>();
        }

        
        private void Grid_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            mainScroll.ScrollToHorizontalOffset(mainScroll.ScrollableWidth);
        }

        private void itemListView_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            ((OfferAction)e.ClickedItem).MenuAction();
        }

        

        


    }
}
