using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thefinalbattleofthedragons
{
    public class SummonGraphics : GraphicsComponent
    {

        private Game currentScene;

        public enum State
        {
            IDLE,
            ATTACK,
            HURT,
            DEAD
        }
        public State _currentState;

        private bool isAlive = true;
        private float timer = 0;
        private int width = 384;
        private int height = 96;

        public SummonGraphics(Game currentScene) : base(currentScene)
        {
            _texture = currentScene.Content.Load<Texture2D>("Images/MotionCreepGate");
            
        }

        public override void Draw(SpriteBatch spriteBatch, GameObject parent)
        {
            if (!isAlive)
            {
                parent.SoundEffects["Dead"].Play();
                //we are hit
                _animationManager.Play(_animations["IsDead"]);
                if (timer >= 1f)
                {
                    parent.IsActive = false;
                }
            }
            else
            {
                switch (_currentState)
                {
                    case State.IDLE:
                        _animationManager.Play(_animations["IsIdle"]);
                        break;
                    case State.ATTACK:
                        _animationManager.Play(_animations["IsAttack"]);
                        if (timer >= 0.8f)
                        {
                            _currentState = State.IDLE;
                            timer = 0;
                        }
                        break;
                    case State.HURT:
                        _animationManager.Play(_animations["IsHurt"]);
                        parent.SoundEffects["Hurt"].Play();
                        if (timer >= 0.8f)
                        {
                            if(parent.Lifepoint <= 0)
                            {
                                _currentState = State.DEAD;
                                isAlive = false;
                            } else
                            {
                                _currentState = State.IDLE;
                            }
                            timer = 0;
                        }
                        break;
                }
            }
            base.Draw(spriteBatch, parent);
        }

        public override void ReceiveMessage(int message, Component sender)
        {
            //hurn
            if(message == 202)
            {
                timer = 0;
                _currentState = State.HURT;
            }

            //attack
            if(message == 203)
            {
                timer = 0;
                _currentState = State.ATTACK;
            }


            //sent from portal
            if (message == 301)
            {
                this.isAlive = false;
            }
            base.ReceiveMessage(message, sender);
        }

        public override void Reset(GameObject parent)
        {
            _currentState = State.IDLE;
            switch (parent._currentTeam)
            {
                case GameObject.TEAM.A:
                    _animations = new Dictionary<string, Animation>()
                    {
                        {  "IsIdle", new Animation(_texture, new Rectangle(0, 130, width, height), 4)},
                        {  "IsAttack", new Animation(_texture, new Rectangle(0, 236, width, height), 4)},
                        {  "IsHurt", new Animation(_texture, new Rectangle(0, 342, width, height), 4) },
                        {  "IsDead", new Animation(_texture, new Rectangle(0, 448, width + 96, height), 5) }
                    };
                    break;
                case GameObject.TEAM.B:
                    _animations = new Dictionary<string, Animation>()
                    {
                        {  "IsIdle", new Animation(_texture, new Rectangle(504, 130, width, height), 4)},
                        {  "IsAttack", new Animation(_texture, new Rectangle(504, 236, width, height), 4) },
                        {  "IsHurt", new Animation(_texture, new Rectangle(504, 342, width, height), 4) },
                        {  "IsDead", new Animation(_texture, new Rectangle(504, 448, width + 96, height), 5) }
                    };
                    break;
            }
            parent.SoundEffects["Hurt"].Volume = Singleton.Instance.SummonSFXVolume;
            parent.SoundEffects["Dead"].Volume = Singleton.Instance.SummonSFXVolume;
            isAlive = true;
            base.Reset(parent);
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects, GameObject parent)
        {
            timer += (float)gameTime.ElapsedGameTime.Ticks / (float)TimeSpan.TicksPerSecond;
            base.Update(gameTime, gameObjects, parent);
        }
    }
}
