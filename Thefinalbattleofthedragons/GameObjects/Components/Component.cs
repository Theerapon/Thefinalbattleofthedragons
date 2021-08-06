using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace Thefinalbattleofthedragons
{
    public class Component
    {
        public Component()
        {

        }

        public virtual void Update(GameTime gameTime, List<GameObject> gameObjects, GameObject parent)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch, GameObject parent)
        {

        }

        public virtual void Reset(GameObject parent)
        {

        }

        public virtual void ReceiveMessage(int message, Component sender)
        {

        }
        
    }
}
