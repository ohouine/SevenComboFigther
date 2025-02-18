using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComboFigther.Attacks
{
    class AttackIconManager
    {
        Texture2D _iconTexture;  // Attack icon texture
        Texture2D _emptySlotTexture; // Empty slot border texture
        Vector2 _screenSize;
        List<Attack> _attacks = new List<Attack>(); // Store attack icons
        SpriteFont _font;
        GraphicsDevice _graphicsDevice;
        int _slotSize = 50; // Square slot dimensions

        public AttackIconManager(GraphicsDevice graphicsDevice, Vector2 _screenSize, SpriteFont _font)
        {
            this._font = _font;
            this._screenSize = _screenSize;
            this._graphicsDevice = graphicsDevice;

            // Load textures (Replace with actual texture loading)
            _iconTexture = new Texture2D(graphicsDevice, _slotSize, _slotSize);
            _emptySlotTexture = new Texture2D(graphicsDevice, _slotSize, _slotSize);

            // Temporary fill textures with color (Replace with actual sprites)
            Color[] colorData = new Color[_slotSize * _slotSize];
            for (int i = 0; i < colorData.Length; i++) colorData[i] = Color.White;
            _iconTexture.SetData(colorData);
            _emptySlotTexture.SetData(colorData);
        }

        public void SetAttacks(List<Attack> attacks)
        {
            _attacks = attacks;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 centerPos = new Vector2(_screenSize.X - _slotSize * 2f, _screenSize.Y - _slotSize * 2f);

            // Attack slot positions (cross shape)
            Vector2[] positions = new Vector2[]
            {
        centerPos + new Vector2(-_slotSize, 0),   // Left (Attack 1)
        centerPos + new Vector2(0, -_slotSize),   // Top (Attack 2)
        centerPos + new Vector2(_slotSize, 0),    // Right (Attack 3)
        centerPos + new Vector2(0, _slotSize)     // Bottom (Attack 4)
            };

            for (int i = 0; i < positions.Length; i++)
            {
                // Background Color
                Color color = Color.Gray;

                // Draw slot border first
                spriteBatch.Draw(_emptySlotTexture, positions[i], color);

                if (i < _attacks.Count)
                {
                    Attack attack = _attacks[i];
                    Texture2D icon = attack.Icon;

                    // Find max dimension (width or height)
                    float maxDimension = Math.Max(icon.Width, icon.Height);

                    // Scale factor ensures the largest dimension fits within _slotSize
                    float scale = _slotSize / maxDimension;

                    // Calculate new scaled size
                    Vector2 iconSize = new Vector2(icon.Width * scale, icon.Height * scale);

                    // Center the scaled icon inside the slot
                    Vector2 iconPos = positions[i] + new Vector2((_slotSize - iconSize.X) / 2, (_slotSize - iconSize.Y) / 2);


                    spriteBatch.Draw(icon, iconPos, null, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                    if (!_attacks[i].IsAvaible)
                    {
                        float _flooredValue = (float)(Math.Floor(attack.CountdownTimer * 100) / 100);
                        float percentage = _flooredValue / attack.AttackRate;

                        float result = percentage * _slotSize;

                        if (result <= 1) result = 1;

                        Texture2D cover = new Texture2D(_graphicsDevice, (int)result, _slotSize);
                        Color[] colorData = new Color[(int)result * _slotSize];
                        for (int j = 0; j < colorData.Length; j++) colorData[j] = Color.White;
                        cover.SetData(colorData);

                        spriteBatch.Draw(cover, positions[i], Color.Red * percentage);
                    }
                }
            }
        }
    }
}
