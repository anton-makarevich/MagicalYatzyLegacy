using Sanet.XNAEngine.Animations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Sanet.XNAEngine
{
    //Represent a screen inside of game, i.e. MainMenu or game level
    public class GameScene
    {
        GameSprite _background;
        
       

        public GameScene(string name)
        {
            SceneName = name;
            SceneObjects2D = new List<GameObject2D>();
            SceneObjects3D = new List<GameObject3D>();
            OtherSceneObjects = new List<IGameObject>();
        }

        #region Properties
        //Unique scene name to access it
        public string SceneName { get; private set; }
        //List of 3d objects in scene
        public List<GameObject2D> SceneObjects2D { get; private set; }
        //List of 2d objects in scene
        public List<GameObject3D> SceneObjects3D { get; private set; }
        //List of 'other' objects like text printers, touch input trackers etc
        public List<IGameObject> OtherSceneObjects { get; private set; }

        
        public XDocument SceneData { get; set; }
        
        #endregion

        #region Methods
        //scene managment methods
        public void AddSceneObject(GameObject2D sceneObject)
        {
            AddSceneObject(sceneObject,false);
        }
        public void AddSceneObject(GameObject2D sceneObject,bool insert)
        {
            if (!SceneObjects2D.Contains(sceneObject))
            {
                sceneObject.Scene = this;
                if (insert)
                {
                    sceneObject.Z = 0;
                }
                else
                {
                    if (sceneObject.Z == 0)
                        sceneObject.Z = SceneObjects2D.Count+1;
                    
                }
                SceneObjects2D.Add(sceneObject);
                SceneObjects2D = SceneObjects2D.OrderBy(f => f.Z).ToList();
            }
        }
        public void RemoveSceneObject(GameObject2D sceneObject)
        {
            if (SceneObjects2D.Remove(sceneObject))
            {
                sceneObject.Scene = null;
            }
        }
        public void AddSceneObject(GameObject3D sceneObject)
        {
            if (!SceneObjects3D.Contains(sceneObject))
            {
                sceneObject.Scene = this;
                SceneObjects3D.Add(sceneObject);
            }
        }
        public void RemoveSceneObject(GameObject3D sceneObject)
        {
            if (SceneObjects3D.Remove(sceneObject))
            {
                sceneObject.Scene = null;
            }
        }
        public void AddSceneObject(IGameObject sceneObject)
        {
            if (!OtherSceneObjects.Contains(sceneObject))
            {
                
                OtherSceneObjects.Add(sceneObject);
                
            }
        }

        public List<GameButton> Buttons
        {
            get
            {
                if (SceneObjects2D != null)
                {
                    return SceneObjects2D.Where(f => f is GameButton).Cast<GameButton>().ToList();
                }
                return new List<GameButton>();
            }
        }

        //standard xna methods
        public virtual void Initialize()
        {
            //load background
            if (SceneData != null)
            {
                var backgroundElement = SceneData.Descendants("Background").FirstOrDefault();
                if (backgroundElement != null)
                {
                    _background = new GameSprite(backgroundElement.Attribute("Content").Value);
                    _background.DrawInFrontOf3D = false;
                    AddSceneObject(_background, true);
                }

                //Load any types of static sprites
                var spritesList = SceneData.Descendants("Sprite").ToList();
                if (spritesList != null)
                    foreach (var query in spritesList)
                    {
                        GameSprite sprite = new GameSprite(query.Attribute("Content").Value);
                        sprite.Translate(float.Parse(query.Attribute("X").Value), float.Parse(query.Attribute("Y").Value));//
                        sprite.Scale(new Vector2(float.Parse(query.Attribute("ScaleX").Value, CultureInfo.InvariantCulture), float.Parse(query.Attribute("ScaleY").Value, CultureInfo.InvariantCulture)));
                        sprite.Z = int.Parse(query.Attribute("Z").Value);
                        sprite.PivotPoint = new Vector2(0, 0);
                        sprite.DrawInFrontOf3D = query.Attribute("IsInFront").Value.ToLower() == "true";
                        //look for FrameAnimation
                        var frameAnimation = query.Elements("FrameAnimation").FirstOrDefault();
                        if (frameAnimation != null)
                        {
                            sprite.FrameAnimation = new FrameAnimation(frameAnimation);
                            sprite.FrameAnimation.PlayAnimation();
                        }

                        var pathAnimation = query.Elements("PathAnimation").FirstOrDefault();
                        if (pathAnimation != null)
                        {
                            sprite.PathAnimation = new PathAnimation(pathAnimation);
                            sprite.PathAnimation.PlayAnimation();
                        }

                        AddSceneObject(sprite);
                    }
            }
            
            foreach (var sceneObject in SceneObjects2D)
                sceneObject.Initialize();

            foreach (var sceneObject in SceneObjects3D)
                sceneObject.Initialize();
        }

        protected GameButton GetButton(XElement xmldata)
        {
            GameButton newButton = new GameButton(xmldata.Attribute("Content").Value, xmldata.Attribute("IsSpriteSheet").Value.ToLower() == "true");
            newButton.Translate(float.Parse(xmldata.Attribute("X").Value), float.Parse(xmldata.Attribute("Y").Value));//
            newButton.Scale(float.Parse(xmldata.Attribute("Scale").Value,CultureInfo.InvariantCulture));
            newButton.Action = xmldata.Attribute("Action").Value;
            newButton.PivotPoint = new Vector2(0, 0);
            return newButton;
        }

        protected GameButton GetButton(string action)
        {
            return Buttons.FirstOrDefault(f => f.Action ==action);
        }


        public virtual void LoadContent(ContentManager contentManager)
        {
            foreach (var sceneObject in SceneObjects2D)
                sceneObject.LoadContent(contentManager);

            foreach (var sceneObject in SceneObjects3D)
                sceneObject.LoadContent(contentManager);

            foreach (var sceneObject in OtherSceneObjects)
                sceneObject.LoadContent(contentManager);
        }

        public virtual void Update(RenderContext renderContext)
        {
            foreach (var sceneObject in SceneObjects2D)
                sceneObject.Update(renderContext);
            
            foreach (var sceneObject in SceneObjects3D)
                sceneObject.Update(renderContext);

            foreach (var sceneObject in OtherSceneObjects)
                sceneObject.Update(renderContext);
        }
        
        /// <summary>
        /// Draw 2d objects of the scene which should be drawn before/after 3d object
        /// according to appropriate var value
        /// </summary>
        public virtual void Draw2D(RenderContext renderContext, bool drawInFrontOf3D)
        {
            
            foreach (var obj in SceneObjects2D)
            {
                if (obj.DrawInFrontOf3D == drawInFrontOf3D)
                    obj.Draw(renderContext);
            }

            foreach (var obj in OtherSceneObjects)
            {
                if (drawInFrontOf3D)
                    obj.Draw(renderContext);
            }
           
        }

        /// <summary>
        /// Draw all 3d objects of the scene
        /// </summary>
        /// <param name="renderContext"></param>
        public virtual void Draw3D(RenderContext renderContext)
        {
            foreach (var sceneObject in SceneObjects3D)
                sceneObject.Draw(renderContext);

        }

        public virtual void Activated() { }
        public virtual void Deactivated() { }
#endregion
    }
}
