using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Thefinalbattleofthedragons
{
    public class ManaGraphics : GraphicsComponent
    {
        private int previousMana;
        private Game currentScene;
        private bool isAlive = true;
        private float timer = 0;

        private int width = 488;
        private int height = 20;

        private enum State
        {
            NORMAL,
            FULL
        }
        private State _state;

        public ManaGraphics(Game currentScene) : base(currentScene)
        {
            this.currentScene = currentScene;
            _texture = currentScene.Content.Load<Texture2D>("Images/Motion");
        }

        public override void Draw(SpriteBatch spriteBatch, GameObject parent)
        {
            switch (_state)
            {
                case State.NORMAL:
                    _animationManager.Play(_animations["IsNormal"]);
                    break;
                case State.FULL:
                    _animationManager.Play(_animations["IsFull"]);
                    break;
            }


            base.Draw(spriteBatch, parent);
        }

        public override void ReceiveMessage(int message, Component sender)
        {
            base.ReceiveMessage(message, sender);
        }

        public override void Reset(GameObject parent)
        {
            parent.SoundEffects["Mana"].Volume = 0.8f;
            switch (parent._currentTeam)
            {
                case GameObject.TEAM.A:
                    _animations = new Dictionary<string, Animation>()
                    {
                        {  "IsNormal", new Animation(_texture, new Rectangle(8, 104, width, height), 1) },
                        {  "IsFull", new Animation(_texture, new Rectangle(8, 104, width * 2, height), 2) },
                    };
                    _animations["IsNormal"].Origin = new Vector2(0, 10);
                    _animations["IsFull"].Origin = new Vector2(0, 10);
                    break;
                case GameObject.TEAM.B:
                    _animations = new Dictionary<string, Animation>()
                    {
                        {  "IsNormal", new Animation(_texture, new Rectangle(8, 140, width, height), 1) },
                        {  "IsFull", new Animation(_texture, new Rectangle(8, 140, width * 2, height), 2) },
                    };
                    _animations["IsNormal"].Origin = new Vector2(0, 10);
                    _animations["IsFull"].Origin = new Vector2(0, 10);
                    parent.Rotation = MathHelper.ToRadians(180);
                    break;
            }
            isAlive = true;
            _state = State.NORMAL;
            base.Reset(parent);
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects, GameObject parent)
        {
            if (_state.Equals(State.NORMAL))
            {
                switch (parent._currentTeam)
                {
                    case GameObject.TEAM.A:
                        if (previousMana >= Singleton.MAX_MANA) return;
                        if (Singleton.Instance.MANA_TEAM_A >= Singleton.MAX_MANA) {
                            Singleton.Instance.MANA_TEAM_A = Singleton.MAX_MANA; //check overload mana
                            _state = State.FULL;
                        }
                        _animationManager.setFrameMana((int)((Singleton.Instance.MANA_TEAM_A / 100f) * width)); //percentage mana per width
                        previousMana = Singleton.Instance.MANA_TEAM_A;
                        break;
                        
                case GameObject.TEAM.B:
                        if (previousMana >= Singleton.MAX_MANA) return;
                        if (Singleton.Instance.MANA_TEAM_B >= Singleton.MAX_MANA)
                        {
                            Singleton.Instance.MANA_TEAM_B = Singleton.MAX_MANA; //check overload mana
                            _state = State.FULL;
                        }
                        _animationManager.setFrameMana((int)((Singleton.Instance.MANA_TEAM_B / 100f) * width));
                        previousMana = Singleton.Instance.MANA_TEAM_B;
                        break;
                }
            } else
            {
                parent.SoundEffects["Mana"].Play();
                switch (parent._currentTeam)
                {
                    case GameObject.TEAM.A:

                        if(Singleton.Instance.MANA_TEAM_A <= 0)
                        {
                            _state = State.NORMAL;
                            previousMana = Singleton.Instance.MANA_TEAM_A;
                        }
                        break;
                        
                    case GameObject.TEAM.B:
                        if (Singleton.Instance.MANA_TEAM_B <= 0)
                        {
                            _state = State.NORMAL;
                            previousMana = Singleton.Instance.MANA_TEAM_B;
                        }
                        break;
                }
            }
            
            
            
            base.Update(gameTime, gameObjects, parent);
        }
    }
}
