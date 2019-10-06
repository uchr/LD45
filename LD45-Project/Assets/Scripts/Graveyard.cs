using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graveyard : MonoBehaviour {
    public GameObject gravePrefab;

    private List<Vector3> positions = new List<Vector3>();
    private List<Quaternion> rotations = new List<Quaternion>();

    private void Start() {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Grave");
        foreach (var go in gameObjects) {
            positions.Add(go.transform.position);
            rotations.Add(go.transform.rotation);
        }
    }

    private void Update() {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] graveObjects = GameObject.FindGameObjectsWithTag("Grave");
        if (playerObjects.Length == 1 && graveObjects.Length == 0) {
            for (int i = 0; i < positions.Count; ++i) {
                Instantiate(gravePrefab, positions[i], rotations[i]);
            }
        }
    }
}
