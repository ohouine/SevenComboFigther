using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ComboFigther.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using ComboFigther.Attacks;
using System.Diagnostics;
using ComboFigther.Sprite;

namespace ComboFigther.Attacks
{
    class Shooter : Attack
    {
        protected float _speed;
        protected float _range;
        protected Vector2 _size;
        protected Character _shootedFrom;

        public Shooter(float attackRate, float rest, Texture2D icon, AnimeSprite sprite, List<int> combo, int damage, float speed,
            float range, Vector2 size, Character shootedFrom) : base(attackRate, rest, icon, sprite, combo, damage)
        {
            this._speed = speed;
            this._range = range;
            this._size = size;
            this._shootedFrom = shootedFrom;
        }

        protected override void LaunchAttack()
        {

            Vector2 velocity = _shootedFrom.Velocity;
            if (velocity.X == 0 && velocity.Y == 0)
            {
                while (velocity.X == 0 && velocity.Y == 0)
                {
                    Random random = new Random();
                    velocity.X = random.Next(-1, 2);
                    velocity.Y = random.Next(-1, 2);
                }
            }

            new Projectile(velocity, _speed, _size, _range, _shootedFrom, _damage, _sprite);
        }
    }
}