using UnityEngine;
using System.Collections;

public class Energizer : MonoBehaviour {
    void OnTriggerEnter(Collider target) {
        Destroy(gameObject);
    }
}
