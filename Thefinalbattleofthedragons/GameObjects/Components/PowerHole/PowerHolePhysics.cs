using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thefinalbattleofthedragons
{
    public class PowerHolePhysics : PhysicsComponent
    {
        private bool isAlive = true;
        private float timer = 0;

        public PowerHolePhysics(Game currentScene) : base(currentScene)
        {
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
            base.Reset(parent);
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects, GameObject parent)
        {
            base.Update(gameTime, gameObjects, parent);
        }

        
    }
}
