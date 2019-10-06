using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour {
    [Header("State")]
    public int hp = 2;
    private int prevHP = 2;
    private float maxHP;
    private float initIntensity;

    [Header("Spawn")]
    public GameObject spawnPoint;
    public GameObject enemyPrefab;
    public float spawnTime = 1.0f;
    public Light lamp;
    
    private float spawnTimer = -1.0f;
    private GameObject[] spawnedEnemes = new GameObject[3];
    private bool isSpawning = true;

    private void Awake() {
        prevHP = hp;
        maxHP = hp;
        initIntensity = lamp.intensity;
    }

    private void Update() {
        spawnTimer -= Time.deltaTime;
        if (hp != prevHP) {
            float intensity = hp / maxHP;
            Vector4 color = new Vector4(intensity, intensity, intensity, 1.0f);
            lamp.intensity = initIntensity * intensity;
            GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
            lamp.transform.parent.GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
            prevHP = hp;
            if (hp == 0)
                --GameObject.Find("@GameLogic").GetComponent<GameLogic>().currentHouses;
        }

        int aliveEnemies = 0;
        for (int i = 0; i < spawnedEnemes.Length; ++i)
            if (spawnedEnemes[i] != null)
                ++aliveEnemies;

        if (hp > 0 && aliveEnemies == 0) {
            if (isSpawning) {
                if (spawnTimer < 0.0f) {
                    for (int i = 0; i < spawnedEnemes.Length; ++i)
                        spawnedEnemes[i] = Instantiate(enemyPrefab, spawnPoint.transform.position, Quaternion.identity, GameObject.Find("@Characters").transform);
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
