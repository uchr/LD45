using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {
    public int initHouses;
    public int currentHouses;

    private void Awake() {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("House");
        initHouses = gameObjects.Length;
        currentHouses = initHouses;
    }

    private void Update() {
        if (currentHouses == 0) {
            Win();
        }
    }

    public void Win() {
        Debug.Log("Win");
    }

    public void Lose() {
        Debug.Log("Lose");
    }
}
