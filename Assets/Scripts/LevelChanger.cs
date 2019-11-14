using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public Animator animator;



    public void FadeToNextLevel()
    {
        animator.SetTrigger("FadeOut");
    }

    public void FadeAnimationComplete()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    
}
