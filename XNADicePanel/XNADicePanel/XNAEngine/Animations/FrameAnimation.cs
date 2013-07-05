using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;


namespace Sanet.XNAEngine.Animations
{
    public class FrameAnimation:AnimationBase
    {
#region Fields
        private readonly int _rowCount;
        private readonly int _columnCount;
        private int _totalFrameTime;
        private Rectangle _frameRect;
#endregion

#region Properties

        public int NumFrames { get; private set; }
        public Vector2 FrameSize { get; private set; }
        public int CurrentFrame { get; private set; }
        


        public int FrameInterval { get; set; }
        

        public Rectangle FrameRect
        {
            get
            { return _frameRect; }
        }
#endregion

#region Constructor
        public FrameAnimation(XElement xmldata)
            :this(
                int.Parse(xmldata.Attribute("Frames").Value),
                int.Parse(xmldata.Attribute("FrameInterval").Value),
                new Vector2( int.Parse(xmldata.Attribute("FrameWidth").Value),int.Parse(xmldata.Attribute("FrameHeight").Value)),
                int.Parse(xmldata.Attribute("FramesInRow").Value))
        
        {
           IsLooping=xmldata.Attribute("IsLooping").Value.ToLower() == "true";

        }

        public FrameAnimation(int numFrames, int frameInterval, Vector2 frameSize) :
            this( numFrames, frameInterval, frameSize, numFrames) { }

        public FrameAnimation(int numFrames, int frameInterval, Vector2 frameSize, int framesPerRow) 
        {
            NumFrames = numFrames;
            FrameInterval = frameInterval;
            FrameSize = frameSize;

            _frameRect = new Rectangle(0, 0, (int)frameSize.X, (int)frameSize.Y);
            _rowCount = 1;
            _columnCount = numFrames;

            if (framesPerRow < numFrames)
            {
                _rowCount = numFrames / framesPerRow;
                _columnCount = framesPerRow;
            }
            IsLooping = true;       
        }
#endregion

#region Methods

        public override void StopAnimation()
        {
            base.StopAnimation();
            CurrentFrame = 0;
            _totalFrameTime = 0;
        }

        public override void Update(RenderContext renderContext)
        {
            if (IsPlaying && !IsPaused)
            {
                _totalFrameTime += renderContext.GameTime.ElapsedGameTime.Milliseconds;

                if (_totalFrameTime >= FrameInterval)
                {
                    _totalFrameTime = 0;

                    if (_rowCount > 1)
                    {
                        _frameRect.Location = new Point(
                            (int)FrameSize.X *
                                    (CurrentFrame % _columnCount),

                                  (int)FrameSize.Y * (int)Math.Floor((float)
                                (CurrentFrame / _columnCount)
                                )
                            );
                    }
                    else _frameRect.Location = new Point(
                        (int)FrameSize.X * CurrentFrame, 0
                        );
                                        
                    ++CurrentFrame;

                    if (CurrentFrame >= NumFrames)
                    {
                        CurrentFrame = 0;
                        IsPlaying = IsLooping;
                    }
                }
            }
            
        }
#endregion
    }
}
