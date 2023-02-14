using System;
using UnityEngine;

public class PlantWarpController : MonoBehaviour, IGrabbable
{
    public static event Action<Vector2, float> OnRootPlantWarpGrab;

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

    public void AnimateGrabbedByRoot(Vector2 pos) //DEPRECATED, DELETE THIS FUNCTION
    {
        animator.Play("Plant_GrabbedByRoot");
    }

    public void GoToNextWaypoint() //USED BY ANIMATION
    {
        if (waypointsLength > 0)
        {
            transform.position = wayPoints[counter % waypointsLength].transform.position;

            counter++;
        }
        spriteRenderer.enabled = true;
        animator.Play("Plant_Growing");
    }

	public void Grab()
    {
        animator.Play("Plant_GrabbedByRoot");
        OnRootPlantWarpGrab?.Invoke(transform.position, sproutForce);
    }
}
