using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Constants;

public class RootIndicatorController : MonoBehaviour
{
    public Sprite head;
    public Sprite body;
    public Sprite tail;
    public Sprite hole;

    public Sprite headVertical;
    public Sprite bodyVertical;
    public Sprite tailVertical;

    Vector2 direction = new Vector2(0f, 1f);
    private float moveSpeed = 0.15f;
    private float speed = 1;
    private int maxTicks = 4;
    public bool isPressed = false;
    public static event Action<int> OnSelect;

    public void Init(Vector2 direction)
    {
        this.direction = direction;
        if (direction.y != 0f) head = headVertical;
        if (direction.y != 0f) body = bodyVertical;
        if (direction.y != 0f) tail = tailVertical;
        isPressed = true;
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        var rootController = Instantiate(Resources.Load("Prefabs/Root") as GameObject, transform.position, Quaternion.identity).GetComponent<RootController>();
        rootController.SetSprite(hole);
        if (direction.x > 0f) rootController.transform.localScale = new Vector3(-1f, 1f, 1f);
        if (direction.y < 0f) rootController.transform.localScale = new Vector3(1f, -1f, 1f);
        //yield return new WaitForSeconds(1f);
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
            if (i == 0) rootController.SetSprite(head);
            else if (i > 0) rootController.SetSprite(body);
            rootController = Instantiate(Resources.Load("Prefabs/Root") as GameObject, transform.position, Quaternion.identity).GetComponent<RootController>();
            rootController.SetSprite(tail);
            if (direction.x > 0f) rootController.transform.localScale = new Vector3(-1f, 1f, 1f);
            if (direction.y < 0f) rootController.transform.localScale = new Vector3(1f, -1f, 1f);
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
