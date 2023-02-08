using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{

    public EnemyController[] enemies;
    public GameObject virtualCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {

        if (other.CompareTag("Player") && !other.isTrigger)
        {
            //Activate all enemies and pots
            for (int i = 0; i < enemies.Length; i++)
            {
                ChangeActivation(enemies[i], true);
            }
            
            virtualCamera.SetActive(true);
        }
    }

    public virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            //Deactivate all enemies and pots
            //Activate all enemies and pots
            for (int i = 0; i < enemies.Length; i++)
            {
                ChangeActivation(enemies[i], false);
            }
            
            virtualCamera.SetActive(false);

        }
    }
    public void ChangeActivation(Component component, bool activation)
    {
        component.gameObject.SetActive(activation);
    }
}
