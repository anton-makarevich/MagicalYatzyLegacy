using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanet.Kniffel.Localization;
using Xamarin.Forms;
using Sanet.Kniffel.Services;

namespace DiceRollerXF.Views
{
    public partial class AboutPage : ContentPage
    {
        IActionsService _service;
        public AboutPage()
        {
            InitializeComponent();
            _service = Xamarin.Forms.DependencyService.Get<IActionsService>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            VersionText.Text = "Version".Localize() + " " + App.Version;
            AboutText.Text = "AboutLabel".Localize();
            OnlineVersionText.Text = "OnlineLabel".Localize();
            OnlineLink.Content = "AboutOnlineLabel".Localize();
            AuthorText.Text = "DevelopedByLabel".Localize();
            SupportText.Text = "SupportLabel".Localize();

            MoreButton.Text = "MoreAppsLabel".Localize();
            CloseButton.Text = "CloseLabel".Localize();
            RateButton.Text = "RateLabel".Localize();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void RateButton_Click(object sender, EventArgs e)
        {
            _service.RateApp("by.sanet.diceroller");
        }

        private void MoreButton_Click(object sender, EventArgs e)
        {
            _service.ShowOtherApps("Anton Makarevich");
        }
    }
}
