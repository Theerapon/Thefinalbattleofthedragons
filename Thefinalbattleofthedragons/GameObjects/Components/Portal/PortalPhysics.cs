using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Thefinalbattleofthedragons
{
    public class PortalPhysics : PhysicsComponent
    {
        
        private Game currentScene;
        private GameObject summon;

        private Vector2 positionSummon;
        private Vector2 velocitySummon;
        private Rectangle reactangleSummon;

        int timeBetweenShots = 10000;
        int shotTimer = 10000;
        private bool isAlive = true;
        private float timer = 0;
        public PortalPhysics(Game currentScene) : base(currentScene)
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
                    reactangleSummon = new Rectangle(0, 120, 96, 96);
                    positionSummon = new Vector2(72, 800);
                    velocitySummon = new Vector2(30, 0);
                    break;
                case GameObject.TEAM.B:
                    reactangleSummon = new Rectangle(504, 120, 96, 96);
                    positionSummon = new Vector2(1592, 800);
                    velocitySummon = new Vector2(-30, 0);
                    break;
            }
            parent.SoundEffects["Summon"].Volume = Singleton.Instance.SummonSFXVolume;
            parent.SoundEffects["Active"].Volume = Singleton.Instance.SummonSFXVolume / 2;
            isAlive = true;
            base.Reset(parent);
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects, GameObject parent)
        {
            if (!isAlive) return;
            else
            {
                parent.SoundEffects["Active"].Play();
                shotTimer += gameTime.ElapsedGameTime.Milliseconds;
                if (shotTimer > timeBetweenShots)
                {
                    shotTimer = 0;
                    parent.SoundEffects["Summon"].Play();
                    summon = new GameObject(null, new SummonPhysics(currentScene), new SummonGraphics(currentScene))
                    {
                        Name = "SUMMON",
                        Viewport = reactangleSummon,
                        Position = positionSummon,
                        Velocity = velocitySummon,
                        Lifepoint = 2,
                        SoundEffects = new Dictionary<string, SoundEffectInstance>()
                            {
                                {"Attack", currentScene.Content.Load<SoundEffect>("Sounds/SummonAttack").CreateInstance() },
                                {"Hurt", currentScene.Content.Load<SoundEffect>("Sounds/SummonHurt").CreateInstance() },
                                {"Dead", currentScene.Content.Load<SoundEffect>("Sounds/SummonDead").CreateInstance() },

                            }
                    };
                    summon._currentTeam = parent._currentTeam;
                    summon._currentType = GameObject.TYPE.NONEPASS;
                    gameObjects.Add(summon);
                    summon.Reset(summon);
                }
            }


            base.Update(gameTime, gameObjects, parent);
        }
        

    }
}
