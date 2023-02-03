using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootController : MonoBehaviour
{
    Vector2 position;
    Vector2 direction;
    PlayerController player;


    public float movingSpeed = .16f;

    private bool rootIsmoving = true;

    private float nextActionTime = 1f;
    public float period = 0.75f;

    public float tileCounter = 1f;
    public float tileSize = 16f;
    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponentInParent(typeof(PlayerController)) as PlayerController;
        direction = player.lastDirection.normalized;
        Debug.Log("direction");
        Debug.Log(direction);

        StartCoroutine(die());
    }

    // Update is called once per frame
    void Update()
    {
        if (rootIsmoving)
        {
            if (Time.time > nextActionTime)
            {
                transform.Translate(direction * (tileSize * tileCounter) * Time.deltaTime);

                tileCounter++;
                nextActionTime += period;
                // execute block of code here
            }
        }

    }
    IEnumerator die()
    {

        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
