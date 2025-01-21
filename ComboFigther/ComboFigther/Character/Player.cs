using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using ComboFigther.Sprite;
using SharpDX.Direct3D11;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;

namespace ComboFigther.Character
{
    internal class Player : Character
    {
        string _lastMovement = "NONE";
        string _currentMovement;

        Dictionary<string, int> _controlls = new Dictionary<string, int>()
        {
            { NameMapping.MOVE_UP , (int)Keys.Up },
            { NameMapping.MOVE_DOWN , (int)Keys.Down },
            { NameMapping.MOVE_RIGHT , (int)Keys.Right },
            { NameMapping.MOVE_LEFT , (int)Keys.Left },
        };

        public Player(int life, float speed, Texture2D sprite) : base(life, speed, sprite)
        {
            _animationManager.Add(NameMapping.MOVE_UP, new AnimeSprite(sprite, 9, 50, 64, 64, 8));
            _animationManager.Add(NameMapping.MOVE_DOWN, new AnimeSprite(sprite, 9, 50, 64, 64, 10));
            _animationManager.Add(NameMapping.MOVE_RIGHT, new AnimeSprite(sprite, 9, 50, 64, 64, 11));
            _animationManager.Add(NameMapping.MOVE_LEFT, new AnimeSprite(sprite, 9, 50, 64, 64, 9));

            _animationManager.changeAnimation(NameMapping.MOVE_DOWN);
            _currentMovement = NameMapping.MOVE_DOWN;
            _lastMovement = NameMapping.MOVE_DOWN;

            Manager.characters.Add(this);
            Manager.players.Add(this);
        }

        public void Update(GameTime gameTime)
        {
            HandleMovement((float)gameTime.ElapsedGameTime.TotalSeconds);
            _animationManager.currentAnimeSprite.Update(gameTime);
        }

        public void HandleMovement(float deltaTime)
        {

            bool move = false;
            var keyboardState = Keyboard.GetState(); // Cache the keyboard state to avoid multiple calls

            if (keyboardState.IsKeyDown((Keys)_controlls[NameMapping.MOVE_UP]))
            {
                _currentMovement = NameMapping.MOVE_UP;
                _velocity.Y = -1;
                move = true;
            }
            else if (keyboardState.IsKeyDown((Keys)_controlls[NameMapping.MOVE_DOWN]))
            {
                _currentMovement = NameMapping.MOVE_DOWN;
                _velocity.Y = 1;
                move = true;
            }
            else
            {
                _velocity.Y = 0;
            }

            if (keyboardState.IsKeyDown((Keys)_controlls[NameMapping.MOVE_RIGHT]))
            {
                _currentMovement = NameMapping.MOVE_RIGHT;
                _velocity.X = 1;
                move = true;
            }
            else if (keyboardState.IsKeyDown((Keys)_controlls[NameMapping.MOVE_LEFT]))
            {
                _currentMovement = NameMapping.MOVE_LEFT;
                _velocity.X = -1;
                move = true;
            }
            else
            {
                _velocity.X = 0;
            }

            if (_velocity.Length() > 0)
            {
                _velocity.Normalize();
            }

            // Apply movement to position based on speed and deltaTime
            position += _velocity * _speed * deltaTime;


            if (move)
            {
                _animationManager.currentAnimeSprite.Start();
            }
            else
            {
                _animationManager.currentAnimeSprite.Stop();
            }
            
            //HandleColision();

            // Only perform movement if the current movement is different from the last one
            if (_currentMovement != NameMapping.NONE && _currentMovement != _lastMovement)
            {
                _animationManager.changeAnimation(_currentMovement);
                // Update the last movement
                _lastMovement = _currentMovement;
            }

        }

    }
}
