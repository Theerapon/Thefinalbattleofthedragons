using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Thefinalbattleofthedragons
{
    public class GraphicsComponent : Component
    {
        protected Dictionary<string, Animation> _animations;
        protected AnimationManager _animationManager; 
        protected Texture2D _texture;

        public GraphicsComponent(Game currentScene)
        {
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects, GameObject parent)
        {
            if (_animationManager != null)
            {
                _animationManager.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameObject parent)
        {
            if (_animationManager == null)
            {
                spriteBatch.Draw(_texture,
                                 parent.Position,
                                 parent.Viewport,
                                 Color.White,
                                 parent.Rotation,
                                 parent.Viewport.Center.ToVector2(),
                                 parent.Scale,
                                 SpriteEffects.None,
                                 parent.Dept);

                if (parent.ShowRectangle)
                {
                    if (parent._rectangleTexture != null)
                    {
                        spriteBatch.Draw(parent._rectangleTexture,
                                 parent.Position,
                                 null,
                                 Color.Red,
                                 parent.Rotation,
                                 parent.Viewport.Center.ToVector2(),
                                 parent.Scale,
                                 SpriteEffects.None,
                                 parent.Dept);
                       
                    }

                }
            }
            else
            {
                _animationManager.Draw(spriteBatch, parent.Position, parent.Rotation, parent.Scale, parent.Dept);
                _animationManager.DrawViewRectangle(spriteBatch, parent);
            }

            
        }

        public override void Reset(GameObject parent)
        {
            if (_animations != null)
            {
                _animationManager = new AnimationManager(_animations.First().Value);
            }
        }

        public override void ReceiveMessage(int message, Component sender)
        {
            base.ReceiveMessage(message, sender);
        }

        
    }
}
