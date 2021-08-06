using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thefinalbattleofthedragons
{
    public class DragonsPhysic : PhysicsComponent
    {
        private Game currentScene;
        private bool isAlive = true;
        private float timer = 0;

        public DragonsPhysic(Game currentScene) : base(currentScene)
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
            switch (parent._currentTeam)
            {
                case GameObject.TEAM.A:
                    parent.Lifepoint = Singleton.Instance.LIFEPOINT_TEAM_A;
                    break;
                case GameObject.TEAM.B:
                    parent.Lifepoint = Singleton.Instance.LIFEPOINT_TEAM_B;
                    break;
            }
            isAlive = true;
            base.Reset(parent);
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects, GameObject parent)
        {
            switch (parent._currentTeam)
            {
                case GameObject.TEAM.A:
                    parent.Lifepoint = Singleton.Instance.LIFEPOINT_TEAM_A;
                    break;
                case GameObject.TEAM.B:
                    parent.Lifepoint = Singleton.Instance.LIFEPOINT_TEAM_B;
                    break;
            }
            base.Update(gameTime, gameObjects, parent);
        }
        
    }
}
