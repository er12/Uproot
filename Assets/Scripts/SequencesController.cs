using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencesController : MonoBehaviour
{
    Coroutine previous = null;
    public IEnumerator DisplayText(string content)
    {
        if (previous != null)
        {
            StopCoroutine(previous);
        }
        //NPC.GetComponent<Animator>().Play("Talking");
        DialogueHelper.instance.active = true;
        previous = StartCoroutine(DialogueHelper.instance.ShowText(content));

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

        while (DialogueHelper.instance.active)
        {
            yield return new WaitForSeconds(0.1f);
        }
    }

	private IEnumerator Start()
	{
        yield return null;
        yield return StartCoroutine(DisplayText("Welcome to the world..."));
    }
}
