using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCopyAround : MonoBehaviour {
    public Transform inner;

    public bool attackHouse = false;
    public bool attackEnemy = false;
    public int damage = 1;
    public float attackTime = 1.0f;
    public float agressionRange = 10.0f;
    public float housesRange = 30.0f;
    public float playerRange = 15.0f;

    public Vector3 relativePosition;

    private float timer = -1.0f;

    public string debugState;
    
    private NavMeshAgent cachedNavMeshAgent;

    private GameObject player;
    private GameObject home;

    private Animator cachedAnimatorController;

    private void Awake() {
        player = GameObject.Find("@Player");
        home = GameObject.Find("Home");
        cachedNavMeshAgent = GetComponent<NavMeshAgent>();

        relativePosition = transform.position - player.transform.position;
        cachedAnimatorController = GetComponentInChildren<Animator>();
    }

    private void Update() {
        timer -= Time.deltaTime;

        bool targetFound= false;

        if (attackHouse) {
            //GameObject targetEnemy = null;
            //GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
            //float minDistance = float.PositiveInfinity;
            //foreach (var go in gameObjects) {
            //    float distance = Vector3.Distance(go.transform.position, player.transform.position + relativePosition);
            //    float distanceToPlayer = Vector3.Distance(go.transform.position, player.transform.position);
            //    if (distance < minDistance && distance < agressionRange && distanceToPlayer < playerRange) {
            //        minDistance = distance;
            //        targetEnemy = go;
            //    }
            //}
            //if (targetEnemy) {
            //    cachedNavMeshAgent.SetDestination(targetEnemy.transform.position);
            //    Debug.DrawLine(transform.position, targetEnemy.transform.position, Color.cyan);
            //    targetFound = true;
            //    debugState = "enemy";
            //}

            if (!targetFound) {
                GameObject targetHouse = null;

                GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("House");
                float minDistance = float.PositiveInfinity;
                foreach (var go in gameObjects) {
                    Vector3 houseDoorPoint = go.GetComponent<House>().spawnPoint.transform.position;
                    float distance = Vector3.Distance(houseDoorPoint, player.transform.position + relativePosition);
                    float distanceToPlayer = Vector3.Distance(houseDoorPoint, player.transform.position);
                    if (distance < minDistance && distance < housesRange && distanceToPlayer < playerRange && go.GetComponent<House>().hp > 0) {
                        minDistance = distance;
                        targetHouse = go;
                    }
                }

                if (targetHouse) {
                    Vector3 targetPositon = targetHouse.GetComponent<House>().spawnPoint.transform.position;
                    NavMeshHit hit;
                    if (!NavMesh.SamplePosition(targetPositon, out hit, 2.0f, NavMesh.AllAreas)) {
                        cachedNavMeshAgent.SetDestination(targetPositon);
                        Debug.DrawLine(transform.position, targetPositon, Color.cyan);
                    }
                    else {
                        cachedNavMeshAgent.SetDestination(hit.position);
                        Debug.DrawLine(transform.position, hit.position, Color.cyan);
                    }

                    if (Vector3.Distance(targetPositon, transform.position) < 2.0f) {
                        if (timer > 0.0f)
                            return;
                        cachedAnimatorController.SetTrigger("HouseAttack");
                        --targetHouse.GetComponent<House>().hp;
                        timer = attackTime;
                        //Destroy(gameObject);
                    }

                    targetFound = true;
                    debugState = "house";
                }
            }
        }

        if (!targetFound) {
            NavMeshHit hit;
            if (!NavMesh.SamplePosition(player.transform.position + relativePosition, out hit, 3.0f, NavMesh.AllAreas))
                cachedNavMeshAgent.SetDestination(player.transform.position + relativePosition);
            else
                cachedNavMeshAgent.SetDestination(hit.position);

            Debug.DrawLine(transform.position, player.transform.position + relativePosition, Color.cyan);
            debugState = "player";
        }

        if (cachedNavMeshAgent.velocity.magnitude > 0.01f) {
            Vector3 dir = cachedNavMeshAgent.velocity;
            dir.y = 0.0f;
            inner.rotation = Quaternion.LookRotation(dir.normalized);
        }
        if (cachedNavMeshAgent.velocity.magnitude > 0.2f)
            cachedAnimatorController.SetFloat("Velocity", cachedNavMeshAgent.velocity.magnitude);
        else
            cachedAnimatorController.SetFloat("Velocity", 0.0f);
    }

    private void OnCollisionStay(Collision collision) {
        if (timer > 0.0f)
            return;

        if (attackEnemy && collision.gameObject.tag == "Enemy") {
            cachedAnimatorController.SetTrigger("Attack");
            CharachterState state = collision.gameObject.GetComponentInParent<CharachterState>();
            state.hp -= damage;
            timer = attackTime;
        }
    }
}
