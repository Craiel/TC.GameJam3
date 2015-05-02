using UnityEngine;
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

    private int weaponsSelected;

    private List<Type> weaponTypes;

    void Awake()
    {
        weaponTypes = new List<Type>(2);
    }

    public void WeaponSelect(string trigger)
    {
        //Have room, trigger animation, update weaponsSelect and save which ones. 
        if(weaponsSelected < 2)
        {
            switch(trigger)
            {
                case "chainTrg":
                    ChainAnimator.SetTrigger(trigger);
                    weaponsSelected++;
                    //t.Add(typeof(Chain));
                    break;
                case "axeTrg":
                    AxeAnimator.SetTrigger(trigger);
                    weaponsSelected++;
                    weaponTypes.Add(typeof(Crossbow));
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
            Debug.Log("You Picked Everything!");
            //GameLoadout a = new GameLoadout();
            //a.SetPlayerWeapons(weaponTypes);
        }
            

    }
}
