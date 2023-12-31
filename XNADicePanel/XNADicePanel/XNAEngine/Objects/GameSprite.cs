using Sanet.XNAEngine.Animations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sanet.XNAEngine
{
    //represents simple 2d sprite
    public class GameSprite : GameObject2D
    {
        private readonly string _assetFile;
        private Texture2D _texture;

        public float Width { get { return _texture.Width; } }
        public float Height { get { return _texture.Height; } }

        public float Depth { get; set; }
        public Color Color { get; set; }
        public SpriteEffects Effect { get; set; }
        public Rectangle? DrawRect { get; set; }

        public FrameAnimation FrameAnimation {get;set;}
        public PathAnimation PathAnimation { get; set; }

        public GameSprite(string assetFile)
        {
            _assetFile = assetFile;
            Color = Color.White;
            Effect = SpriteEffects.None;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);
            _texture = contentManager.Load<Texture2D>(_assetFile);
        }

        public override void Update(RenderContext renderContext)
        {
            if (FrameAnimation != null)
            {
                FrameAnimation.Update(renderContext);
                DrawRect = FrameAnimation.FrameRect;
            }
            if (PathAnimation != null)
            {
                PathAnimation.Update(renderContext);
                Scale(PathAnimation.Scale);
                Rotate(PathAnimation.Rotation);
                Translate(PathAnimation.Position);
                Effect = PathAnimation.Effect;
            }
            base.Update(renderContext);
        }

        public override void Draw(RenderContext renderContext)
        {
            if (CanDraw)
            {
                renderContext.SpriteBatch.Draw(_texture, WorldPosition,
                    DrawRect, Color, MathHelper.ToRadians(WorldRotation),
                    Vector2.Zero, WorldScale, Effect, Depth);
                base.Draw(renderContext);
            }
        }
    }
}
