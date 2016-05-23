using UnityEngine;
using System.Collections;
using System;

public class RandomMatchmaker : Photon.MonoBehaviour {

    private const string roomName = "RoomName";
    private RoomInfo[] roomsList;
    //public GameObject playerPrefab;              //change to pacman?

    private bool isServerHost = false;             //Indicates whether you are the server host, or not

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
        /*
        if (!PhotonNetwork.connected) {
            Debug.Log("Not connected to Photon");
        }
        else if (PhotonNetwork.room == null) {
            if (MainMenuController.instance.multiPlayerEnabled) {
                if (MainMenuController.instance.creatingGame) {
                    Debug.Log("created room");
                    isServerHost = true;
                    PhotonNetwork.CreateRoom(roomName + Guid.NewGuid().ToString("N"),
                        new RoomOptions() { maxPlayers = 3 }, null); //max players is set here, probably needs tweaking

                    //for (int i = 0; i < roomsList.Length; i++) {            //cycle through the current available rooms
                    //    if (GUI.Button(new Rect(100, 250 + (110 * i), 250, 100), "Join Room #" + i + ":")) {
                    //        PhotonNetwork.JoinRoom(roomsList[i].name);
                    //    }
                    //}
                } else if (MainMenuController.instance.joiningGame) {
                    for (int i = 0; i < roomsList.Length; i++) {            //cycle through the current available rooms
                        if (GUI.Button(new Rect(100, 250 + (110 * i), 250, 100), "Join Room #" + i + ":")) {
                            PhotonNetwork.JoinRoom(roomsList[i].name);
                        }
                     }
                }

            }
        }
        */

            Debug.Log("is photon network offline? " + PhotonNetwork.offlineMode);
            
           if (!PhotonNetwork.connected) {
                GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
            } else if (PhotonNetwork.room == null) {

                //Create Room
                if (GUI.Button(new Rect(80, 80, 250, 100), "Start Server")) {
                    isServerHost = true;
                    PhotonNetwork.CreateRoom(roomName + Guid.NewGuid().ToString("N"),
                        new RoomOptions() { maxPlayers = 3 }, null);                                //max players is set here, probably needs tweaking
                }


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

        //Are you the server host? If so, you are Pacman. Otherwise, you will be Blinky.
        if (isServerHost == true) {
            GameObject pacMan = (GameObject)PhotonNetwork.Instantiate("Pacman", new Vector3(-28.97f, 0.172f, -13.991f), Quaternion.Euler(0, 90, 0), 0);
            pacMan.SetActive(true);

            GameObject blinky = (GameObject)PhotonNetwork.Instantiate("Blinky", new Vector3(-28.97f, 0.172f, -18.991f), Quaternion.Euler(0, 90, 0), 0);
            blinky.SetActive(true);
        }
        else {
            GameObject pacMan = (GameObject)PhotonNetwork.Instantiate("Pacman", new Vector3(-28.97f, 0.172f, -13.991f), Quaternion.Euler(0, 90, 0), 0);
            //pacMan.SetActive(true);

            GameObject blinky = (GameObject)PhotonNetwork.Instantiate("Blinky", new Vector3(-28.97f, 0.172f, -18.991f), Quaternion.Euler(0, 90, 0), 0);
            //blinky.SetActive(true);
        }

        //Spawn player
        //PhotonNetwork.Instantiate("Pacman", Vector3.up * 5, Quaternion.identity, 0);
    }
	
	// Update is called once per frame
	void Update () {
            
        
    }
}
