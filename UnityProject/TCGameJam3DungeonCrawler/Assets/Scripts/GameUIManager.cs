using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;

public class GameUIManager : MonoBehaviour
{
    public Game game;
    public Slider slider;

    private int currentHealth;

    void Update()
    {
        if (currentHealth != game.player.HitPoints)
        {
            slider.value = (float)game.player.HitPoints / game.player.TotalHitPoints;
            currentHealth = game.player.HitPoints;
        }
    }
}
