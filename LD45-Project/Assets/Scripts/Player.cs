using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Player : MonoBehaviour {
    [Header("State")]
    public int money = 0;

    [Header("Attack")]
    public int damage = 2;
    public float attackTime = 1.0f;
    public float attackRange = 1.0f;
    private float attackTimer = -1.0f;

    [Header("Movement")]
    public Transform inner;
    public float normalSpeed = 10.0f;
    public float fastSpeed = 20.0f;

    [Header("Resurrection")]
    public float reshuffleTime = 5.0f;
    public float hordeRadious = 10.0f;
    public float resurrectionRange = 5.0f;
    public float resurrectionTime = 3.0f;
    private float resurrectionTimer = -1.0f;

    private float reshuffleTimer = 5.0f;

    [HideInInspector]
    public float resurrectionPercent = 0.0f;

    [Header("Around Copies")]
    public float aroundCooldown = 2.0f;
    public int aroundNumber = 10;
    public GameObject aroundPrefab;
    public float boxSize = 2.0f;

    [Header("Forward Copies")]
    public float forwardCooldown = 1.0f;
    public int forwardNumber = 5;
    public GameObject forwardPrefab;

    private List<GameObject> playerCopies = new List<GameObject>();

    private void Update() {
        attackTimer -= Time.deltaTime;
        reshuffleTimer -= Time.deltaTime;

        float speed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ? fastSpeed : normalSpeed;
        Vector3 forward = new Vector3(0.5f, 0.0f, 0.5f);
        Vector3 right = new Vector3(0.5f, 0.0f, -0.5f);
        if (Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal")) > 0.01f) {
            Vector3 dir = (Input.GetAxis("Vertical") * forward + Input.GetAxis("Horizontal") * right).normalized;
            transform.position += dir * Time.deltaTime * speed;
        }

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.red);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground"))) {
            inner.forward = (hit.point - transform.position).normalized;
            inner.forward = new Vector3(inner.forward.x, 0.0f, inner.forward.z);
        }
        
        if (Input.GetMouseButton(0) && attackTimer < 0.0f) {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var go in gameObjects) {
                float distance = Vector3.Distance(go.transform.position, transform.position);
                if (distance < attackRange) {
                    CharachterState state = go.GetComponentInParent<CharachterState>();
                    state.hp -= damage;
                }
            }
            attackTimer = attackTime;
        }

        if (Input.GetMouseButtonDown(1)) {
            resurrectionTimer = 0.0f;
        }

        if (Input.GetMouseButton(1)) {
            resurrectionTimer += Time.deltaTime;
        }

        if (resurrectionTimer > resurrectionTime) {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Grave");
            foreach (var go in gameObjects) {
                float distance = Vector3.Distance(go.transform.position, transform.position);
                if (distance < resurrectionRange) {
                    playerCopies.Add(Instantiate(aroundPrefab, go.transform.position, Quaternion.identity, GameObject.Find("@Characters").transform));
                    Destroy(go);
                }
            }
            resurrectionTimer = Mathf.NegativeInfinity;
            ReshuffleCopies();
        }

        if (resurrectionTimer > 0.0f) {
            resurrectionPercent = resurrectionTimer / resurrectionTime;
        }
        else
            resurrectionPercent = 0.0f;

        if (reshuffleTimer < 0.0f)
            ReshuffleCopies();

        if (Input.GetKeyDown(KeyCode.Space))
            SpawnPlayerCopiesAround();

        if (Input.GetKeyDown(KeyCode.E))
            SpawnPlayerCopiesForward();
    }

    private void SpawnPlayerCopiesForward() {
        for (int i = 0; i < forwardNumber; ++i) {
            Vector3 copyPosition = Vector3.Cross(inner.forward, Vector3.up) * (i - forwardNumber / 2) + inner.forward;
            Instantiate(forwardPrefab, transform.position + copyPosition, Quaternion.identity, GameObject.Find("@Characters").transform);
        }
    }

    private void SpawnPlayerCopiesAround() {
        for (int i = 0; i < aroundNumber; ++i) {
            Vector3 randomPosition = Utils.RandomCirclePointOnNavMesh(hordeRadious, transform.position);
            playerCopies.Add(Instantiate(aroundPrefab, transform.position + randomPosition, Quaternion.identity, GameObject.Find("@Characters").transform));
        }
    }

    private void ReshuffleCopies() {
        List<GameObject> liveCopies = new List<GameObject>();
        for (int i = 0; i < playerCopies.Count; ++i) {
            if (playerCopies[i] == null) 
                continue;

            Vector3 randomPosition = Utils.RandomCirclePointOnNavMesh(hordeRadious, transform.position);
            playerCopies[i].GetComponent<PlayerCopyAround>().relativePosition = randomPosition;
            liveCopies.Add(playerCopies[i]);
        }

        playerCopies = liveCopies;

        reshuffleTimer = reshuffleTime;
    }
}
