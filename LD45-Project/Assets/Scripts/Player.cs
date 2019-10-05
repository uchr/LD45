using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float normalSpeed = 10.0f;
    public float fastSpeed = 20.0f;
    
    [Header("Copies")]
    public int numberOfCopies = 10;
    public GameObject copyPrefab;
    public float boxSize = 2.0f;

    private List<GameObject> playerCopies = new List<GameObject>();

    private Rigidbody cachedRigidbody;

    private void Awake() {
        cachedRigidbody = GetComponentInChildren<Rigidbody>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
            SpawnPlayerCopiesAround();

        if (Input.GetKeyDown(KeyCode.E))
            SpawnPlayerCopiesForward();

        float speed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ? fastSpeed : normalSpeed;
        Vector3 forward = new Vector3(0.5f, 0.0f, 0.5f);
        Vector3 right = new Vector3(0.5f, 0.0f, -0.5f);
        Vector3 dir = Input.GetAxis("Vertical") * forward + Input.GetAxis("Horizontal") * right;
        transform.position += dir.normalized * Time.deltaTime * speed;
    }

    private void SpawnPlayerCopiesForward() {
        for (int i = 0; i < numberOfCopies; ++i) {
            Vector3 copyPosition = Vector3.right * (i - numberOfCopies / 2) + Vector3.forward;
            playerCopies.Add(Instantiate(copyPrefab, transform.position + copyPosition, Quaternion.identity));
        }
    }

    private void SpawnPlayerCopiesAround() {
        foreach (var go in playerCopies)
            Destroy(go);

        playerCopies.Clear();
        for (int i = 0; i < numberOfCopies; ++i) {
            Vector3 copyPosition = Random.Range(-boxSize, boxSize) * Vector3.right + Random.Range(-boxSize, boxSize) * Vector3.forward;
            playerCopies.Add(Instantiate(copyPrefab, transform.position + copyPosition, Quaternion.identity));
        }
    }
}
