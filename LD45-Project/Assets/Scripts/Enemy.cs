using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {
    public Transform inner;

    public int damage = 2;
    public float attackTime = 1.0f;
    public float agrRange = 30.0f;

    private float timer = -1.0f;

    private GameObject target;

    private NavMeshAgent cachedNavMeshAgent;

    private void Awake() {
        cachedNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        timer -= Time.deltaTime;

        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Player");
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
            cachedNavMeshAgent.isStopped = true;
        }
        else {
            cachedNavMeshAgent.SetDestination(target.transform.position);
            cachedNavMeshAgent.isStopped = false;
            if (cachedNavMeshAgent.velocity.magnitude > 0.01f)
                inner.rotation = Quaternion.LookRotation(cachedNavMeshAgent.velocity.normalized);
        }

    }

    private void OnCollisionStay(Collision collision) {
        if (timer > 0.0f)
            return;

        if (collision.gameObject.tag == "Player") {
            CharachterState state = collision.gameObject.GetComponentInParent<CharachterState>();
            state.hp -= damage;
            timer = attackTime;
        }
    }
}
