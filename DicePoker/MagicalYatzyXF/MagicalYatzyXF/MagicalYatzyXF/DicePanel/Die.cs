using MagicalYatzyXF;
using NGraphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanet.Kniffel.DicePanel
{
    /// <summary>
    /// Dice oject
    /// </summary>
    public class Die
    {

        private const int MAXMOVE = 5;

        //public MediaElement mSound;
        private enum DieStatus
        {
            dsStopped = 0,
            dsRolling = 1,
            dsLanding = 2
        }
        private IImage _png;
        public IImage PNG
        {
            get { return _png; }
            set { _png = value; }
        }

        private int FRollLoop;
        private int h = 72;
        private int w = 72;
        private int FxPos;

        private int FyPos;
        private int dxDir;
        //направление движения
        private int dyDir;
                

        public DicePanel FPanel;

        private DieStatus FStatus = DieStatus.dsLanding;
        public string StyleString
        {
            get { return  FPanel.strStyle; }
        }
        private string strrot;

        //private string strframe;

        private IImage GetFramePic()
        {
            try
            {
                string sPath = StyleString + strrot + Frame.ToString() + ".png";

                return FPanel.DieFrameImages[sPath];
            }
            catch (Exception ex)
            {
                var t = ex.Message;
                return null;
            }


        }
        public Die(DicePanel pn)
            : base()
        {
            //PNG = new Image();
            FPanel = pn;
            pStatus = DieStatus.dsStopped;
        }
        public void PlaySound()
        {
            if (pStatus != DieStatus.dsRolling)
                return;

            int index = FPanel.FRand.Next(1, 4);
            //SoundsProvider.PlaySound(mSound, "dice"+index.ToString());
        }

        private int FFrame;
        private int Frame
        {
            get { return FFrame; }
            set
            {
                FFrame = value;

                if (FFrame < 0)
                    FFrame += 36;
                if (FFrame > 35)
                    FFrame -= 36;
            }
        }


        //результат кости 1-6
        private int FResult = 1;
        public int Result
        {
            get { return FResult; }
            set
            {
                if (value < 1 | value > 6)
                {
                    throw new Exception("Неправильное значение кости");
                }
                else
                {
                    FResult = value;
                }
            }
        }

        private int xPos
        {
            get { return FxPos; }
            set
            {
                int ks = 0;

                FxPos = value;

                if (FxPos < 0 + ks)
                {
                    FxPos = 0 + ks;
                    BounceX();
                }
                double MW = 0;
                try
                {
                    MW = /*(FPanel.Width == 0) ? FPanel.Width :*/ FPanel.ActualWidth;
                }
                catch { }
                if (FxPos > (MW) - w - ks)
                {
                    FxPos = (int)(MW) - w - ks;
                    BounceX();
                }
            }
        }

        private int yPos
        {
            get { return FyPos; }
            set
            {
                FyPos = value;

                if (FyPos < 0)
                {
                    FyPos = 0;
                    BounceY();
                }
                double MH = 0;
                try
                {
                    MH = /*(FPanel.ActualHeight == 0) ? FPanel.Height :*/ FPanel.ActualHeight;
                }
                catch { }
                if (FyPos > MH - h)
                {
                    FyPos = (int)(MH) - h;
                    BounceY();
                }
            }
        }


        private bool FFrozen;
        public bool Frozen
        {
            get { return FFrozen; }

            set { FFrozen = value; }
        }

        public void InitializeLocation()
        {
            try
            {
                var w1 =  FPanel.ActualWidth;
                var h1 =  FPanel.ActualHeight;
                if (w1 > 0 && h1 > 0)
                {
                    int mw = w1 - w;
                    xPos = FPanel.FRand.Next(1, mw);
                    mw = h1 - h;
                    yPos = FPanel.FRand.Next(1, mw);
                }
                else
                {
                    xPos = 0;
                    yPos = 0;
                }
                if (xPos < 0) xPos = 0;
                if (yPos < 0) yPos = 0;
            }
            catch
            {
                xPos = 0;
                yPos = 0;
            }
            UpdatePngPosition();
        }
        public int iSound;

        public void UpdateDiePosition()
        {
            switch (pStatus)
            {
                case DieStatus.dsLanding:

                    Frame -= 1;
                    break;
                case DieStatus.dsRolling:
                    //увеличиваем либо уменьшаем в зависимости отнаправления
                    Frame += (1 * Math.Sign(dyDir));
                    break;
                //mSound.Stop() 'Position = TimeSpan.Zero


                case DieStatus.dsStopped:
                    return;


            }


            xPos += dxDir;
            yPos += dyDir;

            FRollLoop += 1;

            switch (pStatus)
            {
                case DieStatus.dsRolling:
                    //если достигнуто максимальное число вращений останавливаемся
                    if (FRollLoop > FPanel.MaxRollLoop & FPanel.FRand.Next(1, 100) < 10)
                    {
                        pStatus = DieStatus.dsLanding;
                        FRollLoop = 0;

                        Frame = Result * 6;
                    }

                    break;
                case DieStatus.dsLanding:

                    if (FRollLoop > (5 - FPanel.DieAngle))
                    {
                        pStatus = DieStatus.dsStopped;
                    }
                    break;
            }
            UpdatePngPosition();
        }
        private void UpdatePngPosition()
        {
            /*PNG.SetValue(Canvas.LeftProperty, Convert.ToDouble(xPos));
            PNG.SetValue(Canvas.TopProperty, Convert.ToDouble(yPos));
            */
        }
        //new, done for directional changing, collision but
        private DieStatus pStatus
        {
            get { return FStatus; }
            set
            {
                FStatus = value;
                if (value == DieStatus.dsStopped)
                {
                    dxDir = 0;
                    //stop direction
                    dyDir = 0;

                }
            }
        }

        public void InitializeRoll(int iResult = 0)
        {
            if (iResult < 0 | iResult > 6)
                iResult = 0;
            //new
            if (!Frozen)
            {
                //скорость и направление начального движения
                do
                {
                    dxDir = FPanel.FRand.Next(-MAXMOVE, MAXMOVE + 1);
                } while (!(Math.Abs(dxDir) > 2));
                do
                {
                    dyDir = FPanel.FRand.Next(-MAXMOVE, MAXMOVE + 1);
                } while (!(Math.Abs(dyDir) > 2));
                if (iResult == 0)
                {
                    Result = FPanel.FRand.Next(1, 7);
                    //decide what the result will be
                }
                else
                {
                    Result = iResult;
                }
                FRollLoop = 0;
                pStatus = DieStatus.dsRolling;
            }
            else
            {
                pStatus = DieStatus.dsStopped;
            }
        }
        //Private _style As dpStyle = dpStyle.dpsClassic
        //Public Property Style As dpStyle
        //    Get
        //        Return _style
        //    End Get
        //    Set(ByVal value As dpStyle)
        //        _style = value
        //    End Set
        //End Property
        public void DrawDie()
        {
            //RESCALE
            /*if (FPanel.TreeDScale)
            {
                var M = FPanel.Height - 72;
                double k = this.yPos / M * FPanel.TreeDScaleCoef + (1 - FPanel.TreeDScaleCoef);
                this.h = Convert.ToInt32(72 * k);
                this.w = Convert.ToInt32(72 * k);
                /*ScaleTransform st = new ScaleTransform();
                st.ScaleX = k;
                st.ScaleY = k;
                st.CenterX = PNG.Width / 2;
                st.CenterY = PNG.Height / 2;
                PNG.RenderTransform = st;
            }*/
            //select the correct bitmap based on what the die is doing, and what direction it's going
            if (pStatus == DieStatus.dsRolling)
            {
                //PNG.Opacity = 1;
                if ((dxDir * dyDir) > 0)
                {
                    strrot = "yrot.";
                    PNG = GetFramePic();

                }
                else
                {
                    strrot = "xrot.";
                    PNG = GetFramePic();
                }

            }
            else
            {
                Frame = (Result - 1) * 6 + FPanel.DieAngle;
                strrot = "stop.";
                PNG = GetFramePic();
                if (Frozen)
                {
                    //strrot = "stop." '"halo."
                    Opacity = 0.5f;
                }
                else
                {
                    Opacity = 1;
                }
            }


        }
        public float Opacity
        { get; private set; }
        public bool IsNotRolling
        {
            get { return pStatus == DieStatus.dsStopped; }
        }

        public bool IsRolling
        {
            get { return pStatus == DieStatus.dsRolling; }
        }

        public Xamarin.Forms.Rectangle Rect
        {
            get { return new Xamarin.Forms.Rectangle(xPos, yPos , w , h ); }
        }

        public Xamarin.Forms.Rectangle RectScaled
        {
            get { return new Xamarin.Forms.Rectangle(xPos * App.DeviceScale, yPos * App.DeviceScale, w * App.DeviceScale, h * App.DeviceScale); }
        }

        public bool Overlapping(Die d)
        {
            //Return d.Rect.Interse(Me.Rect)

            Xamarin.Forms.Rectangle rect1 =Rect.Intersect(d.Rect);
            var rv= !(rect1 == Xamarin.Forms.Rectangle.Zero);
            return rv;
        }

        public void HandleCollision(Die d)
        {
            if (this.Overlapping(d))
            {
                if (Math.Abs(d.yPos - this.yPos) <= Math.Abs(d.xPos - this.xPos))
                {
                    HandleBounceX(d);
                }
                else
                {
                    HandleBounceY(d);
                }
            }
        }


        private void HandleBounceX(Die d)
        {
            Die dLeft = null;
            Die dRight = null;

            if (this.xPos < d.xPos)
            {
                dLeft = this;
                dRight = d;
            }
            else
            {
                dLeft = d;
                dRight = this;
            }

            //moving toward each other
            if (dLeft.dxDir >= 0 & dRight.dxDir <= 0)
            {
                this.BounceX();
                d.BounceX();
                return;
            }

            //moving right, left one caught up to right one
            if (dLeft.dxDir > 0 & dRight.dxDir >= 0)
            {
                dLeft.BounceX();
                return;
            }

            //moving left, right one caught up to left one
            if (dLeft.dxDir <= 0 & dRight.dxDir < 0)
            {
                dRight.BounceX();
            }

        }


        private void HandleBounceY(Die d)
        {
            Die dTop = null;
            Die dBot = null;

            if (this.yPos < d.yPos)
            {
                dTop = this;
                dBot = d;
            }
            else
            {
                dTop = d;
                dBot = this;
            }

            if (dTop.dyDir >= 0 & dBot.dyDir <= 0)
            {
                this.BounceY();
                d.BounceY();
                return;
            }

            //moving down, top one caught up to bottom one
            if (dTop.dyDir > 0 & dBot.dyDir >= 0)
            {
                dTop.BounceY();
                return;
            }

            //moving left, bottom one caught up to top one
            if (dTop.dyDir <= 0 & dBot.dyDir < 0)
            {
                dBot.BounceY();
            }

        }

        private void BounceX()
        {
            dxDir = -dxDir;

            //no sound if not moving
            if (pStatus != DieStatus.dsStopped)
            {
                FPanel.OnDieBounced();
            }
        }

        private void BounceY()
        {
            dyDir = -dyDir;

            //no sound if not moving
            if (pStatus != DieStatus.dsStopped)
            {
                FPanel.OnDieBounced();
            }
        }

        //new
        public bool ClickedOn(double x, double y)
        {
            return this.Rect.Contains(new Xamarin.Forms.Point(x, y));
        }


        public void Dispose()
        {
            //mSound = null;
            PNG = null;
        }
    }
}
