using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WeaponIndicatorUI : MonoBehaviour
{
    [SerializeField]
    private RawImage image;

    [SerializeField]
    private Slider redBar;

    [SerializeField]
    private Slider greenBar;

    [SerializeField]
    private Slider blueBar;

    [SerializeField]
    private Texture axe;

    [SerializeField]
    private Texture whip;

    [SerializeField]
    private Texture crossbow;

    [SerializeField]
    private RawImage selected;

    private Weapon weapon;

    private bool isInitialized;

    public void Init(Weapon weapon)
    {
        if (weapon is Axe)
        {
            this.image.texture = this.axe;
        }
        else if (weapon is Whip)
        {
            this.image.texture = this.whip;
        }
        else if (weapon is Crossbow)
        {
            this.image.texture = this.crossbow;
        }
        this.weapon = weapon;
        this.selected.enabled = this.weapon.IsActive;
        this.isInitialized = true;
    }

    private void Update()
    {
        if (this.isInitialized)
        {
            this.redBar.value = this.weapon.EnergyStreams[Assets.Scripts.PowerColor.Red] / Weapon.ENERGY_STREAM_MAX;
            this.greenBar.value = this.weapon.EnergyStreams[Assets.Scripts.PowerColor.Green] / Weapon.ENERGY_STREAM_MAX;
            this.blueBar.value = this.weapon.EnergyStreams[Assets.Scripts.PowerColor.Blue] / Weapon.ENERGY_STREAM_MAX;

            if(this.weapon.IsActive != this.selected.enabled)
            {
                this.selected.enabled = this.weapon.IsActive;
            }
        }
    }
}