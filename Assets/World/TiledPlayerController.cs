using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiledPlayerController : MonoBehaviour
{
    public float tileSize = 1f;
    public float moveSpeed = 1f;

    private Rigidbody2D rigidbody2D;
    private Vector2 movement;
    Coroutine courutine = null;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        /*var v = Input.GetAxis("Vertical");
        var h = Input.GetAxis("Horizontal");

        var hit = Physics2D.Raycast(transform.position, new Vector2(h, v), 1f);
        if (hit.transform != null)
        {
            return;
        }

        transform.position += new Vector3(h * moveSpeed * Time.deltaTime, v * moveSpeed * Time.deltaTime);*/

        if (courutine != null) return;

        var v = Input.GetAxis("Vertical");
        var h = Input.GetAxis("Horizontal");
        if (v > 0) v = 1;
        if (h > 0) h = 1;
        if (v < 0) v = -1;
        if (h < 0) h = -1;

        if (v != 0)
        {
            var hit = Physics2D.Raycast(transform.position, new Vector2(0f, v), 1f);
            if (hit.transform != null)
            {
                return;
            }
            movement = new Vector2(0f, v) * tileSize;
            courutine = StartCoroutine(MoveCharacter());
        }
        else if (h != 0)
        {
            var hit = Physics2D.Raycast(transform.position, new Vector2(h, 0f), 1f);
            if (hit.transform != null)
            {
                return;
            }
            movement = new Vector2(h, 0f) * tileSize;
            courutine = StartCoroutine(MoveCharacter());
        }
    }

    IEnumerator MoveCharacter()
    {
        Vector2 start = transform.position;
        Vector2 end = start + movement;
        float elapsedTime = 0f;

        while (elapsedTime < moveSpeed)
        {
            transform.position = Vector2.Lerp(start, end, elapsedTime / moveSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
        courutine = null;
    }
}
