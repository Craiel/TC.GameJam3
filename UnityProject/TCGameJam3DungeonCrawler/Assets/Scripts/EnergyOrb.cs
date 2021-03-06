﻿using UnityEngine;
using Assets.Scripts;
using Assets.Scripts.Enemy;
using System.Collections;

public class EnergyOrb : MonoBehaviour
{
    private static int ENERGY_POINT_VALUE = 5;

    [SerializeField]
    private Material red;

    [SerializeField]
    private Material green;

    [SerializeField]
    private Material blue;

    [SerializeField]
    private PowerColor powerColor;

    [SerializeField]
    private MeshRenderer meshRenderer;

    [SerializeField]
    private Light glowLight;

    private bool isInitialized;

    public void Init(PowerColor powerColor)
    {
        switch (powerColor)
        {
            case PowerColor.Red: this.meshRenderer.material = this.red; break;
            case PowerColor.Green: this.meshRenderer.material = this.green; break;
            case PowerColor.Blue: this.meshRenderer.material = this.blue; break;
        }

        this.glowLight.color = this.meshRenderer.material.color;
        this.isInitialized = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(this.isInitialized)
        {
            Player player = other.GetComponent<Player>();
            if(player != null)
            {
                player.ChargeEnergy(this.powerColor, ENERGY_POINT_VALUE);
                Destroy(this.gameObject);
            }
        }
    }
}