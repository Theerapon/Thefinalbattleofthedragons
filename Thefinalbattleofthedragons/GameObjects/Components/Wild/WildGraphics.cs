using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thefinalbattleofthedragons
{
    class WildGraphics : GraphicsComponent
    {
        private int width = 96;
        private int height = 48;
        public WildGraphics(Game currentScene) : base(currentScene)
        {
            _texture = currentScene.Content.Load<Texture2D>("Images/Motion");
            _animations = new Dictionary<string, Animation>()
                    {
                        {  "IsNormal", new Animation(_texture, new Rectangle(1152, 444, width, height), 1)},
                        {  "IsRight", new Animation(_texture, new Rectangle(1008, 96, width, height), 1)},
                        {  "IsLeft", new Animation(_texture, new Rectangle(1104, 96, width, height), 1)}
                    };
        }

        public override void Draw(SpriteBatch spriteBatch, GameObject parent)
        {
            if (Singleton.Instance.WILD == 0)
            {
                _animationManager.Play(_animations["IsNormal"]);
            }
            else if (Singleton.Instance.WILD < 0)
            {
                _animationManager.Play(_animations["IsLeft"]);
            }
            else
            {
                _animationManager.Play(_animations["IsRight"]);
            }
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
