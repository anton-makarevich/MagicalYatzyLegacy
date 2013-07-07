using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Sanet.XNAEngine;
using Sanet.Kniffel.DicePanel;
using Sanet.Kniffel.Models;


namespace Sanet.Kniffel.Xna
{
   public class DicePanelScene :GameScene
   {
       
       #region Dicepanel fields

       //touch imput to support clicks on dice
       TouchInput _touchInput;

       //text  caption to ask user to select dice
       TextPrinter _captionText;

       //last frame state;
       bool _isWasRolling = false;

       public Random FRand = new Random();

       public List<Die> aDice = new List<Die>();

       public event DieBouncedEventHandler DieBounced;
       public delegate void DieBouncedEventHandler();
       public event DieFrozenEventHandler DieFrozen;
       public delegate void DieFrozenEventHandler(bool @fixed, int Value);
       public delegate void DieChangedEventHandler(bool isfixed, int oldvalue, int newvalue);
       public event DieChangedEventHandler DieChangedManual;
       private DiceStyle FStyle = DiceStyle.dpsClassic;

       private Color dpBackcolor = Color.LightGray;

       public event EndRollEventHandler EndRoll;
       public delegate void EndRollEventHandler();
       public event BeginRollEventHandler BeginRoll;
       public delegate void BeginRollEventHandler();
       #endregion

       #region Properties
       public bool PlaySound { get; set; }

       
        public DiceStyle PanelStyle
        {
            get { return FStyle; }
            set
            {
                FStyle = value;

                
               
            }
        }

        private bool _WithSound = false;
        public bool WithSound
        {
            get { return _WithSound; }
            set
            {
                if (_WithSound != value)
                {
                    _WithSound = value;

                }
            }
        }

        private int _RollDelay = 1;
        public int RollDelay
        {
            get { return _RollDelay; }
            set { _RollDelay = value; }
        }


        private int FDieAngle = 0;
        public int DieAngle
        {
            get { return FDieAngle; }
            set
            {
                if (value < 0)
                    value = 0;
                if (value > 5)
                    value = 5;
                FDieAngle = value;
                
            }
        }
        private int FMaxRollLoop = 50;
        public int MaxRollLoop
        {
            get { return FMaxRollLoop; }
            set
            {
                if (value < 20)
                    value = 20;
                if (value > 150)
                    value = 150;
                FMaxRollLoop = value;
            }
        }

        int _totalFrameTime=0;

        private int FNumDice = 5;

        /// <summary>
        /// Number of Dice in the Panel
        /// </summary>
        public int NumDice
        {
            get { return FNumDice; }
            set
            {
                FNumDice = value;


               

            }
        }


        private bool FDebugDrawMode = false;
        
        /// <summary>
        /// Draws a Box Around the Die for Collision Debugging
        /// </summary>
        public bool DebugDrawMode
        {
            get { return FDebugDrawMode; }
            set { FDebugDrawMode = value; }
        }

        /// <summary>
        /// Allows user to click dice to lock their movement
        /// </summary>
        private bool FClickToFreeze;
        public bool ClickToFreeze
        {
            get { return FClickToFreeze; }
            set { FClickToFreeze = value; }
        }

        /// <summary>
        /// Summed Result of All the Dice
        /// </summary>
        public DieResult Result
        {
            get
            {
                var dr = new List<int>();

                foreach (Die d in aDice)
                {
                    dr.Add(d.Result);

                }
                return new DieResult { DiceResults = dr };
            }
        }

        //new, changed visibility
        public bool AllDiceStopped
        {
            get
            {
                foreach (Die d in aDice)
                {
                    if (d.IsRolling)
                        return false;
                }

                return true;
            }
        }

        bool isRolling
        {
            get
            {
                foreach (Die d in aDice)
                    if (d.IsRolling) return true;
                return false;
            }
        }
       #endregion

        public DicePanelScene(string sceneName) : base(sceneName) { }
        

        private void FindDicePosition()
        {
            
            bool bDone = false;
            int iTry = 0;

                        
            int i =0;

            while (i < NumDice)
            {
                var d = aDice[i];
                iTry = 0;
                
                do
                {
                    iTry += 1;
                    bDone = true;
                    d.InitializeLocation();
                    foreach (Die dOld in aDice)
                    {
                        if (d.HitTest(dOld))
                        {
                            bDone = false;
                        }
                    }
                } while (!(bDone | iTry > 1000));
                
                i++;

            }
            
        }

        
        
        public bool RollDice(List<int> aResults )
        {
            
            if (ManualSetMode)
                ManualSetMode = false;
            //if (isRolling) return;
            Die d = null;


            //don't roll if all frozen
            if (AllDiceFrozen())
            {
                return false;
            }
            if (BeginRoll != null)
            {
                BeginRoll();
            }
            //first values for fixed dices
            int j = aDice.Count(f => f.Frozen);

            for (int i = 0; i <= aDice.Count - 1; i++)
            {
                int iResult = 0;
                d = aDice[i];
                if (d.Frozen)
                    continue;
                if ((aResults != null))
                {
                    if (aResults.Count == aDice.Count)
                    {
                        iResult = aResults[j];
                        j += 1;
                    }

                }
                d.iSound = FRand.Next(1, 10);
                d.InitializeRoll(iResult);

            }
            _totalFrameTime=0;
            return true;
        }

        
        private void HandleCollisions()
        {
            Die di = null;
            Die dj = null;
            int i = 0;
            int j = 0;

            if (NumDice == 1)
                return;

            //can't use foreach loops here, want to start j loop index AFTER first loop
            for (i = 0; i <= aDice.Count - 2; i++)
            {
                for (j = i + 1; j <= aDice.Count - 1; j++)
                {
                    di = aDice[i];
                    dj = aDice[j];
                    di.HandleCollision(dj);
                }
            }

        }

