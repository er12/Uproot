using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueHelper : MonoBehaviour
{
	public Text label;
	public Image frame;
	public GameObject cursor;
	public GameObject dimmer;
	public static DialogueHelper instance;
	public bool active = false;
	public bool forceEnd = false;
	public bool permanent = false;
	public float speed = 0.01f;

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
		dimmer.SetActive(true);
		forceEnd = false;
		frame.color = new Color32(0, 0, 0, 233);
		label.text = "";
		for (int i = 0; i < content.Length; i++)
		{
			label.text += content[i];
			yield return new WaitForSeconds(speed);
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
		if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Return)))
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

	Coroutine previous = null;
	public IEnumerator DisplayText(string content, GameObject portrait, float speed)
	{
		this.speed = speed;
		if (portrait) portrait.SetActive(true);
		if (previous != null)
		{
			StopCoroutine(previous);
		}
		//NPC.GetComponent<Animator>().Play("Talking");
		active = true;
		previous = StartCoroutine(ShowText(content));

		/*int soundId = 4;
        if (Random.Range(0, 2) == 1)
        {
            soundId = 3;
        }
        if (content.Contains("?"))
        {
            soundId = 1;
            if (Random.Range(0, 2) == 1)
                soundId = 2;
        }
        string talkingSound = "Talk" + soundId;
        GameManager.instance.StartCoroutine(GameManager.instance.PlaySound(talkingSound, 0f, 1f));
        LOLSDK.Instance.SpeakText(content);*/

		while (active)
		{
			yield return new WaitForSeconds(0.1f);
		}
		dimmer.SetActive(false);
		if (portrait) portrait.SetActive(false);
	}
}
