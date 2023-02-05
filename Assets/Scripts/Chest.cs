using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
	private bool canInteract = false;
	private bool isOpen = false;
	public Sprite open;

	private void Update()
	{
		if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Return)))
		{
			if (canInteract && !isOpen)
			{
				Open();
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.GetComponent<PlayerController>())
		{
			canInteract = true;
		}
	}

	public void Open()
	{
		isOpen = true;
		GetComponent<SpriteRenderer>().sprite = open;
	}
}
