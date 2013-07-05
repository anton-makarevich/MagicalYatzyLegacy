using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sanet.XNAEngine
{
    public class RenderContext
    {
        public SpriteBatch SpriteBatch { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }
        public GameTime GameTime { get; set; }
        public TouchCollection TouchPanelState { get; set; }
        public BaseCamera Camera { get; set; }

        public Vector2 DeviceScale
        {
            get
            {
                //for landscape
                return new Vector2((float)GraphicsDevice.Viewport.Height / 768.0f, (float)GraphicsDevice.Viewport.Width / 1280.0f);
            }
        }
    }
}
