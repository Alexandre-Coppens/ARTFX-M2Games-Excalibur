using System.Collections.Generic;
using UnityEngine;

public class CameraMain : MonoBehaviour
{
    [Header("Follows")]
    [Tooltip("Put the player here")]
    [SerializeField] private GameObject player;
    [Tooltip("The X difference")]
    [SerializeField] private float xDifference;
    [Tooltip("The Y difference")]
    [SerializeField] private float yDifference;
    [Tooltip("The Y camera speed")]
    [SerializeField] private float ySpeed = 2;
    [Tooltip("The part the player need to reach to move the camera up & down (x = down, y = up")]
    [SerializeField] private Vector2 blindYSpot;

    [Header("Max Camera Spot")]
    [Tooltip("The leftest & rightest points the camera can reach (x = left, y = right")]
    [SerializeField] private Vector2 maxCameraX;
    [Tooltip("The lowest & highest points the camera can reach (x = down, y = up")]
    [SerializeField] private Vector2 maxCameraY;

    [Header("Stops")]
    [Tooltip("Places on x axis where the Camera stop following the player")]
    [SerializeField] private List<float> xStops;
    [Tooltip("Places on y axis where the Camera stop following the player")]
    [SerializeField] private List<float> yStops;
    [Tooltip("It shows the right border of the camera on a stop")]
    [SerializeField] private float xStopsBorder;
    [Tooltip("It shows the up border of the camera on a stop")]
    [SerializeField] private float yStopsBorder;


    [Header("Debug")]
    [SerializeField] private Camera mainCamera;
     private Vector2  screenBounds;


    private void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.x > maxCameraX.x && player.transform.position.x < maxCameraX.y)
        {
            if(xStops.Count > 0) 
            {
                if (player.transform.position.x <= xStops[0])
                {
                    transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
                }
            }
            else
            {
                transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
            }
            
        }

        Vector3 playerCamPos = mainCamera.WorldToViewportPoint(player.transform.position);
        if(playerCamPos.y < blindYSpot.x && transform.position.y > maxCameraY.x) { transform.position -= new Vector3(0,ySpeed * Time.deltaTime); }
        if(playerCamPos.y > blindYSpot.y && transform.position.y < maxCameraY.y) { transform.position += new Vector3(0,ySpeed * Time.deltaTime); }

        //OMFG
        float cameraHeight = mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        float leftBoundary = transform.position.x - cameraWidth - player.transform.localScale.x;
        float rightBoundary = transform.position.x + cameraWidth + player.transform.localScale.x;
        float bottomBoundary = transform.position.y - cameraHeight;
        float topBoundary = transform.position.y + cameraHeight;

        float clampedX = Mathf.Clamp(player.transform.position.x, leftBoundary, rightBoundary);
        //float clampedY = Mathf.Clamp(player.transform.position.y, bottomBoundary, topBoundary);

        player.transform.position = new Vector3(clampedX, player.transform.position.y, player.transform.position.z);
    }

    public void RemoveBlock()
    {
        xStops.RemoveAt(0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        foreach (float stop in xStops)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(new Vector3(stop, 10), new Vector3(stop, -10));
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(new Vector3(stop + xDifference, 10), new Vector3(stop + xDifference, -10));
        }

        Vector2 blindSpotWorldX = mainCamera.ViewportToWorldPoint(new Vector3(0, blindYSpot.x, 10));
        Vector2 blindSpotWorldY = mainCamera.ViewportToWorldPoint(new Vector3(0, blindYSpot.y, 10));


        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, blindSpotWorldX.y), 0.5f);
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, blindSpotWorldY.y), 0.5f);
        Gizmos.color = Color.black;
        Gizmos.DrawLine(new Vector3(transform.position.x - 10, blindSpotWorldX.y), new Vector3(transform.position.x + 10, blindSpotWorldX.y));
        Gizmos.DrawLine(new Vector3(transform.position.x - 10, blindSpotWorldY.y), new Vector3(transform.position.x + 10, blindSpotWorldY.y));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(transform.position.x - 17.5f, maxCameraY.x), new Vector3(transform.position.x + 17.5f, maxCameraY.x));
        Gizmos.DrawLine(new Vector3(transform.position.x - 17.5f, maxCameraY.y), new Vector3(transform.position.x + 17.5f, maxCameraY.y));
        Gizmos.DrawLine(new Vector3(maxCameraX.x, transform.position.y - 12.5f), new Vector3(maxCameraX.x, transform.position.y + 12.5f));
        Gizmos.DrawLine(new Vector3(maxCameraX.y, transform.position.y - 12.5f), new Vector3(maxCameraX.y, transform.position.y + 12.5f));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(transform.position.x - 20f, maxCameraY.x - yDifference), new Vector3(transform.position.x + 20f, maxCameraY.x - yDifference));
        Gizmos.DrawLine(new Vector3(transform.position.x - 20f, maxCameraY.y + yDifference), new Vector3(transform.position.x + 20f, maxCameraY.y + yDifference));
        Gizmos.DrawLine(new Vector3(maxCameraX.x - xDifference, transform.position.y - 12.5f), new Vector3(maxCameraX.x - xDifference, transform.position.y + 12.5f));
        Gizmos.DrawLine(new Vector3(maxCameraX.y + xDifference, transform.position.y - 12.5f), new Vector3(maxCameraX.y + xDifference, transform.position.y + 12.5f));

        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position + new Vector3(0, 0, 10), new Vector3(screenBounds.x, screenBounds.y, 0.01f));
    }
}
