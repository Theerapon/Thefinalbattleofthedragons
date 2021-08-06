using Microsoft.Xna.Framework.Input;
using System;


namespace Thefinalbattleofthedragons
{
    class Singleton
    {
        public const int SCREENWIDTH = 1664;
        public const int SCREENHEIGHT = 936;
        public const int TILESIZE = 24;
        public Random Random = new Random();

        public int LIFEPOINT_TEAM_A;
        public int LIFEPOINT_TEAM_B;

        public const int INIT_LIFEPOINT = 1000;
        public const int MAX_SPEED_FIRE = 1500;
        public const int MAX_MANA = 100;

        public int MANA_TEAM_A;
        public int MANA_TEAM_B;

        public bool gameOver = false;

        public float WILD;
       

        public enum GameState
        {
            MainGame,
            SetGAME,
            StartNewGame,
            GamePlaying,
            GameAction,
            GameOver
        }

        public GameState CurrentGameState;

        public enum PlayerWin
        {
            A,
            B,
            Draw
        }
        public PlayerWin playerWin;

        public KeyboardState PreviousKey, CurrentKey;

        public float MasterBGMVolume;
        public float MasterSFXVolume;

        public float SummonSFXVolume;


        

        private static Singleton instance;

        private Singleton() { }

        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }
                return instance;
            }
        }
    }
}
