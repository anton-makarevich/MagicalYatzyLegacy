using Sanet.Models;
using Sanet.AllWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if WinRT
using MyToolkit.UI;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
#else
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
#endif


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
                return aDice.FirstOrDefault(f => f.Result == panel.SelectedIndex + 1);
            }
            set 
            {
                panel.SelectedIndex = value.Result - 1;
            }
        }

        //Border border = new Border();
#if WinRT
        GridView panel = new GridView();
#else
        ListBox panel = new ListBox();
#endif

        TextBlock caption = new TextBlock();

        public DiceValueSelectionPanel()
        {
            //border.Child = panel;

            panel.HorizontalAlignment = HorizontalAlignment.Center;
           

            this.Background = new SolidColorBrush(Colors.Black);

            panel.SelectionChanged += panel_SelectionChanged;

            //Add cption
            caption.Foreground = Brushes.SolidSanetBlue;
            caption.Margin = new Thickness(15);
#if WinRT
             panel.Height = 90;
            caption.FontSize = 28;
#endif
#if WINDOWS_PHONE
            panel.Width = 80;
            panel.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            caption.FontSize = 20;
#endif
#if VK
            panel.ItemsPanel = Application.Current.Resources["WrapPanelItemsPanelTemplate"] as ItemsPanelTemplate;
            panel.Height = 100;
            panel.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            caption.FontSize = 20;
#endif
            this.Children.Add(caption);
            this.Children.Add(panel);
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
            
            caption.Text = "SelectNewDiceValueMessage".Localize();
            
            foreach (var dice in aDice)
                dice.DrawDie();
        }
        Rect GetElementRect(FrameworkElement element)
        {
#if WinRT
            GeneralTransform buttonTransform = element.TransformToVisual(null);
            Windows.Foundation.Point point = buttonTransform.TransformPoint(new Windows.Foundation.Point());

            return new Windows.Foundation.Rect(point, new Windows.Foundation.Size(element.ActualWidth, element.ActualHeight));
#endif
#if WINDOWS_PHONE
            return new Rect(0, 100, 480, 700);
#endif
#if VK
            GeneralTransform gt = element.TransformToVisual(Application.Current.RootVisual as UIElement);
            Point offset = gt.Transform(new Point(0, 0));
            return new Rect(offset,new Size( element.ActualWidth, element.ActualHeight));
#endif
        }

        public void Dispose()
        {
            panel.SelectionChanged -= panel_SelectionChanged;
            parentPopup.IsOpen = false;
            
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
