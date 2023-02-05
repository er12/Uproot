using UnityEngine;

public class PlantWarpController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform[] wayPoints;
    private int waypointsLength;
    private int counter = 0;
    private Animator animator;
    private SpriteRenderer spriteRenderer;


    private void OnEnable()
    {
        //RootController.OnRootPlantWarpGrab += AnimateGrabbedByRoot;

    }

    private void OnDisable()
    {
        //RootController.OnRootPlantWarpGrab -= AnimateGrabbedByRoot;

    }
    void Start()
    {
        waypointsLength = wayPoints.Length;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

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
