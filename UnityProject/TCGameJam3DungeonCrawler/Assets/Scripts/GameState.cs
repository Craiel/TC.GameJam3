namespace Assets.Scripts
{
    using System.Collections.Generic;

    public class GameState
    {
        private static GameState instance;

        private readonly IDictionary<string, bool> states;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public GameState()
        {
            this.states = new Dictionary<string, bool>();
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public static GameState Instance
        {
            get
            {
                return instance ?? (instance = new GameState());
            }
        }

        public bool GetState(string key)
        {
            if (!this.states.ContainsKey(key))
            {
                return false;
            }

            return this.states[key];
        }

        public void SetState(string key, bool value = true)
        {
            if (!this.states.ContainsKey(key))
            {
                this.states.Add(key, value);
            }
            else
            {
                this.states[key] = value;
            }
        }
    }
}
