using System.Collections;
using UnityEngine;

public class RootController : MonoBehaviour
{
    Vector2 direction = new Vector2(0f, 1f);
    Rigidbody2D rb;
    private float moveSpeed = 0.35f;
    private bool rootIsmoving = false;

    PlayerController player;
    private void Awake()
	{
        player = GameObject.FindObjectOfType<PlayerController>();
        rb = gameObject.GetComponent<Rigidbody2D>();
	}

	void Start()
    {
        StartCoroutine(die());
    }

    public void Init(Vector2 direction)
    {
        this.direction = direction;
        rootIsmoving = true;
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        for (int i = 0; i < player.maxRootTicks; i++)
        {
            yield return new WaitForSeconds(moveSpeed);
            transform.position += (Vector3)direction;
        }

    }

    /*void Update()
    {
        if (rootIsmoving)
        {
            rb.velocity = direction * moveSpeed;
        }

    }*/

    IEnumerator die()
    {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }
}
