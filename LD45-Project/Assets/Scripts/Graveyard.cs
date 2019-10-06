using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graveyard : MonoBehaviour {
    public GameObject gravePrefab;
    public float spawnTime = 10.0f;
    public int numberOfGraves = 10;

    private List<Vector3> positions = new List<Vector3>();
    private List<Quaternion> rotations = new List<Quaternion>();

    private float spawnTimer = 0.0f;

    private void Start() {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Grave");
        foreach (var go in gameObjects) {
            positions.Add(go.transform.position);
            rotations.Add(go.transform.rotation);
            Destroy(go);
        }

        for (int i = 0; i < numberOfGraves; ++i) {
            int randomInd = Random.Range(0, positions.Count);
            Instantiate(gravePrefab, positions[randomInd], rotations[randomInd]);
        }

        spawnTimer = spawnTime;
    }

    private void Update() {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer < 0.0f) {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Grave");
            if (gameObjects.Length < numberOfGraves) {
                int randNumber = Random.Range(3, 5);
                for (int i = 0; i < randNumber; ++i) {
                    int randomInd = Random.Range(0, positions.Count);
                    Instantiate(gravePrefab, positions[randomInd], rotations[randomInd]);
                }
            }
        }
    }
}
