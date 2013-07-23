using Microsoft.Xna.Framework;
using Sanet.Kniffel.DicePanel;
using Sanet.Models;
using Sanet.XNAEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if NETFX_CORE
using Windows.UI.Xaml.Controls;
#endif
namespace Sanet.Kniffel.Xna
{
    /// <summary>
    /// Dice oject
    /// </summary>
    public class Die : GameObject2D
    {

        private const int MAXMOVE = 7;
       
        //sprites for different dice styles
        GameSprite _blueSprite;
        GameSprite _whiteSprite;
        GameSprite _redSprite;
        //active style sprite
        GameSprite _ActiveSprite 
        {
            get
            {
                if (FPanel.PanelStyle == DiceStyle.dpsBlue)
                    return _blueSprite;
                else if (Style == DiceStyle.dpsBrutalRed)
                    return _redSprite;
                else
                    return _whiteSprite;
            }
        }

        
        /*
         *0 for stop
         *1 for xrot
         *2 for yrot
         */
        int _spritePosMultiplier=0;

        private int FRollLoop;
        private int h = 72;
        private int w = 72;
        private int FxPos;

        private int FyPos;
        private int dxDir;
        //направление движения
        private int dyDir;
        //результат кости 1-6
        private int FResult = 1;
        DiceStyle _Style=  DiceStyle.dpsClassic;
        DicePanelScene FPanel;

        public int iSound;
#if NETFX_CORE
        public MediaElement mSound;
#endif
        private DieStatus FStatus = DieStatus.dsLanding;

        private bool FFrozen;
        private int _Frame;

        #region Constructor
        public Die(DicePanelScene pn)
            : base()
        {

            FPanel = pn;
            Style = pn.PanelStyle;
            pStatus = DieStatus.dsStopped;
        }
        #endregion



        #region Properties
        private DieStatus pStatus
        {
            get { return FStatus; }
            set
            {
                FStatus = value;
                if (value == DieStatus.dsStopped)
                {
                    dxDir = 0;
                    dyDir = 0;

                }
            }
        }

        int wMin
        {
            get
            {return  FPanel.Margin.X;}
        }
        int hMin
        {
            get
            { return FPanel.Margin.Y; }
        }
        int w1
        {
            get
            { return FPanel.Width - FPanel.Margin.Width; }
        }

        int h1
        {
            get
            { return FPanel.Height - FPanel.Margin.Height; }
        }
            
        public DiceStyle Style
        {
            get
            {
                return _Style;
            }
            set
            {
                

                _Style = value;
                Frame = _Frame;
            }
        }

        public bool IsNotRolling
        {
            get { return pStatus == DieStatus.dsStopped; }
        }

        public bool IsRolling
        {
            get { return pStatus == DieStatus.dsRolling; }
        }


        private int Frame
        {
            get { return _Frame; }
            set
            {
                _Frame = value;

                if (_Frame < 0)
                    _Frame += 36;
                if (_Frame > 35)
                    _Frame -= 36;

                var y = _spritePosMultiplier * 6 + Math.Ceiling((double)(_Frame+1) / 6);
                var x = (_Frame + 1) % 6;
                if (x == 0) 
                    x = 6;
                if (_ActiveSprite!=null)
                    _ActiveSprite.DrawRect = new Rectangle((x-1)*w, (int)(y-1)*h, w, h);
            }
        }



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
                
                FxPos = value;

                if (FxPos < wMin )
                {
                    FxPos = wMin;
                    BounceX();
                }
                
                if (FxPos > (w1) - w )
                {
                    FxPos = (w1) - w ;
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

                if (FyPos <hMin)
                {
                    FyPos = hMin;
                    BounceY();
                }
                
                if (FyPos > h1 - h)
                {
                    FyPos = h1 - h;
                    BounceY();
                }
            }
        }

        DateTime lastTimeFrozen = DateTime.MinValue;

        public bool Frozen
        {
            get { return FFrozen; }

            set 
            {
                if ((DateTime.Now - lastTimeFrozen).TotalMilliseconds <1000)
                    return;
                FFrozen = value;
                lastTimeFrozen = DateTime.Now;
            }
        }

        #endregion

