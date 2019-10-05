using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour {
    public float radious = 20.0f;

    public GameObject prefab;
    public int numberOfObjects = 10;

    private void Start() {
        Spawn();
    }

    public void Spawn() {
        for (int i = 0; i < numberOfObjects; ++i) {
            Vector3 randomPosition = Utils.RandomCirclePointOnNavMesh(radious, transform.position);
            Instantiate(prefab, transform.position + randomPosition, Quaternion.identity, GameObject.Find("@Characters").transform);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, radious);
    }
}