        public int Width { get; set; }
        public int Height { get; set; }

        Die _lastClickedDie;
        //new

        public void DieClicked()

        {
            Point pointClicked = new Point((int)_touchInput.ClickPosition.X, (int)_touchInput.ClickPosition.Y);
            //determine if die was clicked
            _lastClickedDie = null;
            foreach (Die d in aDice)
            {
                if (d.ClickedOn(pointClicked.X, pointClicked.Y))
                {
                    _lastClickedDie = d;
                    break; // TODO: might not be correct. Was : Exit For
                }
            }
            //no die was clicked
            if (_lastClickedDie == null)
                return;

            if (ManualSetMode)
            {
                SceneManager.SetActiveScene( "DiceSelectorScene");
            }
            else if (ClickToFreeze)
            {
                _lastClickedDie.Frozen = !_lastClickedDie.Frozen;
                
                if (DieFrozen != null)
                {
                    DieFrozen(_lastClickedDie.Frozen, _lastClickedDie.Result);
                }

            }
        }

        private bool DiceGenerated()
        {
            return (aDice != null);
        }

        public void OnDieBounced()
        {
            if (DieBounced != null)
            {
                DieBounced();
            }
        }

        public int FrozenCount()
        {
            int num = 0;
            foreach (Die d in aDice)
                if (d.Frozen) num++;
            return num;
        }

        //new
        //don't roll if all frozen
        public bool AllDiceFrozen()
        {

            
            foreach (Die d in aDice)
            {
                if (!d.Frozen)
                {
                    return false;
                }
            }
            return true;


        }
        public void FixDice(int index, bool isfixed)
        {
            foreach (Die d in aDice)
            {
                if (d.Result == index && d.Frozen==!isfixed)
                {
                    d.Frozen = isfixed;
                    return;
                }
            }
        }

        //new
        public void ClearFreeze()
        {
            Die d = null;
            //If ClickToFreeze Then
            foreach (Die d_loopVariable in aDice)
            {
                d = d_loopVariable;
                if (d.Frozen)
                {
                    d.Frozen = false;
                    
                }
            }
            //End If

        }

        
        private bool _ManualSetMode=false;
        public bool ManualSetMode
        {
            get { return _ManualSetMode; }
            set
            {
                if (_ManualSetMode != value)
                {
                    _ManualSetMode = value;
                    if (value)
                    {
                        //caption.Text = "SelectDiceToChangeMessage".Localize();
                        //caption.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        //caption.Text = string.Empty;
                        //caption.Visibility = Visibility.Collapsed;
                        //if (_popup.IsOpen)
                        //    _popup.IsOpen = false;
                    }
                    
                }
            }
        }


       #region XNA Things
        public override void Initialize()
        {
            //prepare caption
            //TODO: make FontSprite for this
            _captionText = new TextPrinter("Fonts/DefFont");
            _captionText.Position = new Vector2(320, 30);
            AddSceneObject(_captionText);


            _touchInput = new TouchInput();
            _touchInput.OnClick += DieClicked;
            AddSceneObject(_touchInput);

            NumDice = 5;
            aDice = new List<Die>();
            for (int i = 0; i < NumDice; i++)
            {
                var d = new Die(this);
                d.Initialize();
                aDice.Add(d);
            }

            
            

            PanelStyle = DiceStyle.dpsBlue;

            base.Initialize();

        }

        public override void Update(RenderContext renderContext)
        {
            if (Width != renderContext.GraphicsDevice.Viewport.Width ||
                Height != renderContext.GraphicsDevice.Viewport.Height)
            {
                Width = renderContext.GraphicsDevice.Viewport.Width;
                Height = renderContext.GraphicsDevice.Viewport.Height;
                if (aDice.Count == NumDice)
                    FindDicePosition();
                else
                    return;
            }
            switch (FStyle)
            {
                case DiceStyle.dpsClassic:
                    if (DieAngle < 2) DieAngle = 0;

                    break;
                case DiceStyle.dpsBrutalRed:
                    if (DieAngle < 2) DieAngle = 1;

                    break;
                case DiceStyle.dpsBlue:
                    if (DieAngle < 2) DieAngle = 1;

                    break;
            }


            dpBackcolor = Color.Transparent;
            

            //if (isRolling)
            //{
                _totalFrameTime += renderContext.GameTime.ElapsedGameTime.Milliseconds;

                if (_totalFrameTime >= RollDelay)
                {
                    _totalFrameTime = 0;
                    foreach (var d in aDice)
                    {
                        d.Style = FStyle;
                        _isWasRolling = isRolling;
                        d.Update(renderContext);
                        if (_isWasRolling && !isRolling)
                        {
                            if (EndRoll != null)
                            {
                                EndRoll();
                            }
                        }
                    }
                }
            //}


            base.Update(renderContext);
        }

        public override void LoadContent(ContentManager contentManager)
        {
            foreach (var d in aDice)
                d.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Draw2D(RenderContext renderContext, bool drawInFrontOf3D)
        {
            base.Draw2D(renderContext, drawInFrontOf3D);
            
            foreach (var f in aDice)
                f.Draw(renderContext);
        }
       #endregion
   }
    
}
