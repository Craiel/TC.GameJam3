using UnityEngine;
using Assets.Scripts;
using Assets.Scripts.Enemy;

public class EnergyOrb : MonoBehaviour
{
    private static int ENERGY_POINT_VALUE = 5;

    [SerializeField]
    private PowerColor powerColor;

    public void DeliverPayload(Player player)
    {
        player.ChargeEnergy(this.powerColor, ENERGY_POINT_VALUE);
        Destroy(this.gameObject);
    }
}