        #region Methods
        public void PlaySound()
        {
            if (pStatus != DieStatus.dsRolling)
                return;
#if NETFX_CORE
            if (mSound == null)
                mSound = new MediaElement();
            
            int index = FPanel.FRand.Next(1, 4);
            SoundsProvider.PlaySound(mSound, "dice" + index.ToString());
#endif
        }

        public override void Initialize()
        {
            _blueSprite = new GameSprite("d2");
            //AddChild(_blueSprite);

            _redSprite = new GameSprite("d1");
            //AddChild(_redSprite);

            _whiteSprite = new GameSprite("d0");
            //AddChild(_whiteSprite);
            Frame =  6;
                    
        }

        public void InitializeLocation()
        {
            
            try
            {
                
                if (w1 > wMin && h1 > hMin)
                {
                    int mw = w1 - w;
                    xPos = FPanel.FRand.Next(1, mw);
                    mw = h1 - h;
                    yPos = FPanel.FRand.Next(1, mw);
                }
                else
                {
                    xPos = wMin;
                    yPos = hMin;
                }
                if (xPos < wMin) xPos = wMin;
                if (yPos < hMin) yPos = hMin;
            }
            catch
            {
                xPos = wMin;
                yPos = hMin;
            }
            BoundingRect = new Rectangle(xPos, yPos, w, h);
        }

        public override void Translate(Vector2 position)
        {
            if (_ActiveSprite!=null)
                _ActiveSprite.Translate(position);
            
            base.Translate(position);
        }

        public void SetPosition(int x, int y)
        {
            xPos = x;
            yPos = y;
            Translate(new Vector2(x, y));
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager contentManager)
        {
            _blueSprite.LoadContent(contentManager);
            _blueSprite.CreateBoundingRect(w, h);

            _redSprite.LoadContent(contentManager);
            _redSprite.CreateBoundingRect(w, h);

            _whiteSprite.LoadContent(contentManager);
            _whiteSprite.CreateBoundingRect(w, h);
                        
            base.LoadContent(contentManager);
            
            
        }

        public override void Update(RenderContext renderContext)
        {
            if (_ActiveSprite != null)
                _ActiveSprite.Update(renderContext);
            else
            {
                Style = FPanel.PanelStyle;
            }

            if (_ActiveSprite == null)
                return;

            BoundingRect = new Rectangle(xPos, yPos, w, h);

            xPos += dxDir;
            yPos += dyDir;

            //can't use foreach loops here, want to start j loop index AFTER first loop
            foreach (var d in FPanel.aDice)
            {
                if (d != this)
                    d.HandleCollision(this);
            }
        
            Translate(new Vector2(xPos, yPos));

            //select the correct bitmap based on what the die is doing, and what direction it's going
            if (pStatus == DieStatus.dsRolling)
            {
                _ActiveSprite.Color = Color.White;
                if ((dxDir * dyDir) > 0)
                {
                    _spritePosMultiplier = 2;

                }
                else
                {
                    _spritePosMultiplier = 1;
                }

            }
            else
            {
                Frame = (Result - 1) * 6 + FPanel.DieAngle;
                _spritePosMultiplier = 0;
                if (Frozen)
                {
                    _ActiveSprite.Color = Color.FromNonPremultiplied(0, 255, 255, 100);
                }
                else
                {
                    _ActiveSprite.Color = Color.White;
                }
            }


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
            
            
            base.Update(renderContext);
        }
        

        public void InitializeRoll(int iResult)
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
                } while (!(Math.Abs(dxDir) > 3));
                do
                {
                    dyDir = FPanel.FRand.Next(-MAXMOVE, MAXMOVE + 1);
                } while (!(Math.Abs(dyDir) > 3));
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




        public override void Draw(RenderContext renderContext)
        {
            if (_ActiveSprite!=null)
                _ActiveSprite.Draw(renderContext);
            base.Draw(renderContext);
        }

        #endregion



        public void HandleCollision(Die d)
        {
            if (this.HitTest(d))
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
        public bool ClickedOn(int x, int y)
        {
            if (!BoundingRect.HasValue)
                return false;
            return BoundingRect.Value.Contains(new Point(x, y));
        }


        

        public void Dispose()
        {

        }
    }
}
