using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float delay = 7f;
    float countdown;
    public static bool isCounting;
    bool hasExploded = false;
    public float blastRadius = 5f;
    public float force = 3000f;

    public GameObject particleArea;

    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCounting)
        {
            countdown -= Time.deltaTime;
        }

        if (countdown <= 0f && !hasExploded)
        {
            Explode();
            hasExploded = true;
            isCounting = false;
        }
    }

    public void Explode()
    {

        Instantiate(particleArea, transform.position, Quaternion.identity);

        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius); 

        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, blastRadius,0f, ForceMode.Impulse);
            }
        }
        Destroy(gameObject);
    }
}
