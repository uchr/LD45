using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI = UnityEngine.UI;

public class TempUI : MonoBehaviour {
    public Player player;

    public TMPro.TMP_Text housesText;
    public UI.Slider hp;
    public UI.Slider resurrectionTime;

    private void Update() {
        hp.value = player.hpPercent;
        if (player.resurrectionPercent > 0.01f) {
            resurrectionTime.gameObject.SetActive(true);
            resurrectionTime.value = player.resurrectionPercent;
        }
        else {
            resurrectionTime.gameObject.SetActive(false);
        }
    }
}
