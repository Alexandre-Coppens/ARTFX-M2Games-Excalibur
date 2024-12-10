using System.Collections;
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

    private void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.x <= xStops[0])
        {
            transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        }

        Vector3 playerCamPos = mainCamera.WorldToScreenPoint(player.transform.position);
        if(playerCamPos.y < blindYSpot.x && transform.position.y > maxCameraY.x) { transform.position -= new Vector3(0,ySpeed * Time.deltaTime); }
        if(playerCamPos.y > blindYSpot.y && transform.position.y < maxCameraY.y) { transform.position += new Vector3(0,ySpeed * Time.deltaTime); }
    }

    //public void RemoveBlock()
    //{
    //    xStops.RemoveAt(0)
    //}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        foreach (float stop in xStops)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(new Vector3(stop, 10), new Vector3(stop, -10));
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(stop + xDifference, 10), new Vector3(stop + xDifference, -10));
        }

        Vector2 blindSpotWorldX = mainCamera.ScreenToWorldPoint(new Vector3(0, 275 - blindYSpot.x,-10));
        Vector2 blindSpotWorldY = mainCamera.ScreenToWorldPoint(new Vector3(0, 275 - blindYSpot.y,-10));


        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, blindSpotWorldX.y), 0.5f);
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, blindSpotWorldY.y), 0.5f);
        Gizmos.color = Color.black;
        Gizmos.DrawLine(new Vector3(transform.position.x - 10, blindSpotWorldX.y), new Vector3(transform.position.x + 10, blindSpotWorldX.y));
        Gizmos.DrawLine(new Vector3(transform.position.x - 10, blindSpotWorldY.y), new Vector3(transform.position.x + 10, blindSpotWorldY.y));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(transform.position.x - 17.5f, maxCameraY.x), new Vector3(transform.position.x + 17.5f, maxCameraY.x));
        Gizmos.DrawLine(new Vector3(transform.position.x - 17.5f, maxCameraY.y), new Vector3(transform.position.x + 17.5f, maxCameraY.y));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(transform.position.x - 20f, maxCameraY.x - yDifference), new Vector3(transform.position.x + 20f, maxCameraY.x - yDifference));
        Gizmos.DrawLine(new Vector3(transform.position.x - 20f, maxCameraY.y + yDifference), new Vector3(transform.position.x + 20f, maxCameraY.y + yDifference));
    }
}
