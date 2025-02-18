using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using ComboFigther.Attacks;
using ComboFigther.Characters;
using ComboFigther.Decor;

namespace ComboFigther.Utility
{
    class Manager
    {
        public static List<Character> Characters = new List<Character>();
        public static Player Player;
        public static List<Enemy> Enemys = new List<Enemy>();
        public static List<Slime> Slimes = new List<Slime>();
        public static List<Slime> ToAddSlime = new List<Slime>();
        public static List<Projectile> Projectiles = new List<Projectile>();
        public static List<Obstacle> Obstacles = new List<Obstacle>();

        ///////////////////////////////////////////////// Update
        public void UpdateCharacter(GameTime gameTime)
        {
            UpdatePlayer(gameTime);
            UpdateEnemy(gameTime);

            Characters.RemoveAll(character => character.IsDead);
        }

        void UpdatePlayer(GameTime gameTime)
        {
            Player.Update(gameTime);
            foreach (var attack in Player.Attacks)
            {
                attack.Update(gameTime);
            }
        }

        public void UpdateEnemy(GameTime gameTime)
        {
            UpdateSlime(gameTime);

            Enemys.RemoveAll(enemy => enemy.IsDead);

        }

        public void UpdateSlime(GameTime gameTime)
        {
            foreach (var slime in Slimes)
            {
                slime.Update(gameTime);
            }

            Slimes.RemoveAll(slime => slime.IsDead);

            Slimes = Slimes.Concat(ToAddSlime).ToList();
            Enemys = Enemys.Concat(ToAddSlime).ToList();
            Characters = Characters.Concat(ToAddSlime).ToList();
            ToAddSlime.Clear();
        }

        public void UpdateProjectile(GameTime gameTime)
        {
            foreach (var projectile in Projectiles)
            {
                projectile.Update(gameTime);
            }
            Projectiles.RemoveAll(proj => proj.destroyed);
        }

        /////////////////////////////////////////////////  Draw
        public void DrawCharacter(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var character in Characters)
            {
                character.Draw(gameTime, spriteBatch);
            }
        }

        public void DrawProjectile(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var projectile in Projectiles)
            {
                projectile.Draw(gameTime, spriteBatch);
            }
        }

        public void DrawObstacle(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var obstacle in Obstacles)
            {
                obstacle.Draw(gameTime, spriteBatch);
            }
        }
    }
}
