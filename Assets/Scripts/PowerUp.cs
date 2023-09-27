using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] GameObject slingshot;
    Slingshot shotScript;

    private void Start()
    {
        slingshot = GameObject.Find("SlingShot");
        shotScript = slingshot.GetComponent<Slingshot>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.StartsWith("Projectile"))
        {
            if (gameObject.tag.StartsWith("BombPU"))
            {
                shotScript.powerUp = 1;
            }
            else if (gameObject.tag.StartsWith("BlackHolePU"))
            {
                shotScript.powerUp = 2;
            }
            Destroy(gameObject);
        }
    }
}
