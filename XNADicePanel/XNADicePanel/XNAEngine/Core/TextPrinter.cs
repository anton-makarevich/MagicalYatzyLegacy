using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sanet.XNAEngine
{
    public class TextPrinter:IGameObject
    {
        SpriteFont _spriteFont;
        string _spriteFontName;

        /// <summary>
        /// Text to print
        /// </summary>
        public string Text { get; set; }
        
        /// <summary>
        /// Position where to print
        /// </summary>  
        private Vector2 _Position= new Vector2(5,5);
        public Vector2 Position
        {
            get { return _Position; }
            set
            {
                if (_Position != value)
                {
                    _Position = value;
                   
                }
            }
        }

        /// <summary>
        /// Text color
        /// </summary>
        private Color _TextColor=Color.White;
        public Color TextColor
        {
            get { return _TextColor; }
            set
            {
                if (_TextColor != value)
                {
                    _TextColor = value;
                    
                }
            }
        }   

        /// <summary>
        /// TextRotation
        /// </summary>
        private float _Rotation=0.0f;
        public float Rotation
        {
            get { return _Rotation; }
            set
            {
                if (_Rotation != value)
                {
                    _Rotation = value;
                    
                }
            }
        }   
          
        /// <summary>
        /// Text scale
        /// </summary>
        private float _Scale=1.0f;
        public float Scale
        {
            get { return _Scale; }
            set
            {
                if (_Scale != value)
                {
                    _Scale = value;
                }
            }
        }


        public TextPrinter(string spriteFontName)
        {
            _spriteFontName = spriteFontName;
        }

        public virtual void LoadContent(ContentManager contentManager)
        {
            _spriteFont=contentManager.Load<SpriteFont>(_spriteFontName);
        }

        public virtual void Draw(RenderContext renderContext)
        {
            if (string.IsNullOrEmpty(Text))
                return;
            renderContext.SpriteBatch.DrawString(
                _spriteFont,
                Text,
                (Position*renderContext.DeviceScale),
                TextColor,
                Rotation,
                new Vector2(0, 0),
                (Scale*renderContext.DeviceScale),
                SpriteEffects.None,
                1.0f);
        }

        public void Update(RenderContext renderContext)
        {
           
        }
        public void Initialize() { }
    }
}
