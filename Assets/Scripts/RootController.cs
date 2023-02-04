using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RootController : MonoBehaviour
{


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
        RootIndicatorController.OnRootNothingGrab += OnNothingGrabbed;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnDestroy()
    {
        RootIndicatorController.OnRootNothingGrab -= OnNothingGrabbed;
    }

    void OnNothingGrabbed()
    {
       // StartCoroutine(RetractRoot());
    }

    void Start()
    {
    }

    void Update()
    {

    }

    private IEnumerator RetractRoot()
    {
        var player = FindObjectOfType<PlayerController>();

        // Vector2 direction = player.lastDirection.normalized;
        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }


    


}
