using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyStart : MonoBehaviour
{
    public Material mat;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        switch (mat.name)
        {
            case "Wood":
                rb.mass = 5;
                break;

            case "Brick":
                rb.mass = 50;
                break;

            case "Steel":
                rb.mass = 70;
                break;
        }

        //Set RigidBodies to sleep on start so the castle does not collapse
        if (rb != null)
        {
            rb.Sleep();
        }
    }
}