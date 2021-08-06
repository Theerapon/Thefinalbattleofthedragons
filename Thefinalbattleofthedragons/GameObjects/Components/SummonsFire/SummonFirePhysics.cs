using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thefinalbattleofthedragons
{
    public class SummonFirePhysics : PhysicsComponent
    {

        private bool isAlive = true;
        private float timer = 0;
        private int DAMAGED = 10;


        public SummonFirePhysics(Game currentScene) : base(currentScene)
        {

        }

        public override void Draw(SpriteBatch spriteBatch, GameObject parent)
        {
            base.Draw(spriteBatch, parent);
        }

        public override void ReceiveMessage(int message, Component sender)
        {
            //sent from portal
            if( message == 301)
            {
                isAlive = false;
            }
            base.ReceiveMessage(message, sender);
        }

        public override void Reset(GameObject parent)
        {
            //set velocity summonfire
            parent.LinearVelocity = 500f;
            int rnd = Singleton.Instance.Random.Next(30);
            switch (parent._currentTeam)
            {
                case GameObject.TEAM.A:
                    parent.Viewport = new Rectangle(12, 756, 30, 30);
                    parent.Rotation = -MathHelper.ToRadians(rnd);
                    break;
                case GameObject.TEAM.B:
                    parent.Viewport = new Rectangle(612, 756, 30, 30);
                    parent.Rotation = MathHelper.ToRadians(rnd);
                    break;
            }

            parent.Velocity.X = parent.LinearVelocity * (float)Math.Cos(MathHelper.ToRadians(rnd));
            parent.Velocity.Y = parent.LinearVelocity * (float)Math.Sin(MathHelper.ToRadians(rnd));

            parent.SoundEffects["Bounce"].Volume = Singleton.Instance.SummonSFXVolume;
            isAlive = true;
            
            base.Reset(parent);
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects, GameObject parent)
        {

            //check alive
            if (!isAlive) return;
            else
            {
                //loop all object
                foreach (GameObject collidedObj in gameObjects)
                {
                    //check object that this object can pass
                    if (collidedObj._currentType.Equals(GameObject.TYPE.CANPASS)) continue;
                    if (parent == collidedObj) continue;
                    if (parent._currentTeam.Equals(collidedObj._currentTeam)) continue;

                    //check what is This Object colli?
                    if (Collided(parent, collidedObj))
                    {
                        //summon fire
                        if (collidedObj.Name.Equals("SUMMON_FIRE") || collidedObj.Name.Equals("GROUND") || collidedObj.Name.Equals("DRAGONFIRE") || collidedObj.Name.Equals("METEOR"))
                        {
                            parent.SoundEffects["Bounce"].Play();
                            isAlive = false;
                            parent.SendMessage(101, this);
                        }

                        if (collidedObj.Name.Equals("SUMMON"))
                        {
                            parent.SoundEffects["Bounce"].Play();
                            parent._currentType = GameObject.TYPE.CANPASS;
                            isAlive = false;
                            collidedObj.Lifepoint -= DAMAGED;
                            parent.SendMessage(101, this);
                            collidedObj.SendMessage(202, this); //summon hurt

                        }

                        if (collidedObj.Name.Equals("DRAGON") || collidedObj.Name.Equals("PORTAL"))
                        {
                            parent.SoundEffects["Bounce"].Play();
                            parent._currentType = GameObject.TYPE.CANPASS;
                            isAlive = false;
                            switch (collidedObj._currentTeam)
                            {
                                case GameObject.TEAM.A:
                                    Singleton.Instance.LIFEPOINT_TEAM_A -= DAMAGED;
                                    Console.WriteLine("TEAM " + parent._currentTeam + " Atteck to HP_A " + Singleton.Instance.LIFEPOINT_TEAM_A);
                                    break;
                                case GameObject.TEAM.B:
                                    Singleton.Instance.LIFEPOINT_TEAM_B -= DAMAGED;
                                    Console.WriteLine("TEAM " + parent._currentTeam + " Atteck to HP_B " + Singleton.Instance.LIFEPOINT_TEAM_B);
                                    break;
                            }
                            parent.SendMessage(101, this);

                        }

                    }


                }


                //check out scean
                if (parent.Position.X < 0 || parent.Position.X > Singleton.SCREENWIDTH
                    || parent.Position.Y < 0 || parent.Position.Y > Singleton.SCREENHEIGHT)
                {
                    parent.IsActive = false;
                }

                switch (parent._currentTeam)
                {
                    case GameObject.TEAM.A:
                        parent.Position.X += parent.Velocity.X * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                        parent.Position.Y -= parent.Velocity.Y * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                        break;
                    case GameObject.TEAM.B:
                        parent.Position.X -= parent.Velocity.X * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                        parent.Position.Y -= parent.Velocity.Y * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                        break;
                }
            }
            


            base.Update(gameTime, gameObjects, parent);
        }
        
    }
}
