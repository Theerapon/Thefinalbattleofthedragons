using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thefinalbattleofthedragons
{
    public class SpecialSkillPhysics : PhysicsComponent
    {
        private bool isAlive = true;
        private float timer = 0;


        private int DAMAGED = 45;

        public SpecialSkillPhysics(Game currentScene) : base(currentScene)
        {
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
                    parent.Velocity = new Vector2(700, parent.LinearVelocity);
                    break;
                case GameObject.TEAM.B:
                    parent.Velocity = new Vector2(700, parent.LinearVelocity);
                    break;
            }
            timer = 1f;
            base.Reset(parent);
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects, GameObject parent)
        {
            if (!isAlive)
            {
                return;
            } else
            {
                timer += (float)gameTime.ElapsedGameTime.Ticks / (float)TimeSpan.TicksPerSecond;
                if (timer >= 1f)
                {
                    timer = 0;
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

                            if (collidedObj.Name.Equals("GROUND"))
                            {
                                //isAlive = false;
                            }

                            if (collidedObj.Name.Equals("SUMMON"))
                            {
                                parent._currentType = GameObject.TYPE.CANPASS;
                                //isAlive = false;
                                collidedObj.Lifepoint -= DAMAGED;
                                collidedObj.SendMessage(202, this); //summon hurt

                            }

                            if (collidedObj.Name.Equals("DRAGON") || collidedObj.Name.Equals("PORTAL"))
                            {
                                parent._currentType = GameObject.TYPE.CANPASS;
                                //isAlive = false;
                                switch (collidedObj._currentTeam)
                                {
                                    case GameObject.TEAM.A:
                                        //lifepoint
                                        Singleton.Instance.LIFEPOINT_TEAM_A -= DAMAGED;
                                        break;
                                    case GameObject.TEAM.B:
                                        //lifepoint
                                        Singleton.Instance.LIFEPOINT_TEAM_B -= DAMAGED;
                                        break;
                                }
                                collidedObj.SendMessage(402, this); //sent to dragon

                            }

                        }
                    }
                }
                

            }

            
            base.Update(gameTime, gameObjects, parent);
        }
        
    }
}
