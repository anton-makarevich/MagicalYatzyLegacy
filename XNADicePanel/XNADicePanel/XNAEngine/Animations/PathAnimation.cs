using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Sanet.XNAEngine.Animations
{
    public class PathAnimation:AnimationBase
    {
#region Fields
        int _currentPointIndex;

        int _nextPointIndex;

        Vector2 _position;
        float _speed;
        float _rotation;
        Vector2 _scale;

        //bool _isreversing = false;
#endregion

#region Constructor
        public PathAnimation(XElement xmldata)
        {
            Points=new List<PathAnimationPoint>();
            var pointsList = xmldata.Elements("PathPoint").ToList();
            if (pointsList != null)
            {
                foreach (var point in pointsList)
                {
                    Points.Add(new PathAnimationPoint(point));
                }

            }
            _speed = Points[0].Speed;
            _rotation = Points[0].Rotation;
            _position = Points[0].Position;
            _scale = Points[0].Scale;
            _currentPointIndex = 0;
            if (Points.Count > 1)
                _nextPointIndex = 1;
            IsLooping = xmldata.Attribute("IsLooping").Value.ToLower() == "true";
        }

        public PathAnimation(Vector2 position, Vector2 scale)
        {

        }

#endregion

#region Properties
                   
        public bool IsClosed{get;set;}

        public SpriteEffects Effect { get; set; }
        //public bool IsReversed{get;set;}

        public Vector2 Position
        {
            get
            { return _position; }
        }
        public Vector2 Scale
        {
            get
            {
                return _scale;
            }
        }
        public float Rotation
        {
            get
            {
                return _rotation;
            }
        }
        
        public List<PathAnimationPoint> Points { get; set; }
#endregion
        
#region methods
        public override void Update(RenderContext renderContext)
        {
            //if only one point - nothing will ever change
            if (Points.Count == 1 || !IsPlaying)
            {
                _position = Points[0].Position;
                _scale = Points[0].Scale;
                _rotation = Points[0].Rotation;
                return;
            }

            if (IsPlaying && !IsPaused)
            {
                //if a lot of points
                var cp = Points[_currentPointIndex]; //point we are moving from
                var np = Points[_nextPointIndex];    //point we are moving to

                //if position unknown 
                if (_position == null)
                {
                    _position = cp.Position;        //set it to point we are moving from
                    _speed = cp.Speed;              //the same with speed
                    _scale = cp.Scale;
                    _rotation = cp.Rotation;
                }

                //the distance to go
                var dist = (np.Position - _position).Length();
                //distance to move on current step
                var dD = _speed * renderContext.GameTime.ElapsedGameTime.Milliseconds / 1000;

                if (dD > dist)
                    dD = dist;

                if (dD == 0)
                {
                    //we reached final point and don't looping so - go to first
                    //actually should never happen. but better to add to ve sure we are save of DevidedByZero exceptions
                    IsPlaying = false;
                    _position = Points[0].Position;
                    _scale = Points[0].Scale;
                    _rotation = Points[0].Rotation;
                    return;
                }

                //updating data
                _position.X = GetInterpolatedValue(_position.X, np.Position.X, dist, dD);
                _position.Y = GetInterpolatedValue(_position.Y, np.Position.Y, dist, dD);
                _speed = GetInterpolatedValue(_speed, np.Speed, dist, dD);
                _scale.X = GetInterpolatedValue(_scale.X, np.Scale.X, dist, dD);
                _scale.Y = GetInterpolatedValue(_scale.Y, np.Scale.Y, dist, dD);
                _rotation = GetInterpolatedValue(_rotation, np.Rotation, dist, dD);

                Effect = np.Effect;

                //Now should check if are close enough to change direction
                dist = (np.Position - _position).Length();  //distance reamaining after this step
                if (dist < dD)                                //distance we are moving in one step is longer then remaining to next point
                {
                    _currentPointIndex = _nextPointIndex;
                    if (_nextPointIndex == Points.Count - 1)      //We moved to last point
                    {
                        _position = Points[0].Position;
                        _scale = Points[0].Scale;
                        _rotation = Points[0].Rotation;
                        _currentPointIndex = 0;
                        if (Points.Count > 1)
                        _nextPointIndex = 1;

                        IsPlaying = IsLooping;
                    }
                    else
                    _nextPointIndex++;
                    
                    
                }

            }
        }

        static float GetInterpolatedValue(float value1, float value2, float dist, float distTo)
        {
            var rv= value1 + (distTo / dist) * (value2 - value1);
            return rv;
        }

#endregion
    }
}
