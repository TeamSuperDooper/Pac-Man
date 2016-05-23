using UnityEngine;
using System.Collections;

public class Initialization : MonoBehaviour {

	void Awake () {
        PhotonNetwork.offlineMode = true;
	}
}
