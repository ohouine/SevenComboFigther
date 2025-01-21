using ComboFigther.Character;
using Microsoft.VisualBasic.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ComboFigther
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Manager _manager;
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
        List<string> comboBoxMidiInDevices = new List<string>();
        List<string> comboBoxMidiOutDevices = new List<string>();

        MidiIn midiIn = new MidiIn(0);
        protected override void LoadContent()
        {
            midiIn.MessageReceived += midiIn_MessageReceived;
            midiIn.ErrorReceived += midiIn_ErrorReceived;
            midiIn.Start();

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _manager = new Manager();
            new Player(100, 150, Content.Load<Texture2D>("PlayerSpriteBatsh"));

        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _manager.UpdateCharacter(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            _manager.DrawCharacter(gameTime, _spriteBatch);
            // TODO: Add your drawing code here
            _spriteBatch.End();
            base.Draw(gameTime);
        }
        
        void midiIn_ErrorReceived(object sender, MidiInMessageEventArgs e)
        {
            Debug.WriteLine(String.Format("ERROR : Time {0} Message 0x{1:X8} Event {2}",
                e.Timestamp, e.RawMessage, e.MidiEvent));
        }
        int interval = 10;
        int currentloop = 0;
        void midiIn_MessageReceived(object sender, MidiInMessageEventArgs e)
        {
            //Debug.WriteLine(String.Format("INPUT : Time {0} Message 0x{1:X8} Event {2}",e.Timestamp, e.RawMessage, e.MidiEvent));

            MidiEvent midiEvent = e.MidiEvent;

            Debug.WriteLine(midiEvent);

        }
    }
}
