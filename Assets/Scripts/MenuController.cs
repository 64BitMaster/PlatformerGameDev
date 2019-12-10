using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController:MonoBehaviour {

	//	Moves to the next Scene in the Build Order
    public void StartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

	
    public void ExitGame() {
        Debug.Log("Exited");
        Application.Quit();
    }

}
