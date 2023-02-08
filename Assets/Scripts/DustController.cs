using UnityEngine;

public class DustController : MonoBehaviour
{
    public void DeleteDust() //USED BY ANIMATION
    {
        Destroy(gameObject);
    }
}
