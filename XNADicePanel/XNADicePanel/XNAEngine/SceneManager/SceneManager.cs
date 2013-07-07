using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sanet.XNAEngine
{
    //a scene manager class - the main class we will use in game
    //static as we should have only one for app accessable from everywhere 
    static class SceneManager
    {
        private static GameScene _newActiveScene;

        //set this to true to draw debug sprites
        public static bool IsDebug = false;

        static SceneManager()
        {
            GameScenes = new List<GameScene>();
            RenderContext = new RenderContext();
            //Default Camera
            RenderContext.Camera = new BaseCamera();
        }

        #region Properties
        public static Microsoft.Xna.Framework.Game MainGame { get; set; }
        public static List<GameScene> GameScenes { get; private set; }
        public static GameScene ActiveScene { get; private set; }
        public static RenderContext RenderContext { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Add new game scene
        /// </summary>
        public static void AddGameScene(GameScene gameScene)
        {
            if (!GameScenes.Contains(gameScene))
                GameScenes.Add(gameScene);
        }

        /// <summary>
        /// Removes gamescene and deactivates it if was active
        /// </summary>
        public static void RemoveGameScene(GameScene gameScene)
        {
            GameScenes.Remove(gameScene);

            if (ActiveScene == gameScene) ActiveScene = null;
        }

        /// <summary>
        /// Activate scene
        /// </summary>
        public static bool SetActiveScene(string name)
        {
            _newActiveScene = GameScenes.FirstOrDefault(scene => scene.SceneName.Equals(name));
            return _newActiveScene != null;
        }

        //standard xna methods implementation
        public static void Initialize()
        {
            foreach (var scene in GameScenes)
                scene.Initialize();
        }

        public static void LoadContent(ContentManager contentManager)
        {
            foreach (var scene in GameScenes)
                scene.LoadContent(contentManager);
        }

        public static void Update(GameTime gameTime)
        {
            //active scene change
            if (_newActiveScene != null)
            {
                if (ActiveScene != null) ActiveScene.Deactivated();
                ActiveScene = _newActiveScene;
                ActiveScene.Activated();
                _newActiveScene = null;
            }

            if (ActiveScene != null)
            {
                RenderContext.GameTime = gameTime;
                RenderContext.TouchPanelState = TouchPanel.GetState();
                ActiveScene.Update(RenderContext);
            }
        }

        public static void Draw()
        {
            if (ActiveScene != null)
            {
                //2D Before 3D
                RenderContext.SpriteBatch.Begin();
                ActiveScene.Draw2D(RenderContext, false);
                RenderContext.SpriteBatch.End();

                //Draw 3D
                //Reset Renderstate
                RenderContext.GraphicsDevice.BlendState = BlendState.Opaque;
                RenderContext.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                RenderContext.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
                ActiveScene.Draw3D(RenderContext);

                //2D After 3D
                RenderContext.SpriteBatch.Begin();
                ActiveScene.Draw2D(RenderContext, true);
                RenderContext.SpriteBatch.End();
            }
        }
        #endregion
    }
}
