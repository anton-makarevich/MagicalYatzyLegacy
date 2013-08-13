using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sanet.XNAEngine
{
    public class TouchInput:IGameObject
    {
        public event Action OnClick;
        public event Action OnEnter;
        public event Action OnLeave;

        private bool _isPressed;
        private int _touchId;
        public bool IsPressed { get { return _isPressed; } }

        public Vector2 ClickPosition { get; set; }

        public virtual bool CanDraw { get; set; }

        public void Update(RenderContext renderContext)
        {
            
            var touchStates = renderContext.TouchPanelState;
            //if were touch inputs
            if (touchStates.Count > 0)
            {
                if (!_isPressed)
                {

                    foreach (var touchLoc in touchStates)
                    {
                        if (touchLoc.State == TouchLocationState.Pressed )
                        {
                            _isPressed = true;
                            _touchId = touchLoc.Id;

                            //Entered
                            if (OnEnter != null) OnEnter();

                            break;
                        }

                    }


                }
                else
                {
                    var touchLoc = touchStates.FirstOrDefault(tLocation => tLocation.Id == _touchId);

                    if (touchLoc == null || touchLoc.State == TouchLocationState.Invalid )
                    {
                        _touchId = -1;
                        _isPressed = false;

                        //Left
                        if (OnLeave != null) OnLeave();
                    }
                    else
                    {
                        if (touchLoc.State == TouchLocationState.Released)
                        {
                            _touchId = -1;
                            _isPressed = false;

                            ClickPosition = touchLoc.Position;
                            //Clicked
                            if (OnClick != null) OnClick();
                        }

                    }
                }
            }
            //no touches - let's check mouse
            else
            {
                if (!_isPressed)
                {
                    //mouse support for Windows 8
                    var mouse = Mouse.GetState();
                    if (mouse.LeftButton == ButtonState.Pressed)
                    {
                        _isPressed = true;
                            //Entered
                            if (OnEnter != null) OnEnter();
                        
                    }
                }
                else
                {
                    //mouse support for Windows 8
                    var mouse = Mouse.GetState();
                    if (mouse.LeftButton == ButtonState.Released)
                        {
                            _isPressed = false;
                            //Clicked
                            ClickPosition = new Vector2(mouse.X, mouse.Y);
                            if (OnClick != null) OnClick();
                        }
                    
                }
            }
        }
        public void Initialize() { }
        public void LoadContent(ContentManager contentManager) { }
        public void Draw(RenderContext renderContext) { }
    }
}
