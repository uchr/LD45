using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCopyAround : MonoBehaviour {
    private Vector3 relativePosition;
    
    private NavMeshAgent cachedNavMeshAgent;
    
    private GameObject player;

    private void Awake() {
        player = GameObject.Find("@Player");
        cachedNavMeshAgent = GetComponent<NavMeshAgent>();

        relativePosition = transform.position - player.transform.position;
    }

    private void Update() {
        cachedNavMeshAgent.SetDestination(player.transform.position + relativePosition);
    }
}
