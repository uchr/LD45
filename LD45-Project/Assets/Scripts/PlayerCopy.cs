using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCopy : MonoBehaviour {
    private GameObject player;

    private Vector3 prevPlayerPosition;
    private Vector3 relativePosition;

    private NavMeshAgent cachedNavMeshAgent;

    private void Awake() {
        player = GameObject.Find("@Player");
        relativePosition = transform.position - player.transform.position;
        cachedNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        cachedNavMeshAgent.SetDestination(player.transform.position + relativePosition);
    }
}
