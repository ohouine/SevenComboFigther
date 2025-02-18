using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ComboFigther.Sprite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComboFigther.Characters;
using ComboFigther.Utility;
using ComboFigther.Attacks;
using ComboFigther.Decor;

namespace ComboFigther.Attacks
{
    class Projectile
    {




        bool _spectral = false;//mean it can move trough obstacle
        int _piercing = 1;//number of ennemy he can go trough
        float _speed;
        float _range;
        int _damage;

        AnimeSprite _sprite;
        Character _shootedFrom;
        Vector2 _direction;
        Vector2 _position;
        Vector2 _size;

        public Rectangle Rectangle { get => new Rectangle((int)_position.X, (int)_position.Y, (int)_size.X, (int)_size.Y); }
        public bool destroyed = false;
        public Projectile(Vector2 direction, float speed, Vector2 size, float range, Character shootedFrom, int damage, AnimeSprite sprite, bool spectral = false, int piercing = 1)
        {
            _position = shootedFrom.Position;
            _shootedFrom = shootedFrom;
            _direction = direction;
            _spectral = spectral;
            _piercing = piercing;
            _sprite = sprite;
            _damage = damage;
            _speed = speed;
            _range = range;
            _size = size;

            Manager.Projectiles.Add(this);
        }

        float distance = 0;//distance in pixel that have been traveled
        public void Update(GameTime gameTime)
        {
            this._position.X += _speed * _direction.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            this._position.Y += _speed * _direction.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

            distance += (_speed * (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (distance >= _range)
            {
                destroyed = true;
            }

            CheckColision();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _sprite.Draw(gameTime, spriteBatch, _position);
        }

        public void CheckColision()
        {
            if (_shootedFrom is Player)
            {

                foreach (Enemy enemy in Manager.Enemys)
                {


                    if (Rectangle.Intersects(enemy.Rectangle))
                    {
                        enemy.GetAttacked(_damage);
                        _piercing--;
                        if (_piercing <= 0) destroyed = true;
                    }

                }
            }
            else
            {
                    if (Rectangle.Intersects(Manager.Player.Rectangle))
                    {
                        Manager.Player.GetAttacked(_damage);
                        _piercing--;
                        if (_piercing <= 0) destroyed = true;
                    }
                

            }

            foreach (var obstacle in Manager.Obstacles)
            {
                    if (Rectangle.Intersects(obstacle.Rectangle))
                    {
                        if (!_spectral)
                        {
                            destroyed = true;
                        }
                }
            }
        }

    }
}
