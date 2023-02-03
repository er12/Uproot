using System.Collections;
using System;
using UnityEngine;
using Constants;

public class RootController : MonoBehaviour
{
    Vector2 direction = new Vector2(0f, 1f);
    Rigidbody2D rb;
    private float moveSpeed = 0.15f;
    public bool isPressed = false;

    PlayerController player;
    CapsuleCollider2D collider;

    public static event Action OnRootPlantWarpGrab;
    public static event Action OnRootEnemyGrab;
    public static event Action OnRootItemGrab;
    public static event Action OnRootNothingGrab;


    private void Awake()
    {
        collider = gameObject.GetComponent<CapsuleCollider2D>();
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
            if (!isPressed)
            {
                collider.enabled = true;
                yield break;
            }
            yield return new WaitForSeconds(moveSpeed);
            transform.position += (Vector3)direction;
        }
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
    private IEnumerator NothingGrabbed()
    {
        OnRootNothingGrab?.Invoke();
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


}
