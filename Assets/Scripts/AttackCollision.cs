using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollision : MonoBehaviour
{
	public PlayerController player;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject == player.gameObject) return;

		if (player.isAttacking)
		{
			Debug.Log("HIT: " + collision.name);
		}
	}
}
