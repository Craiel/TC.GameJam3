using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Assets.Scripts;

public class WeaponSelectionUIManager : MonoBehaviour
{
    public Animator AxeAnimator;
    public Animator ChainAnimator;
    public Animator CrossbowAnimator;

    [SerializeField]
    private Button startButton;

    private List<Type> weaponTypes = new List<Type>();

    void Start()
    {
        startButton.onClick.AddListener(OnStartClicked);
    }

    public void WeaponSelect(string trigger)
    {
        Animator currentAnimator = null;
        Type currentType = null;

        switch (trigger)
        {
            case "chainTrg":
                currentAnimator = ChainAnimator;
                currentType = typeof(Whip);                
                break;
            case "axeTrg":
                currentAnimator = AxeAnimator;
                currentType = typeof(Axe);                
                break;
            case "crossbowTrg":
                currentAnimator = CrossbowAnimator;
                currentType = typeof(Crossbow);                
                break;
        }

        if(weaponTypes.Contains(currentType))
        {
            weaponTypes.Remove(currentType);
            currentAnimator.SetTrigger(trigger);
        }
        else if(weaponTypes.Count < 2)
        {
            weaponTypes.Add(currentType);
            currentAnimator.SetTrigger(trigger);
        }

        startButton.interactable = (weaponTypes.Count == 2);
    }

    private void OnStartClicked()
    {
        GameLoadout.Instance.SetPlayerWeapons(weaponTypes);
        Application.LoadLevel("GameplayFirstPass");
    }
}
