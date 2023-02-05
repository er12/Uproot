using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Constants;


[RequireComponent(typeof(AudioSource))]
public class RootController : MonoBehaviour
{
    /*public Sprite head;
    public Sprite body;
    public Sprite tail;
    public Sprite hole;

    public Sprite headVertical;
    public Sprite bodyVertical;
    public Sprite tailVertical;*/

    Vector2 direction = new Vector2(0f, 1f);
    private float moveSpeed = 0.05f;
    private float speed = 0.2f;
    private int maxTicks = 15;
    public bool isPressed = false;

    public static event Action <Vector2>OnRootPlantWarpGrab;
    public static event Action OnRootEnemyGrab;
    public static event Action OnRootItemGrab;
    public static event Action OnRootNothingGrab;

    public Animator animator;
    SpriteRenderer spriteRenderer;

    public AudioSource audioSource;
    public AudioClip handOutOfGround;
    public AudioClip groundPierce;

    public void Init(Vector2 direction)
    {
        this.direction = direction;
        /*if (direction.y != 0f) head = headVertical;
        if (direction.y != 0f) body = bodyVertical;
        if (direction.y != 0f) tail = tailVertical;*/
        isPressed = true;
        StartCoroutine(Move());
    }

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        animator.enabled = true;

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = groundPierce;
        audioSource.Play();
    }

    private IEnumerator Move()
    {
        /*var rootController = Instantiate(Resources.Load("Prefabs/Root") as GameObject, transform.position, Quaternion.identity).GetComponent<RootController>();
        rootController.SetSprite(hole);
        if (direction.x > 0f) rootController.transform.localScale = new Vector3(-1f, 1f, 1f);
        if (direction.y < 0f) rootController.transform.localScale = new Vector3(1f, -1f, 1f);*/
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
            /*if (i == 0) rootController.SetSprite(head);
            else if (i > 0) rootController.SetSprite(body);
            rootController = Instantiate(Resources.Load("Prefabs/Root") as GameObject, transform.position, Quaternion.identity).GetComponent<RootController>();
            rootController.SetSprite(tail);
            if (direction.x > 0f) rootController.transform.localScale = new Vector3(-1f, 1f, 1f);
            if (direction.y < 0f) rootController.transform.localScale = new Vector3(1f, -1f, 1f);*/
        }
        //yield return new WaitForSeconds(.2f);
        StartCoroutine(NothingGrabbed());
    }

    private void Update()
    {
        if (!Input.GetButton("Root"))
        {
            isPressed = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == Constants.GrabableObjects.PlantWarp)
        {
            animator.enabled = false;
            PlantWarpController plant = other.gameObject.GetComponent<PlantWarpController>();
            plant.AnimateGrabbedByRoot(transform.position);

            //For playerContorller\
            
            OnRootPlantWarpGrab?.Invoke(other.transform.position);
            Destroy(gameObject);
        }
        else if (other.tag == Constants.GrabableObjects.Enemy)
        {
            OnRootEnemyGrab?.Invoke();
        }
        else if (other.tag == Constants.GrabableObjects.Item)
        {
            OnRootItemGrab?.Invoke();
        }
    }
    private IEnumerator NothingGrabbed()
    {
        spriteRenderer.enabled = true;
        //fixSprite();
        //OnRootNothingGrab?.Invoke();
        string sideString = "Side";//direction.x == 0 ? "Up" : "Side";

        audioSource.clip = handOutOfGround;
        animator.Play("Root_HandEmpty_" + sideString);
        audioSource.Play();

        yield return new WaitForSeconds(1f);
        OnRootNothingGrab?.Invoke();
        Destroy(gameObject);
    }

    public void retractRoot()
    {
        audioSource.Pause();
        spriteRenderer.enabled = false;
        spriteRenderer.transform.position -= (Vector3)new Vector2(0, direction.y); // to move pivot back

    }

    // public void retractRoot()
    // {
    //     audioSource.Pause();
    //     spriteRenderer.enabled = false;
    //     spriteRenderer.transform.position -= (Vector3)new Vector2(0, direction.y); // to move pivot back

    // }

    // //for Down and Left
    // void fixSprite()
    // {
    //     spriteRenderer.flipX = (direction == new Vector2(1f, 0f));
    //     if (direction == new Vector2(0f, -1f))
    //     {
    //         spriteRenderer.transform.position += (Vector3)new Vector2(0, direction.y); // to move pivot
    //     }
    // }
}
