using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using System.Diagnostics;

namespace ComboFigther.Character
{
    class Manager
    {
        public static List<Character> characters = new List<Character>();
        public static List<Player> players = new List<Player>();
        //public static List<Enemy> enemys = new List<Enemy>();
        //public static List<Slime> slimes = new List<Slime>();
        //public static List<Slime> toAddSlime = new List<Slime>();
        //public static List<Projectile> projectiles = new List<Projectile>();
        //public static List<Obstacle> obstacles = new List<Obstacle>();

        // Update
        public void UpdateCharacter(GameTime gameTime)
        {
            UpdatePlayer(gameTime);
            //UpdateEnemy(gameTime);

            characters.RemoveAll(character => character.isDead);
        }

        void UpdatePlayer(GameTime gameTime)
        {
            foreach (var player in players)
            {
                player.Update(gameTime);
            }
        }

        //public void UpdateEnemy(GameTime gameTime)
        //{
        //    UpdateSlime(gameTime);

        //    enemys.RemoveAll(enemy => enemy.isDead);

        //}

        //public void UpdateSlime(GameTime gameTime)
        //{
        //    foreach (var slime in slimes)
        //    {
        //        slime.Update(gameTime);
        //    }

        //    slimes.RemoveAll(slime => slime.isDead);

        //    //Debug.WriteLine(slimes.Count);
        //    //Debug.WriteLine(enemys.Count);
        //    //Debug.WriteLine(characters.Count);
        //    //        Debug.WriteLine("--------------------------------");


        //    slimes = slimes.Concat(toAddSlime).ToList();
        //    enemys = enemys.Concat(toAddSlime).ToList();
        //    characters = characters.Concat(toAddSlime).ToList();
        //    toAddSlime.Clear();
        //}

        //public void UpdateProjectile(GameTime gameTime)
        //{
        //    foreach (var projectile in projectiles)
        //    {
        //        projectile.Update(gameTime);
        //    }
        //    projectiles.RemoveAll(proj => proj.destroyed);
        //}

        //  Draw
        public void DrawCharacter(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var character in characters)
            {
                character.Draw(gameTime, spriteBatch);

            }
        }

        //public void DrawProjectile(GameTime gameTime, SpriteBatch spriteBatch)
        //{
        //    foreach (var projectile in projectiles)
        //    {
        //        projectile.Draw(gameTime, spriteBatch);
        //    }
        //}

        //public void DrawObstacle(GameTime gameTime, SpriteBatch spriteBatch)
        //{
        //    foreach (var obstacle in obstacles)
        //    {
        //        if (obstacle.existInRoom)
        //        {
        //            obstacle.Draw(gameTime, spriteBatch);
        //        }
        //    }
        //}
    }
}
