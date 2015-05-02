namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class GameLoadout : MonoBehaviour
    {
        public void Update()
        {
            if (Input.GetAxis("Submit") > 0)
            {
                Application.LoadLevel("GameplayFirstPass");
            }
        }

        public void SetPlayerWeapons(List<Type> weapons)
        {

        }
    }
}
