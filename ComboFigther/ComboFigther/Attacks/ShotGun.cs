using ComboFigther.Characters;
using ComboFigther.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComboFigther.Attacks
{
    class ShotGun : Shooter
    {
        int _nbProjectile;
        float _maxSpread = 30f;
        public ShotGun(float attackRate, float rest, Texture2D icon, AnimeSprite sprite, List<int> combo, int damage, float speed, float range, Vector2 size, Character shootedFrom, int nbProjectile) : base(attackRate, rest, icon, sprite, combo, damage, speed, range, size, shootedFrom)
        {
            _nbProjectile = nbProjectile;
        }

        /// <summary>
        /// chatgpt powered
        /// </summary>
        protected override void LaunchAttack()
        {

            // Normalize direction for consistency
            Vector2 direction = _shootedFrom.Velocity;

            // Convert direction to an angle (radians)
            float baseAngle = (float)Math.Atan2(direction.Y, direction.X);

            // If only one projectile, fire it straight with no spread
            if (_nbProjectile == 1)
            {
                new Projectile(direction * _speed, _speed, _size, _range, _shootedFrom, _damage, _sprite);
                return;
            }

            // Calculate the actual spread range (reduces if fewer projectiles)
            float spreadAngle = MathHelper.ToRadians(_maxSpread); // Convert degrees to radians
            float angleStep = spreadAngle / (_nbProjectile - 1); // Angle gap between projectiles

            for (int i = 0; i < _nbProjectile; i++)
            {
                // Calculate the new angle for this projectile
                float newAngle = baseAngle - (spreadAngle / 2) + (angleStep * i);

                // Rotate the velocity vector
                Vector2 newVelocity = new Vector2((float)Math.Cos(newAngle), (float)Math.Sin(newAngle));

                // Create and add the projectile
                new Projectile(newVelocity, _speed, _size, _range, _shootedFrom, _damage, _sprite);
            }
        }
    }
}
