using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Thefinalbattleofthedragons
{
    public class PowerHoleGraphics : GraphicsComponent
    {

        private Game currentScene;
        private bool isAlive = true;
        private float timer = 5f;

        private int width = 576;
        private int height = 144;


        private enum Area
        {
            TOP,
            CENTER,
        }
        private Area _currentArea;

        public PowerHoleGraphics(Game currentScene, int area) : base(currentScene)
        {
            this.currentScene = currentScene;
            _texture = currentScene.Content.Load<Texture2D>("Images/Motion");
            _animations = new Dictionary<string, Animation>()
                    {
                        {  "IsNormal", new Animation(_texture, new Rectangle(0, 214, width, height), 4){
                            FrameSpeed = 0.2f
                        } },

                    };

            if(area == 1)
            {
                _currentArea = Area.TOP;
            } else if(area == 2)
            {
                _currentArea = Area.CENTER;
            }
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

            parent.Position = RandomPosition();

            base.Reset(parent);
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects, GameObject parent)
        {
            timer += (float)gameTime.ElapsedGameTime.Ticks / (float)TimeSpan.TicksPerSecond;
            if(timer >= 8f && Singleton.Instance.CurrentGameState.Equals(Singleton.GameState.GamePlaying))
            {
                parent.Position = RandomPosition();
                timer = 0;
            }
            base.Update(gameTime, gameObjects, parent);
        }

        private Vector2 RandomPosition()
        {
            Vector2 returningPoint = new Vector2();
            returningPoint.X = (int)(Singleton.Instance.Random.Next(Singleton.SCREENWIDTH / 5)) + 666;
            int pathHeight = 300;
            switch (_currentArea)
            {
                case Area.TOP:
                    returningPoint.Y = (int)(Singleton.Instance.Random.Next(pathHeight)) + 48; //random Area from pathHeight + mergin + half height graphics + area Top(0)
                    break;
                case Area.CENTER:
                    returningPoint.Y = (int)(Singleton.Instance.Random.Next(pathHeight)) + 48 + (pathHeight); //random Area from pathHeight + mergin + half height graphics + area Center(1)
                    break;
            }
            return returningPoint;
        }
    }
}
