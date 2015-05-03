using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;

public class GameUIManager : MonoBehaviour
{
    [SerializeField]
    private Game game;

    [SerializeField]
    private Slider slider;

    [SerializeField]
    private WeaponIndicatorUI weapon1;

    [SerializeField]
    private WeaponIndicatorUI weapon2;

    [SerializeField]
    private Text redStream;

    [SerializeField]
    private Text greenStream;

    [SerializeField]
    private Text blueStream;
    
    private int currentHealth;

    private bool hasSetLoadout;

    void Update()
    {
        if(!hasSetLoadout && this.game.player.HasWeaponLoadout)
        {
            this.weapon1.Init(this.game.player.PrimaryWeapon);
            this.weapon2.Init(this.game.player.SecondaryWeapon);
            this.hasSetLoadout = true;
        }

        if (currentHealth != game.player.HitPoints)
        {
            slider.value = (float)game.player.HitPoints / game.player.TotalHitPoints;
            currentHealth = game.player.HitPoints;
        }
    }
}
