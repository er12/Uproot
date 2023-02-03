using System.Collections;
using UnityEngine;

public class RootController : MonoBehaviour
{
    Vector2 direction = new Vector2(0f, 1f);
    Rigidbody2D rb;
    private float moveSpeed = 0.15f;
    public bool isPressed = false;

    PlayerController player;

    private void Awake()
	{
        player = GameObject.FindObjectOfType<PlayerController>();
        rb = gameObject.GetComponent<Rigidbody2D>();
	}

    public void Init(Vector2 direction)
    {
        this.direction = direction;
        isPressed = true;
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        for (var i = 0; i < player.maxRootTicks; i++)
        {
            yield return null;
            if (!isPressed) break;
            yield return new WaitForSeconds(moveSpeed);
            transform.position += (Vector3)direction;
        }
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

	private void Update()
	{
        if (!Input.GetButton("Root"))
        {
            isPressed = false;
        }
        if (Input.GetButtonUp("Root"))
        {
            isPressed = false;
        }
	}
}
