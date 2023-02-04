using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleEnemyController : MonoBehaviour
{
    public bool isEnemyFacingRight = true;  // For determining which way the enemy is currently facing.

    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        isEnemyFacingRight = !isEnemyFacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
