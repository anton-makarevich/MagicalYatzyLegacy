using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sanet.XNAEngine
{
    static class Extensions
    {
        public static Rectangle Update(this Rectangle rectangle, Matrix transform, Vector2 deviceScale)
        {
            var corners = new Vector2[] 
            {
                new Vector2(rectangle.Left,rectangle.Top),
                new Vector2(rectangle.Right,rectangle.Bottom),
                new Vector2(rectangle.Left,rectangle.Bottom),
                new Vector2(rectangle.Right,rectangle.Top)
            };
            var transformedCorners = new Vector2[corners.Length];
            Vector2.Transform(corners, ref transform, transformedCorners);

            var newMin = new Vector3(float.MaxValue);
            var newMax = new Vector3(float.MinValue);

            foreach (var corner in transformedCorners)
            {
                newMin.X = Math.Min(newMin.X, corner.X);
                newMin.Y = Math.Min(newMin.Y, corner.Y);

                newMax.X = Math.Max(newMax.X, corner.X);
                newMax.Y = Math.Max(newMax.Y, corner.Y);
            }

            int width = (int)(newMax.X - newMin.X);
            int height = (int)(newMax.Y - newMin.Y);
            return new Rectangle(
                (int)(newMin.X * deviceScale.X),
                (int)(newMin.Y * deviceScale.Y),
                (int)(width * deviceScale.X),
                (int)(height* deviceScale.Y));
        }

        public static BoundingBox Update(this BoundingBox boundingBox, Matrix transform)
        {
            var corners = boundingBox.GetCorners();
            var transformedCorners = new Vector3[corners.Length];
            Vector3.Transform(corners, ref transform, transformedCorners);

            var newMin = new Vector3(float.MaxValue);
            var newMax = new Vector3(float.MinValue);

            foreach (var corner in transformedCorners)
            {
                newMin.X = Math.Min(newMin.X, corner.X);
                newMin.Y = Math.Min(newMin.Y, corner.Y);
                newMin.Z = Math.Min(newMin.Z, corner.Z);

                newMax.X = Math.Max(newMax.X, corner.X);
                newMax.Y = Math.Max(newMax.Y, corner.Y);
                newMax.Z = Math.Max(newMax.Z, corner.Z);
            }

            return new BoundingBox(newMin, newMax);
        }

        public static Rectangle ToRect(this BoundingBox boundingBox, Matrix transform)
        {
            var corners = boundingBox.GetCorners();
            var transformedCorners = new Vector3[corners.Length];
            Vector3.Transform(corners, ref transform, transformedCorners);

            var newMin = new Vector3(float.MaxValue);
            var newMax = new Vector3(float.MinValue);

            foreach (var corner in transformedCorners)
            {
                newMin.X = Math.Min(newMin.X, corner.X);
                newMin.Y = Math.Min(newMin.Y, corner.Y);
                newMin.Z = Math.Min(newMin.Z, corner.Z);

                newMax.X = Math.Max(newMax.X, corner.X);
                newMax.Y = Math.Max(newMax.Y, corner.Y);
                newMax.Z = Math.Max(newMax.Z, corner.Z);
            }

            return new Rectangle((int)newMin.X, (int)newMin.Y, (int)(newMax.X - newMin.X), (int)(newMax.Y - newMin.Y));
        }

        #region DrawBoundingCode
        private static Texture2D _pixel;
        private static BasicEffect _basicEffect;

        public static void LoadContent(ContentManager contentManager)
        {
            _pixel = contentManager.Load<Texture2D>("WhitePixel");
        }

        public static Point ToPoint(this Vector2 vector)
        {
            return new Point((int)vector.X, (int)vector.Y);
        }

        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color)
        {
            var distance = Vector2.Distance(point1, point2);
            var angle = Helpers.GetAngle(point1, point2);

            spriteBatch.Draw(_pixel, point1, null, color, angle, Vector2.Zero, new Vector2(distance, 1), SpriteEffects.None, 1.0f);
        }

        public static void DrawPolygon(this SpriteBatch spriteBatch, List<Vector2> corners, Color color)
        {
            for (int i = 0; i < corners.Count-1; i++)
            {
                spriteBatch.DrawLine(corners[i], corners[i + 1], color);
            }
            spriteBatch.DrawLine(corners[corners.Count-1], corners[0], color);
        }

        public static void Draw(this Rectangle rectangle, RenderContext renderContext, Color color)
        {
            renderContext.SpriteBatch.DrawLine(new Vector2(rectangle.Left, rectangle.Top), new Vector2(rectangle.Right, rectangle.Top), color);
            renderContext.SpriteBatch.DrawLine(new Vector2(rectangle.Left, rectangle.Top), new Vector2(rectangle.Left, rectangle.Bottom), color);
            renderContext.SpriteBatch.DrawLine(new Vector2(rectangle.Left, rectangle.Bottom), new Vector2(rectangle.Right, rectangle.Bottom), color);
            renderContext.SpriteBatch.DrawLine(new Vector2(rectangle.Right, rectangle.Bottom), new Vector2(rectangle.Right, rectangle.Top), color);
        }

        public static void Draw(this BoundingBox boundingBox, RenderContext renderContext, Color color)
        {
            if (_basicEffect == null)
                _basicEffect = new BasicEffect(renderContext.GraphicsDevice);

            var lineList = new VertexPositionColor[8];
            var lineListIndices = new short[24];

            var index = 0;
            var min = boundingBox.Min;
            var max = boundingBox.Max;

            var boundingCorners = boundingBox.GetCorners();

            for (var i = 0; i < 8; ++i)
                lineList[i] = new VertexPositionColor(boundingCorners[i], color);

            index = 0;
            lineListIndices[index] = 0;
            lineListIndices[++index] = 1;
            lineListIndices[++index] = 1;
            lineListIndices[++index] = 2;
            lineListIndices[++index] = 2;
            lineListIndices[++index] = 3;
            lineListIndices[++index] = 3;
            lineListIndices[++index] = 0;

            lineListIndices[++index] = 4;
            lineListIndices[++index] = 5;
            lineListIndices[++index] = 5;
            lineListIndices[++index] = 6;
            lineListIndices[++index] = 6;
            lineListIndices[++index] = 7;
            lineListIndices[++index] = 7;
            lineListIndices[++index] = 4;

            lineListIndices[++index] = 0;
            lineListIndices[++index] = 4;
            lineListIndices[++index] = 1;
            lineListIndices[++index] = 5;
            lineListIndices[++index] = 2;
            lineListIndices[++index] = 6;
            lineListIndices[++index] = 3;
            lineListIndices[++index] = 7;

            _basicEffect.Projection = renderContext.Camera.Projection;
            _basicEffect.View = renderContext.Camera.View;
            _basicEffect.DiffuseColor = color.ToVector3();

            foreach (var pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                renderContext.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList, lineList, 0, lineList.Length, lineListIndices, 0, 12);
            }
        }
        #endregion
        /// <summary>
        /// Rotates vector to specified angle
        /// NOTE: Maybe more correct to implement thoroug rotation matrix due to performance, 
        /// but I had problems with rotation direction determination (cw - ccw)
        /// </summary>
        /// <param name="vector">vector to rotate</param>
        /// <param name="pivotPoint">point to rotate around</param>
        /// <param name="angleRadians">rotation angle in radians</param>
        /// <returns></returns>
        public static Vector2 Rotate(this Vector2 vector, Vector2 pivotPoint, float angleRadians)
        {
            return new Vector2(
                (float)(pivotPoint.X + Math.Cos(angleRadians) * (vector.X - pivotPoint.X)
                    - Math.Sin(angleRadians) * (vector.Y - pivotPoint.Y)),
                (float)(pivotPoint.Y + Math.Sin(angleRadians) * (vector.X - pivotPoint.X)
                    + Math.Cos(angleRadians) * (vector.Y - pivotPoint.Y)));
            
        }
        /// <summary>
        /// Vector2.Rotate with precalculated values for rotation 90 dg 
        /// </summary>
        public static Vector2 Rotate90(this Vector2 vector,bool iscw)
        {
            if (iscw)
                return new Vector2(-vector.Y ,vector.X );
            else
                return new Vector2(vector.Y ,-vector.X );

        }

        
    }
}
