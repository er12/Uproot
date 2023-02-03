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

        if (player.lastDirection.x > 0.1) direction.x = 1;
        if (player.lastDirection.y > 0.1) direction.y = 1;
        if (player.lastDirection.x < 0.1) direction.x = -1;
        if (player.lastDirection.y < 0.1) direction.y = -1;

        // direction.x = player.lastDirection.x > 0 ? 1 : -1;
        // direction.y = player.lastDirection.y > 0 ? 1 : -1;

        StartCoroutine(die());
                Debug.Log(direction);
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
