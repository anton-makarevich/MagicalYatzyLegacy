using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sanet.XNAEngine
{
    public class GameButton : GameSprite
    {
        public event Action OnClick;
        public event Action OnEnter;
        public event Action OnLeave;

        public bool IsPressed { get { return _isPressed; } }

        public string Action { get; set; }

        private bool _isSpriteSheet;
        private Rectangle? _normalRect, _pressedRect;
        private bool _isPressed;
        private int _touchId;

        public GameButton(string assetFile) :
            this(assetFile, false) { }


        public GameButton(string assetFile, bool isSpriteSheet) :
            base(assetFile)
        {
            _isSpriteSheet = isSpriteSheet;
        }

        
        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);

            //Set Dimensions after the button texture is loaded, otherwise we can't extract the width and height
            if (_isSpriteSheet)
            {
                CreateBoundingRect((int)Width/2, (int)Height);
                _normalRect = new Rectangle(0, 0, (int)(Width/2f), (int)(Height));
                _pressedRect = new Rectangle( (int)(Width / 2f),0, (int)(Width / 2f), (int)(Height));
            }
            else CreateBoundingRect((int)Width, (int)Height);
        }

        public override void Update(RenderContext renderContext)
        {
            base.Update(renderContext);

            if (!CanDraw)
                return;

            var touchStates = renderContext.TouchPanelState;
            if (!_isPressed)
            {
                DrawRect = _normalRect;

                foreach (var touchLoc in touchStates)
                {
                    if (touchLoc.State == TouchLocationState.Pressed && HitTest(touchLoc.Position, false))
                    {
                        _isPressed = true;
                        _touchId = touchLoc.Id;

                        //Entered
                        if (OnEnter != null) OnEnter();
                        DrawRect = _pressedRect;
                        break;
                    }
                }
            }
            else
            {
                var touchLoc = touchStates.FirstOrDefault(tLocation => tLocation.Id == _touchId);

                if (touchLoc == null || !HitTest(touchLoc.Position, false))
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
    }
}
