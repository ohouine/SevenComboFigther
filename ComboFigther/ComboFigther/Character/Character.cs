using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComboFigther.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ComboFigther.Character
{
    internal class Character
    {
        protected int _life;
        protected float _speed;

        public bool isDead;

        public Vector2 position = new Vector2();
        protected Vector2 _velocity;
        protected AnimationManager _animationManager = new AnimationManager();
        protected Texture2D _sprite;

        public Character(int life, float speed, Texture2D sprite)
        {
            _life = life;
            _speed = speed;
            _sprite = sprite;
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _animationManager.Draw(gameTime, spriteBatch, position);

            //drawForDebug.DrawRectangle(rectangle,5,Color.Black,spriteBatch);
        }

        public Rectangle rectangle { get => new Rectangle((int)position.X, (int)position.Y, _animationManager.currentAnimeSprite.frameWidth, _animationManager.currentAnimeSprite.frameHeight); }
    }
}
