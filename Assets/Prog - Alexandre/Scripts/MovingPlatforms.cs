using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    [Tooltip("The position the platform goes when the lock is activated")]
    public Vector2 openPosition;
    [Tooltip("The position the platform goes when the lock is deactivated")]
    public Vector2 closedPosition;

    public bool continousMovement;
    public float speed;

    private Vector2 currentTarget = Vector2.zero;
    private bool move = false;

    private void Start()
    {
        currentTarget = transform.position;
    }

    public void OpenEvent()
    {
        if(!continousMovement) currentTarget = openPosition;
        else move = true;
    }

    public void CloseEvent()
    {
        if (!continousMovement) currentTarget = closedPosition;
        else move = false;
    }

    private void Update()
    {
        if (!continousMovement)
        {
            if (Vector3.Distance(currentTarget, transform.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
            }
        }
        else
        {
            if(move)
            {
                if (Vector3.Distance(currentTarget, transform.position) > 0.1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
                }
                else if (currentTarget == openPosition) currentTarget = closedPosition;
                else currentTarget = openPosition;
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(openPosition, 0.1f);
        Gizmos.DrawWireSphere(closedPosition, 0.1f);
    }
}
