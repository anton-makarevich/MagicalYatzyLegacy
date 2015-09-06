using Sanet.Kniffel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DiceRollerXF.Controls
{
    public partial class HyperlinkButton : Grid
    {
        IActionsService _service;
        public HyperlinkButton()
        {
            InitializeComponent();
            Label.TextColor=Line.BackgroundColor = Color.Accent;
            _service = Xamarin.Forms.DependencyService.Get<IActionsService>();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var el = sender as VisualElement;
            el.AnimateClick();

            if (!string.IsNullOrEmpty(Tag))
            {
                if (Tag.StartsWith("mailto:"))
                {
                    var to = Tag.Replace("mailto:", "");
                    _service.SendEmail(to, "", "");
                }
                else
                    _service.NavigateToUrl(Tag);
            }
        }

        public string Content
        {
            get
            {
                return this.Label.Text;
            }
            set
            {
                Label.Text = value;
                InvalidateMeasure();
                //Label.InvalidateLayout();
            }
        }

        public string Tag { get; set; }
    }
}
