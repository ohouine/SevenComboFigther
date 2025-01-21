using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1.Effects;
using SharpDX.Direct3D9;
using System.Security.Policy;

namespace ComboFigther.Sprite
{
    class AnimeSprite
    {
        private float _scale;

        private int _time;
        private int _threshold;

        private int _frameWidth;
        private int _frameHeight;
        private int _nbFrames;
        private Rectangle[] _spriteFrames;
        private int _selectedSpriteFrame;
        private int _ypositionOnSpriteBatsh;

        private bool _stop = true;
        private bool _flip = false;

        public Texture2D texture2D;

        public int frameWidth { get => Convert.ToInt32(Math.Floor(_spriteFrames[_selectedSpriteFrame].Width * _scale)); set => _frameWidth = value; }
        public int frameHeight { get => Convert.ToInt32(Math.Floor(_spriteFrames[_selectedSpriteFrame].Height * _scale)); set => _frameHeight = value; }

        public AnimeSprite(Texture2D texture2D, int nbFrame, int holdFrmTime, int height, int width, int row, float scale = 1, bool flip = false)
        {
            this.texture2D = texture2D;
            _scale = scale;
            _nbFrames = nbFrame;

            _frameWidth = width;
            _frameHeight = height;

            _threshold = holdFrmTime;
            _spriteFrames = new Rectangle[_nbFrames];
            _ypositionOnSpriteBatsh = row * height;

            _flip = flip;
            SplitImage();
        }
        private void SplitImage()
        {
            // Extract the pixel data from the entire sprite sheet
            Color[] textureData = new Color[texture2D.Width * texture2D.Height];
            texture2D.GetData(textureData); // Get pixel data from the texture2D

            for (int i = 0; i < _nbFrames; i++)
            {
                // Approximate frame rectangle (before trimming)
                Rectangle frame = new Rectangle(i * _frameWidth, _ypositionOnSpriteBatsh, _frameWidth, _frameHeight);

                Rectangle preciseFrame = FindPreciseRectangle(textureData, frame, texture2D.Width);

                // Replace the original rectangle with the precise one
                _spriteFrames[i] = preciseFrame;
            }
        }
        /// <summary>
        /// find precise rectangle based on pixel in frame (chatGPT powered)
        /// </summary>
        /// <param name="textureData"></param>
        /// <param name="frame"></param>
        /// <param name="textureWidth"></param>
        /// <returns></returns>
        private Rectangle FindPreciseRectangle(Color[] textureData, Rectangle frame, int textureWidth)
        {
            // Initialize boundaries with reversed values to find min/max
            int left = frame.Right, right = frame.Left, top = frame.Bottom, bottom = frame.Top;

            // Loop through each pixel of the frame to find the bounding box of non-transparent pixels
            for (int y = frame.Top; y < frame.Bottom - 1; y++) // Adjust to avoid the bottom pixel from the next frame
            {
                for (int x = frame.Left; x < frame.Right - 1; x++) // Adjust to avoid the right pixel from the next frame
                {
                    Color pixelColor = textureData[x + y * textureWidth];

                    // Check if the pixel is non-transparent
                    if (pixelColor.A != 0) // Only considering non-transparent pixels
                    {
                        // Adjust the boundaries based on pixel positions
                        if (x < left) left = x;
                        if (x > right) right = x;
                        if (y < top) top = y;
                        if (y > bottom) bottom = y;
                    }
                }
            }

            // Return the new precise rectangle that removes wasted space
            return new Rectangle(left, top, right - left + 1, bottom - top + 1);
        }


        public void SelectStartingSprite(int sprite)
        {
            _selectedSpriteFrame = sprite;
        }

        /// <summary>
        /// stop prevent time to incrase
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (_time >= _threshold)
            {
                if (_selectedSpriteFrame == _spriteFrames.Length - 1)
                {
                    _selectedSpriteFrame = 0;
                }
                else
                {
                    _selectedSpriteFrame += 1;
                }

                _time = 0;
            }
            else
            {
                if (_stop == false)
                {
                    _time += gameTime.ElapsedGameTime.Milliseconds;
                }
            }
        }

        public void Reset()
        {
            _selectedSpriteFrame = 0;
            _time = 0;
        }
        public void Start() => _stop = false;
        public void Stop() => _stop = true;

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position)
        {
            RasterizerState state = new RasterizerState();
            state.FillMode = Microsoft.Xna.Framework.Graphics.FillMode.WireFrame;
            spriteBatch.GraphicsDevice.RasterizerState = state;

            //loop this for all sprites!
            float rotation = _flip ? 180 : 0;
            spriteBatch.Draw(texture2D, position, _spriteFrames[_selectedSpriteFrame], Color.White, 0, new Vector2(0, 0), new Vector2(_scale, _scale), SpriteEffects.None, 1);
        }

    }

}
