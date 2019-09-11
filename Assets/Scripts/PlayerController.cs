using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController:PhysicsObject {

	GameObject Player;
	Vector3 StartingPositon;

	public float maxSpeed = 7;
	public float jumpTakeOffSpeed = 7;

	void Start() {
		Player = GameObject.Find("Player");
		StartingPositon = Player.transform.position;
	}

	protected override void ComputeVelocity() {
		Vector2 move = Vector2.zero;

		move.x = Input.GetAxis("Horizontal");

		if (Input.GetButtonDown("Jump") && grounded) {
			velocity.y = jumpTakeOffSpeed;
		} else if (Input.GetButtonUp("Jump")) {
			if (velocity.y > 0) {
				velocity.y = velocity.y * 0.5f;
			}
		}

		targetVelocity = move * maxSpeed;

		if (Player.transform.position.y < -30F) {

			Player.transform.position = StartingPositon;

		}
	}
}