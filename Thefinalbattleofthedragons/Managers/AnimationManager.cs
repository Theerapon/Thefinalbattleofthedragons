using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace Thefinalbattleofthedragons
{
    public class AnimationManager
    {
        private Animation _animation;
        private float _timer;
        public float TimePlayed;


        public AnimationManager(Animation animation)
        {
            _animation = animation;
        }

        public void Play(Animation animation)
        {
            if (_animation.Equals(animation))
                return;

            _animation = animation;
            _animation.CurrentFrame = 0;
            _timer = 0;
            TimePlayed = 0;
        }

        public void Stop()
        {
            _timer = 0f;

            _animation.CurrentFrame = 0;
        }

        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
            TimePlayed += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

            if (_timer > _animation.FrameSpeed)
            {
                _timer = 0f;
                _animation.CurrentFrame++;

                if (_animation.CurrentFrame >= _animation.FrameCount)
                    _animation.CurrentFrame = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, float rotation, Vector2 scale, float dept)
        {
            spriteBatch.Draw(_animation.Texture,
                            position,
                            new Rectangle(_animation.AnimationRectangle.X + _animation.CurrentFrame * _animation.FrameWidth,
                                          _animation.AnimationRectangle.Y,
                                          _animation.FrameWidth,
                                          _animation.FrameHeight),
                            Color.White,
                            rotation,
                            _animation.Origin, 
                            scale,
                            SpriteEffects.None,
                            dept);

            
        }

        public void DrawViewRectangle(SpriteBatch spriteBatch, GameObject parent)
        {
            if (parent.ShowRectangle)
            {

                if (parent._rectangleTexture != null)
                {
                    spriteBatch.Draw(parent._rectangleTexture,
                             parent.Position,
                             null,
                             Color.Red,
                             parent.Rotation,
                             _animation.Origin,
                             parent.Scale,
                             SpriteEffects.None,
                             parent.Dept);

                }

                

            }
        }

        public void setFrameLifePoint(int currentLifepoint, int initLifepoint)
        {
            _animation.AnimationRectangle.Width = (int)((float)(currentLifepoint / (float)initLifepoint) * _animation.InitAnimationRectangle.Width);
            //Console.WriteLine(_animation.AnimationRectangle.Width);
        }

        public void setFrameTabCharge(float speed)
        {
            _animation.AnimationRectangle.Width = (int)((speed / Singleton.MAX_SPEED_FIRE) * _animation.InitAnimationRectangle.Width);
            //Console.WriteLine(_animation.AnimationRectangle.Width);
        }

        public void setFrameMana(int width)
        {
            _animation.AnimationRectangle.Width = width;
            //Console.WriteLine(_animation.AnimationRectangle.Width);
        }

    }

}
