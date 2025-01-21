using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D11;

namespace ComboFigther.Sprite
{
    class AnimationManager
    {
        Dictionary<string, AnimeSprite> sprites = new Dictionary<string, AnimeSprite>();
        //set currentAnimeSprite on change
        string _currentAnimation;
        string currentAnimation { get { return _currentAnimation; } set { _currentAnimeSprite = sprites[value]; _currentAnimation = value; } }
        AnimeSprite _currentAnimeSprite;
        public AnimeSprite currentAnimeSprite { get => _currentAnimeSprite; }

        public void changeAnimation(string animation)
        {
            if (currentAnimation != null)
            {
                sprites[currentAnimation].Stop();

            }
            if (sprites.ContainsKey(animation))
            {
                currentAnimation = animation;
                sprites[currentAnimation].Reset();
                sprites[animation].Start();
            }
        }

        public void Add(string key, AnimeSprite animation) => sprites.Add(key, animation);

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position) => currentAnimeSprite.Draw(gameTime, spriteBatch, position);
    }
}
