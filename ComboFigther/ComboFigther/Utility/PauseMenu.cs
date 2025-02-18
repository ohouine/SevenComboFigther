using ComboFigther.Attacks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;

namespace ComboFigther.Utility
{
    class PauseMenu
    {

        SpriteFont _font;
        Vector2 _basePosition;
        int _spacing = 80; // Space between attack icons
        int _size = 80; // Space between attack icons

        public PauseMenu(SpriteFont _font, Vector2 _basePosition)
        {
            this._font = _font;
            this._basePosition = _basePosition; // Center of the screen
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            List<Attack> attacks = Manager.Player.Attacks;

            // Draw a semi-transparent overlay to indicate pause
            spriteBatch.DrawString(_font, "PAUSED", new Vector2(_basePosition.X - 50, _basePosition.Y - 150), Color.White);

            for (int i = 0; i < attacks.Count; i++)
            {
                Attack attack = attacks[i];

                // Position each attack icon below the previous one
                Vector2 iconPosition = _basePosition + new Vector2(-100, i * _spacing);
                
                // Find max dimension (width or height)
                float maxDimension = Math.Max(attack.Icon.Width, attack.Icon.Height);

                // Scale factor ensures the largest dimension fits within _slotSize
                float scale = 50 / maxDimension;

                // Calculate new scaled size
                Vector2 iconSize = new Vector2(attack.Icon.Width * scale, attack.Icon.Height * scale);
                
                // Center the scaled icon inside the slot
                Vector2 iconPos = iconPosition + new Vector2((_size - iconSize.X) / 2, (_size - iconSize.Y) / 2);


                spriteBatch.Draw(attack.Icon, iconPos, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                //spriteBatch.Draw(attack.Icon, iconPosition, Color.White);

                // Draw combo numbers next to the icon
                Vector2 comboPosition = iconPosition + new Vector2(80, 10); // Offset to the right of the icon
                string comboText = string.Join(" -> ", attack.Combo);

                spriteBatch.DrawString(_font, comboText, comboPosition, Color.Black);
            }

        }
    }
}
