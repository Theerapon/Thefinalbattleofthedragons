using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thefinalbattleofthedragons
{
    public class TabChargeGraphics : GraphicsComponent
    {
        private Game currentScene;
        private bool isAlive = false;
        private float timer = 0;

        private int width = 144;
        private int height = 260;

        public TabChargeGraphics(Game currentScene) : base(currentScene)
        {
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
                        {  "IsNormal", new Animation(_texture, new Rectangle(144, 372, width, height), 1)},
                        {  "IsNone", new Animation(_texture, new Rectangle(0, 0, 0, 0), 1)}
                    };
                    _animations["IsNormal"].Origin = new Vector2(0, 130);
                    break;
                case GameObject.TEAM.B:
                    _animations = new Dictionary<string, Animation>()
                    {
                        {  "IsNormal", new Animation(_texture, new Rectangle(144, 372, width, height), 1) },
                        {  "IsNone", new Animation(_texture, new Rectangle(0, 0, 0, 0), 1)}
                    };
                    _animations["IsNormal"].Origin = new Vector2(0, 130);
                    parent.Rotation = MathHelper.ToRadians(180);
                    break;
            }
            
            isAlive = true;
            base.Reset(parent);
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects, GameObject parent)
        {
            if (Singleton.Instance.CurrentGameState.Equals(Singleton.GameState.StartNewGame))
            {
                return;
            } else
            {
                switch (parent._currentTeam)
                {
                    case GameObject.TEAM.A:
                        _animationManager.setFrameTabCharge(parent.LinearVelocity);
                        break;
                    case GameObject.TEAM.B:
                        _animationManager.setFrameTabCharge(parent.LinearVelocity);
                        break;
                }
            }
           
            base.Update(gameTime, gameObjects, parent);
        }

        
    }
}
