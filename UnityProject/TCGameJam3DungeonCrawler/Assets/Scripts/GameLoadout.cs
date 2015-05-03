namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class GameLoadout : MonoBehaviour
    {
        private static GameLoadout instance;
        public static GameLoadout Instance { get { return instance; } }

        private bool hasSelectedWeapons;

        public List<Type> Weapons {get; private set;}

        private void Awake()
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        public void SetPlayerWeapons(List<Type> weapons)
        {
            Weapons = weapons;
        }
    }
}