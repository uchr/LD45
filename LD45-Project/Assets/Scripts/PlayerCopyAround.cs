using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCopyAround : MonoBehaviour {
    public int damage = 1;
    public float attackTime = 1.0f;
    public float agrRange = 20.0f;

    private float timer = -1.0f;

    private GameObject target;
    private Vector3 relativePosition;

    private NavMeshAgent cachedNavMeshAgent;

    private GameObject player;

    private void Awake() {
        player = GameObject.Find("@Player");
        cachedNavMeshAgent = GetComponent<NavMeshAgent>();

        relativePosition = transform.position - player.transform.position;
    }

    private void Update() {
        timer -= Time.deltaTime;

        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        float minDistance = float.PositiveInfinity;
        foreach (var go in gameObjects) {
            float distance = Vector3.Distance(go.transform.position, transform.position);
            if (distance < minDistance) {
                minDistance = distance;
                target = go;
            }
        }

        if (minDistance > agrRange) {
            target = null;
            if (!cachedNavMeshAgent.SetDestination(player.transform.position + relativePosition))
                Debug.Log("Bad path");
        }
        else {
            if (!cachedNavMeshAgent.SetDestination(target.transform.position))
                Debug.Log("Bad path");
        }
    }

    private void OnCollisionStay(Collision collision) {
        if (timer > 0.0f)
            return;

        if (collision.gameObject.tag == "Enemy") {
            CharachterState state = collision.gameObject.GetComponentInParent<CharachterState>();
            state.hp -= damage;
            timer = attackTime;
        }
    }
}
