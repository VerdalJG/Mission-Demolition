using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI; // The static point of interest

    [Header("Set Dynamically")]
    public float camZ; // The desired z pos of the camera
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;



    private void Awake()
    {
        camZ = transform.position.z;
    }

    private void FixedUpdate()
    {
        Vector3 destination;

        // If there's only one line following an if, it doesnt need brackets
        if (POI == null)
        {
            destination = Vector3.zero; // No POI = Position: (0, 0, 0)
        }

        else
        {
            // Get the position of the POI
            destination = POI.transform.position;
            //If POI is a projectile, check to see if it's at rest
            if (POI.tag == "Projectile")
            {
                //If  it is sleeping (not moving)
                if (POI.GetComponent<Rigidbody>().IsSleeping())
                {
                    //Set the gameobject POI to null
                    POI = null;
                    //In the next update
                    return;
                }
            }

        }

        // Force destination.z to camZ to keep the camera far enough away
        destination.z = camZ;

        //Interpolate from the current camera position to the destination
        destination = Vector3.Lerp(transform.position, destination, easing);

        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);

        //Set the orthographic size of the camera to keep ground in view
        Camera.main.orthographicSize = destination.y + 10;

        //Set the camera to the destination
        transform.position = destination;

    }

}
