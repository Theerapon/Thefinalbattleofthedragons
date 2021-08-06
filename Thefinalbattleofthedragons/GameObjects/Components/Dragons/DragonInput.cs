using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace Thefinalbattleofthedragons
{
    public class DragonsInput : InputComponent
    {
        private Game currentScene;
        private GameObject dragonfire;
        private bool isAlive = true;

        private float RotationVelocity = 1f;
        private float rotation;
        private float linearVelocity = 0f;

        public KeyboardState PreviousKey, CurrentKey;
        private float timer = 0;

        private enum TURN
        {
            INCREASE,
            DECREASE
        }
        private TURN _currentTurn;

        private enum FORCE
        {
            TRUE,
            FALSE
        }
        private FORCE _currentForce;

        public DragonsInput(Game currentScene) : base(currentScene)
        {
            this.currentScene = currentScene;
        }

        public override void ChangeMappingKey(string Key, Keys newInput)
        {
            base.ChangeMappingKey(Key, newInput);
        }

        public override void ReceiveMessage(int message, Component sender)
        {
            base.ReceiveMessage(message, sender);
        }

        public override void Reset(GameObject parent)
        {
            _currentTurn = TURN.INCREASE;
            _currentForce = FORCE.FALSE;
            switch (parent._currentTeam)
            {
                case GameObject.TEAM.A:
                    InputList["Fire"] = Keys.D;
                    InputList["Specail"] = Keys.Space;
                    InputList["Up"] = Keys.W;
                    InputList["Down"] = Keys.S;
                    break;
                case GameObject.TEAM.B:
                    InputList["Fire"] = Keys.Left;
                    InputList["Specail"] = Keys.Enter;
                    InputList["Down"] = Keys.Up;
                    InputList["Up"] = Keys.Down;
                    break;
            }

            parent.SoundEffects["Ultimate"].Volume = 0.5f;
            parent.SoundEffects["Charge"].Volume = Singleton.Instance.SummonSFXVolume;
            parent.SoundEffects["Rotate"].Volume = Singleton.Instance.SummonSFXVolume;
            parent.SoundEffects["Attack"].Volume = Singleton.Instance.MasterSFXVolume;
            isAlive = true;
            timer = 3f;
            base.Reset(parent);
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects, GameObject parent)
        {
            CurrentKey = Keyboard.GetState();
            
            switch (Singleton.Instance.CurrentGameState)
            {
                case Singleton.GameState.GamePlaying:
                    if (!isAlive) return;
                    //delay 1s for fire agian

                    switch (parent._currentTeam)
                    {
                        case GameObject.TEAM.A:
                            if (CurrentKey.IsKeyDown(InputList["Specail"]) && Singleton.Instance.MANA_TEAM_A >= Singleton.MAX_MANA)
                            {
                                parent.SoundEffects["Ultimate"].Play();
                                for (int i = 0; i < 10; i++)
                                {
                                    GameObject lightning = new GameObject(null, new SpecialSkillPhysics(currentScene), new SpecialSkillGraphics(currentScene))
                                    {
                                        Name = "SPECIAL",
                                        Viewport = new Rectangle(0, 0, 256, 610),
                                    };
                                    lightning._currentTeam = parent._currentTeam;
                                    gameObjects.Add(lightning);
                                    lightning.Reset(lightning);
                                    lightning._currentType = GameObject.TYPE.NONEPASS;
                                    lightning.Position.Y = Singleton.Instance.Random.Next(Singleton.SCREENHEIGHT - Singleton.SCREENHEIGHT / 3);
                                    lightning.Position.X = Singleton.Instance.Random.Next(166) + i * 166;
                                }

                                Singleton.Instance.CurrentGameState = Singleton.GameState.GameAction;
                                Singleton.Instance.MANA_TEAM_A = 0;
                            }
                            break;
                        case GameObject.TEAM.B:
                            if (CurrentKey.IsKeyDown(InputList["Specail"]) && Singleton.Instance.MANA_TEAM_B <= Singleton.MAX_MANA)
                            {
                                parent.SoundEffects["Ultimate"].Play();
                                for (int i = 0; i < 10; i++)
                                {
                                    GameObject lightning = new GameObject(null, new SpecialSkillPhysics(currentScene), new SpecialSkillGraphics(currentScene))
                                    {
                                        Name = "SPECIAL",
                                        Viewport = new Rectangle(0, 0, 256, 610),
                                    };
                                    lightning._currentTeam = parent._currentTeam;
                                    gameObjects.Add(lightning);
                                    lightning.Reset(lightning);
                                    lightning._currentType = GameObject.TYPE.NONEPASS;
                                    lightning.Position.Y = Singleton.Instance.Random.Next(Singleton.SCREENHEIGHT - Singleton.SCREENHEIGHT / 3);
                                    lightning.Position.X = Singleton.Instance.Random.Next(166) + i * 166;
                                }

                                Singleton.Instance.CurrentGameState = Singleton.GameState.GameAction;
                                Singleton.Instance.MANA_TEAM_B = 0;
                            }
                            break;
                    }

                    if (timer > 1f)
                    {
                        //press key fire for increase or decreas velocity
                        if (CurrentKey.IsKeyDown(InputList["Fire"]))
                        {

                            parent.SoundEffects["Charge"].Play();
                            switch (_currentTurn)
                            {
                                case TURN.INCREASE:
                                    linearVelocity += 10;
                                    if (linearVelocity >= Singleton.MAX_SPEED_FIRE) _currentTurn = TURN.DECREASE;
                                    break;
                                case TURN.DECREASE:
                                    linearVelocity -= 10;
                                    if (linearVelocity <= 0) _currentForce = FORCE.TRUE;
                                    break;
                            }

                            parent.gameObjectForDragon["TABCHARG"].LinearVelocity = linearVelocity;
                            //Console.WriteLine(linearVelocity);
                        }


                        //Release key fire or velocity decrease to zero
                        if (PreviousKey.IsKeyDown(InputList["Fire"]) && CurrentKey.IsKeyUp(InputList["Fire"]) || _currentForce.Equals(FORCE.TRUE))
                        {
                            //sent massage for action attack
                            parent.SendMessage(403, this);

                            
                            parent.SoundEffects["Attack"].Play();
                            dragonfire = new GameObject(null, new DragonFirePhysic(currentScene), new DragonFireGraphics(currentScene))
                            {
                                Name = "DRAGONFIRE",
                                Viewport = new Rectangle(0, 0, 96, 72),
                                LinearVelocity = linearVelocity,
                                Rotation = rotation,
                                SoundEffects = new Dictionary<string, SoundEffectInstance>()
                                {
                                    {"Bounce", currentScene.Content.Load<SoundEffect>("Sounds/DragonFireBounce").CreateInstance() },
                                    {"Fire", currentScene.Content.Load<SoundEffect>("Sounds/Fire").CreateInstance() },
                                    {"Start", currentScene.Content.Load<SoundEffect>("Sounds/Start").CreateInstance() },

                                }
                            };
                            dragonfire._currentTeam = parent._currentTeam;
                            gameObjects.Add(dragonfire);
                            dragonfire.Reset(dragonfire);
                            dragonfire._currentType = GameObject.TYPE.NONEPASS;
                            linearVelocity = 0;
                            _currentForce = FORCE.FALSE;
                            _currentTurn = TURN.INCREASE;


                            //after fire timer to 0
                            timer = 0;
                            parent.gameObjectForDragon["TABCHARG"].LinearVelocity = 0;
                        }


                        //when key fired can't move rotation up/down
                        if (CurrentKey.IsKeyDown(InputList["Up"]) && !CurrentKey.IsKeyDown(InputList["Fire"]))
                        {
                            if (rotation >= -1.04)
                            {
                                parent.SoundEffects["Rotate"].Play();
                                this.rotation -= MathHelper.ToRadians(RotationVelocity);
                                parent.gameObjectForDragon["GUILDLINE"].Rotation -= MathHelper.ToRadians(RotationVelocity);
                                Console.WriteLine(MathHelper.ToDegrees(rotation));
                            }


                        }
                        else if (CurrentKey.IsKeyDown(InputList["Down"]) && !CurrentKey.IsKeyDown(InputList["Fire"]))
                        {

                            if (rotation <= 1.04)
                            {
                                parent.SoundEffects["Rotate"].Play();
                                this.rotation += MathHelper.ToRadians(RotationVelocity);
                                parent.gameObjectForDragon["GUILDLINE"].Rotation += MathHelper.ToRadians(RotationVelocity);
                                Console.WriteLine(MathHelper.ToDegrees(rotation));

                            }

                        }

                    }
                    else //when can't fired you can move rotaion up/down
                    {

                        if (CurrentKey.IsKeyDown(InputList["Up"]))
                        {
                            if (rotation >= -1)
                            {

                                this.rotation -= MathHelper.ToRadians(RotationVelocity);
                                parent.gameObjectForDragon["GUILDLINE"].Rotation -= MathHelper.ToRadians(RotationVelocity);
                            }


                        }
                        else if (CurrentKey.IsKeyDown(InputList["Down"]))
                        {

                            if (rotation <= 1)
                            {
                                this.rotation += MathHelper.ToRadians(RotationVelocity);
                                parent.gameObjectForDragon["GUILDLINE"].Rotation += MathHelper.ToRadians(RotationVelocity);
                            }

                        }
                    }

                    timer += (float)gameTime.ElapsedGameTime.Ticks / (float)TimeSpan.TicksPerSecond;
                    break;
                case Singleton.GameState.GameOver:
                    if (timer > 1f)
                    {
                        //Action for gameOver
                        if (PreviousKey.IsKeyDown(InputList["Fire"]) && CurrentKey.IsKeyUp(InputList["Fire"]) || _currentForce.Equals(FORCE.TRUE))
                        {
                            //sent massage for action attack
                            parent.SendMessage(403, this);

                            //after fire timer to 0
                            timer = 0;

                        }
                    }
                    timer += (float)gameTime.ElapsedGameTime.Ticks / (float)TimeSpan.TicksPerSecond;
                    break;
            }

            PreviousKey = CurrentKey;

            base.Update(gameTime, gameObjects, parent);
        }
    }
}
