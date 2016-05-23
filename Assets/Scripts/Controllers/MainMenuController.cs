using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuController : MonoBehaviour {

    public bool multiPlayerEnabled = false;
    public bool joiningGame = false;
    public bool creatingGame = false;
    public bool localCoopEnabled = false;
    public bool singlePlayerEnabled = false;

    private GameObject pacMan;

    [SerializeField]
    private GameObject mainMenuPanel, multiplayerPanel, quitToMainMenuButton, pacManResource, matchMakingScript;

    [SerializeField]
    private Button backButton, exitButton, createServerButton, joinServerButton,
                   startSinglePlayerButton, startLocalCoopButton, startMultiplayerButton;

    public static MainMenuController instance;

    void Awake() {
        if(instance == null) {
            instance = this;
        }
    }

    public void ShowMultiPlayerPanel() {
        mainMenuPanel.SetActive(false);
        multiplayerPanel.SetActive(true);
    }

    public void StartSinglePlayerGame() {
        Time.timeScale = 1;

        singlePlayerEnabled = true;
        PhotonNetwork.offlineMode = true;

        mainMenuPanel.SetActive(false);
        quitToMainMenuButton.SetActive(true);

        pacMan = (GameObject)Instantiate(pacManResource, pacManResource.transform.position, pacManResource.transform.rotation);
        pacMan.SetActive(true);
    }

    public void StartLocalCoopGame() {
        Time.timeScale = 1;

        localCoopEnabled = true;
        PhotonNetwork.offlineMode = true;

        mainMenuPanel.SetActive(false);
        quitToMainMenuButton.SetActive(true);
    }

    public void MultiplayerCreateButtonPressed() {
        Time.timeScale = 1;

        PhotonNetwork.offlineMode = false;
        multiPlayerEnabled = true;
        creatingGame = true;

        quitToMainMenuButton.SetActive(true);
        multiplayerPanel.SetActive(false);

        matchMakingScript.SetActive(true);
    }

    public void MultiplayerJoinButtonPressed() {
        Time.timeScale = 1;

        PhotonNetwork.offlineMode = false;
        multiPlayerEnabled = true;
        joiningGame = true;

        quitToMainMenuButton.SetActive(true);
        multiplayerPanel.SetActive(false);

        matchMakingScript.SetActive(true);
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void BackToMainMenuFromMultiplayer() {
        mainMenuPanel.SetActive(true);
        multiplayerPanel.SetActive(false);
    }

    public void QuitToMainMenuFromGame() {
        Time.timeScale = 0;
        mainMenuPanel.SetActive(true);
        quitToMainMenuButton.SetActive(false);

        singlePlayerEnabled = false;
        multiPlayerEnabled = false;
        localCoopEnabled = false;
        joiningGame = false;
        creatingGame = false;

        if(pacManResource != null) {
            Destroy(pacMan);
        }
    }

}
