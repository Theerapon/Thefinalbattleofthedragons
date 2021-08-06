using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thefinalbattleofthedragons
{
    public class GuildLineGraphics : GraphicsComponent
    {
        
        private bool isAlive = true;
        private float timer = 0;
        private int width = 144;
        private int height = 10;

        public GuildLineGraphics(Game currentScene) : base(currentScene)
        {
            _texture = currentScene.Content.Load<Texture2D>("Images/Motion");
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
                        {  "IsActive", new Animation(_texture, new Rectangle(598, 584, width * 2, height), 2) },
                    };
                    _animations["IsActive"].Origin = new Vector2(0, 5);
                    break;
                case GameObject.TEAM.B:
                    _animations = new Dictionary<string, Animation>()
                    {
                        {  "IsActive", new Animation(_texture, new Rectangle(598, 584, width * 2, height), 2) },
                    };
                    _animations["IsActive"].Origin = new Vector2(0, 5);
                    parent.Rotation = MathHelper.ToRadians(180);
                    break;
            }
            isAlive = true;
            base.Reset(parent);
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects, GameObject parent)
        {
            base.Update(gameTime, gameObjects, parent);
        }
    }
}
