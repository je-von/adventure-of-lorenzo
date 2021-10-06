using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    public bool isShooting = false;
    public ParticleSystem flash, hitEffect;
    public Transform raycastSource;
    public Transform raycastDest;

    public TrailRenderer tracer;

    Ray ray;
    RaycastHit hitInfo;
    public void StartShooting()
    {
        isShooting = true;
        flash.Emit(1);

        ray.origin = raycastSource.position;
        ray.direction = raycastDest.position - raycastSource.position;

        var t = Instantiate(tracer, ray.origin, Quaternion.identity);
        t.AddPosition(ray.origin);
        if(Physics.Raycast(ray, out hitInfo))
        {
            //Debug.Log(hitInfo.collider.gameObject.name);
            //Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1.0f);
            hitEffect.transform.position = hitInfo.point;
            hitEffect.transform.forward = hitInfo.normal;
            hitEffect.Emit(1);

            t.transform.position = hitInfo.point;
        }
    }

    public void StopShooting()
    {
        isShooting = false;
    }
}
