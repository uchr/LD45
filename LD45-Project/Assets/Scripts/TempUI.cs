﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI = UnityEngine.UI;

public class TempUI : MonoBehaviour {
    public Player player;

    public TMPro.TMP_Text housesText;
    public UI.Slider hp;
    public UI.Slider resurrectionTime;

    private GameLogic gameLogic;

    private void Awake() {
        gameLogic = GameObject.Find("@GameLogic").GetComponent<GameLogic>();
    }

    private void Update() {
        housesText.text = "Houses: " + gameLogic.currentHouses + " from " + gameLogic.initHouses;
        hp.value = player.hpPercent;
        resurrectionTime.value = player.resurrectionPercent;
    }
}
