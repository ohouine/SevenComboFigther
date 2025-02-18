using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.ActiveDirectory;
using ComboFigther.Characters;
using ComboFigther.Utility;

namespace ComboFigther.Decor
{
    internal class Obstacle
    {

        Texture2D _image;
        Microsoft.Xna.Framework.Vector2 _position;
        Microsoft.Xna.Framework.Vector2 _size;
        public Rectangle Rectangle { get => new Rectangle((int)_position.X, (int)_position.Y, (int)_size.X, (int)_size.Y); }

        public Obstacle(Texture2D image, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Vector2 size)
        {
            _image = image;
            _position = position;
            _size = size;
            Manager.Obstacles.Add(this);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_image, _position, Color.White);
        }

    }
}
