using UnityEngine;
using UnityEngine.Events;

public class CanBeHit : MonoBehaviour
{
    [SerializeField] private UnityEvent functionAssigned;

    public void Attacked()
    {
        functionAssigned.Invoke();
    }
}
