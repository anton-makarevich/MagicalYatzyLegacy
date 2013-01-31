using MyToolkit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace Sanet.Kniffel.DicePanel
{
    public class DiceValueSelectionPanel : Grid
    {
        List<Die> aDice = new List<Die>();
        public Die SelectedDice
        {
            get
            {
                if (panel.SelectedItem == null)
                    return null;
                return aDice.Find(f => f.Result == panel.SelectedIndex + 1);
            }
            set 
            {
                panel.SelectedIndex = value.Result - 1;
            }
        }

        //Border border = new Border();
        GridView panel = new GridView();

        public DiceValueSelectionPanel()
        {
            //border.Child = panel;
            this.Children.Add(panel);
            panel.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            panel.Height = 90;

            this.Background = new SolidColorBrush(Colors.Black);

            panel.SelectionChanged += panel_SelectionChanged;
        }

        void panel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            parentPopup.IsOpen = false;
        }

        
        public void AddDice(Die dice)
        {
            aDice.Add(dice);
            panel.Items.Add(dice.PNG);
            //dice.Tapped += dice_Tapped;

        }

        public void Draw()
        {
            Rect bounds = GetElementRect(aDice[0].FPanel);
            this.Width = bounds.Width;
            this.Height = bounds.Height;

            parentPopup.HorizontalOffset = bounds.Left;
            parentPopup.VerticalOffset = bounds.Top;

            foreach (var dice in aDice)
                dice.DrawDie();
        }
        Rect GetElementRect(FrameworkElement element)
        {
            GeneralTransform buttonTransform = element.TransformToVisual(null);
            Windows.Foundation.Point point = buttonTransform.TransformPoint(new Windows.Foundation.Point());

            return new Windows.Foundation.Rect(point, new Windows.Foundation.Size(element.ActualWidth, element.ActualHeight));
        }

        Popup parentPopup
        {
            get
            {
                return this.Tag as Popup;
            }
        }
    }
}
