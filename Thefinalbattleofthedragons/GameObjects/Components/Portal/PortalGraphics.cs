using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Thefinalbattleofthedragons
{
    public class PortalGraphics : GraphicsComponent
    {
        private Game currentScene;

        private bool isAlive = true;
        private float timer = 0;
        private int width = 432;
        private int height = 120;

        public PortalGraphics(Game currentScene) : base(currentScene)
        {
            this.currentScene = currentScene;

            _texture = currentScene.Content.Load<Texture2D>("Images/MotionCreepGate");

            
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

                        {  "IsActive", new Animation(_texture, new Rectangle(0, 0, width, height), 6) }
                    };
                    break;
                case GameObject.TEAM.B:
                    _animations = new Dictionary<string, Animation>()
                    {
                        {  "IsActive", new Animation(_texture, new Rectangle(504, 0, width, height), 6) }
                    };
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
