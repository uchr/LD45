using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour {
    [Header("State")]
    public int hp = 2;
    private int prevHP = 2;

    [Header("Spawn")]
    public GameObject spawnPoint;
    public GameObject enemyPrefab;
    public float spawnTime = 1.0f;
    private float spawnTimer = -1.0f;
    private GameObject spawnedEnemy = null;
    private bool isSpawning = true;

    private void Awake() {
        prevHP = hp;
    }

    private void Update() {
        spawnTimer -= Time.deltaTime;
        if (hp != prevHP) {
            Vector4 color = new Vector4(hp / 2.0f, hp / 2.0f, hp / 2.0f, 1.0f);
            GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
            prevHP = hp;
            if (hp == 0)
                --GameObject.Find("@GameLogic").GetComponent<GameLogic>().currentHouses;
        }

        if (hp > 0 && spawnedEnemy == null) {
            if (isSpawning) {
                if (spawnTimer < 0.0f) {
                    spawnedEnemy = Instantiate(enemyPrefab, spawnPoint.transform.position, Quaternion.identity, GameObject.Find("@Characters").transform);
                    isSpawning = false;
                }
            }
            else {
                spawnTimer = spawnTime;
                isSpawning = true;
            }
        }
    }
}
