using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {
    public TMPro.TMP_Text text;
    
    public int currentHouses;
    public bool isEnd = false;
    
    public int tutorialStage = 0;

    private int initHouses;

    private float time0 = 10000.0f;
    private string help0 = "Hold <b>SPACE</b> to resurrect the deads.";

    private float time1 = 1.5f;
    private string help1 = "Hold <b>LMB</b> to protect yourself.";

    private float time2 = 1.5f;
    private string help2 = "Hold <b>RMB</b> to attack houses.";

    private string goalMessage = "It's time to destroy this city.\nThere are <b>{0}</b> buildings out of <b>{1}</b>.";

    private string graveyardHelp = "Your army has fallen.\nReturn to <b>graveyard</b> to restore it.";

    private float timer = 0.0f;

    private List<float> times = new List<float>();
    private List<string> helps = new List<string>();

    private void Awake() {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("House");
        initHouses = gameObjects.Length;
        currentHouses = initHouses;

        text.text = help0;

        times.Add(time0);
        times.Add(time1);
        times.Add(time2);

        helps.Add(help0);
        helps.Add(help1);
        helps.Add(help2);
    }

    private void Update() {
        if (tutorialStage < times.Count) {
            timer += Time.deltaTime;
            if (tutorialStage < times.Count && timer > times[tutorialStage]) {
                NextStage();
            }
        }
        else {
            GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
            if (playerObjects.Length == 1) {
                text.text = graveyardHelp;
            }
            else {
                text.text = System.String.Format(goalMessage, currentHouses, initHouses);
            }
        }

        if (currentHouses == 0) {
            Win();
        }

        if (isEnd && Input.anyKeyDown) {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
    public void NextStage() {
        if (++tutorialStage >= times.Count)
            return;
        timer = 0.0f;
        text.text = helps[tutorialStage];
    }

    public void ResetTimer() {
        timer = 0.0f;
    }

    public void Win() {
        text.text = "Сity destroyed!\nTank you for playing.\nPress any key to restart.";
        isEnd = true;
    }

    public void Lose() {
        text.text = "You killed.\nPress any key to restart.";
        isEnd = true;
    }
}
