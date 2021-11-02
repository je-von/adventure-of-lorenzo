using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject cannonBullet;
    public Transform point;
    private float startTime, currentTime;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        currentTime = Time.time - startTime;
        if (currentTime >= 3)
        {
            shootCannon();
            startTime = Time.time;
        }
    }

    void shootCannon()
    {
        GameObject o = Instantiate(cannonBullet, point.position, Quaternion.identity);
        o.transform.rotation = point.rotation;
        Rigidbody rb = o.GetComponent<Rigidbody>();
        rb.AddForce(o.transform.forward * 1200);
    }
}
