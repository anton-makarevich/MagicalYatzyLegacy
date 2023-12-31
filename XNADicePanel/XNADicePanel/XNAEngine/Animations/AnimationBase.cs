using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sanet.XNAEngine.Animations
{
    public abstract class AnimationBase
    {
        public bool IsLooping { get; set; }
        public bool IsPlaying { get; protected set; }
        public bool IsPaused { get; protected set; }

        
        public void PlayAnimation()
        {
            if (IsPaused)
            {
                IsPaused = false;
                return;
            }

            IsPlaying = true;
            
        }

        public virtual void StopAnimation()
        {
            IsPlaying = false;
            
        }

        public void PauzeAnimation()
        {
            IsPaused = true;
        }

        public abstract void Update(RenderContext renderContext);
        
    }
}
