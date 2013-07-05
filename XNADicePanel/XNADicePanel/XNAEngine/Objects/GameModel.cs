using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sanet.XNAEngine
{
    public class GameModel:GameObject3D
    {
        string _assetFile;
        Model _model;

        public GameModel(string assetfile)
        {
            _assetFile = assetfile;
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            _model = content.Load<Model>(_assetFile);
            base.LoadContent(content);
        }

        public override void Draw(RenderContext context)
        {
            var transforms = new Matrix[_model.Bones.Count];
            _model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in _model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.View = context.Camera.View;
                    effect.Projection = context.Camera.Projection;
                    effect.World = transforms[mesh.ParentBone.Index]*WorldMatrix;
                }
                mesh.Draw();
            }
            base.Draw(context);
        }
    }
}
