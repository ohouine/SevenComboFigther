using ComboFigther.Characters;
using ComboFigther.Attacks;
using ComboFigther.Utility;
using Microsoft.VisualBasic.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using ComboFigther.Sprite;
using ComboFigther.Scene;
using SharpDX.Direct2D1;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace ComboFigther
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static Texture2D SlimeSprite;
        public static Texture2D RockSprite;

        SceneManager _sceneManager;

        private AttackIconManager _attackIconManager;
        private Manager _manager;
        //pause
        private PauseMenu _pauseMenu;
        private SpriteFont _font;
        private bool _isPaused = false;

        //Midi var
        List<string> comboBoxMidiInDevices = new List<string>();
        List<string> comboBoxMidiOutDevices = new List<string>();

        MidiIn midiIn = new MidiIn(0);
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _graphics.PreferredBackBufferWidth = 1920;    // Set the desired width
            _graphics.PreferredBackBufferHeight = 1080;  // Set the desired height
            _graphics.IsFullScreen = false;           // Set fullscreen mode
            _graphics.ApplyChanges();

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //set midi reciver
            midiIn.MessageReceived += midiIn_MessageReceived;
            midiIn.ErrorReceived += midiIn_ErrorReceived;
            midiIn.Start();

            //global var
            SlimeSprite = Content.Load<Texture2D>("hop");
            RockSprite = Content.Load<Texture2D>("Rock");


            //pause menu
            Vector2 menuPosition = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            _font = Content.Load<SpriteFont>("Arial");
            _pauseMenu = new PauseMenu(_font, menuPosition);

            //manger and player
            _manager = new Manager();
            _attackIconManager = new AttackIconManager(GraphicsDevice, new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), _font);
            new Player(Content.Load<Texture2D>("PlayerSpriteBatsh"));
            _attackIconManager.SetAttacks(Manager.Player.Attacks);

            //Init Attacks
            AnimeSprite sprite = new AnimeSprite(Content.Load<Texture2D>("Bullet"), 5, 50, 16, 16, 19);
            Texture2D icon = Content.Load<Texture2D>("Gun");
            List<int> combo = new List<int> { 48, 49, 50 };
            Manager.Player.Attacks.Add(new Shooter(0.5f, 1000, icon, sprite, combo, 3000, 500, 1000, new Vector2(16, 16), Manager.Player));
            icon = Content.Load<Texture2D>("shotgun");
            combo = new List<int> { 51, 52, 51 };
            Manager.Player.Attacks.Add(new ShotGun(0.5f, 1000, icon, sprite, combo, 3000, 500, 1000, new Vector2(16, 16), Manager.Player,3));

            //Scene
            _sceneManager = new SceneManager();
            _sceneManager.LoadScene(1);

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.P)) // Press 'P' to toggle pause
            {
                _isPaused = !_isPaused;
                System.Threading.Thread.Sleep(200); // Prevent rapid toggling
            }
            //if game is not paused 
            if (!_isPaused)
            {
                _manager.UpdateCharacter(gameTime);
                _manager.UpdateProjectile(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            //draw background
            _spriteBatch.Draw(Content.Load<Texture2D>("background"),
                      new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height),
                      Color.White);

            _manager.DrawCharacter(gameTime, _spriteBatch);
            _manager.DrawProjectile(gameTime, _spriteBatch);
            _manager.DrawObstacle(gameTime, _spriteBatch);
            _attackIconManager.Draw(_spriteBatch);

            if (_isPaused)
                _pauseMenu.Draw(_spriteBatch); // Pass player's attacks

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        void midiIn_ErrorReceived(object sender, MidiInMessageEventArgs e)
        {
            Debug.WriteLine(String.Format("ERROR : Time {0} Message 0x{1:X8} Event {2}",
                e.Timestamp, e.RawMessage, e.MidiEvent));
        }

        void midiIn_MessageReceived(object sender, MidiInMessageEventArgs e)
        {
            MidiEvent midiEvent = e.MidiEvent;

            try
            {

                NoteEvent noteEvent = (NoteEvent)midiEvent;

                foreach (Attack attack in Manager.Player.Attacks)
                {
                    attack.Listen(noteEvent.NoteNumber);
                }
            }
            catch (Exception ex) { }

        }
    }
}
