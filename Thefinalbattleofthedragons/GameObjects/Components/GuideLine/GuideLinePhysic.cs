using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thefinalbattleofthedragons
{
    public class GuildeLinePhysic : PhysicsComponent
    {
        private Game currentScene;
        private bool isAlive = true;
        private float timer = 0;

        public GuildeLinePhysic(Game currentScene) : base(currentScene)
        {
            this.currentScene = currentScene;
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
            if (!isAlive) return;

            

            base.Update(gameTime, gameObjects, parent);
        }
        
    }
}
