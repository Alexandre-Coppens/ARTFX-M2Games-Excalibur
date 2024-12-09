using UnityEngine;
using UnityEngine.Events;

public class Interactible : MonoBehaviour
{
    [SerializeField] private UnityEvent functionAssigned;

    public void Interacted()
    {
        functionAssigned.Invoke();
    }
}
