using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thefinalbattleofthedragons
{
    class WildPhysics : PhysicsComponent
    {

        private int countAtest;
        private int countBtest;
        private float timer;
        public WildPhysics(Game currentScene) : base(currentScene)
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
            Singleton.Instance.WILD = 0f;
            timer = 10;
            base.Reset(parent);
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects, GameObject parent)
        {
            timer += (float)gameTime.ElapsedGameTime.Ticks / (float)TimeSpan.TicksPerSecond;
            if (timer > 10f)
            {
                float rnd = Singleton.Instance.Random.Next(41);
                float team = Singleton.Instance.Random.Next(2);
                if(team == 0)
                {
                    Singleton.Instance.WILD = rnd / 10.0f;
                } else
                {
                    Singleton.Instance.WILD = -(rnd / 10.0f);
                }


                timer = 0f;
            }
            base.Update(gameTime, gameObjects, parent);
        }
    }
}
