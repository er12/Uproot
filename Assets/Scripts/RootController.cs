using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Constants;


[RequireComponent(typeof(AudioSource))]
public class RootController : MonoBehaviour
{
    public static event Action<Vector2, float> OnRootPlantWarpGrab;
    public static event Action OnRootItemGrab;
    public static event Action OnRootNothingGrab;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector2 direction = new Vector2(0f, 1f);
    private float moveSpeed = 0.05f;
    private float speed = 0.2f;
    private int maxTicks = 15;
    private bool isPressed = false;
    private AudioSource audioSource;
    public AudioClip handOutOfGround;
    public AudioClip groundPierce;

    public void Init(Vector2 direction)
    {
        this.direction = direction;
        isPressed = true;
        StartCoroutine(Move());
    }

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = groundPierce;
        audioSource.Play();
        animator.enabled = true;
    }

    private IEnumerator Move()
    {
        GetComponent<Collider2D>().enabled = true;
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
        yield return null;
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
        if (other.tag == "Foreground")
        {
            spriteRenderer.color = new Color(
                spriteRenderer.color.r,
                spriteRenderer.color.g,
                spriteRenderer.color.b,
               0.50f);
            return;
        }
        if (other.tag == Constants.GrabableObjects.PlantWarp)
        {
            animator.enabled = false;
            PlantWarpController plant = other.gameObject.GetComponent<PlantWarpController>();
            plant.AnimateGrabbedByRoot(transform.position);

            //For playerContorller
            
            OnRootPlantWarpGrab?.Invoke(other.transform.position, plant.sproutForce);
            GetComponentInChildren<GroundParticles>().Detach();

            Destroy(gameObject);
        }
        else if (other.tag == Constants.GrabableObjects.Enemy)
        {
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
            enemy.AnimateGrabbedByRoot();
            GetComponentInChildren<GroundParticles>().Detach();
            Destroy(gameObject);
        }
        else if (other.tag == Constants.GrabableObjects.Item)
        {
            OnRootItemGrab?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Foreground")
        {
            spriteRenderer.color = new Color(
                spriteRenderer.color.r,
                spriteRenderer.color.g,
                spriteRenderer.color.b,
               1f);
            return;
        }
    }

    private IEnumerator NothingGrabbed()
    {
        GetComponent<Collider2D>().enabled = false;
        spriteRenderer.enabled = true;
        string sideString = "Side"; //direction.x == 0 ? "Up" : "Side";

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
}
