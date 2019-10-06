using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Player : MonoBehaviour {
    [Header("Movement")]
    public Transform inner;
    public float normalSpeed = 10.0f;
    public float fastSpeed = 20.0f;

    [Header("Resurrection")]
    public GameObject playerCopyPrefab;
    public float reshuffleTime = 5.0f;
    public float hordeRadious = 10.0f;
    public float resurrectionRange = 5.0f;
    public float resurrectionTime = 3.0f;

    public GameObject castEffect;
    public float resurrectionEffectSpeed = 10.0f;

    public int debugNumberOfCopies = 10;

    private bool resurrectionMode = false;
    private float resurrectionTimer = -1.0f;
    private float reshuffleTimer = 5.0f;

    [HideInInspector]
    public float resurrectionPercent = 0.0f;
    [HideInInspector]
    public float hpPercent = 0.0f;

    private List<GameObject> playerCopies = new List<GameObject>();

    private Animator cachedAnimatorController;

    private int initHP = 0;

    private void Awake() {
        initHP = GetComponent<CharachterState>().hp;
        cachedAnimatorController = GetComponentInChildren<Animator>();
    }

    private void Update() {
        reshuffleTimer -= Time.deltaTime;
        if (reshuffleTimer < 0.0f)
            ReshuffleCopies();

        bool makingSomeMagick = false;
        if (Input.GetKeyDown(KeyCode.Space)) {
            resurrectionMode = true;
            resurrectionTimer = 0.0f;
        }

        if (Input.GetKey(KeyCode.Space)) {
            cachedAnimatorController.SetBool("Resurrection", true);
            resurrectionTimer += Time.deltaTime;
            castEffect.transform.localPosition += Time.deltaTime * resurrectionEffectSpeed * Vector3.up;
            resurrectionMode = true;
            makingSomeMagick = true;
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            cachedAnimatorController.SetBool("Resurrection", false);
            castEffect.transform.localPosition = 15 * Vector3.down;
            resurrectionMode = false;
        }

        cachedAnimatorController.SetBool("HordeAttack", Input.GetMouseButton(0));
        if (Input.GetMouseButton(0)) {
            Attack(true);
            makingSomeMagick = true;
        }
        else {
            if (GameObject.Find("@Tutorial").GetComponent<Tutorial>().stage == 2) {
                GameObject.Find("@Tutorial").GetComponent<Tutorial>().ResetTimer();
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            Attack(false);
        }

        cachedAnimatorController.SetBool("CircleAttack", Input.GetMouseButton(1));
        if (Input.GetMouseButton(1)) {
            CircleCopies();
            makingSomeMagick = true;
        }
        else {
            if (GameObject.Find("@Tutorial").GetComponent<Tutorial>().stage == 1) {
                GameObject.Find("@Tutorial").GetComponent<Tutorial>().ResetTimer();
            }
        }
        if (Input.GetMouseButtonUp(1))
            ReshuffleCopies();

        if (Input.GetKeyDown(KeyCode.E))
            SpawnPlayerCopiesAround();

        if (resurrectionMode && resurrectionTimer > resurrectionTime) {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Grave");
            foreach (var go in gameObjects) {
                float distance = Vector3.Distance(go.transform.position, transform.position);
                if (distance < resurrectionRange) {
                    playerCopies.Add(Instantiate(playerCopyPrefab, go.transform.position, Quaternion.identity, GameObject.Find("@Characters").transform));
                    Destroy(go);
                }
            }
            if (GameObject.Find("@Tutorial").GetComponent<Tutorial>().stage == 0) {
                transform.position = new Vector3(-15, 0, -54);
                SpawnPlayerCopiesAround();
                GameObject.Find("@Tutorial").GetComponent<Tutorial>().NextStage();
            }
            resurrectionMode = false;
            castEffect.transform.localPosition = 15 * Vector3.down;
            cachedAnimatorController.SetBool("Resurrection", false);
            GetComponent<CharachterState>().hp = initHP;
            ReshuffleCopies();
        }
        resurrectionPercent = resurrectionMode ? resurrectionTimer / resurrectionTime : 0.0f;
        hpPercent = (float) GetComponent<CharachterState>().hp / initHP;

        if (!makingSomeMagick) {
            float speed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ? fastSpeed : normalSpeed;
            Vector3 forward = new Vector3(0.5f, 0.0f, 0.5f);
            Vector3 right = new Vector3(0.5f, 0.0f, -0.5f);
            float velocity = Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal"));
            cachedAnimatorController.SetFloat("Velocity", velocity);
            if (velocity > 0.01f) {
                Vector3 dir = (Input.GetAxis("Vertical") * forward + Input.GetAxis("Horizontal") * right).normalized;
                transform.position += dir * Time.deltaTime * speed;
                inner.forward = dir;
            }
        }
        else {
            cachedAnimatorController.SetFloat("Velocity", 0.0f);
        }
    }

    public void SpawnPlayerCopiesAround() {
        foreach (var go in playerCopies) {
            Destroy(go);
        }
        playerCopies.Clear();

        for (int i = 0; i < debugNumberOfCopies; ++i) {
            Vector3 randomPosition = Utils.RandomCirclePointOnNavMesh(hordeRadious, transform.position);
            NavMeshHit hit;
            if (!NavMesh.SamplePosition(transform.position + randomPosition, out hit, 2.0f, NavMesh.AllAreas))
                playerCopies.Add(Instantiate(playerCopyPrefab, transform.position + randomPosition, Quaternion.identity, GameObject.Find("@Characters").transform));
            else
                playerCopies.Add(Instantiate(playerCopyPrefab, hit.position, Quaternion.identity, GameObject.Find("@Characters").transform));
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

    private void CircleCopies() {
        List<GameObject> liveCopies = new List<GameObject>();
        for (int i = 0; i < playerCopies.Count; ++i) {
            if (playerCopies[i] == null)
                continue;
            liveCopies.Add(playerCopies[i]);
        }

        playerCopies = liveCopies;
        for (int i = 0; i < playerCopies.Count; ++i) {
            float angle = i *  2.0f * Mathf.PI / playerCopies.Count;
            playerCopies[i].GetComponent<PlayerCopyAround>().relativePosition = new Vector3(hordeRadious * Mathf.Cos(angle), 0.0f, hordeRadious * Mathf.Sin(angle));
        }

        reshuffleTimer = reshuffleTime;
    }

    private void Attack(bool attack) {
        List<GameObject> liveCopies = new List<GameObject>();
        for (int i = 0; i < playerCopies.Count; ++i) {
            if (playerCopies[i] == null)
                continue;
            liveCopies.Add(playerCopies[i]);
        }

        playerCopies = liveCopies;
        for (int i = 0; i < playerCopies.Count; ++i) {
            playerCopies[i].GetComponent<PlayerCopyAround>().canAttack = attack;
        }

        reshuffleTimer = reshuffleTime;
    }
}
