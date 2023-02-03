using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RootController : MonoBehaviour
{
    public static event Action OnRootPlantWarpGrab;
    public static event Action OnRootEnemyGrab;
    public static event Action OnRootItemGrab;
    public static event Action OnRootNothingGrab;

    private float moveSpeed = 0.15f;
    private float speed = 1;
    public int ticks = 0;

    private SpriteRenderer spriteRenderer;

    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    private void Awake()
	{
        RootIndicatorController.OnSelect += OnSelect;
        spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void OnDestroy()
	{
		RootIndicatorController.OnSelect -= OnSelect;
	}

	void OnSelect(int ticks)
    {
        this.ticks = ticks;
        //Debug.Log("INSTANTIATING ROOT");
        StartCoroutine(Move());
    }

	void Start()
    {
    }

    void Update()
    {

    }

    private IEnumerator Move()
    {
        var player = FindObjectOfType<PlayerController>();

        Vector2 direction = player.lastDirection.normalized;

        /*for (var i = 0; i < ticks; i++)
        {
            yield return new WaitForSeconds(moveSpeed);
            transform.position += (Vector3)direction * speed;
        }*/
        //OnSelect?.Invoke();
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == Constants.GrabableObjects.PlantWarp)
        {
            OnRootPlantWarpGrab?.Invoke();
        }
        else if (other.tag == Constants.GrabableObjects.Enemy)
        {
            OnRootEnemyGrab?.Invoke();
        }
        else if (other.tag == Constants.GrabableObjects.Item)
        {
            OnRootItemGrab?.Invoke();
        }
        else
        {
            StartCoroutine(NothingGrabbed());
            return;
        }

        Destroy(gameObject);
    }
    private IEnumerator NothingGrabbed()
    {
        OnRootNothingGrab?.Invoke();
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
