using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public static bool isActive;
    bool hasImploded = false;
    public float blastRadius = 5f;
    public float force = -3f;

    public GameObject particleArea;

    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive && !hasImploded)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Implode();
                hasImploded = true;
            }
        }
    }

    public void Implode()
    {

        Instantiate(particleArea, transform.position, Quaternion.identity);

        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);

        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, blastRadius, 0f, ForceMode.Impulse);
            }
        }
        Destroy(gameObject);
    }
}
