using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComboFigther.Utility
{
    class DrawForDebug
    {
        public void DrawLineBetween(
Vector2 startPos,
Vector2 endPos,
int thickness,
Color color,
SpriteBatch _spriteBatch)
        {
            //Création d'une texture entre les 2 points et suivant l'épaisseur
            var distance = (int)Vector2.Distance(startPos, endPos);
            var texture = new Texture2D(_spriteBatch.GraphicsDevice,
           distance, thickness);
            // Coloriage de la texture (couleur unie)
            var data = new Color[distance * thickness];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = color;
            }
            texture.SetData(data);
            // rotation de la ligne entre start et end
            var rotation = (float)Math.Atan2(endPos.Y - startPos.Y,
           endPos.X - startPos.X);
            var origin = new Vector2(0, thickness / 2);
            _spriteBatch.Draw(
            texture,
            startPos,
            null,
            Color.White,
            rotation,
            origin,
            1.0f,
            SpriteEffects.None,
            1.0f);
        }

        public void DrawRectangle(Rectangle rectangle, int thickness, Color color, SpriteBatch _spriteBatch)
        {
            DrawLineBetween(new Vector2(rectangle.X, rectangle.Y), new Vector2(rectangle.X + rectangle.Width, rectangle.Y), thickness, color, _spriteBatch);
            DrawLineBetween(new Vector2(rectangle.X, rectangle.Y), new Vector2(rectangle.X, rectangle.Y + rectangle.Height), thickness, color, _spriteBatch);
            DrawLineBetween(new Vector2(rectangle.X + rectangle.Width, rectangle.Y), new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height), thickness, color, _spriteBatch);
            DrawLineBetween(new Vector2(rectangle.X, rectangle.Y + rectangle.Height), new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Width), thickness, color, _spriteBatch);
        }
    }
}
