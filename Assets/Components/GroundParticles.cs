using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundParticles : MonoBehaviour
{
	public void Detach()
	{
		transform.parent = null;
		StartCoroutine(StopAfterDelay(0.5f));
	}

	public IEnumerator StopAfterDelay(float delay)
	{
		yield return new WaitForSeconds(delay);
		GetComponent<ParticleSystem>().Stop();
	}
}
