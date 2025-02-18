using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComboFigther.Sprite;
using ComboFigther.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ComboFigther.Characters
{
    abstract class Character
    {
        float _speed;

        protected int _life;
        protected Vector2 _velocity;
        protected AnimationManager _animationManager = new AnimationManager();
        protected Texture2D _sprite;
        bool _isDead;
        public virtual bool IsDead
        {
            get { return _isDead; }
            set
            {
                _isDead = value;
            }
        }
        public Vector2 Position = new Vector2();
        public Vector2 Velocity { get => _velocity; set => _velocity = value; }

        public Character(Vector2 position, Texture2D sprite)
        {
            Position = position;
            _sprite = sprite;
        }


        public Rectangle Rectangle { get => new Rectangle((int)Position.X, (int)Position.Y, _animationManager.currentAnimeSprite.frameWidth, _animationManager.currentAnimeSprite.frameHeight); }
        public float Speed { get => _speed; set => _speed = value; }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _animationManager.Draw(gameTime, spriteBatch, Position);

            //draw rectangle around character
            //drawForDebug.DrawRectangle(Rectangle,5,Color.Black,spriteBatch);
        }

        public void GetAttacked(int damage)
        {
            _life -= damage;
            if (_life < 0)
            {
                IsDead = true;
            }
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exceptedPosition"></param>
        /// <returns></returns>
        protected void HandleColision()
        {
            foreach (var obstacle in Manager.Obstacles)
            {

                while (Rectangle.Intersects(obstacle.Rectangle))
                {

                    // Calculate the depth of intersection on the X-axis
                    float intersectionDepthX = Math.Min(Rectangle.Right, obstacle.Rectangle.Right) - Math.Max(Rectangle.Left, obstacle.Rectangle.Left);

                    // Calculate the depth of intersection on the Y-axis
                    float intersectionDepthY = Math.Min(Rectangle.Bottom, obstacle.Rectangle.Bottom) - Math.Max(Rectangle.Top, obstacle.Rectangle.Top);

                    // Determine which axis the collision is primarily on
                    if (Math.Abs(intersectionDepthX) < Math.Abs(intersectionDepthY))
                    {
                        Position.X = (Position.X > obstacle.Rectangle.X) ? obstacle.Rectangle.Right + 5 : obstacle.Rectangle.Left - Rectangle.Width;
                    }
                    else
                    {
                        Position.Y = (Position.Y < obstacle.Rectangle.Y) ? obstacle.Rectangle.Top - Rectangle.Height : obstacle.Rectangle.Bottom;
                    }
                }
            }
        }

    }
}
