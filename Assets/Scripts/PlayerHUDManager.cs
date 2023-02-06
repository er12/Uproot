using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHUDManager : MonoBehaviour
{
	public static bool hasKey = false;

	private void Start()
	{
		PlayerTakingDamageState.OnTakeDamage += OnTookDamage;
		Chest.OnObtainKey += OnObtainKey;
		Chest.OnUseKey += OnUseKey;
	}

	private void OnDestroy()
	{
		PlayerTakingDamageState.OnTakeDamage -= OnTookDamage;
		Chest.OnObtainKey -= OnObtainKey;
		Chest.OnUseKey -= OnUseKey;
	}

	private void OnTookDamage()
	{
		if (transform.GetChild(4).gameObject.activeSelf)
		{
			transform.GetChild(4).gameObject.SetActive(false);
		}
		else if (transform.GetChild(3).gameObject.activeSelf)
		{
			transform.GetChild(3).gameObject.SetActive(false);
		}
		else if (transform.GetChild(2).gameObject.activeSelf)
		{
			transform.GetChild(2).gameObject.SetActive(false);
		}
		else if (transform.GetChild(1).gameObject.activeSelf)
		{
			transform.GetChild(1).gameObject.SetActive(false);
		}
		else if (transform.GetChild(0).gameObject.activeSelf)
		{
			transform.GetChild(0).gameObject.SetActive(false);
			//CALL DEATH
		}
	}

	private void OnObtainKey()
	{
		hasKey = true;
		if (!transform.GetChild(5).gameObject.activeSelf)
		{
			transform.GetChild(5).gameObject.SetActive(true);
		}
		else if (!transform.GetChild(6).gameObject.activeSelf)
		{
			transform.GetChild(6).gameObject.SetActive(true);
		}
		else if (!transform.GetChild(7).gameObject.activeSelf)
		{
			transform.GetChild(7).gameObject.SetActive(true);
		}
	}

	private void OnUseKey()
	{
		if (transform.GetChild(7).gameObject.activeSelf)
		{
			transform.GetChild(7).gameObject.SetActive(false);
		}
		else if (transform.GetChild(6).gameObject.activeSelf)
		{
			transform.GetChild(6).gameObject.SetActive(false);
		}
		else if (transform.GetChild(5).gameObject.activeSelf)
		{
			transform.GetChild(5).gameObject.SetActive(false);
			hasKey = false;
		}
	}
}
