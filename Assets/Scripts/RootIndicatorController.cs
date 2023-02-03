using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Constants;

public class RootIndicatorController : MonoBehaviour
{
    Vector2 direction = new Vector2(0f, 1f);
    private float moveSpeed = 0.15f;
    private float speed = 1;
    private int maxTicks = 4;
    public bool isPressed = false;
    public static event Action<int> OnSelect;

    public void Init(Vector2 direction)
    {
        this.direction = direction;
        isPressed = true;
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        int i = 0;
        for (i = 0; i < maxTicks; i++)
        {
            yield return null;
            if (!isPressed)
            {
                break;
            }
            yield return new WaitForSeconds(moveSpeed);
            transform.position += (Vector3)direction * speed;
        }
        yield return new WaitForSeconds(.2f);
        OnSelect?.Invoke(i);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (!Input.GetButton("Root"))
        {
            isPressed = false;
        }
    }
}
