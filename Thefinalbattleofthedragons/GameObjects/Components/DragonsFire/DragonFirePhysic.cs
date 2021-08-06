using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thefinalbattleofthedragons
{
    public class DragonFirePhysic : PhysicsComponent
    {
        Game currentScene;
        private bool isAlive = true;
        private float RotationVelocity;
        private int DAMAGED = 50;

        private float timer = 0;
        private bool pointStart = true;

        private int manaFromSummon = 10;
        private int manaFromDragon = 15;

        public DragonFirePhysic(Game currentScene) : base(currentScene)
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
            parent.SoundEffects["Bounce"].Volume = Singleton.Instance.MasterSFXVolume;
            parent.SoundEffects["Fire"].Volume = Singleton.Instance.MasterSFXVolume;
            parent.SoundEffects["Start"].Volume = Singleton.Instance.MasterSFXVolume;
            RotationVelocity = parent.Rotation;
            switch (parent._currentTeam)
            {
                case GameObject.TEAM.A:
                    parent.Position = new Vector2(360, 426);
                    parent.Velocity.X = parent.LinearVelocity * (float)Math.Cos(parent.Rotation);
                    parent.Velocity.X += Singleton.Instance.WILD * 30; //wild(+) velocity++ but wild(-) velocity--
                    parent.Velocity.Y = parent.LinearVelocity * -(float)Math.Sin(parent.Rotation);
                    break;
                case GameObject.TEAM.B:
                    parent.Position = new Vector2(1304, 426);
                    parent.Velocity.X = parent.LinearVelocity * (float)Math.Cos(parent.Rotation);
                    parent.Velocity.X -= Singleton.Instance.WILD * 30; //wild(+) velocity-- but wild-) velocity++
                    parent.Velocity.Y = parent.LinearVelocity * (float)Math.Sin(parent.Rotation);
                    break;
            }

            isAlive = false;
            pointStart = true;
            base.Reset(parent);
        }



        public override void Update(GameTime gameTime, List<GameObject> gameObjects, GameObject parent)
        { 
            if (!isAlive && pointStart)
            {
                parent.SoundEffects["Start"].Play();
                timer += (float)gameTime.ElapsedGameTime.Ticks / (float)TimeSpan.TicksPerSecond;
                if (timer > 0.2f)
                {
                    isAlive = true;
                    pointStart = false;
                }
                
            }
            else if (!isAlive)
            {
                return;
            }
            else
            {
                parent.SoundEffects["Fire"].Play();
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
                        //dragon fire
                        if (collidedObj.Name.Equals("GROUND") || collidedObj.Name.Equals("DRAGONFIRE") || collidedObj.Name.Equals("METEOR"))
                        {
                            parent.SoundEffects["Bounce"].Play();
                            isAlive = false;
                            parent.SendMessage(301, this); //collided so that is false
                        }

                        if (collidedObj.Name.Equals("SUMMON"))
                        {
                            parent.SoundEffects["Bounce"].Play();
                            parent._currentType = GameObject.TYPE.CANPASS;
                            isAlive = false;
                            collidedObj.Lifepoint -= DAMAGED;
                            parent.SendMessage(301, this); //collided so that is false
                            collidedObj.SendMessage(202, this); //summon hurt
                            switch (parent._currentTeam)
                            {
                                case GameObject.TEAM.A:
                                    Singleton.Instance.MANA_TEAM_A += manaFromSummon;
                                    break;
                                case GameObject.TEAM.B:
                                    Singleton.Instance.MANA_TEAM_B += manaFromSummon;
                                    break;
                            }

                        }

                        if (collidedObj.Name.Equals("DRAGON") || collidedObj.Name.Equals("PORTAL"))
                        {
                            parent.SoundEffects["Bounce"].Play();
                            parent._currentType = GameObject.TYPE.CANPASS;
                            isAlive = false;
                            switch (collidedObj._currentTeam)
                            {
                                case GameObject.TEAM.A:
                                    //mana
                                    Singleton.Instance.MANA_TEAM_B += manaFromDragon;
                                    //lifepoint
                                    Singleton.Instance.LIFEPOINT_TEAM_A -= DAMAGED;
                                    Console.WriteLine("TEAM " + parent._currentTeam + " Atteck to HP_A " + Singleton.Instance.LIFEPOINT_TEAM_A);
                                    break;
                                case GameObject.TEAM.B:
                                    //mana
                                    Singleton.Instance.MANA_TEAM_A += manaFromDragon;
                                    //lifepoint
                                    Singleton.Instance.LIFEPOINT_TEAM_B -= DAMAGED;
                                    Console.WriteLine("TEAM " + parent._currentTeam + " Atteck to HP_B " + Singleton.Instance.LIFEPOINT_TEAM_B);
                                    break;
                            }
                            parent.SendMessage(301, this); //collided so that is false
                            collidedObj.SendMessage(402, this); //sent to dragon

                        }

                    }


                }

                //move
                switch (parent._currentTeam)
                {
                    case GameObject.TEAM.A:
                        parent.Position.X += parent.Velocity.X * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                        parent.Velocity.Y -= parent.Gravity;
                        parent.Position.Y -= parent.Velocity.Y * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                        if (parent.Rotation <= -RotationVelocity) parent.Rotation -= MathHelper.ToRadians(RotationVelocity);
                        break;
                    case GameObject.TEAM.B:
                        parent.Position.X -= parent.Velocity.X * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                        parent.Velocity.Y -= parent.Gravity;
                        parent.Position.Y -= parent.Velocity.Y * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                        if (parent.Rotation >= -RotationVelocity) parent.Rotation -= MathHelper.ToRadians(RotationVelocity);
                        break;
                }

                //check out scean
                if(parent.Position.Y >= 1000)
                {
                    parent.IsActive = false;
                }
            }
            


            base.Update(gameTime, gameObjects, parent);
        }
        
    }
}
