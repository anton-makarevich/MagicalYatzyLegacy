using DiceRollerXF.Controls;
using DiceRollerXF.Views;
using NControl.Abstractions;
using NControlDemo.FormsApp.Controls;
using NGraphics;
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
        CircularButtonControl _clearButton;
        CircularButtonControl _helpButton;
        CircularButtonControl _rollButton;
        public RollerPage()
        {
            InitializeComponent();

            StyleButton.SetImageString("DiceRollerXF.Resources.2.png");
            CountButton.SetTextString("5", true);
            SpeedButton.SetTextString("very fast");
            ProjectionButton.SetTextString("low");

            ClassicButton.SetImageString("DiceRollerXF.Resources.0.png");
            BlueButton.SetImageString("DiceRollerXF.Resources.2.png");
            RedButton.SetImageString("DiceRollerXF.Resources.1.png");

            var grid = new Grid();

            _clearButton  = new CircularButtonControl();
            _clearButton.FillColor = Xamarin.Forms.Color.FromHex("#666666");
            _clearButton.Content = new Image { Source = ImageSource.FromResource("DiceRollerXF.Resources.clear.png") };
            _clearButton.OnTouchesBegan += ClearButton_OnTouchesBegan;

            _helpButton = new CircularButtonControl();
            _helpButton.FillColor = Xamarin.Forms.Color.FromHex("#666666");
            _helpButton.Content = new Image { Source = ImageSource.FromResource("DiceRollerXF.Resources.help.png") };
            _helpButton.OnTouchesBegan += _helpButton_OnTouchesBegan;

            _rollButton = new CircularButtonControl();
            _rollButton.FillColor = Xamarin.Forms.Color.FromHex("#666666");
            _rollButton.Content = new Image { Source = ImageSource.FromResource("DiceRollerXF.Resources.dice.png") };
            _rollButton.OnTouchesBegan += _rollButton_OnTouchesBegan;

            grid.Children.Add(new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Padding = 9,
                Children = {
                    _helpButton,
                    _clearButton,
                    _rollButton
                    }
            }, 0, 0);

            //var buttonOverlay = new BlueFrameControl();

            //grid.Children.Add(buttonOverlay, 0, 0);

            var _bottomBar = new NControlView
            {

                BackgroundColor = _helpButton.FillColor,
                DrawingFunction = (ICanvas canvas, Rect rect) =>
                    canvas.DrawLine(0, 0, rect.Width, 0, NGraphics.Colors.Gray,2)
                ,
                Content = grid
            };

            layoutRoot.Children.Add(_bottomBar, 0, 2);
        }

        private void _helpButton_OnTouchesBegan(object sender, IEnumerable<NGraphics.Point> e)
        {
            Navigation.PushModalAsync(new AboutPage());
        }

        private void _rollButton_OnTouchesBegan(object sender, IEnumerable<NGraphics.Point> e)
        {
            dicePanel.RollDice(null);
        }

        private void ClearButton_OnTouchesBegan(object sender, IEnumerable<NGraphics.Point> e)
        {
            dicePanel.ClearFreeze();
            _rollButton.IsEnabled = true;
            _clearButton.IsEnabled = false;
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

        private void StyleButton_Click(object sender, EventArgs e)
        {
            StyleButtons.IsVisible=true;
            SettingsButtons.IsVisible = false;
        }

        private void CountButton_Click(object sender, EventArgs e)
        {

        }

        private void SpeedButton_Click(object sender, EventArgs e)
        {

        }

        private void ProjectionButton_Click(object sender, EventArgs e)
        {

        }

        private async void RedButton_Click(object sender, EventArgs e)
        {
            var b = sender as WPButton;
            
            if (b.Tag.ToString() == "Red")
                await dicePanel.SetStyleAsync( Kniffel.DicePanel.DiceStyle.dpsBrutalRed);
            else if (b.Tag.ToString() == "Blue")
                await dicePanel.SetStyleAsync(Kniffel.DicePanel.DiceStyle.dpsBlue);
            else
                await dicePanel.SetStyleAsync(Kniffel.DicePanel.DiceStyle.dpsClassic);
            StyleButton.SetImageString(b.ImageSource);
            HideSettings();
        }
        private void HideSettings()
        {
            StyleButtons.IsVisible = false;
            //CountButtons.IsVisible = false;
            //SpeedButtons.IsVisible = false;
            //ProjectionButtons.IsVisible = false;
            SettingsButtons.IsVisible = true;

        }
    }
}
