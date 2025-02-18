using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ComboFigther.Decor;
using ComboFigther.Sprite;
using ComboFigther.Utility;
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D9;

namespace ComboFigther.Characters
{
    class Slime : Enemy
    {

        int _attackRate = 1000;// attack each ms
        int _pathCalculatingLatency = new Random().Next(100, 200); // time before updating target posiion
        Pathfinding _pathfinding;
        int _currentLatency;
        List<Vector2> _nodes = new List<Vector2>();
        Vector2 _direction;
        private DateTime _lastCallTime = DateTime.MinValue;

        public Slime(Vector2 Position, Texture2D texture2D) : base(Position, texture2D)
        {
            _life = 100;
            Speed = 50;

            _animationManager.Add("MOVE_UP", new AnimeSprite(texture2D, 6, 150, 32, 32, 2));
            _animationManager.Add("MOVE_DOWN", new AnimeSprite(texture2D, 6, 150, 32, 32, 0));
            _animationManager.Add("MOVE_RIGHT", new AnimeSprite(texture2D, 6, 150, 32, 32, 1, 1, true));
            _animationManager.Add("MOVE_LEFT", new AnimeSprite(texture2D, 6, 150, 32, 32, 1));

            _animationManager.changeAnimation("MOVE_DOWN");

            _pathfinding = new Pathfinding(Manager.Obstacles, 32);
        }

        /// <summary>
        /// call every game Update()
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            ChasePlayer(gameTime);

            if (Manager.Player.Rectangle.Intersects(this.Rectangle))
            {
                DateTime now = DateTime.Now;
                if (_lastCallTime == DateTime.MinValue) _lastCallTime = now;

                double msElapsed = (now - _lastCallTime).TotalMilliseconds;

                if (msElapsed >= _attackRate)
                {
                    Manager.Player.GetAttacked(Damage);
                }
            }

        }

        /// <summary>
        /// move toward the closest enemy
        /// </summary>
        void ChasePlayer(GameTime gameTime)
        {
            if (_pathfinding != null)
            {
                //calcule nodes every pathCalculatingLatency
                if (_currentLatency >= _pathCalculatingLatency || _nodes.Count <= 0)
                {
                    _nodes = _pathfinding.FindPath(Position, Manager.Player.Position);
                    _currentLatency = 0;
                }
                else _currentLatency++;

                //follow target if there is no node
                _direction = (_nodes.Count > 1 && _nodes[0] != Position) ? _nodes[0] - Position : Manager.Player.Position - Position;

                // Normalize the direction (so it doesn't depend on distance)
                if (_direction != Vector2.Zero)
                {
                    _direction.Normalize();
                }

                Position += _direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_nodes.Count > 1)
                {
                    if (Vector2.Distance(Position, _nodes[0]) < Speed * gameTime.ElapsedGameTime.TotalSeconds) // Example threshold of 10 pixels
                    {
                        _nodes.RemoveAt(0);
                    }
                }

            }

        }
    }
}

