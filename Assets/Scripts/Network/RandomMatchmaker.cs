using UnityEngine;
using System.Collections;
using System;

public class RandomMatchmaker : MonoBehaviour {

    private const string roomName = "RoomName";
    private RoomInfo[] roomsList;
    public GameObject playerPrefab;              //change to pacman?

    private bool connectedToPhotonNetwork = false;         //might be deprecated

	// Use this for initialization
	void Start () {
        PhotonNetwork.ConnectUsingSettings("0.1");

        //Might be useful to show if connected. Might be useless if Photon does its own gui overlay for this. Only checks if connected to photon server, not if connected to another players game.
        if (PhotonNetwork.connected) {
            connectedToPhotonNetwork = true;
        }
	}

    void OnGUI() {
        if (!PhotonNetwork.connected) {
            GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        } else if (PhotonNetwork.room == null) {

            //Create Room
            if (GUI.Button(new Rect(80, 80, 250, 100), "Start Server"))
                PhotonNetwork.CreateRoom(roomName + Guid.NewGuid().ToString("N"), 
                    new RoomOptions() { maxPlayers = 3 }, null);                                //max players is set here, probably needs tweaking

            //Join Room
            if (roomsList != null) {
                for (int i = 0; i < roomsList.Length; i++) {            //cycle through the current available rooms
                    if (GUI.Button(new Rect(100, 250 + (110 * i), 250, 100), "Join Room #" + i + ":")) {
                        PhotonNetwork.JoinRoom(roomsList[i].name);
                    }
                }
            }

        }
    }

    void OnReceivedRoomListUpdate() {
        roomsList = PhotonNetwork.GetRoomList();
    }

    void OnJoinedRoom() {
        Debug.Log("Connected to Room");

        //Spawn player
        PhotonNetwork.Instantiate(playerPrefab.name, Vector3.up * 5, Quaternion.identity, 0);
    }
	
	// Update is called once per frame
	void Update () {
            
        
    }
}
