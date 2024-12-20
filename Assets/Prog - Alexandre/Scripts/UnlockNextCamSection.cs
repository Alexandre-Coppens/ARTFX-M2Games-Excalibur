using UnityEngine;

public class UnlockNextCamSection : MonoBehaviour
{
    private bool wasInteracted = false;
    public void Activated()
    {
        if(wasInteracted) return;
        wasInteracted = true;
        Camera.main.GetComponent<CameraMain>().RemoveBlock();
    }
}
