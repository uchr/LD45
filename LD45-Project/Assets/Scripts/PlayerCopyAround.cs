using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCopyAround : MonoBehaviour {
    public Transform inner;

    public int damage = 1;
    public float attackTime = 1.0f;
    public float agressionRange = 7.5f;

    public Vector3 relativePosition;

    public bool hasMoney = false;

    private float timer = -1.0f;
    
    private NavMeshAgent cachedNavMeshAgent;

    private GameObject player;

    private void Awake() {
        player = GameObject.Find("@Player");
        cachedNavMeshAgent = GetComponent<NavMeshAgent>();

        relativePosition = transform.position - player.transform.position;
    }

    private void Update() {
        timer -= Time.deltaTime;

        GameObject targetEnemy = Utils.ClosestObjectByTag("Enemy", transform.position, agressionRange);
        if (targetEnemy) {
            cachedNavMeshAgent.SetDestination(targetEnemy.transform.position);
        }
        else if (!hasMoney) {
            GameObject targetMoney = Utils.ClosestObjectByTag("Money", transform.position, agressionRange);
            if (targetMoney)
                cachedNavMeshAgent.SetDestination(targetMoney.transform.position);
        }
        else {
            cachedNavMeshAgent.SetDestination(player.transform.position + relativePosition);
        }

        if (cachedNavMeshAgent.velocity.magnitude > 0.01f)
            inner.rotation = Quaternion.LookRotation(cachedNavMeshAgent.velocity.normalized);
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
