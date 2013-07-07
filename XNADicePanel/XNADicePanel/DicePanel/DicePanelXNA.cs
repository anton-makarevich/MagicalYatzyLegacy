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

namespace Sanet.Kniffel.Xna
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class DicePanel : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;




        public DicePanel()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
                        
            graphics.PreferMultiSampling = true;
            graphics.IsFullScreen = false;
            this.IsMouseVisible = true;

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);

        }

        #region DicePanel Properties

        DicePanelScene DPanel
        {
            get
            {
                return (DicePanelScene)SceneManager.GameScenes.FirstOrDefault(f => f is DicePanelScene);
            }
        }

        #endregion

        #region DicePanel Methods
        public bool RollDice(List<int> aResults)
        {
            return DPanel.RollDice(aResults);
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
            SceneManager.RenderContext.GraphicsDevice = graphics.GraphicsDevice;

            DicePanelScene dicePanelScene = new DicePanelScene("DicePanelScene");
            SceneManager.AddGameScene(dicePanelScene);

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
