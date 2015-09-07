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
using Sanet.Kniffel.Localization;
using Xamarin.Forms;
using Sanet.Kniffel;
using Sanet.Kniffel.Utils;
using DiceRollerXF.Models;

namespace Sanet.Kniffel.XF.Views
{
    public partial class RollerPage : ContentPageEx
    {
        CircularButtonControl _clearButton;
        CircularButtonControl _helpButton;
        CircularButtonControl _rollButton;
        public RollerPage()
        {
            InitializeComponent();

            StyleButton.SetImageString("DiceRollerXF.Resources.2.png");
            CountButton.SetTextString("5", true);
            SpeedButton.SetTextString("VeryFastLabel".Localize());
            ProjectionButton.SetTextString("LowLabel".Localize());

            ClassicButton.SetImageString("DiceRollerXF.Resources.0.png");
            BlueButton.SetImageString("DiceRollerXF.Resources.2.png");
            RedButton.SetImageString("DiceRollerXF.Resources.1.png");

            OneButton.SetTextString("1", true);
            TwoButton.SetTextString("2", true);
            ThreeButton.SetTextString("3", true);
            FourButton.SetTextString("4", true);
            FiveButton.SetTextString("5", true);
            SixButton.SetTextString("6", true);

            VerySlowButton.SetTextString("VerySlowLabel".Localize());
            SlowButton.SetTextString("SlowLabel".Localize());
            FastButton.SetTextString("FastLabel".Localize());
            VeryFastButton.SetTextString("VeryFastLabel".Localize());

            LowButton.SetTextString("LowLabel".Localize());
            HighButton.SetTextString("HighLabel".Localize());
            VeryHighButton.SetTextString("VeryHighLabel".Localize());

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

            layoutRoot.Children.Add(_bottomBar, 0, 3);

            dicePanel.PanelIsBusy += OnBusy;

            MotionHelper.DeviceShaked += () => dicePanel.RollDice(null);
        }

        private void OnBusy(bool isBusy)
        {
            if (isBusy)
                DialogsHelper.ShowLoading();
            else
                DialogsHelper.HideLoading();
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
            DialogsHelper.ShowLoading();
        }
        async void PreparePanel()
        {
            DialogsHelper.HideLoading();
            if (dicePanel.NumDice != 0)
                return;
            await dicePanel.SetStyleAsync(DiceStyle.dpsBlue);
            dicePanel.RollDelay = 20;
            dicePanel.ClickToFreeze = true;
            dicePanel.NumDice = 5;
            dicePanel.EndRoll += DicePanel_EndRoll;
        }

        private void DicePanel_EndRoll()
        {
            TipsProvider1.Children.Clear();
            //MessageBox.Show(DicePanel1.Result.ToString());
            foreach (Die d in dicePanel.aDice)
                TipsProvider1.Children.Add(new Label()
                {
                    Text = d.Result.ToString(),
                    TextColor = Xamarin.Forms.Color.Blue,
                    XAlign= Xamarin.Forms.TextAlignment.Start,
                    HorizontalOptions = LayoutOptions.Start
                });

            if (dicePanel.NumDice > 1)
                TipsProvider1.Children.Insert(0, new Label() { Text = dicePanel.Result.Total.ToString(), TextColor = Xamarin.Forms.Color.Red });

            if (dicePanel.NumDice > 3 && dicePanel.Result.NumPairs() > 1)
            {
                TipsProvider1.Children.Insert(0,new Label() { Text = string.Format("{0} {1}", dicePanel.Result.NumPairs(), "PairsLabel".Localize()), TextColor = Xamarin.Forms.Color.Lime });
            }
            if (dicePanel.NumDice == 5)
            {
                if (dicePanel.Result.KniffelFullHouseScore() > 0)
                    TipsProvider1.Children.Insert(0, new Label() { Text = "FullHouseLabel".Localize(), TextColor = Xamarin.Forms.Color.Lime });
                if (dicePanel.Result.KniffelLargeStraightScore() > 0)
                    TipsProvider1.Children.Insert(0, new Label() { Text = "LargeStraightLabel".Localize(), TextColor = Xamarin.Forms.Color.Lime });
                else
                {
                    if (dicePanel.Result.KniffelSmallStraightScore() > 0)
                        TipsProvider1.Children.Insert(0, new Label() { Text = "SmallStraight".Localize(), TextColor = Xamarin.Forms.Color.Lime });
                }
            }

            for (int i = 6; i > 2; i--)
                if (dicePanel.Result.KniffelOfAKindScore(i) > 1)
                {
                    TipsProvider1.Children.Insert(0, new Label() { Text = string.Format("{0} {1}", i, "OfAKindLabel".Localize()), TextColor = Xamarin.Forms.Color.Lime });
                    break;
                }
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
            CountButtons.IsVisible = true;
            SettingsButtons.IsVisible = false;
        }

        private void SpeedButton_Click(object sender, EventArgs e)
        {
            SpeedButtons.IsVisible = true;
            SettingsButtons.IsVisible = false;
        }

        private void ProjectionButton_Click(object sender, EventArgs e)
        {
            ProjectionButtons.IsVisible = true;
            SettingsButtons.IsVisible = false;
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
            CountButtons.IsVisible = false;
            SpeedButtons.IsVisible = false;
            ProjectionButtons.IsVisible = false;
            SettingsButtons.IsVisible = true;

        }

        private void OneButton_Click(object sender, EventArgs e)
        {
            var b = sender as WPButton;
            
            dicePanel.NumDice = int.Parse(b.Label);
            CountButton.SetTextString(b.Label, true);
            HideSettings();
            TipsProvider1.Children.Clear();
            _rollButton.IsEnabled = true;
            _clearButton.IsEnabled = false;
        }

        private void SlowButton_Click(object sender, EventArgs e)
        {
            var b = sender as WPButton;
            
            dicePanel.RollDelay = int.Parse(b.Tag);
            SpeedButton.SetTextString(b.Label);
            HideSettings();
        }

        private void LowButton_Click(object sender, EventArgs e)
        {
            var b = sender as WPButton;
            dicePanel.DieAngle = int.Parse(b.Tag);
            ProjectionButton.SetTextString(b.Label);
            HideSettings();
        }
    }
}
