using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
	public int maxHealth = 100;
	int currentHealth;
	//public bool damaged = false;

	public Text healthUI;






    // Start is called before the first frame update
    void Start()
    {
		currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
		//adjustDamageIndicator();
		healthUI.text = currentHealth.ToString();
    }



	void applyDamage(int damageTaken) {

		if (damageTaken < currentHealth) {
			currentHealth -= damageTaken;
		}
		else {
			// Death, idk how we want to handle player death, instant respawn? Or just a normal game over screen?
		}

		//damaged = true;
	}

	void applyHeal(int healAmount) {

		currentHealth += healAmount;

		// Cap heal amount to maximum health
		if (currentHealth > maxHealth) {
			currentHealth = maxHealth;
		}

	}

	void adjustHUD() {

	}

	void adjustDamageIndicator() {

		GameObject player = GameObject.Find("Player");
		Image playerSprite = player.GetComponent<Image>();

		//Color FullyOpaque = new Color(1, 1, 1, 1);
		//Color FullyTransparent = new Color(1, 1, 1, 0);

		playerSprite.color = Color.Lerp(playerSprite.color, new Color(1, 1, 1, Mathf.PingPong(Time.time, 3f)), 10*Time.deltaTime);


	}


}
