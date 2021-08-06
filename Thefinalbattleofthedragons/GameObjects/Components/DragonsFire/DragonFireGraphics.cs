using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thefinalbattleofthedragons
{
    public class DragonFireGraphics : GraphicsComponent
    {
        Game currentScene;
        private bool isAlive = true;
        private float timer = 0;

        private int width = 96;
        private int height = 72;

        public DragonFireGraphics(Game currentScene) : base(currentScene)
        {
            this.currentScene = currentScene;
            _texture = currentScene.Content.Load<Texture2D>("Images/DragonFire");
        }

        public override void Draw(SpriteBatch spriteBatch, GameObject parent)
        {
            if (!isAlive)
            {
                //we are hit
                _animationManager.Play(_animations["IsDead"]);
                if (timer >= 0.8f)
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
            //sent from  dragonfire
            if (message == 301)
            {
                this.isAlive = false;
            }
            base.ReceiveMessage(message, sender);
        }

        public override void Reset(GameObject parent)
        {
            switch(parent._currentTeam)
            {
                case GameObject.TEAM.A:
                    _animations = new Dictionary<string, Animation>()
                    {

                        {  "IsActive", new Animation(_texture, new Rectangle(0, 0, width * 4, height), 4) },
                        {  "IsDead", new Animation(_texture, new Rectangle(384, 0, width * 5, height), 5) }
                    };
                break;
                case GameObject.TEAM.B:
                    _animations = new Dictionary<string, Animation>()
                    {
                        {  "IsActive", new Animation(_texture, new Rectangle(0, 72, width * 4, height), 4) },
                        {  "IsDead", new Animation(_texture, new Rectangle(384, 72, width * 5, height), 5) }
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
