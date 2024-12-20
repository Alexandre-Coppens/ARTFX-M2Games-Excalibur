using UnityEngine;
using UnityEngine.Events;

public class Interactible : MonoBehaviour
{
    [SerializeField] private UnityEvent functionAssigned;

    public void Interacted()
    {
        if (functionAssigned == null) return;
        functionAssigned.Invoke();
    }
}
