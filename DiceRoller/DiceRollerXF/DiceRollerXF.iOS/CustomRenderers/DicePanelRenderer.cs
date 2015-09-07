using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using Sanet.Kniffel.DicePanel;
using DiceRollerXF.Controls;
using DiceRollerXF.Models;
using Sanet.Kniffel.iOS.Renderers;

[assembly: ExportRenderer(typeof(ContentPageEx), typeof(DicePanelRenderer))]
namespace Sanet.Kniffel.iOS.Renderers
{
    public class DicePanelRenderer :PageRenderer
    {
        public override bool CanBecomeFirstResponder
        {
            get
            {
                return true;
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            BecomeFirstResponder();
        }

        public override void MotionEnded(UIEventSubtype motion, UIEvent evt)
        {
            if (motion == UIEventSubtype.MotionShake)
                MotionHelper.ShakeNotify();
        }
    }

}