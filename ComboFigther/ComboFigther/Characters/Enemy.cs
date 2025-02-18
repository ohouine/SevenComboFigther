using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ComboFigther.Decor;
using ComboFigther.Characters;
using ComboFigther.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComboFigther.Characters
{
    class Enemy : Character
    {
        int _damage;
        protected Pathfinding pathfinding;
        protected int pathCalculatingLatency = new Random().Next(20,100);
        int currentLatency;
        Vector2 direction;
        List<Vector2> nodes = new List<Vector2>();

        public Enemy(Vector2 position, Texture2D texture, float scale = 1) : base(position, texture)
        {
            
            currentLatency = pathCalculatingLatency;
        }
        DrawForDebug drawForDebug = new DrawForDebug();

        public int Damage { get => _damage; set => _damage = value; }

        protected void targetPlayer(Player player, GameTime gameTime)
        {
            if (pathfinding != null)
            {
                //calcule nodes every pathCalculatingLatency
                if (currentLatency >= pathCalculatingLatency || nodes.Count <= 0)
                {
                    nodes = pathfinding.FindPath(Position, player.Position);
                    currentLatency = 0;
                }
                else currentLatency++;

                //follow player if there is no node
                direction = (nodes.Count > 0 && nodes[0] != Position) ? nodes[0] - Position : player.Position - Position;

                // Normalize the direction (so it doesn't depend on distance)
                if (direction != Vector2.Zero)
                {
                    direction.Normalize();
                }

                Position += direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (nodes.Count > 0)
                {
                    if (Vector2.Distance(Position, nodes[0]) < Speed * gameTime.ElapsedGameTime.TotalSeconds) // Example threshold of 10 pixels
                    {
                        nodes.RemoveAt(0);
                    }
                }
            }
        }
    }
}
