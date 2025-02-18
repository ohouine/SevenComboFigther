using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComboFigther.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ComboFigther.Attacks
{
    abstract class Attack
    {
        Texture2D _icon;

        //far left is 48 then incrase of one each note (on the tab)
        List<int> _combo;
        int _currentNoteIndex = 0;

        DateTime _lastCallTime = DateTime.MinValue;

        bool _isAvaible = true;


        // Time before _currentNoteIndex is reset (in milliseconds)
        protected float _rest;
        protected int _damage;
        //time in second between attack
        private float _attackRate;
        protected AnimeSprite _sprite;


        public Texture2D Icon { get => _icon; set => _icon = value; }
        public List<int> Combo { get => _combo; set => _combo = value; }
        public float CountdownTimer = 0;
        public bool IsAvaible { get => _isAvaible; set {
                if (!value)
                {
                    CountdownTimer = AttackRate;
                }
                _isAvaible = value; 
            } }

        public float AttackRate { get => _attackRate; set => _attackRate = value; }

        public Attack(float attackRate, float rest, Texture2D icon, AnimeSprite sprite, List<int> combo, int damage)
        {
            _attackRate = attackRate;
            _rest = rest;
            Icon = icon;
            _sprite = sprite;
            Combo = combo;
            _damage = damage;
        }


        /// <summary>
        /// Listen to sent notes and check if the required combo is completed.
        /// </summary>
        /// <param name="note">NoteEvent.NoteNumber</param>
        public void Listen(int note)
        {
            if (!IsAvaible) return;
            // If the note matches the current combo note, proceed
            if (note == _combo[_currentNoteIndex])
            {
                DateTime now = DateTime.Now;
                if (_lastCallTime == DateTime.MinValue) _lastCallTime = now;

                double msElapsed = (now - _lastCallTime).TotalMilliseconds;

                if (msElapsed > _rest)
                    _currentNoteIndex = 0;

                // Check if this is the last note in the combo
                if (_currentNoteIndex + 1 >= _combo.Count)
                {
                    _currentNoteIndex = 0;
                    LaunchAttack();
                    IsAvaible = false;
                }
                else
                {
                    _currentNoteIndex++;
                }

                _lastCallTime = now;
            }
        }

        /// <summary>
        /// mean to be override by children so they get an event when combo is completed
        /// </summary>
        protected abstract void LaunchAttack();

        /// <summary>
        /// Handle if attack is Avaible
        /// </summary>
        public void Update(GameTime gameTime)
        {
            if (!IsAvaible)
            {
                if (CountdownTimer != 0)
                {
                    CountdownTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                    // Ensure it doesn't go below zero
                    if (CountdownTimer < 0)
                    {
                        IsAvaible = true;
                        CountdownTimer = 0;
                    }
                }
            }
        }

    }
}
