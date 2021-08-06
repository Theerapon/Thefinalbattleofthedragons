using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thefinalbattleofthedragons
{
    public class LifepointsPhysic : PhysicsComponent
    {
        private bool isAlive = true;
        private float timer = 0;

        public LifepointsPhysic(Game currentScene) : base(currentScene)
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
            if (Singleton.Instance.LIFEPOINT_TEAM_A <= 0 && Singleton.Instance.LIFEPOINT_TEAM_B <= 0)
            {
                Singleton.Instance.playerWin = Singleton.PlayerWin.Draw;
                Singleton.Instance.gameOver = true;

            }
            else if (Singleton.Instance.LIFEPOINT_TEAM_A <= 0)
            {
                Singleton.Instance.playerWin = Singleton.PlayerWin.B;
                Singleton.Instance.gameOver = true;
            }
            else if (Singleton.Instance.LIFEPOINT_TEAM_B <= 0)
            {
                Singleton.Instance.playerWin = Singleton.PlayerWin.A;
                Singleton.Instance.gameOver = true;
            }
            base.Update(gameTime, gameObjects, parent);
        }
        
    }
}
