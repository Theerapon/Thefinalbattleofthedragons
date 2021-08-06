using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thefinalbattleofthedragons
{
    public class SummonFireGraphics : GraphicsComponent
    {

        private bool isAlive = true;
        private float timer = 0;

        public SummonFireGraphics(Game currentScene) : base(currentScene)
        {
            _texture = currentScene.Content.Load<Texture2D>("Images/MotionCreepGate");
            
        }

        public override void Draw(SpriteBatch spriteBatch, GameObject parent)
        {
            if (!isAlive)
            {
                //we are hit
                _animationManager.Play(_animations["IsDead"]);
                if(timer >= 0.8f)
                {
                    parent.IsActive = false;
                }
            }
            else
            {
                _animationManager.Play(_animations["IsActive"]);
            }

            base.Draw(spriteBatch, parent);
        }

        public override void ReceiveMessage(int message, Component sender)
        {
            //sent from summonfire itself
            if(message == 101 )
            {
                this.isAlive = false;
            }

            
            base.ReceiveMessage(message, sender);
        }

        public override void Reset(GameObject parent)
        {
            switch (parent._currentTeam)
            {
                case GameObject.TEAM.A:
                    _animations = new Dictionary<string, Animation>()
                    {
                        {  "IsActive", new Animation(_texture, new Rectangle(0, 554, 120, 22), 4) },
                        {  "IsDead", new Animation(_texture, new Rectangle(0, 586, 120, 22), 4) }
                    };
                    break;
                case GameObject.TEAM.B:
                    _animations = new Dictionary<string, Animation>()
                    {
                        {  "IsActive", new Animation(_texture, new Rectangle(504, 554, 80, 30), 4) },
                        {  "IsDead", new Animation(_texture, new Rectangle(504, 586, 80, 30), 4) }
                    };
                    break;
            }

           isAlive = true;
            base.Reset(parent);
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects, GameObject parent)
        {
            if (!isAlive)
            {
                timer += (float)gameTime.ElapsedGameTime.Ticks / (float)TimeSpan.TicksPerSecond;
            }
            else
            {
                
                
            }
            base.Update(gameTime, gameObjects, parent);
        }
    }
}
