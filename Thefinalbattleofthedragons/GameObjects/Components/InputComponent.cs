using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Thefinalbattleofthedragons
{
    public class InputComponent : Component
    {
        public Dictionary<string, Keys> InputList;

        public InputComponent(Game currentScene)
        {
            InputList = new Dictionary<string, Keys>();
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

        public virtual void ChangeMappingKey(String Key, Keys newInput)
        {
            InputList[Key] = newInput;
        }
    }
}
