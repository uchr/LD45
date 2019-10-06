using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {
    public Transform inner;

    public int damage = 2;
    public float attackTime = 1.0f;
    public float agressionRange = 20.0f;

    private float timer = -1.0f;

    private NavMeshAgent cachedNavMeshAgent;
    private Animator cachedAnimatorController;

    private void Awake() {
        cachedNavMeshAgent = GetComponent<NavMeshAgent>();
        cachedAnimatorController = GetComponentInChildren<Animator>();
    }

    private void Update() {
        timer -= Time.deltaTime;

        GameObject targetPlayer = Utils.ClosestObjectByTag("Player", transform.position, agressionRange);
        if (targetPlayer != null) {
            cachedNavMeshAgent.isStopped = false;
            cachedNavMeshAgent.SetDestination(targetPlayer.transform.position);
        }
        else
            cachedNavMeshAgent.isStopped = true;

        if (cachedNavMeshAgent.velocity.magnitude > 0.01f) {
            inner.rotation = Quaternion.LookRotation(cachedNavMeshAgent.velocity.normalized);
        }
        if (cachedNavMeshAgent.velocity.magnitude > 0.2f)
            cachedAnimatorController.SetFloat("Velocity", cachedNavMeshAgent.velocity.magnitude);
        else
            cachedAnimatorController.SetFloat("Velocity", 0.0f);
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
