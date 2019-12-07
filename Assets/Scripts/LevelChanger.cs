using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public Animator animator;
    public GameObject playerHUD;
    public GameObject deathOverlay;



    public void FadeToNextLevel()
    {
        animator.SetTrigger("FadeOut");
    }

    public void FadeAnimationComplete()
    {
        playerHUD.SetActive(true);
        deathOverlay.SetActive(false);
        Time.timeScale = 1F;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    
}
