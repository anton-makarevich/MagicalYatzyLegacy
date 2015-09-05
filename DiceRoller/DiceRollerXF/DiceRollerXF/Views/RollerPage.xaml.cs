using Sanet.Kniffel.DicePanel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Sanet.Kniffel.XF.Views
{
    public partial class RollerPage : ContentPage
    {
        public RollerPage()
        {
            InitializeComponent();
        }

        

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Xamarin.Forms.Device.StartTimer(TimeSpan.FromMilliseconds(500),CheckPanel);
        }
        async void PreparePanel()
        {
            if (dicePanel.NumDice == 5)
                return;
            await dicePanel.SetStyleAsync(DiceStyle.dpsBlue);
            dicePanel.RollDelay = 30;
            dicePanel.ClickToFreeze = true;
            dicePanel.NumDice = 5;
            
        }

        bool CheckPanel()
        {
            if (dicePanel.Height>1)
            {
                PreparePanel();
                return false;
            }
            return true;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            dicePanel.RollDice(new List<int> { 2, 3, 4, 2, 5 });
        }
    }
}
