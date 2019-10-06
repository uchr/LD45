﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCopyAround : MonoBehaviour {
    public Transform inner;

    public bool canAttack = false;
    public int damage = 1;
    public float attackTime = 1.0f;
    public float agressionRange = 10.0f;
    public float moneyRange = 15.0f;

    public Vector3 relativePosition;

    public bool hasMoney = false;

    private float timer = -1.0f;

    public string debugState;
    
    private NavMeshAgent cachedNavMeshAgent;

    private GameObject player;
    private GameObject home;

    private void Awake() {
        player = GameObject.Find("@Player");
        home = GameObject.Find("Home");
        cachedNavMeshAgent = GetComponent<NavMeshAgent>();

        relativePosition = transform.position - player.transform.position;
    }

    private void Update() {
        timer -= Time.deltaTime;

        bool targetFound= false;
        bool disable = true;
        GameObject targetEnemy = Utils.ClosestObjectByTag("Enemy", player.transform.position, agressionRange);
        if (targetEnemy && canAttack) {
            cachedNavMeshAgent.SetDestination(targetEnemy.transform.position);
            Debug.DrawLine(transform.position, targetEnemy.transform.position, Color.cyan);
            targetFound = true;
            debugState = "enemy";
        }

        if (!targetFound && !hasMoney && !disable) {
            GameObject targetMoney = Utils.ClosestObjectByTag("Money", transform.position, moneyRange);
            if (targetMoney) {
                cachedNavMeshAgent.SetDestination(targetMoney.transform.position);
                Debug.DrawLine(transform.position, targetMoney.transform.position, Color.cyan);

                if (Vector3.Distance(targetMoney.transform.position, transform.position) < 1.0f) {
                    Destroy(targetMoney);
                    hasMoney = true;
                }

                targetFound = true;
                debugState = "money";
            }
        }

        if (!targetFound && hasMoney && !disable) {
            if (Vector3.Distance(home.transform.position, transform.position) < moneyRange) {
                Debug.DrawLine(transform.position, home.transform.position, Color.cyan);
                cachedNavMeshAgent.SetDestination(home.transform.position);
                
                if (Vector3.Distance(home.transform.position, transform.position) < 1.0f) {
                    hasMoney = false;
                    player.GetComponent<Player>().money += 100;
                }

                targetFound = true;
                debugState = "home";
            }
        }

        if (!targetFound) {
            cachedNavMeshAgent.SetDestination(player.transform.position + relativePosition);
            Debug.DrawLine(transform.position, player.transform.position + relativePosition, Color.cyan);
            debugState = "player";
        }

        if (cachedNavMeshAgent.velocity.magnitude > 0.01f) {
            Vector3 dir = cachedNavMeshAgent.velocity;
            dir.y = 0.0f;
            inner.rotation = Quaternion.LookRotation(dir.normalized);
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
