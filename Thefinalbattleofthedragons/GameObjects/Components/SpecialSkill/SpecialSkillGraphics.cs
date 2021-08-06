using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thefinalbattleofthedragons
{
    public class SpecialSkillGraphics : GraphicsComponent
    {
        private bool isAlive = true;
        private float timer = 0;

        private Game currentScene;
        private int width = 2560;
        private int height = 610;

        public SpecialSkillGraphics(Game currentScene) : base(currentScene)
        {
            this.currentScene = currentScene;
            _texture = currentScene.Content.Load<Texture2D>("Images/skill");
        }

        public override void Draw(SpriteBatch spriteBatch, GameObject parent)
        {
            base.Draw(spriteBatch, parent);
        }

        public override void ReceiveMessage(int message, Component sender)
        {
            base.ReceiveMessage(message, sender);
        }

        public override void Reset(GameObject parent)
        {
            
            switch (parent._currentTeam)
            {
                case GameObject.TEAM.A:
                    _animations = new Dictionary<string, Animation>()
                    {
                        {  "IsNormal", new Animation(_texture, new Rectangle(0, 0, width, height), 10){
                            FrameSpeed = 0.2f
                        } },
                    };
                    parent.Rotation = MathHelper.ToRadians(-30);
                    break;
                case GameObject.TEAM.B:
                    _animations = new Dictionary<string, Animation>()
                    {
                        {  "IsNormal", new Animation(_texture, new Rectangle(0, 610, width, height), 10)},

                    };
                    parent.Rotation = MathHelper.ToRadians(30);
                    break;
            }
            
            isAlive = true;
            base.Reset(parent);
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects, GameObject parent)
        {
            timer += (float)gameTime.ElapsedGameTime.Ticks / (float)TimeSpan.TicksPerSecond;
            if(timer >= 2f)
            {
                parent.IsActive = false;
            }
            base.Update(gameTime, gameObjects, parent);
        }
    }
}
