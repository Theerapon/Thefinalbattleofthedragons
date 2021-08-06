using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thefinalbattleofthedragons
{
    public class LifepointsGraphics : GraphicsComponent
    {
        private int previousLifepoint;
        private Game currentScene;
        private int initLifepoint;

        private int width = 608; //624
        private int height = 32;

        public LifepointsGraphics(Game currentScene) : base(currentScene)
        {
            initLifepoint = Singleton.INIT_LIFEPOINT;
            this.currentScene = currentScene;
            _texture = currentScene.Content.Load<Texture2D>("Images/Motion");
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
                        {  "IsNormal", new Animation(_texture, new Rectangle(8, 8, width, height), 1)  },
                    };
                   
                     _animations["IsNormal"].Origin = new Vector2(0, 16);
                    break;
                case GameObject.TEAM.B:
                    _animations = new Dictionary<string, Animation>()
                    {
                        {  "IsNormal", new Animation(_texture, new Rectangle(8, 56, width, height), 1) },
                    };
                    _animations["IsNormal"].Origin = new Vector2(0, 16);
                    parent.Rotation = MathHelper.ToRadians(180);
                    break;
            }
            base.Reset(parent);
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects, GameObject parent)
        {
            switch (parent._currentTeam)
            {
                case GameObject.TEAM.A:
                    if (previousLifepoint == Singleton.Instance.LIFEPOINT_TEAM_A) return;
                        _animationManager.setFrameLifePoint(Singleton.Instance.LIFEPOINT_TEAM_A, initLifepoint);
                        previousLifepoint = Singleton.Instance.LIFEPOINT_TEAM_A;
                    break;
                case GameObject.TEAM.B:
                    if (previousLifepoint == Singleton.Instance.LIFEPOINT_TEAM_B) return;
                        _animationManager.setFrameLifePoint(Singleton.Instance.LIFEPOINT_TEAM_B, initLifepoint);
                        previousLifepoint = Singleton.Instance.LIFEPOINT_TEAM_B;
                        break;
            }




            base.Update(gameTime, gameObjects, parent);
        }
    }
}
