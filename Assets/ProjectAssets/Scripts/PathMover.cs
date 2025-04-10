using UnityEngine;

public class PathMover : MonoBehaviour
{
    public Transform[] pathPoints;
    public float speed = 1f;

    private int currentIndex = 0;
    private Transform target;
    public GameObject movingTarget;

    void Start()
    {
        if (pathPoints.Length > 0)
        {
            movingTarget.transform.position = pathPoints[0].position;
            target = pathPoints[0]; 
            SetNextTarget();
        }
    }

    void Update()
    {
        if (target == null) return;
      
        movingTarget.transform.position = Vector3.MoveTowards(movingTarget.transform.position, target.position, speed * Time.deltaTime);
        
        Vector3 direction = target.position - movingTarget.transform.position;
        direction.y = 0f; // vi ignorerar höjdskillnad så det bara roterar i horisontellt plan

        // 2. Om riktningen har längd, rotera ditåt
        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            movingTarget.transform.rotation = Quaternion.Slerp(
                movingTarget.transform.rotation,
                targetRotation,
                Time.deltaTime * 5f // ← justera 5f för hur snabbt den ska svänga
            );
        }

        if (Vector3.Distance(movingTarget.transform.position, target.position) < 0.01f)
        {
            SetNextTarget();
        }
    }

    void SetNextTarget()
    {
        currentIndex = (currentIndex + 1) % pathPoints.Length;
        target = pathPoints[currentIndex];
    }
}