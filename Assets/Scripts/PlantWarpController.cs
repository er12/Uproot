using UnityEngine;

public class PlantWarpController : MonoBehaviour
{
    private int waypointsLength;
    private int counter = 0;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public float sproutForce = 40f;
    public Transform[] wayPoints;

    void Start()
    {
        waypointsLength = wayPoints.Length;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void AnimateGrabbedByRoot(Vector2 pos)
    {
        animator.Play("Plant_GrabbedByRoot");
    }

    public void GoToNextWaypoint()
    {
        transform.position = wayPoints[counter % waypointsLength].transform.position;
        
        counter++;
        spriteRenderer.enabled = true;
        animator.Play("Plant_Growing");
    }
}
