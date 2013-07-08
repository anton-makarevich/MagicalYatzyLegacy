using System;
namespace Sanet.XNAEngine
{
    public interface IGameObject
    {
        
        void Draw(RenderContext renderContext);
        void Initialize();
        void LoadContent(global::Microsoft.Xna.Framework.Content.ContentManager contentManager);
        void Update(RenderContext renderContext);
        bool CanDraw { get; set; }
    }
}
