using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    [Tooltip("The position the platform goes when the lock is activated")]
    public Vector2 openPosition;
    [Tooltip("The position the platform goes when the lock is deactivated")]
    public Vector2 closedPosition;

    private Vector2 currentTarget = Vector2.zero;

    private void Start()
    {
        currentTarget = transform.position;
    }

    public void OpenEvent()
    {
        currentTarget = openPosition;
    }

    public void CloseEvent()
    {
        currentTarget = closedPosition;
    }

    private void Update()
    {
        if(Vector3.Distance(currentTarget, transform.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTarget, Time.deltaTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(openPosition, 0.1f);
        Gizmos.DrawWireSphere(closedPosition, 0.1f);
    }
}
