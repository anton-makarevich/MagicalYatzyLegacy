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
using Sanet.Models;


namespace Sanet.Kniffel.Xna
{
   public class DiceSelectorScene :GameScene
   {
       //touch imput to support clicks on dice
       TouchInput _touchInput;

       //text  caption to ask user to select dice
       TextPrinter _captionText;


       public DiceSelectorScene(string sceneName) : base(sceneName) { }

       public List<Die> aDice = new List<Die>();

       Rectangle _Margin;
       public Rectangle Margin
       {
           get { return _Margin; }
           set
           {
               _Margin = value;
               FindDicePosition();
           }

       }

       public int Width { get; set; }
       public int Height { get; set; }

       public void FindDicePosition()
       {
            if (Height + Width == 0)
               return;

           int i = 0;
           

           var y = (Height - Margin.Height - Margin.Top) / 2 + Margin.Top-36;
           var x = (Width - Margin.Left - Margin.Width) / 2 + Margin.Left - 216;
           
           while (i < 6)
           {
               var d = aDice[i];
               var x1=x + i * 72;
               d.Result = i + 1;
               d.SetPosition(x1, y);

               

               i++;

           }

       }

       Die _lastClickedDie;

       public Die LastClickedDice
       {
           get
           {
               return _lastClickedDie;
           }
           set
           {
               _lastClickedDie = null;
           }
       }

       DicePanelScene DPanel
       {
           get
           {
               return (DicePanelScene)SceneManager.GameScenes.FirstOrDefault(f => f is DicePanelScene);
           }
       }

       public void DieClicked()
       {
           Point pointClicked = new Point((int)_touchInput.ClickPosition.X, (int)_touchInput.ClickPosition.Y);
           //determine if die was clicked
           OnClick(pointClicked);
       }

       void OnClick(Point pointClicked)
       {
           _lastClickedDie = null;
           foreach (Die d in aDice)
           {
               if (d.ClickedOn(pointClicked.X, pointClicked.Y))
               {
                   _lastClickedDie = d;
                   SceneManager.SetActiveScene("DicePanelScene");
                   return;
               }
           }
           //no die was clicked
           
           
       }
       
       #region XNA Things
        public override void Initialize()
        {
             //prepare caption
            //TODO: make FontSprite for this
            _captionText = new TextPrinter("Fonts/DefFont");
            _captionText.Position = new Vector2(320, 30);
            _captionText.CanDraw = true;
#if NETFX_CORE
            _captionText.Text = "SelectNewDiceValueMessage".Localize();
#endif
            AddSceneObject(_captionText);


            _touchInput = new TouchInput();
            _touchInput.OnClick += DieClicked;
            AddSceneObject(_touchInput);

            
            aDice = new List<Die>();
            for (int i = 0; i < 6; i++)
            {
                var dp = (DicePanelScene)SceneManager.GameScenes.FirstOrDefault(f => f is DicePanelScene);
                var d = new Die(dp);
                d.Initialize();
                aDice.Add(d);
            }
             

            base.Initialize();

        }

        int _sinceLastClick = 0;

        public override void Update(RenderContext renderContext)
        {
            _sinceLastClick += renderContext.GameTime.ElapsedGameTime.Milliseconds;

            _captionText.Position = new Vector2(Margin.Left+10, Margin.Top+10);
            //if (renderContext.GameTime.IsRunningSlowly)
            //    return;

            var vpw= renderContext.GraphicsDevice.Viewport.Width;
            var vph = renderContext.GraphicsDevice.Viewport.Height;

                if (vph+vpw==0)
                    return;

            if (Width != vpw ||
                Height !=vph )
            {
                Width = renderContext.GraphicsDevice.Viewport.Width;
                Height = renderContext.GraphicsDevice.Viewport.Height;
                if (aDice.Count == 6)
                    FindDicePosition();
                else
                    return;
            }
            var dp = (DicePanelScene)SceneManager.GameScenes.FirstOrDefault(f => f is DicePanelScene);
            foreach (var d in aDice)
            {
                d.Style = dp.PanelStyle;
                d.Update(renderContext);
            }
            //}
            var mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed && _sinceLastClick>200)
            {
                Point pointClicked = new Point(mouse.X, mouse.Y);
                //determine if die was clicked
                OnClick(pointClicked);
                _sinceLastClick = 0;
            }
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
