namespace Assets.Scripts
{
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
    }
}
