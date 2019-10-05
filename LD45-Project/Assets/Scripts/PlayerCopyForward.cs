using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCopyForward : MonoBehaviour {
    public Transform inner;
    public int damage = 2;
    public float speed = 15.0f;
    public float maxDistance = 5.0f;

    private Vector3 dir;
    private Vector3 initPosition;
    
    private GameObject player;

    private void Awake() {
        player = GameObject.Find("@Player");
        GetComponent<NavMeshAgent>().enabled = false;

        dir = player.GetComponent<Player>().inner.forward;
        inner.forward = dir;
        initPosition = transform.position;
    }

    private void Update() {
        transform.position += dir * speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, initPosition) > maxDistance)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Enemy") {
            CharachterState state = collision.gameObject.GetComponentInParent<CharachterState>();
            state.hp -= damage;
            Destroy(gameObject);
        }
    }
}
