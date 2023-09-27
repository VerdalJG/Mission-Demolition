using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    MissionDemolition msScript; //Reference to MissionDemolition script

    // Fields set in Unity Inspector pane
    [Header("Set in Inspector")]
    public GameObject prefabProjectileSteel; // References to all the projectile types
    public GameObject prefabProjectileWood;
    public GameObject prefabProjectileBrick;
    public GameObject prefabProjectileBomb;
    public GameObject prefabProjectileBlackHole;
    public float velocityMult = 8f; // Velocity multiplier
    

    // Fields set dynamically
    [Header("Set Dynamically")]

    public GameObject launchPoint; // Reference to empty game object, which we use as a point of launch, to instantiate the projectiles
    public Vector3 launchPos; // Vector 3 for launch position
    public GameObject projectile; // The current active projectile
    public bool aimingMode; // Boolean to know if we are aiming a projectile
    private Rigidbody projectileRB; // Reference to the projectiles rigid body to apply movement to it
    public int powerUp = 0; // Powerup counter
    
    

    private void Awake()
    {
        Transform launchPointTrans = transform.Find("LaunchPoint"); // Variable of transform type = launchpoint's transform
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
    }

    private void OnMouseEnter()
    {
        // print("Slingshot:OnMouseEnter()");
        launchPoint.SetActive(true);
    }

    private void OnMouseExit()
    {
        // print("Slingshot:OnMouseExit()");
        launchPoint.SetActive(false);
    }

    private void OnMouseDown()
    {
        // The player has pressed the mouse button while over the Slingshot
        aimingMode = true;
        msScript = Camera.main.GetComponent<MissionDemolition>();
        if (powerUp == 0)
        {
            if (msScript.shotsTaken < 2)
            {
                projectile = Instantiate(prefabProjectileWood) as GameObject;
            }
            else if (msScript.shotsTaken < 4 && msScript.shotsTaken >= 2)
            {
                projectile = Instantiate(prefabProjectileBrick) as GameObject;
            }
            else if (msScript.shotsTaken >= 4)
            {
                projectile = Instantiate(prefabProjectileSteel) as GameObject;
            }
        }
        else if (powerUp == 1)
        {
            projectile = Instantiate(prefabProjectileBomb) as GameObject;   
        }
        else if (powerUp == 2)
        {
            projectile = Instantiate(prefabProjectileBlackHole) as GameObject;
        }
        
        // Start it at the launchPoint
        projectile.transform.position = launchPos;
        // Set is to isKinematic for now - While we are moving it with our mouse, preserve the physics for later.
        projectileRB = projectile.GetComponent<Rigidbody>();
        projectileRB.isKinematic = true;
    }

    private void Update()
    {
        //If slingshot is not in aimingMode, don't run this code
        if (!aimingMode) return;

        //Get the current mouse position in 2D screen coordinates
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        //Find the delta from the launchPos to the mousePos3D
        Vector3 mouseDelta = mousePos3D - launchPos;
        // Limit mouseDelta to the radius of the Slingshot SphereCollider
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if(mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        // Move the projectile to this new position
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        if (Input.GetMouseButtonUp(0))
        {
            aimingMode = false;
            projectileRB.isKinematic = false;
            projectileRB.velocity = -mouseDelta * velocityMult;
            FollowCam.POI = projectile;
            projectile = null;
            MissionDemolition.ShotFired(); // Call ShotFired();
            Bomb.isCounting = true; //Set boolean to is counting, to start its timer.
            BlackHole.isActive = true; // Set BlackHole ability to active for use.
        }
    }
}