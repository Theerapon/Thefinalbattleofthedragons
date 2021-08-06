using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thefinalbattleofthedragons
{
    public class Animation
    {
        public Rectangle InitAnimationRectangle;
        public Rectangle AnimationRectangle;
        public int CurrentFrame { get; set; }
        public int FrameCount { get; private set; }
        public int FrameHeight { get { return AnimationRectangle.Height; } }
        public float FrameSpeed { get; set; }
        public int FrameWidth { get { return AnimationRectangle.Width / FrameCount; } set { } }
        public bool IsLooping { get; set; }
        public Texture2D Texture { get; private set; }
        public Vector2 Origin;

        public Animation(Texture2D texture, Rectangle rect, int frameCount)
        {
            Texture = texture;
            AnimationRectangle = rect;
            FrameCount = frameCount;
            IsLooping = true;
            FrameSpeed = 0.2f;
            Origin = new Vector2(FrameWidth / 2.0f, FrameHeight / 2.0f);
            InitAnimationRectangle = rect;
        }

    }
}
