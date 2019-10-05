using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour {
    public float spawnTime = 5.0f;
    public float radious = 20.0f;

    public GameObject enemyPrefab;
    public int numberOfEnemies = 10;
    public int increaseCount = 2;

    private float timer = -1.0f;

    private void Update() {
        if (timer < 0.0f) {
            for (int i = 0; i < numberOfEnemies; ++i) {
                float angle = Random.Range(0, 2.0f * Mathf.PI);
                float randomRadious = Random.Range(0, radious);
                Vector3 position = new Vector3(randomRadious * Mathf.Cos(angle), 1.3f, randomRadious * Mathf.Sin(angle));
                NavMeshHit hit;
                if (NavMesh.SamplePosition(transform.position + position, out hit, 1.0f, NavMesh.AllAreas))
                    Instantiate(enemyPrefab, transform.position + position, Quaternion.identity);
                else 
                    --i;
            }
            timer = spawnTime;
            numberOfEnemies += increaseCount;
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, radious);
    }
}
