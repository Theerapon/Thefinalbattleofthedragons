using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thefinalbattleofthedragons
{
    public class GroudGraphics : GraphicsComponent
    {
        private bool isAlive = true;
        private float timer = 0;

        public GroudGraphics(Game currentScene) : base(currentScene)
        {
            _texture = currentScene.Content.Load<Texture2D>("Images/Motion");
            _animations = new Dictionary<string, Animation>()
            {
                { "IsActive", new Animation(_texture, new Rectangle(648, 420, 72, 72), 1) }
            };
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
            isAlive = true;
            base.Reset(parent);
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects, GameObject parent)
        {
            
            base.Update(gameTime, gameObjects, parent);
        }
    }
}
