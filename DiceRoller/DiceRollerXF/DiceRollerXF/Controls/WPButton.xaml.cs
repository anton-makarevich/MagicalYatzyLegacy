using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DiceRollerXF.Controls
{
    public partial class WPButton : Grid
    {
        public event EventHandler Clicked;

        string _imageSource;
        public WPButton()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            border.AnimateClick();
            if (Clicked != null)
                Clicked(this,null);
        }

        public void SetImageString(string value)
        {
            _imageSource = value;
                Image.Source = Xamarin.Forms.ImageSource.FromResource(value);
                Image.IsVisible = true;
                Text.IsVisible = false;
        }

        public void SetTextString(string value, bool extraLarge=false)
        {
            Text.Text = value;
            if (extraLarge)
                Text.FontSize = 40;
            Text.IsVisible = true;
            Image.IsVisible = false;
        }

        public string Tag { get; set; }

        public string ImageSource
        {
            get
            {
                return _imageSource;
            }
        }

        public string Label { get { return Text.Text; } }
    }
}
