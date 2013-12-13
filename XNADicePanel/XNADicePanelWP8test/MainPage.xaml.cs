using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using XNADicePanelWP8test.Resources;
using MonoGame.Framework.WindowsPhone;
using Sanet.Kniffel.Xna;

namespace XNADicePanelWP8test
{
    public partial class MainPage : PhoneApplicationPage
    {
        DicePanel game;
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            game = XamlGame<DicePanel>.Create("", this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            game.RollDice(new List<int> {2,4,3,2,4 });
        }

        
    }
}