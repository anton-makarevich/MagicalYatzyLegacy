using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sanet.XNAEngine
{
    /// <summary>
    /// The base class for all plain things
    /// </summary>
    public class GameObject2D:IGameObject
    {
        public GameObject2D()
        {
            LocalScale = WorldScale = Vector2.One;
            Children = new List<GameObject2D>();
            CanDraw = true;
            DrawInFrontOf3D = true;
        }

        #region Properties
        //Local positon - related to this object itself
        public Vector2 LocalPosition { get; set; }
        //position related to parent object
        public Vector2 WorldPosition { get; private set; }

        //the same concept for scaling
        public Vector2 LocalScale { get; set; }
        public Vector2 WorldScale { get; private set; }

        //and rotation
        public float LocalRotation { get; set; }
        public float WorldRotation { get; private set; }

        //'central' point to rotate around/translate
        public Vector2 PivotPoint { get; set; }

        //World
        protected Matrix WorldMatrix;

        //Parent object
        public GameObject2D Parent { get; set; }

        //Children collection
        public List<GameObject2D> Children { get; private set; }

        private Rectangle? _relativeBoundingRect;
        public Rectangle? BoundingRect { get; protected set; }

        private bool _drawBoundingRect;
        public bool DrawBoundingRect
        {
            get
            {
                if (Parent != null)
                    return Parent.DrawBoundingRect || _drawBoundingRect;

                return _drawBoundingRect;
            }
            set
            {
                _drawBoundingRect = value;
            }
        }

        public int ID { get; set; }

        public int Z { get; set; }

        public GameObject2D this[int childIndex]
        {
            get
            {
                if (childIndex >= 0 && childIndex < Children.Count)
                    return Children[childIndex];

                return null;
            }
        }

        //when to draw this sprite?
        //for example background should be drawn before 3D and buttons after
        public bool DrawInFrontOf3D { get; set; }

        //equal to 'isvisible'
        public virtual bool CanDraw { get; set; }

        //represents current object scene
        private GameScene _scene;
        public GameScene Scene
        {
            get
            {
                if (_scene != null) return _scene;
                if (Parent != null) return Parent.Scene;
                return null;
            }

            set { _scene = value; }
        }

        #endregion

        #region Methods
        //children methods
        public void AddChild(GameObject2D child)
        {
            if (!Children.Contains(child))
            {
                Children.Add(child);
                child.Parent = this;
            }
        }

        public void RemoveChild(GameObject2D child)
        {
            if (Children.Remove(child))
                child.Parent = null;
        }

        
        public void Rotate(float rotation)
        {
            LocalRotation = rotation;
        }

        public void Translate(float posX, float posY)
        {
            Translate(new Vector2(posX, posY));
        }

        public void Translate(Vector2 position)
        {
            LocalPosition = position;
        }

        public void Scale(float scale)
        {
            Scale(new Vector2(scale, scale));
        }

        public void Scale(float scaleX, float scaleY)
        {
            Scale(new Vector2(scaleX, scaleY));
        }

        public void Scale(Vector2 scale)
        {
            LocalScale = scale;
        }

        public void CreateBoundingRect(int width, int height, Vector2 offset)
        {
            _relativeBoundingRect = new Rectangle(0, 0, width + (int)offset.X, height + (int)offset.Y);
            BoundingRect = _relativeBoundingRect;
        }

        public void CreateBoundingRect(int width, int height)
        {
            CreateBoundingRect(width, height, Vector2.Zero);
        }

        public bool HitTest(GameObject2D gameObj)
        {
            if (!gameObj.BoundingRect.HasValue) return false;
            if (BoundingRect.HasValue && BoundingRect.Value.Intersects(gameObj.BoundingRect.Value)) return true;

            return Children.FirstOrDefault(child => child.HitTest(gameObj)) != null;
        }

        public bool HitTest(Vector2 position, bool includechildren)
        {
            if (BoundingRect.HasValue &&
                BoundingRect.Value.Contains(
                (int)position.X, (int)position.Y)
                ) return true;

            if (includechildren)
                return Children.FirstOrDefault(
                    child => child.HitTest(position, includechildren))
                    != null;
            return false;
        }

        public bool HitTest(Vector2 position)
        {
            return HitTest(position, true);
        }

        public virtual void Initialize()
        {
            Children.ForEach(child => child.Initialize());
        }

        public virtual void LoadContent(ContentManager contentManager)
        {
            Children.ForEach(child => child.LoadContent(contentManager));
        }

        public virtual void Update(RenderContext renderContext)
        {
            WorldMatrix =
                Matrix.CreateTranslation(new Vector3(-PivotPoint, 0)) *
                Matrix.CreateScale(new Vector3(LocalScale, 1)) *
                Matrix.CreateRotationZ(MathHelper.ToRadians(LocalRotation)) *
                Matrix.CreateTranslation(new Vector3(LocalPosition, 0));

            if (Parent != null)
            {
                WorldMatrix = Matrix.Multiply(WorldMatrix, Matrix.CreateTranslation(new Vector3(Parent.PivotPoint, 0)));
                WorldMatrix = Matrix.Multiply(WorldMatrix, Parent.WorldMatrix);
            }

            Vector3 pos, scale;
            Quaternion rot;
            var decomposite = WorldMatrix.Decompose(out scale, out rot, out pos);
                
            var direction = Vector2.Transform(Vector2.UnitX, rot);
            WorldRotation = (float)Math.Atan2(direction.Y, direction.X);
            WorldRotation = float.IsNaN(WorldRotation) ? 0 : MathHelper.ToDegrees(WorldRotation);

            
            WorldPosition = new Vector2(pos.X, pos.Y) * renderContext.DeviceScale;
            WorldScale = new Vector2(scale.X, scale.Y) * renderContext.DeviceScale;

            Children.ForEach(child => child.Update(renderContext));

            //calculate bounding rect for correct positioning
            if (_relativeBoundingRect.HasValue)
            {
                BoundingRect = _relativeBoundingRect.Value.Update(WorldMatrix, renderContext.DeviceScale);
                
            }
        }

        public virtual void Draw(RenderContext renderContext)
        {
            if (CanDraw)
            {
                Children.ForEach(child => { if (child.CanDraw) child.Draw(renderContext); });

                if (SceneManager.IsDebug && BoundingRect.HasValue)
                    BoundingRect.Value.Draw(renderContext, Color.Blue);
            }

            
        }
#endregion
    }
}
