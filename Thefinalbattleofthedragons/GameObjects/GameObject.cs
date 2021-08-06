using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace Thefinalbattleofthedragons
{
    public class GameObject : ICloneable
    {
        #region PUBLIC_VARIABLES

        public Dictionary<string, GameObject> gameObjectForDragon;
        public Dictionary<string, SoundEffectInstance> SoundEffects;


        public float Dept;
        public Vector2 Position;

        public float Rotation;
        public Vector2 Scale;

        public float LinearVelocity;
        public Vector2 Velocity;
        public Vector2 Acceleration;
        public float Gravity;

        public string Name;

        public bool IsActive;
        public Texture2D _rectangleTexture;
        public Texture2D _bodyTexture;
        public bool ShowRectangle { get; set; }

        public int Lifepoint;

        public enum TEAM
        {
            A,
            B,
            NONE
        }
        public TEAM _currentTeam;

        public enum TYPE
        {
            CANPASS,
            NONEPASS,
        }
        public TYPE _currentType;

        public Rectangle Viewport;

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X - Viewport.Width / 2, (int)Position.Y - Viewport.Height / 2, Viewport.Width, Viewport.Height);
            }
        }

        
        #endregion

        public InputComponent Input;
        public PhysicsComponent Physics;
        public GraphicsComponent Graphics;


        public GameObject(InputComponent input, PhysicsComponent physics, GraphicsComponent graphics)
        {
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Acceleration = Vector2.Zero;
            Velocity = Vector2.Zero;
            Rotation = 0f;
            IsActive = true;
            Input = input;
            Physics = physics;
            Graphics = graphics;
            Dept = 0f;
            LinearVelocity = 0f;
            Gravity = 9.8f;
            Lifepoint = 0;
            _currentTeam = TEAM.NONE;
            _currentType = TYPE.CANPASS;

            
            

        }

        

        public virtual void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
            if (Input != null) Input.Update(gameTime, gameObjects, this);
            if (Physics != null) Physics.Update(gameTime, gameObjects, this);
            if (Graphics != null) Graphics.Update(gameTime, gameObjects, this);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Graphics != null) Graphics.Draw(spriteBatch, this);
        }

        public virtual void Reset(GameObject parent)
        {
            if (Input != null) Input.Reset(parent);
            if (Physics != null) Physics.Reset(parent);
            if (Graphics != null) Graphics.Reset(parent);
        }

        public void SendMessage(int message, Component sender)
        {
            if (Input != null) Input.ReceiveMessage(message, sender);
            if (Physics != null) Physics.ReceiveMessage(message, sender);
            if (Graphics != null) Graphics.ReceiveMessage(message, sender);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }


        


    }


}
