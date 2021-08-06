using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Thefinalbattleofthedragons
{
    public class SummonPhysics : PhysicsComponent
    {
        private Game currentScene;
        private GameObject summonfire;
        private int DAMAGED = 20;

        private bool isAlive = true;
        private Rectangle reactangleSummonFire;


        public SummonPhysics(Game currentScene) : base(currentScene)
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
            
            switch (parent._currentTeam)
            {
                case GameObject.TEAM.A:
                    reactangleSummonFire = new Rectangle(12, 756, 30, 22);
                    break;
                case GameObject.TEAM.B:
                    reactangleSummonFire = new Rectangle(612, 756, 20, 30);
                    break;
            }

            parent.SoundEffects["Attack"].Volume = Singleton.Instance.SummonSFXVolume;
            
            parent.Lifepoint = 20;
            isAlive = true;
            base.Reset(parent);
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects, GameObject parent)
        {
            if (!isAlive) {
                parent._currentType = GameObject.TYPE.CANPASS;
            }
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

                        if (collidedObj.Name.Equals("SUMMON"))
                        {
                            isAlive = false;
                            parent.SendMessage(202, this); //hurt
                            collidedObj.Lifepoint -= DAMAGED;

                        }


                        if (collidedObj.Name.Equals("PORTAL"))
                        {
                            
                            isAlive = false;
                            parent.SendMessage(202, this); //hurt
                            parent.Lifepoint -= DAMAGED;
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

                        }

                    }
                }

                //movement summon
                switch (parent._currentTeam)
                {
                    case GameObject.TEAM.A:
                        parent.Position = parent.Position + parent.Velocity * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                        break;
                    case GameObject.TEAM.B:
                        parent.Position = parent.Position + parent.Velocity * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                        break;
                }

                //random summon fire
                if (Singleton.Instance.Random.Next(500) <= 0 && isAlive)
                {
                    parent.SoundEffects["Attack"].Play();
                    parent.SendMessage(203, this); //attack
                    summonfire = new GameObject(null, new SummonFirePhysics(currentScene), new SummonFireGraphics(currentScene))
                    {
                        Name = "SUMMON_FIRE",
                        Position = new Vector2(parent.Position.X, parent.Position.Y),
                        Viewport = reactangleSummonFire,
                        SoundEffects = new Dictionary<string, SoundEffectInstance>()
                            {
                                {"Bounce", currentScene.Content.Load<SoundEffect>("Sounds/SummonAttackBounce").CreateInstance() },

                            }

                    };
                    summonfire._currentTeam = parent._currentTeam;
                    summonfire._currentType = GameObject.TYPE.NONEPASS;
                    gameObjects.Add(summonfire);
                    summonfire.Reset(summonfire);
                }



                if (parent.Lifepoint <= 0)
                {
                    parent._currentType = GameObject.TYPE.CANPASS;
                    //sent to itself
                    isAlive = false;
                    parent.SendMessage(202, this); //hurt
                }
            }
            
            base.Update(gameTime, gameObjects, parent);
        }
        


    }
}
