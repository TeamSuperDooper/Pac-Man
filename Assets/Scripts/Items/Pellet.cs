using UnityEngine;
using System.Collections;

public class Pellet : MonoBehaviour {
    void OnTriggerEnter(Collider target) {
        Destroy(gameObject);
    }
}
