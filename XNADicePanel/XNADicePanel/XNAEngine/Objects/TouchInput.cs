using Microsoft.Xna.Framework.Content;
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

        public void Update(RenderContext renderContext)
        {
            var touchStates = renderContext.TouchPanelState;
            if (!_isPressed)
            {
                
                foreach (var touchLoc in touchStates)
                {
                    if (touchLoc.State == TouchLocationState.Pressed)
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

                if (touchLoc == null  ||touchLoc.State== TouchLocationState.Invalid)
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

                        //Clicked
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
