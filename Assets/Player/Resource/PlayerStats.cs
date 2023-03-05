using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerStats", menuName = "Player/PlayerStats")]
public class PlayerStats : ScriptableObject
{  


    private int _health;
    public int health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
            // Call event
        }
    }

}

