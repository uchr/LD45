using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharachterState : MonoBehaviour {
    public int hp = 2;

    private void Update() {
        if (hp < 0) {
            if (GetComponent<Enemy>() != null) {
                GetComponent<Enemy>().SpawnGrave();
            }
            Destroy(gameObject);
        }
    }
}
