using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
	private bool canInteract = false;
	private bool isOpen = false;
	public Sprite open;
	public Sprite obtained;
	public bool hasToInteract = true;
	public bool requiresKey = false;

	public static event System.Action OnObtainKey;
	public static event System.Action OnUseKey;

	private void Update()
	{
		if (!hasToInteract) return;

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
		if (isOpen) return;

		if (collision.gameObject.GetComponent<PlayerController>())
		{
			if (requiresKey && !PlayerHUDManager.hasKey) return;

			if (!hasToInteract)
			{
				Open();
				return;
			}
			canInteract = true;
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (isOpen) return;

		if (collision.gameObject.GetComponent<PlayerController>())
		{
			if (!hasToInteract)
			{
				return;
			}
			canInteract = false;
		}
	}

	public void Open()
	{
		isOpen = true;
		GetComponent<SpriteRenderer>().sprite = open;
		if (open == null) GetComponent<Collider2D>().enabled = false;
		StartCoroutine(PlayerObtainsItem());
	}

	public IEnumerator PlayerObtainsItem()
	{
		var player = FindObjectOfType<PlayerController>();
		player.TransitionToState(player.TalkingState);
		player.animator.enabled = false;
		player.GetComponent<SpriteRenderer>().sprite = obtained;
		if (!requiresKey) OnObtainKey?.Invoke();
		else OnUseKey?.Invoke();
		yield return new WaitForSeconds(1.5f);
		player.TransitionToIdle();
		player.animator.enabled = true;
	}
}
