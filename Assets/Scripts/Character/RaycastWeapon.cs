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
    public GameObject StartShooting()
    {
        Debug.Log("masukkk");

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

            if(hitEffect != null)
            {
                hitEffect.transform.position = hitInfo.point;
                hitEffect.transform.forward = hitInfo.normal;
                hitEffect.Emit(1);

            }

            t.transform.position = hitInfo.point;

            //Debug.Log(hitInfo.collider.gameObject.name);
            //if(hitInfo.collider.gameObject.name == "Robot Kyle")
            //{
            //    KyleController kc = hitInfo.collider.gameObject.GetComponentInChildren<KyleController>();
            //    kc.kyle

            //}

            return hitInfo.collider.gameObject;
        }

        return null;
    }

    public void StopShooting()
    {
        isShooting = false;
    }
}
