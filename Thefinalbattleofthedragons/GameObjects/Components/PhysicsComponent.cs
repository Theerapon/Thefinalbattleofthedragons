using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Thefinalbattleofthedragons
{
    public class PhysicsComponent : Component
    {
        public PhysicsComponent(Game currentScene)
        {
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects, GameObject parent)
        {
        }

        public override void ReceiveMessage(int message, Component sender)
        {
            base.ReceiveMessage(message, sender);
        }

        public override void Reset(GameObject parent)
        {
        }

        public bool Collided(GameObject parent, GameObject g)
        {
            float d1x = g.Rectangle.Left - parent.Rectangle.Right;
            float d1y = g.Rectangle.Top - parent.Rectangle.Bottom;
            float d2x = parent.Rectangle.Left - g.Rectangle.Right;
            float d2y = parent.Rectangle.Top - g.Rectangle.Bottom;

            if (d1x > 0 || d1y > 0) return false;
            if (d2x > 0 || d2y > 0) return false;

            return true;
        }

    }
}
