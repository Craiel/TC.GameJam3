using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;

public class UIManager : MonoBehaviour
{

    public Animator AxeAnimator;
    public Animator ChainAnimator;
    public Animator CrossbowAnimator;

    public GameLoadout gameLoadout;

    public Game game;

    public Slider slider;

    private int weaponsSelected;
    private int currentHealth;

    private List<Type> weaponTypes = new List<Type>();

    void Start()
    {
        currentHealth = game.player.HitPoints;
        slider.value = 1f;
    }

    void Update()
    {
        if(currentHealth != game.player.HitPoints)
        {

            slider.value = (float)game.player.HitPoints / game.player.TotalHitPoints;
            currentHealth = game.player.HitPoints;
        }
    }

    public void WeaponSelect(string trigger)
    {
        Debug.Log("triggered weapon select");
        //Have room, trigger animation, update weaponsSelect and save which ones. 
        if (weaponsSelected < 2)
        {
            switch (trigger)
            {
                case "chainTrg":
                    ChainAnimator.SetTrigger(trigger);
                    weaponsSelected++;
                    weaponTypes.Add(typeof(Whip));
                    break;
                case "axeTrg":
                    AxeAnimator.SetTrigger(trigger);
                    weaponsSelected++;
                    weaponTypes.Add(typeof(Axe));
                    break;
                case "crossbowTrg":
                    CrossbowAnimator.SetTrigger(trigger);
                    weaponsSelected++;
                    weaponTypes.Add(typeof(Crossbow));
                    break;

            }

        }
        if (weaponsSelected == 2)
        {
            //GameLoadout a = new GameLoadout();
            gameLoadout.SetPlayerWeapons(weaponTypes);
        }


    }
}
