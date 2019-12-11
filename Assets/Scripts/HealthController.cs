using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    //public bool damaged = false;
    public GameObject deathOverlay;
    public GameObject playerHUD;
    public Text healthUI;
    private PlayerController pc;





    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        pc = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        //adjustDamageIndicator();
        healthUI.text = currentHealth.ToString();
    }



    public void applyDamage(int damageTaken)
    {

        if (damageTaken <= currentHealth)
        {
            currentHealth -= damageTaken;
        }
        else {
            currentHealth = 0;
        }


        //damaged = true;
    }

    public void applyHeal(int healAmount)
    {

        currentHealth += healAmount;

        // Cap heal amount to maximum health
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

    }

    void adjustHUD()
    {

        if (currentHealth <= 0)
        {
            playerHUD.SetActive(false);
            deathOverlay.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            //playerHUD.SetActive(true);
            //deathOverlay.SetActive(false);
            //Time.timeScale = 1F;
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
        }
    }

    void adjustDamageIndicator()
    {

        GameObject player = GameObject.Find("Player");
        Image playerSprite = player.GetComponent<Image>();

        //Color FullyOpaque = new Color(1, 1, 1, 1);
        //Color FullyTransparent = new Color(1, 1, 1, 0);

        playerSprite.color = Color.Lerp(playerSprite.color, new Color(1, 1, 1, Mathf.PingPong(Time.time, 3f)), 10 * Time.deltaTime);


    }


}
