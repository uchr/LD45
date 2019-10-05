using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI = UnityEngine.UI;

public class TempUI : MonoBehaviour {
    public Player player;

    public TMPro.TMP_Text playerHP;
    public TMPro.TMP_Text playerMoney;
    public UI.Slider resurrectionTime;

    private void Update() {
        playerHP.text = player.GetComponent<CharachterState>().hp.ToString();
        playerMoney.text = player.money.ToString();
        resurrectionTime.value = player.resurrectionPercent;
    }
}
