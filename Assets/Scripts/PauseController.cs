using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController:MonoBehaviour {

    public static bool gamePaused = false;
    public GameObject pauseMenuCanvas;
    public GameObject playerHUD;


    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if(gamePaused == true) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    public void Resume() {
        pauseMenuCanvas.SetActive(false);
        playerHUD.SetActive(true);
        Time.timeScale = 1F;
        gamePaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause() {
        pauseMenuCanvas.SetActive(true);
        playerHUD.SetActive(false);
        Time.timeScale = 0;
        gamePaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ReturnToMenuScene() {
        SceneManager.LoadScene(0);
    }


}
