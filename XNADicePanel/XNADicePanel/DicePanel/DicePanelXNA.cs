using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using Sanet.Kniffel.DicePanel;
using Sanet.XNAEngine;
using Sanet.Kniffel.Models;

namespace Sanet.Kniffel.Xna
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class DicePanel : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        #region Events
        public event Action EndRoll;
        public event Sanet.Kniffel.Xna.DicePanelScene.DieFrozenEventHandler DieFrozen;
        public event Sanet.Kniffel.Xna.DicePanelScene.DieChangedEventHandler DieChangedManual;
        #endregion


        public DicePanel()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
                        
            graphics.PreferMultiSampling = true;
            graphics.IsFullScreen = false;
            Window.OrientationChanged += Window_OrientationChanged;
            this.IsMouseVisible = true;

            IsFixedTimeStep = false;
            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(20000);//333333

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);

        }

        void Window_OrientationChanged(object sender, EventArgs e)
        {
            try
            {
                DPanel.FindDicePosition();
            }
            catch { }
        }

        #region DicePanel Properties

        DicePanelScene DPanel
        {
            get
            {
                return (DicePanelScene)SceneManager.GameScenes.FirstOrDefault(f => f is DicePanelScene);
            }
        }

        DiceSelectorScene DScene
        {
            get
            {
                return (DiceSelectorScene)SceneManager.GameScenes.FirstOrDefault(f => f is DiceSelectorScene);
            }
        }

        public DiceStyle PanelStyle
        {
            get
            { return DPanel.PanelStyle; }
            set
            {
                DPanel.PanelStyle = value;
            }
        }

        public bool IsRolling
        {
            get 
            {
                return DPanel.IsRolling;
            }
        }

        //just fake to keep APIs compartible
        public double TreeDScaleCoef { get; set; }

        //do not change for now
        public int NumDice { get; set; }

        public int RollDelay
        {
            get { return DPanel.RollDelay; }
            set { DPanel.RollDelay = value; }
        }

        public int DieAngle
        {
            get { return DPanel.DieAngle; }
            set
            {
                DPanel.DieAngle = value;
            }
        }

        public int MaxRollLoop
        {
            get { return DPanel.MaxRollLoop; }
            set
            {
                DPanel.MaxRollLoop = value;
            }
        }

        public DieResult Result
        {
            get
            {
                return DPanel.Result;
            }
        }

        public bool ClickToFreeze
        {
            get { return DPanel.ClickToFreeze; }
            set { DPanel.ClickToFreeze = value; }
        }

        public bool WithSound
        {
            get
            { return DPanel.WithSound; }
            set
            {
                DPanel.WithSound = value;
            }
        }

        public bool ManualSetMode
        {
            get { return DPanel.ManualSetMode; }
            set
            { DPanel.ManualSetMode = value; }
        }

        public Rectangle Margin
        {
            get
            {
                return DPanel.Margin;
            }
            set
            {
                DPanel.Margin = value;
                DScene.Margin = new Rectangle(value.Left, value.Top, value.Width, value.Height);
            }
        }

        public bool AllDiceFrozen()
        {
            return DPanel.AllDiceFrozen();
        }

        #endregion

        #region DicePanel Methods
        public bool RollDice(List<int> aResults)
        {
            return DPanel.RollDice(aResults);
        }

        public void ClearFreeze()
        {
            DPanel.ClearFreeze();
        }

        public void FixDice(int index, bool isfixed)
        {
            DPanel.FixDice(index, isfixed);
        }

        public void ChangeDice(int oldValue, int newValue)
        {
            DPanel.ChangeDice(oldValue, newValue);
        }

        

        #endregion

        #region Helpers
        public void AddHandlers()
        {
            DPanel.EndRoll += () =>
            {
                if (EndRoll != null)
                    EndRoll();
            };

            DPanel.DieFrozen += (frozen, value) =>
            {
                if (DieFrozen != null)
                    DieFrozen(frozen, value);
            };

            DPanel.DieChangedManual += (frozen, oldvalue, newvalue) =>
            {
                if (DieChangedManual != null)
                    DieChangedManual(frozen, oldvalue, newvalue);
            };
        }
        #endregion

        #region XNAGame
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            SceneManager.GameScenes.Clear();
            SceneManager.RenderContext.GraphicsDevice = graphics.GraphicsDevice;

            DicePanelScene dicePanelScene = new DicePanelScene("DicePanelScene");
            SceneManager.AddGameScene(dicePanelScene);

            DiceSelectorScene diceSelectorScene = new DiceSelectorScene("DiceSelectorScene");
            SceneManager.AddGameScene(diceSelectorScene);

            SceneManager.SetActiveScene("DicePanelScene");
            SceneManager.Initialize();
                        
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            SceneManager.RenderContext.SpriteBatch = spriteBatch;
            SceneManager.LoadContent(Content);
            Extensions.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            SceneManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);

            SceneManager.Draw();

            base.Draw(gameTime);
        }
        #endregion
    }


    
}
