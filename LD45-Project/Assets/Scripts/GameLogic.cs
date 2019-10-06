using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {
    public int initHouses;
    public int currentHouses;

    public UnityEngine.UI.Text text;

    public bool isEnd = false;

    private void Awake() {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("House");
        initHouses = gameObjects.Length;
        currentHouses = initHouses;
    }

    private void Update() {
        if (currentHouses == 0) {
            Win();
        }

        if (isEnd && Input.anyKeyDown) {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }

    public void Win() {
        text.gameObject.SetActive(true);
        text.text = "Победа";
        isEnd = true;
    }

    public void Lose() {
        text.gameObject.SetActive(true);
        text.text = "Поражение";
        isEnd = true;
    }
}
