using System.Collections;
using UnityEngine;

public class RootController : MonoBehaviour
{
    Vector2 direction = new Vector2(0f,1f);
    Rigidbody2D rb;
    private float moveSpeed = 0.5f;
    private bool rootIsmoving = false;
    private float maxTicks = 4f;

	private void Awake()
	{
		rb = gameObject.GetComponent<Rigidbody2D>();
	}

	void Start()
    {
        StartCoroutine(die());
    }

    public void Init(Vector2 direction)
    {
        this.direction = direction;
        Debug.Log(direction);
        rootIsmoving = true;
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        for (int i = 0; i < maxTicks; i++)
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
