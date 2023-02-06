using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
	public GameObject portrait;
	[TextArea(5, 10)]
	public string line;
	public float speed = 0.01f;
}

public class NPC : MonoBehaviour
{
	public Sprite horizontal;
	public Sprite up;
	public Sprite down;

	public List<DialogueLine> lines = new List<DialogueLine>();
	private bool canTalk = false;
	private Coroutine coroutine = null;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.GetComponent<PlayerController>())
		{
			canTalk = true;
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.GetComponent<PlayerController>())
		{
			canTalk = false;
		}
	}

	public IEnumerator Talk()
	{
		var player = FindObjectOfType<PlayerController>();
		var originalSprite = GetComponent<SpriteRenderer>().sprite;

		player.stateMachine.CurrentState = PlayerController.PlayerState.Talking;

		var horizontalDistance = Mathf.Abs(player.transform.position.x - transform.position.x);
		var verticalDistance = Mathf.Abs(player.transform.position.y - transform.position.y);
		if (horizontalDistance > verticalDistance)
		{
			if (player.transform.position.x > transform.position.x)
			{
				GetComponent<SpriteRenderer>().sprite = horizontal;
			}
			if (player.transform.position.x < transform.position.x)
			{
				GetComponent<SpriteRenderer>().sprite = horizontal;
				transform.localScale = new Vector3(-1, 1, 1);
			}
		}
		else
		{
			if (player.transform.position.y > transform.position.y)
			{
				GetComponent<SpriteRenderer>().sprite = up;
			}
			if (player.transform.position.y < transform.position.y)
			{
				GetComponent<SpriteRenderer>().sprite = down;
			}
		}

		yield return null;
		foreach (var dialogue in lines)
		{
			yield return coroutine = StartCoroutine(DialogueHelper.instance.DisplayText(dialogue.line, dialogue.portrait, dialogue.speed));
		}
		coroutine = null;
		transform.localScale = new Vector3(1, 1, 1);
		GetComponent<SpriteRenderer>().sprite = originalSprite;
		player.stateMachine.CurrentState = PlayerController.PlayerState.Idle;

	}

	private void Update()
	{
		if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Return)))
		{
			if (canTalk && coroutine == null)
			{
				StartCoroutine(Talk());
			}
		}
	}
}
