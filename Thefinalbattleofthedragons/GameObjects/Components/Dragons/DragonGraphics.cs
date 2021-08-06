using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thefinalbattleofthedragons
{
    public class DragonGraphics : GraphicsComponent
    {
        private Game currentScene;
        private bool isAlive = true;
        private float timer = 0;

        private int height = 430;

        public enum State
        {
            IDLE,
            ATTACK,
            HURT,
            DEADACTION,
            DEAD
        }
        public State _currentState;

        public DragonGraphics(Game currentScene) : base(currentScene)
        {
            this.currentScene = currentScene;
            _texture = currentScene.Content.Load<Texture2D>("Images/MotionDragon");

        }

        public override void Draw(SpriteBatch spriteBatch, GameObject parent)
        {
            if (!isAlive)
            {
                switch (_currentState)
                {
                    case State.DEADACTION:
                        parent.SoundEffects["Dead"].Play();
                        _animationManager.Play(_animations["IsDeadAction"]);
                        if (timer >= 1f)
                        {
                            _currentState = State.DEAD;
                        }
                        break;
                    case State.DEAD:
                        _animationManager.Play(_animations["IsDead"]);
                        break;
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
                        _animationManager.Play(_animations["IsAtk"]);
                        if (timer >= 0.8f)
                        {
                            _currentState = State.IDLE;
                            timer = 0;
                        }
                        break;
                    case State.HURT:
                        parent.SoundEffects["Hurt"].Play();
                        _animationManager.Play(_animations["IsHurt"]);
                        if (timer >= 0.7f)
                        {
                            if (parent.Lifepoint <= 0)
                            {
                                _currentState = State.DEADACTION;
                                isAlive = false;
                            }
                            else
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
            //hurt
            if (message == 402)
            {
                timer = 0;
                _currentState = State.HURT;
            }

            //attack
            if (message == 403)
            {
                timer = 0;
                _currentState = State.ATTACK;
            }

            base.ReceiveMessage(message, sender);
        }

        public override void Reset(GameObject parent)
        {
            parent.SoundEffects["Hurt"].Volume = Singleton.Instance.MasterSFXVolume;
            parent.SoundEffects["Dead"].Volume = Singleton.Instance.MasterSFXVolume;
            switch (parent._currentTeam)
            {
                case GameObject.TEAM.A:
                    _animations = new Dictionary<string, Animation>()
                    {
                        {  "IsIdle", new Animation(_texture, new Rectangle(0, 0, 1440, height), 4) },
                        {  "IsAtk", new Animation(_texture, new Rectangle(0, 480, 1440, height), 4) },
                        {  "IsUlti", new Animation(_texture, new Rectangle(0, 960, 1440, height), 4) },
                        {  "IsHurt", new Animation(_texture, new Rectangle(0, 1440, 1440, height), 4) },
                        {  "IsDeadAction", new Animation(_texture, new Rectangle(0, 1920, 1800, height), 5) },
                        {  "IsDead", new Animation(_texture, new Rectangle(1440, 1920, 360, height), 1) }
      
                    };
                    break;
                case GameObject.TEAM.B:
                    _animations = new Dictionary<string, Animation>()
                    {
                        {  "IsIdle", new Animation(_texture, new Rectangle(1850, 0, 1440, height), 4) },
                        {  "IsAtk", new Animation(_texture, new Rectangle(1850, 480, 1440, height), 4) },
                        {  "IsUlti", new Animation(_texture, new Rectangle(1850, 960, 1440, height), 4) },
                        {  "IsHurt", new Animation(_texture, new Rectangle(1850, 1440, 1440, height), 4) },
                        {  "IsDeadAction", new Animation(_texture, new Rectangle(1850, 1920, 1800, height), 5) },
                        {  "IsDead", new Animation(_texture, new Rectangle(3290, 1920, 360, height), 1) }
                    };
                    break;
            }
            base.Reset(parent);
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects, GameObject parent)
        {
            timer += (float)gameTime.ElapsedGameTime.Ticks / (float)TimeSpan.TicksPerSecond;
            if(parent.Lifepoint <= 0 && !_currentState.Equals(State.DEAD))
            {
                _currentState = State.DEADACTION;
                isAlive = false;
            }
            base.Update(gameTime, gameObjects, parent);
        }
    }
}
