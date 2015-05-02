using UnityEngine;
using System.Collections;

public class Ladder : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        CharacterMovementController movementController = collider.gameObject.GetComponent<CharacterMovementController>();
        if(movementController != null)
        {
            movementController.HandleLadderEntry(this);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        CharacterMovementController movementController = collider.gameObject.GetComponent<CharacterMovementController>();
        if (movementController != null)
        {
            movementController.HandleLadderExit(this);
        }
    }
}
