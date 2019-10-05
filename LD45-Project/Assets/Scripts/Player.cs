using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Player : MonoBehaviour {
    [Header("Attack")]
    public int damage = 2;
    public float attackTime = 1.0f;
    public float attackRange = 1.0f;
    private float attackTimer = -1.0f;

    [Header("Movement")]
    public Transform inner;
    public float normalSpeed = 10.0f;
    public float fastSpeed = 20.0f;
    
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

        if (Input.GetKeyDown(KeyCode.Space))
            SpawnPlayerCopiesAround();

        if (Input.GetKeyDown(KeyCode.E))
            SpawnPlayerCopiesForward();
    }

    private void SpawnPlayerCopiesForward() {
        for (int i = 0; i < forwardNumber; ++i) {
            Vector3 copyPosition = Vector3.Cross(inner.forward, Vector3.up) * (i - forwardNumber / 2) + inner.forward;
            Instantiate(forwardPrefab, transform.position + copyPosition, Quaternion.identity);
        }
    }

    private void SpawnPlayerCopiesAround() {
        for (int i = 0; i < aroundNumber; ++i) {
            Vector3 copyPosition = Random.Range(-boxSize, boxSize) * Vector3.right + Random.Range(-boxSize, boxSize) * Vector3.forward;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position + copyPosition, out hit, 1.0f, NavMesh.AllAreas))
                playerCopies.Add(Instantiate(aroundPrefab, transform.position + copyPosition, Quaternion.identity));
            else 
                --i;
        }
    }
}
