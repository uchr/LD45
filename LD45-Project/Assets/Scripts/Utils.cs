using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class Utils {
    public static GameObject ClosestObjectByTag(string tag, Vector3 position, float maxRange) {
        GameObject result = null;

        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
        float minDistance = float.PositiveInfinity;
        foreach (var go in gameObjects) {
            float distance = Vector3.Distance(go.transform.position, position);
            if (distance < minDistance && distance < maxRange) {
                minDistance = distance;
                result = go;
            }
        }

        return result;
    }

    public static Vector3 RandomCirclePointOnNavMesh(float raidous, Vector3 position) {
        float angle = Random.Range(0, 2.0f * Mathf.PI);
        float randomRadious = Random.Range(0, raidous);
        Vector3 randomPosition = new Vector3(randomRadious * Mathf.Cos(angle), 0.0f, randomRadious * Mathf.Sin(angle));

        int currentTry = 0;
        NavMeshHit hit;
        while (!NavMesh.SamplePosition(position + randomPosition, out hit, 1.0f, NavMesh.AllAreas) && ++currentTry < 10) {
            angle = Random.Range(0, 2.0f * Mathf.PI);
            randomRadious = Random.Range(0, raidous);
            randomPosition = new Vector3(randomRadious * Mathf.Cos(angle), 0.0f, randomRadious * Mathf.Sin(angle));
        }

        if (currentTry == 10)
            Debug.LogError("Too much tries");

        return randomPosition;
    }

}
