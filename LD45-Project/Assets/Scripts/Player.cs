using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [Header("Movement")]
    public Transform inner;
    public float normalSpeed = 10.0f;
    public float fastSpeed = 20.0f;
    
    [Header("Around Copies")]
    public int aroundNumber = 10;
    public GameObject aroundPrefab;
    public float boxSize = 2.0f;

    [Header("Forward Copies")]
    public int forwardNumber = 5;
    public GameObject forwardPrefab;

    private List<GameObject> playerCopies = new List<GameObject>();

    private void Update() {
        float speed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ? fastSpeed : normalSpeed;
        Vector3 forward = new Vector3(0.5f, 0.0f, 0.5f);
        Vector3 right = new Vector3(0.5f, 0.0f, -0.5f);
        if (Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal")) > 0.01f) {
            Vector3 dir = (Input.GetAxis("Vertical") * forward + Input.GetAxis("Horizontal") * right).normalized;
            transform.position += dir * Time.deltaTime * speed;
            inner.forward = dir;
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
        foreach (var go in playerCopies)
            Destroy(go);

        playerCopies.Clear();
        for (int i = 0; i < aroundNumber; ++i) {
            Vector3 copyPosition = Random.Range(-boxSize, boxSize) * Vector3.right + Random.Range(-boxSize, boxSize) * Vector3.forward;
            playerCopies.Add(Instantiate(aroundPrefab, transform.position + copyPosition, Quaternion.identity));
        }
    }
}
