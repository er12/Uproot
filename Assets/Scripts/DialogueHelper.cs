using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueHelper : MonoBehaviour
{
	public Text label;
	public Image frame;
	public GameObject cursor;
	public static DialogueHelper instance;
	public bool active = false;
	public bool forceEnd = false;
	public bool permanent = false;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		frame.color = new Color32(0, 0, 0, 0);
		label.text = "";
	}

	public IEnumerator ShowText(string content)
	{
		frame.color = new Color32(0, 0, 0, 233);
		label.text = "";
		for (int i = 0; i < content.Length; i++)
		{
			label.text += content[i];
			yield return new WaitForSeconds(0.01f);
			if (forceEnd)
			{
				label.text = content;
				break;
			}
		}
		yield return new WaitForSeconds(0.01f);
		forceEnd = false;
		if (active) cursor.SetActive(true);
	}

	public IEnumerator DisplayDialogue(string content/*, PlayerInteraction interaction*/)
	{
		active = true;
		//interaction.enabled = false;
		//interaction.GetComponent<PlayerMove>().enabled = false;
		yield return StartCoroutine(ShowText(content));
		yield return StartCoroutine(WaitToCloseDialogue());
		//interaction.GetComponent<PlayerMove>().enabled = true;
		//interaction.enabled = true;
	}

	public IEnumerator WaitToCloseDialogue()
	{
		while (cursor.activeInHierarchy)
		{
			yield return new WaitForEndOfFrame();
		}
	}

	private void Update()
	{
		if (permanent) return;
		if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)))
		{
			if (cursor.activeInHierarchy)
			{
				EndDialogue();
			}
			else
			{
				forceEnd = true;
			}
		}
	}

	public void EndDialogue()
	{
		forceEnd = true;
		cursor.SetActive(false);
		active = false;
		//GameManager.instance.StartCoroutine(GameManager.instance.PlaySound("Tap", 0f, 1f));
		frame.color = new Color32(0, 0, 0, 0);
		label.text = "";
		active = false;
	}
}
