using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace Thefinalbattleofthedragons
{
    public class MainScene : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<GameObject> _gameObjects;
        int _numObject;

        Texture2D bgMain;
        Texture2D bgWhite;
        Texture2D bgPlaying;
        SpriteFont _fontGameResult;
        SpriteFont _fontWild;

        Song _bgmPlaying;
        Song _bgmMain;
        SoundEffect _sfxCount;
        SoundEffect _sfxStart;
        SoundEffect _sfxClick;



        //count time to start
        float timeBetweenShots = 4; // 4 seconds time for count to play a game 3 > 2 > 1 > start
        float shotTimer = 0;

        //count time to start for draw
        float timeBetweenOneSecond = 1;
        float countOneSecond = 0;
        int countTime = 3;

        //count time to start for sound
        float countTimeSound = 1;
        float countSound = 0;

        float timer = 0;


        public MainScene()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = Singleton.SCREENWIDTH;
            graphics.PreferredBackBufferHeight = Singleton.SCREENHEIGHT;
            graphics.ApplyChanges();

            _gameObjects = new List<GameObject>();
            Singleton.Instance.CurrentGameState = Singleton.GameState.MainGame;
     
            

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            bgPlaying = this.Content.Load<Texture2D>("Images/Background");
            _fontGameResult = Content.Load<SpriteFont>("Fonts/FontFFF");
            _fontWild = Content.Load<SpriteFont>("Fonts/FontFFF18");
            bgWhite = this.Content.Load<Texture2D>("Images/White");
            bgMain = this.Content.Load<Texture2D>("Images/Main");

            _bgmMain = Content.Load<Song>("Sounds/BGMMain2");
            _bgmPlaying = Content.Load<Song>("Sounds/BGMPlaying");
            _sfxCount = Content.Load<SoundEffect>("Sounds/Count");
            _sfxStart = Content.Load<SoundEffect>("Sounds/Start");

            _sfxClick = Content.Load<SoundEffect>("Sounds/Click");

            Singleton.Instance.MasterSFXVolume = 0.15f;
            Singleton.Instance.SummonSFXVolume = 0.1f;
            Singleton.Instance.MasterBGMVolume = 0.1f;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = Singleton.Instance.MasterBGMVolume;
            MediaPlayer.Play(_bgmMain);

            // TODO: use this.Content to load your game content here
        }

        
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        
        protected override void Update(GameTime gameTime)
        {
            Singleton.Instance.CurrentKey = Keyboard.GetState();
            _numObject = _gameObjects.Count;
            //Console.WriteLine("numobject" +  _numObject);

            switch (Singleton.Instance.CurrentGameState)
            {
                case Singleton.GameState.MainGame:
                    //Space keys pressed to start
                    if (!Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey) && Singleton.Instance.CurrentKey.IsKeyDown(Keys.Space))
                    {
                        _sfxClick.Play(Singleton.Instance.MasterSFXVolume, pitch: 0.0f, pan: 0.0f);
                        countTime = 3;
                        _gameObjects.Clear();
                        Singleton.Instance.CurrentGameState = Singleton.GameState.SetGAME;
                        MediaPlayer.Stop();

                    }

                    break;
                case Singleton.GameState.SetGAME:
                    Reset();
                    countSound = 0;
                    Singleton.Instance.CurrentGameState = Singleton.GameState.StartNewGame;
                    break;
                case Singleton.GameState.StartNewGame:
                    //update graphics
                    for (int i = 0; i < _numObject; i++)
                    {
                        if (_gameObjects[i].IsActive && _gameObjects[i].Graphics != null) _gameObjects[i].Graphics.Update(gameTime, _gameObjects, _gameObjects[i]);
                    }

                    //sound count
                    countTimeSound += gameTime.ElapsedGameTime.Ticks / (float)TimeSpan.TicksPerSecond;
                    if (countTimeSound >= 1 && countSound <= 2)
                    {
                        countTimeSound = 0;
                        countSound++;
                        _sfxCount.Play(Singleton.Instance.MasterSFXVolume, pitch: 0.0f, pan: 0.0f);
                    } else if (countTimeSound >= 1 && countSound <= 3)
                    {
                        countTimeSound = 0;
                        countSound++;
                        _sfxStart.Play(Singleton.Instance.MasterSFXVolume, pitch: 0.0f, pan: 0.0f);
                    }

                    //time count
                    shotTimer += (float)gameTime.ElapsedGameTime.Ticks / (float)TimeSpan.TicksPerSecond;
                    if (shotTimer >= timeBetweenShots)
                    {
                        shotTimer = 0;
                        MediaPlayer.IsRepeating = true;
                        MediaPlayer.Volume = Singleton.Instance.MasterBGMVolume;
                        MediaPlayer.Play(_bgmPlaying);
                        Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
                    }
                    break;
                case Singleton.GameState.GamePlaying:



                    //loop update all objects
                    for (int i = 0; i < _numObject; i++)
                    {
                        if (_gameObjects[i].IsActive) _gameObjects[i].Update(gameTime, _gameObjects);
                    }

                    //loop Remove object non active
                    for (int i = 0; i < _numObject; i++)
                    {
                        if (!_gameObjects[i].IsActive)
                        {
                            _gameObjects.RemoveAt(i);
                            i--;
                            _numObject--;
                        }
                    }

                    if (Singleton.Instance.gameOver)
                    {
                        MediaPlayer.IsRepeating = true;
                        MediaPlayer.Volume = Singleton.Instance.MasterBGMVolume;
                        MediaPlayer.Play(_bgmMain);
                        Singleton.Instance.CurrentGameState = Singleton.GameState.GameOver;
                    }

                    break;
                case Singleton.GameState.GameAction:
                    
                    //update lightning
                    for (int i = 0; i < _numObject; i++)
                    {
                        if (_gameObjects[i].IsActive && _gameObjects[i].Graphics != null && _gameObjects[i].Name.Equals("SPECIAL")) _gameObjects[i].Graphics.Update(gameTime, _gameObjects, _gameObjects[i]);
                    }
                    

                    timer += (float)gameTime.ElapsedGameTime.Ticks / (float)TimeSpan.TicksPerSecond;
                    if (timer >= 0.4f)
                    {
                        timer = 0;

                        Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
                    }
                    break;
                case Singleton.GameState.GameOver:
                    //loop update all objects
                    for (int i = 0; i < _numObject; i++)
                    {
                        if (_gameObjects[i].IsActive && _gameObjects[i].Name.Equals("DRAGON")) _gameObjects[i].Update(gameTime, _gameObjects);
                    }

                    //Space keys pressed to start
                    if (!Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey) && Singleton.Instance.CurrentKey.IsKeyDown(Keys.Space))
                    {
                        
                        Singleton.Instance.CurrentGameState = Singleton.GameState.MainGame;
                    }
                    break;
            }


            // TODO: Add your update logic here
            Singleton.Instance.PreviousKey = Singleton.Instance.CurrentKey;
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            Vector2 fontSize;

            //set background each GameState
            switch (Singleton.Instance.CurrentGameState)
            {
                case Singleton.GameState.MainGame:
                    //draw backgroud
                    spriteBatch.Draw(bgMain, new Vector2(0, 0), Color.White);
                    break;
                case Singleton.GameState.StartNewGame:
                    //draw backgroud
                    spriteBatch.Draw(bgPlaying, new Vector2(0, 0), Color.White);

                    //Draw all objects
                    for (int i = 0; i < _gameObjects.Count; i++)
                    {
                        _gameObjects[i].Draw(spriteBatch);
                    }

                    countOneSecond += (float)gameTime.ElapsedGameTime.Ticks / (float)TimeSpan.TicksPerSecond;
                    if (countOneSecond >= timeBetweenOneSecond)
                    {
                        countOneSecond = 0;
                        countTime--;
                        
                    }

                    if(countTime <= 0)
                    {

                        fontSize = _fontGameResult.MeasureString("START");
                        spriteBatch.DrawString(_fontGameResult, "START", new Vector2((Singleton.SCREENWIDTH - fontSize.X) / 2, (Singleton.SCREENHEIGHT - fontSize.Y) / 2), Color.WhiteSmoke);
                    } else
                    {
                        fontSize = _fontGameResult.MeasureString(countTime.ToString());
                        spriteBatch.DrawString(_fontGameResult, countTime.ToString(), new Vector2((Singleton.SCREENWIDTH - fontSize.X) / 2, (Singleton.SCREENHEIGHT - fontSize.Y) / 2), Color.WhiteSmoke);
                    }

                    //font wild
                    fontSize = _fontWild.MeasureString(Singleton.Instance.WILD.ToString());
                    spriteBatch.DrawString(_fontWild, Singleton.Instance.WILD.ToString(), new Vector2((Singleton.SCREENWIDTH - fontSize.X) / 2, 64), Color.White);

                    break;
                case Singleton.GameState.GamePlaying:
                    //draw backgroud
                    spriteBatch.Draw(bgPlaying, new Vector2(0, 0), Color.White);

                    //Draw all objects
                    for (int i = 0; i < _gameObjects.Count; i++)
                    {
                        _gameObjects[i].Draw(spriteBatch);
                    }

                    //font wild
                    fontSize = _fontWild.MeasureString(Singleton.Instance.WILD.ToString());
                    spriteBatch.DrawString(_fontWild, Singleton.Instance.WILD.ToString(), new Vector2((Singleton.SCREENWIDTH - fontSize.X) / 2, 64), Color.White);

                    break;
                case Singleton.GameState.GameAction:
                    spriteBatch.Draw(bgPlaying, new Vector2(0, 0), Color.White);
                    spriteBatch.Draw(bgWhite, new Vector2(0, 0), Color.White);

                    //Draw all objects
                    for (int i = 0; i < _gameObjects.Count; i++)
                    {
                        _gameObjects[i].Draw(spriteBatch);
                    }

                    //font wild
                    fontSize = _fontWild.MeasureString(Singleton.Instance.WILD.ToString());
                    spriteBatch.DrawString(_fontWild, Singleton.Instance.WILD.ToString(), new Vector2((Singleton.SCREENWIDTH - fontSize.X) / 2, 64), Color.White);

                    break;
                case Singleton.GameState.GameOver:
                    switch (Singleton.Instance.playerWin)
                    {
                        case Singleton.PlayerWin.A:
                            for (int i = 0; i < _gameObjects.Count; i++)
                            {
                                if(_gameObjects[i].Name.Equals("DRAGON")) {
                                    _gameObjects[i].Draw(spriteBatch);
                                }
                            }
                            fontSize = _fontGameResult.MeasureString("The dragon of West WIN");
                            spriteBatch.DrawString(_fontGameResult, "The dragon of West WIN", new Vector2((Singleton.SCREENWIDTH - fontSize.X) / 2, (Singleton.SCREENHEIGHT - fontSize.Y) / 2), Color.WhiteSmoke);
                            break;
                        case Singleton.PlayerWin.B:
                            for (int i = 0; i < _gameObjects.Count; i++)
                            {
                                if (_gameObjects[i].Name.Equals("DRAGON"))
                                {
                                    _gameObjects[i].Draw(spriteBatch);
                                }
                            }
                            fontSize = _fontGameResult.MeasureString("The dragon of East WIN");
                            spriteBatch.DrawString(_fontGameResult, "The dragon of East WIN", new Vector2((Singleton.SCREENWIDTH - fontSize.X ) / 2, (Singleton.SCREENHEIGHT - fontSize.Y) / 2), Color.WhiteSmoke);
                            break;
                        case Singleton.PlayerWin.Draw:
                            for (int i = 0; i < _gameObjects.Count; i++)
                            {
                                if (_gameObjects[i].Name.Equals("DRAGON"))
                                {
                                    _gameObjects[i].Draw(spriteBatch);
                                }
                            }
                            fontSize = _fontGameResult.MeasureString("You DRAW");
                            spriteBatch.DrawString(_fontGameResult, "You DRAW", new Vector2((Singleton.SCREENWIDTH - fontSize.X) / 2, (Singleton.SCREENHEIGHT - fontSize.Y) / 2), Color.WhiteSmoke);
                            break;
                    }
                    break;
            }


            spriteBatch.End();
            graphics.BeginDraw();

            base.Draw(gameTime);
        }

        private void Reset()
        {
            Singleton.Instance.gameOver = false;

            Singleton.Instance.LIFEPOINT_TEAM_A = 1000;
            Singleton.Instance.LIFEPOINT_TEAM_B = 1000;

            Singleton.Instance.MANA_TEAM_A = 0;
            Singleton.Instance.MANA_TEAM_B = 0;


            _gameObjects.Clear();

            //ground
            GameObject ground = new GameObject(null, null, new GroudGraphics(this))
            {
                Name = "GROUND",
                Viewport = new Rectangle(648, 420, 72, 72),

            };
            ground._currentType = GameObject.TYPE.NONEPASS;

            //clone ground
            for (int i = 0; i < 24; i++)
            {
                var clone = ground.Clone() as GameObject;
                clone.Position = new Vector2(i * 72, 900);
                _gameObjects.Add(clone);

            }

            //meteor
            GameObject meteorTop = new GameObject(null, null, new PowerHoleGraphics(this, 1))
            {
                Name = "METEOR",
                Viewport = new Rectangle(0, 214, 144, 144),
            };
            meteorTop._currentType = GameObject.TYPE.NONEPASS;
            _gameObjects.Add(meteorTop);

            GameObject meteorCenter = new GameObject(null, null, new PowerHoleGraphics(this, 2))
            {
                Name = "METEOR",
                Viewport = new Rectangle(0, 214, 144, 144),
            };
            meteorCenter._currentType = GameObject.TYPE.NONEPASS;
            _gameObjects.Add(meteorCenter);

            //init portal Team B
            GameObject portal_B = new GameObject(null, new PortalPhysics(this), new PortalGraphics(this))
            {
                Name = "PORTAL",
                Viewport = new Rectangle(588, 12, 72, 120),
                Position = new Vector2(1592, 800),
                SoundEffects = new Dictionary<string, SoundEffectInstance>()
                            {
                                {"Summon", Content.Load<SoundEffect>("Sounds/Summon").CreateInstance() },
                                {"Active", Content.Load<SoundEffect>("Sounds/Portal").CreateInstance() },

                            }

            };
            portal_B._currentTeam = GameObject.TEAM.B;
            portal_B._currentType = GameObject.TYPE.NONEPASS;
            _gameObjects.Add(portal_B);

            
            //init dragon TeamB
            GameObject dragon_B = new GameObject(new DragonsInput(this), new DragonsPhysic(this), new DragonGraphics(this))
            {
                Name = "DRAGON",
                Viewport = new Rectangle(0, 0, 360, 432),
                Position = new Vector2(1484, 450),
                SoundEffects = new Dictionary<string, SoundEffectInstance>()
                            {
                                {"Ultimate", Content.Load<SoundEffect>("Sounds/Thunder").CreateInstance() },
                                {"Attack", Content.Load<SoundEffect>("Sounds/DragonAttack").CreateInstance() },
                                {"Hurt", Content.Load<SoundEffect>("Sounds/DragonHurt").CreateInstance() },
                                {"Dead", Content.Load<SoundEffect>("Sounds/DragonDead").CreateInstance() },
                                {"Charge", Content.Load<SoundEffect>("Sounds/Charge").CreateInstance() },
                                {"Rotate", Content.Load<SoundEffect>("Sounds/Fire").CreateInstance() },

                            }
            };
            dragon_B._currentTeam = GameObject.TEAM.B;
            dragon_B._currentType = GameObject.TYPE.NONEPASS;
            _gameObjects.Add(dragon_B);
            

            //tabCharge
            GameObject tabCharge_B = new GameObject(null, null, new TabChargeGraphics(this))
            {
                Name = "TABCHARG",
                Viewport = new Rectangle(0, 480, 0, 144),
                Position = new Vector2(1304, 426)
            };
            tabCharge_B._currentTeam = GameObject.TEAM.B;
            _gameObjects.Add(tabCharge_B);

            //Lifepoint_B
            GameObject lifepoint_B = new GameObject(null, new LifepointsPhysic(this), new LifepointsGraphics(this))
            {
                Name = "HP",
                Viewport = new Rectangle(8, 56, 608, 32),
                Position = new Vector2(1616, 72)
            };
            lifepoint_B._currentTeam = GameObject.TEAM.B;
            _gameObjects.Add(lifepoint_B);

            //Mana_B
            GameObject mana_B = new GameObject(null, new ManaPhysics(this), new ManaGraphics(this))
            {
                Name = "MANA",
                Viewport = new Rectangle(8, 104, 488, 20),
                Position = new Vector2(1616, 120),
                SoundEffects = new Dictionary<string, SoundEffectInstance>()
                            {
                                {"Mana", Content.Load<SoundEffect>("Sounds/Mana").CreateInstance() },

                            }
            };
            mana_B._currentTeam = GameObject.TEAM.B;
            _gameObjects.Add(mana_B);

            //GuildLine_B
            GameObject guildLine_B = new GameObject(null, new GuildeLinePhysic(this), new GuildLineGraphics(this))
            {
                Name = "GUILDLINE",
                Viewport = new Rectangle(576, 622, 144, 10),
                Position = new Vector2(1304, 426)
            };
            guildLine_B._currentTeam = GameObject.TEAM.B;
            _gameObjects.Add(guildLine_B);

            //add object to dictionary of dragon
            dragon_B.gameObjectForDragon = new Dictionary<string, GameObject>();
            dragon_B.gameObjectForDragon.Add("TABCHARG", tabCharge_B);
            dragon_B.gameObjectForDragon.Add("LIFEPOINT", lifepoint_B);
            dragon_B.gameObjectForDragon.Add("MANA", mana_B);
            dragon_B.gameObjectForDragon.Add("GUILDLINE", guildLine_B);


            //init portal Team A
            GameObject portal_A = new GameObject(null, new PortalPhysics(this), new PortalGraphics(this))
            {
                Name = "PORTAL",
                Viewport = new Rectangle(12, 12, 72, 120),
                Position = new Vector2(72, 800),
                SoundEffects = new Dictionary<string, SoundEffectInstance>()
                            {
                                {"Summon", Content.Load<SoundEffect>("Sounds/Summon").CreateInstance() },
                                {"Active", Content.Load<SoundEffect>("Sounds/Portal").CreateInstance() },

                            }

            };
            portal_A._currentTeam = GameObject.TEAM.A;
            portal_A._currentType = GameObject.TYPE.NONEPASS;
            _gameObjects.Add(portal_A);


            //init dragon TeamA
            GameObject dragon_A = new GameObject(new DragonsInput(this), new DragonsPhysic(this), new DragonGraphics(this))
            {
                Name = "DRAGON",
                Viewport = new Rectangle(0, 0, 360, 432),
                Position = new Vector2(180, 450),
                SoundEffects = new Dictionary<string, SoundEffectInstance>()
                            {
                                {"Ultimate", Content.Load<SoundEffect>("Sounds/Thunder").CreateInstance() },
                                {"Attack", Content.Load<SoundEffect>("Sounds/DragonAttack").CreateInstance() },
                                {"Hurt", Content.Load<SoundEffect>("Sounds/DragonHurt").CreateInstance() },
                                {"Dead", Content.Load<SoundEffect>("Sounds/DragonDead").CreateInstance() },
                                {"Charge", Content.Load<SoundEffect>("Sounds/Charge").CreateInstance() },
                                {"Rotate", Content.Load<SoundEffect>("Sounds/Fire").CreateInstance() },

                            }
            };
            dragon_A._currentTeam = GameObject.TEAM.A;
            dragon_A._currentType = GameObject.TYPE.NONEPASS;
            _gameObjects.Add(dragon_A);

            //tabCharge
            GameObject tabCharge_A = new GameObject(null, null, new TabChargeGraphics(this))
            {
                Name = "TABCHARG",
                Viewport = new Rectangle(0, 480, 0, 144),
                Position = new Vector2(360, 426)
            };
            tabCharge_A._currentTeam = GameObject.TEAM.A;
            _gameObjects.Add(tabCharge_A);

            //Lifepoint_A
            GameObject lifepoint_A = new GameObject(null, new LifepointsPhysic(this), new LifepointsGraphics(this))
            {
                Name = "LIFEPOINT",
                Viewport = new Rectangle(8, 8, 608, 32),
                Position = new Vector2(48, 72)
            };
            lifepoint_A._currentTeam = GameObject.TEAM.A;
            _gameObjects.Add(lifepoint_A);

            //Mana_A
            GameObject mana_A = new GameObject(null, new ManaPhysics(this), new ManaGraphics(this))
            {
                Name = "MANA",
                Viewport = new Rectangle(8, 104, 488, 20),
                Position = new Vector2(48, 120),
                SoundEffects = new Dictionary<string, SoundEffectInstance>()
                            {
                                {"Mana", Content.Load<SoundEffect>("Sounds/Mana").CreateInstance() },

                            }
            };
            mana_A._currentTeam = GameObject.TEAM.A;
            _gameObjects.Add(mana_A);


            //GuildLine_A
            GameObject guildLine_A = new GameObject(null, new GuildeLinePhysic(this), new GuildLineGraphics(this))
            {
                Name = "GUILDLINE",
                Viewport = new Rectangle(576, 622, 144, 10),
                Position = new Vector2(360, 426)
            };
            guildLine_A._currentTeam = GameObject.TEAM.A;
            _gameObjects.Add(guildLine_A);

            //add object to dictionary of dragon
            dragon_A.gameObjectForDragon = new Dictionary<string, GameObject>();
            dragon_A.gameObjectForDragon.Add("TABCHARG", tabCharge_A);
            dragon_A.gameObjectForDragon.Add("LIFEPOINT", lifepoint_A);
            dragon_A.gameObjectForDragon.Add("MANA", mana_A);
            dragon_A.gameObjectForDragon.Add("GUILDLINE", guildLine_A);

            //WILD
            GameObject wild = new GameObject(null, new WildPhysics(this), new WildGraphics(this))
            {
                Name = "WILD",
                Viewport = new Rectangle(152, 444, 96, 48),
                Position = new Vector2((Singleton.SCREENWIDTH / 2), 72)
            };
            wild._currentTeam = GameObject.TEAM.NONE;
            _gameObjects.Add(wild);

            foreach (GameObject s in _gameObjects)
            {
                s.Reset(s);
            }
        }

        
    }
}
